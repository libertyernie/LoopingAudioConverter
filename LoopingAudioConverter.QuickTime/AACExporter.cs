using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using RunProcessAsTask;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.QuickTime {
	public class AACExporter : IPCMAudioExporter {
		private readonly string ExePath;
		private readonly bool Adts;
		private readonly string EncodingParameters;

		public AACExporter(string exePath, string encodingParameters = null, bool adts = false) {
			ExePath = exePath;
			Adts = adts;
			EncodingParameters = encodingParameters ?? "";
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			string outPath = Path.Combine(output_dir, original_filename_no_ext + (Adts ? ".aac" : ".m4a"));
			if (outPath.Contains("\"")) {
				throw new AudioExporterException("Invalid character (\") found in output filename");
			}

			string infile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");
			File.WriteAllBytes(infile, lwav.Export());

			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = $"--silent {(Adts ? "--adts " : "")} {EncodingParameters} {infile} -o \"{outPath}\""
			};
			var pr = await ProcessEx.RunAsync(psi);
			File.Delete(infile);

			if (pr.ExitCode != 0) {
				foreach (string s in pr.StandardError) Console.WriteLine(s);
				throw new AudioExporterException("qaac quit with exit code " + pr.ExitCode);
			}
		}
	}
}
