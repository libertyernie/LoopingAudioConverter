using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class AACExporter : IAudioExporter {
		private string ExePath;
        private bool Adts;
        private string EncodingParameters;

		public AACExporter(string exePath, string encodingParameters = null, bool adts = false) {
            ExePath = exePath;
            Adts = adts;
            EncodingParameters = encodingParameters ?? "";
        }

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			string outPath = Path.Combine(output_dir, original_filename_no_ext + (Adts ? ".aac" : ".m4a"));
			if (outPath.Contains("\"")) {
				throw new AudioExporterException("Invalid character (\") found in output filename");
			}

            string infile = TempFiles.Create("wav");
            File.WriteAllBytes(infile, lwav.Export());

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = $"--silent {(Adts ? "--adts " : "")} {EncodingParameters} {infile} -o \"{outPath}\""
			};
			Process p = Process.Start(psi);
			p.WaitForExit();
            File.Delete(infile);

			if (p.ExitCode != 0) {
				throw new AudioExporterException("qaac quit with exit code " + p.ExitCode);
			}
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			Task t = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			t.Start();
			return t;
		}

		public string GetExporterName() {
			return "AAC (qaac)";
		}
	}
}
