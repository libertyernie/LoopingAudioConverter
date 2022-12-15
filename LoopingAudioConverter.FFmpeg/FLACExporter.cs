using LoopingAudioConverter.PCM;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.FFmpeg {
	public class FLACExporter : IAudioExporter {
		private readonly FFmpegEngine effectEngine;
		private readonly string encoding_parameters;
		private readonly string metaflac_path;

		public FLACExporter(FFmpegEngine effectEngine, string encoding_parameters, string metaflac_path) {
			this.effectEngine = effectEngine;
			this.encoding_parameters = encoding_parameters;
			this.metaflac_path = metaflac_path;
		}

		private async Task<string[]> MetaflacAsync(string parameters)
		{
			if (metaflac_path == null || !File.Exists(metaflac_path))
				throw new FileNotFoundException("Could not find metaflac");
			var result = await RunProcessAsTask.ProcessEx.RunAsync(new ProcessStartInfo
			{
				FileName = metaflac_path,
				Arguments = parameters,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
			});
			if (result.ExitCode != 0)
				throw new Exception($"metaflac quit with exit code {result.ExitCode}");
			return result.StandardOutput;
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".flac");

			string temp_filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".flac");

			await effectEngine.WriteFileAsync(lwav, temp_filename, encoding_parameters, progress);

            File.Move(temp_filename, output_filename);

            if (lwav.Looping)
			{
				await MetaflacAsync($"--set-tag=LOOPSTART={lwav.LoopStart} \"{output_filename}\"");
				await MetaflacAsync($"--set-tag=LOOPLENGTH={lwav.LoopEnd - lwav.LoopStart} \"{output_filename}\"");
            }
        }
    }
}
