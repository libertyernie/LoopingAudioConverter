using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace LoopingAudioConverter.VGM {
	/// <summary>
	/// A class to use either FFmpeg or VGMPlay to render VGM/VGZ files to WAV format.
	/// </summary>
	public class VGMImporter : IAudioImporter {
		private readonly FFmpegEngine Engine;
		private readonly string VGMPlayPath;

		/// <summary>
		/// Initializes the VGM importer.
		/// </summary>
		/// <param name="engine">The FFmpeg handler</param>
		public VGMImporter(FFmpegEngine engine) {
			Engine = engine;
		}

		/// <summary>
		/// Initializes the VGM importer.
		/// </summary>
		/// <param name="vgmPlayPath">The path to the VGMPlay executable</param>
		public VGMImporter(string vgmPlayPath) {
			VGMPlayPath = vgmPlayPath;
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

        private class Hints : IRenderingHints {
            public int RenderingSampleRate { get; set; }
			public int SampleCount { get; set; }

			TimeSpan? IRenderingHints.RequiredDecodingDuration => TimeSpan.FromSeconds(SampleCount / (double)RenderingSampleRate);
		}

        /// <summary>
        /// Renders a file to WAV and reads it into a PCM16Audio object.
        /// </summary>
        /// <param name="filename">The path of the file to read</param>
        /// <param name="preferredSampleRate">The sample rate to render the VGM at</param>
        /// <returns>A PCM16Audio, which may or may not be looping</returns>
        public async Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints, IProgress<double> progress) {
			try {
				// Check format
				bool compressed;
				using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				using (var br = new BinaryReader(fs)) {
					int tag = br.ReadUInt16();
					compressed = tag == 0x8B1F;
				}

				int samples, loopSamples;

				// Read loop points from file
				using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				using (var gz = compressed ? new GZipStream(fs, CompressionMode.Decompress) : fs as Stream)
				using (var br = new BinaryReader(gz)) {
					int tag = br.ReadInt32();
					if (tag != 0x206D6756) throw new Exception($"File not in Vgm format ({tag:X8})");

					for (int i = 0; i < 5; i++) br.ReadInt32();

					samples = br.ReadInt32();
					br.ReadInt32();
					loopSamples = br.ReadInt32();
				}

				PCM16Audio data;
				if (VGMPlayPath != null) {
					string tmpDir = Path.Combine(Path.GetTempPath(), "LoopingAudioConverter-" + Guid.NewGuid());
					Directory.CreateDirectory(tmpDir);

					string inFile = Path.Combine(tmpDir, "audio" + Path.GetExtension(filename));
					File.Copy(filename, inFile);
					using (var sw = new StreamWriter(new FileStream(Path.Combine(tmpDir, "VGMPlay.ini"), FileMode.Create, FileAccess.Write))) {
						sw.WriteLine("[General]");
						sw.WriteLine("SampleRate = " + hints.RenderingSampleRate);
						sw.WriteLine("FadeTime = 0");
						sw.WriteLine("LogSound = 1");
						sw.WriteLine("MaxLoops = 0x01");
					}

					ProcessStartInfo psi = new ProcessStartInfo {
						FileName = Path.GetFullPath(VGMPlayPath),
						WorkingDirectory = tmpDir,
						UseShellExecute = false,
						CreateNoWindow = true,
						RedirectStandardOutput = true,
						Arguments = inFile
					};

					var process = Process.Start(psi);

					string line;
					double pr = 0.0;
					using (var sr = process.StandardOutput) {
						while ((line = await sr.ReadLineAsync()) != null) {
							if (line.StartsWith("Playing ")) {
								int stop = line.IndexOf('%');
								if (stop > 8 && double.TryParse(line.Substring(8, stop - 8), out double percentage) && percentage > pr) {
									progress?.Report((pr = percentage) / 100.0);
								}
							}
						}
					}

					process.WaitForExit();
					if (process.ExitCode != 0)
						throw new AudioImporterException($"VGMPlay exited with status code {process.ExitCode}");

					data = WaveConverter.FromFile(Path.Combine(tmpDir, "audio.wav"), true);
					Directory.Delete(tmpDir, true);
				} else if (Engine != null) {
					data = await Engine.ReadFileAsync(filename, new Hints {
						RenderingSampleRate = hints.RenderingSampleRate,
						SampleCount = samples
					}, progress);
                } else {
					throw new NotImplementedException();
                }
				if (loopSamples != 0) {
					data.Looping = true;
					data.LoopStart = (int)Math.Round((samples - loopSamples) * (hints.RenderingSampleRate / 44100.0));
					data.LoopEnd = (int)Math.Round(samples * (hints.RenderingSampleRate / 44100.0));
				}
				return data;
			} catch (Exception e) when (!(e is AudioImporterException)) {
				throw new AudioImporterException("Could not read VGM: " + e.Message, e);
			}
		}
	}
}
