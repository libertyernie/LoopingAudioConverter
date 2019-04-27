using RunProcessAsTask;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class MP3Exporter : IAudioExporter {
		private readonly string ExePath;
		private readonly string EncodingParameters;

		public MP3Exporter(string exePath, string encodingParameters = null) {
			ExePath = exePath;
			EncodingParameters = encodingParameters ?? "";
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			string outPath = Path.Combine(output_dir, original_filename_no_ext + ".mp3");
			if (outPath.Contains("\"")) {
				throw new AudioExporterException("Invalid character (\") found in output filename");
			}

			if (lwav.OriginalMP3 != null) {
				File.WriteAllBytes(outPath, lwav.OriginalMP3);
				return;
			}

			string infile = TempFiles.Create("wav");
			File.WriteAllBytes(infile, lwav.Export());

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = "--silent " + EncodingParameters + " " + infile + " \"" + outPath + "\""
			};
			var pr = await ProcessEx.RunAsync(psi);
			File.Delete(infile);

			if (pr.ExitCode != 0) {
				throw new AudioExporterException("LAME quit with exit code " + pr.ExitCode);
			}
		}
	}
}
