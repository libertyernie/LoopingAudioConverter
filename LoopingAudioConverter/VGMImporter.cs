using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace LoopingAudioConverter {
	/// <summary>
	/// A class to use vgm2wav to render VGM/VGM files to WAV format.
	/// </summary>
	public class VGMImporter : IRenderingAudioImporter {
		private string ExePath;

		public int? SampleRate { get; set; }

		/// <summary>
		/// Initializes the VGM importer.
		/// </summary>
		/// <param name="exePath">Path to vgm2wav.exe (relative or absolute.)</param>
		public VGMImporter(string exePath) {
			ExePath = exePath;
			SampleRate = null;
		}

		/// <summary>
		/// Returns whether the extension matches that of a VGM file (vgm or vgz), ignoring any leading period.
		/// </summary>
		/// <param name="extension">Filename extension</param>
		/// <returns>true if vgm or vgz, false otherwise</returns>
		public bool SupportsExtension(string extension) {
			while (extension.StartsWith(".")) extension = extension.Substring(1);
			return string.Equals(extension, "vgm", StringComparison.InvariantCultureIgnoreCase)
				|| string.Equals(extension, "vgz", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Renders a file to WAV using vgm2wav or VGMPlay and reads it into a PCM16Audio object.
		/// </summary>
		/// <param name="filename">The path of the file to read</param>
		/// <param name="preferredSampleRate">The sample rate to render the VGM at (not supported for vgm2wav)</param>
		/// <returns>An LWAV, which may or may not be looping</returns>
		public PCM16Audio ReadFile(string filename) {
			if (!File.Exists(ExePath)) {
				throw new AudioImporterException("vgm2wav / VGMPlay not found at path: " + ExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

			if (Path.GetFileNameWithoutExtension(filename).Equals("vgm2wav", StringComparison.CurrentCultureIgnoreCase)) {
				return ReadFile_vgm2wav(filename);
			} else {
				return ReadFile_VGMPlay(filename);
			}
		}

		private PCM16Audio ReadFile_vgm2wav(string filename) {
			string outfile = TempFiles.Create("wav");
			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = "--loop-count 1 --fade-ms 500 \"" + filename + "\" " + outfile
			};
			Process p = Process.Start(psi);
			p.WaitForExit();
			try {
				return PCM16Factory.FromFile(outfile, true);
			} catch (Exception e) {
				throw new AudioImporterException("Could not read output of vgm2wav: " + e.Message);
			}
		}

		private PCM16Audio ReadFile_VGMPlay(string filename) {
			try {
				string tmpDir = Path.Combine(Path.GetTempPath(), "LoopingaudioConverter-" + Guid.NewGuid());
				Directory.CreateDirectory(tmpDir);

				string inFile = Path.Combine(tmpDir, "audio" + Path.GetExtension(filename));
				File.Copy(filename, inFile);
				using (var sw = new StreamWriter(new FileStream(Path.Combine(tmpDir, "VGMPlay.ini"), FileMode.Create, FileAccess.Write))) {
					sw.WriteLine("[General]");
					if (SampleRate != null) {
						sw.WriteLine("SampleRate = " + SampleRate);
					}
					sw.WriteLine("FadeTime = 500");
					sw.WriteLine("LogSound = 1");
					sw.WriteLine("MaxLoops = 0x01");
				}

				ProcessStartInfo psi = new ProcessStartInfo {
					FileName = Path.GetFullPath(ExePath),
					WorkingDirectory = tmpDir,
					UseShellExecute = false,
					CreateNoWindow = true,
					Arguments = inFile
				};
				Process p = Process.Start(psi);
				p.WaitForExit();
				var data = PCM16Factory.FromFile(Path.Combine(tmpDir, "audio.wav"), true);
				Directory.Delete(tmpDir, true);

				// Read loop points from file
				using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				using (var gz = new GZipStream(fs, CompressionMode.Decompress))
				using (var br = new BinaryReader(gz)) {
					int tag = br.ReadInt32();
					if (tag == 0x56676D20) throw new Exception("Machine is big-endian");
					if (tag != 0x206D6756) throw new Exception($"File not in Vgm format ({tag.ToString("X8")})");

					for (int i = 0; i < 5; i++) br.ReadInt32();

					int samples = br.ReadInt32();
					br.ReadInt32();
					int loopSamples = br.ReadInt32();

					double sampleRateRatio = data.SampleRate / 44100.0;
					samples = (int)(samples * sampleRateRatio);
					loopSamples = (int)(loopSamples * sampleRateRatio);

					if (loopSamples == 0) {
						data.NonLooping = true;
					} else {
						data.Looping = true;
						data.LoopStart = samples - loopSamples;
						data.LoopEnd = samples;
					}
				}

				return data;
			} catch (Exception e) {
				Console.Error.WriteLine(e.GetType());
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				throw new AudioImporterException("Could not read output of VGMPlay: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "VGMImporter";
		}
	}
}
