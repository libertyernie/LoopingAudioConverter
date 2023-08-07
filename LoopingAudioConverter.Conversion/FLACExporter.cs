using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Conversion {
	public abstract class FLACExporter : IAudioExporter {
		private readonly string metaflac_path;

		public FLACExporter(IConverterEnvironment env) {
			metaflac_path = env.MetaflacPath;
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

		protected abstract Task EncodeAsync(PCM16Audio lwav, string outputPath, IProgress<double> progress);

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".flac");

			string temp_filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".flac");

			await EncodeAsync(lwav, temp_filename, progress);

			File.Move(temp_filename, output_filename);

			if (lwav.Looping)
			{
				await MetaflacAsync($"--set-tag=LOOPSTART={lwav.LoopStart} \"{output_filename}\"");
				await MetaflacAsync($"--set-tag=LOOPLENGTH={lwav.LoopEnd - lwav.LoopStart} \"{output_filename}\"");
			}
		}

		public bool TryWriteCompressedAudioToFile(object audio, ILoopPoints loopPoints, string output_dir, string original_filename_no_ext) {
			return false;
		}
	}

	public class FFmpegFLACExporter : FLACExporter {
		private readonly FFmpegEngine ffmpeg;

		public FFmpegFLACExporter(IConverterEnvironment env, FFmpegEngine ffmpeg) : base(env) {
			this.ffmpeg = ffmpeg;
		}

		protected override async Task EncodeAsync(PCM16Audio lwav, string outputPath, IProgress<double> progress) {
			await ffmpeg.WriteFileAsync(lwav, outputPath, "", progress);
		}
	}

	public class MediaFoundationFLACExporter : FLACExporter {
		public MediaFoundationFLACExporter(IConverterEnvironment env) : base(env) { }

		protected override async Task EncodeAsync(PCM16Audio lwav, string outputPath, IProgress<double> progress) {
			await Task.Run(() => MediaFoundation.FLACEncoder.WriteFile(lwav, outputPath));
		}
	}
}
