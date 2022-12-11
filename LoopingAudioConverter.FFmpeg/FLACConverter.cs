using LoopingAudioConverter.PCM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LoopingAudioConverter.FFmpeg {
	public class FLACConverter : IAudioExporter, IAudioImporter {
		private readonly FFmpegEngine effectEngine;
		private readonly string encoding_parameters;
		private readonly string metaflac_path;

		public FLACConverter(FFmpegEngine effectEngine, string encoding_parameters, string metaflac_path) {
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
			string ext = lwav.Looping ? ".lflac" : ".flac";
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ext);

			string temp_filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".flac");

			await effectEngine.WriteFileAsync(lwav, temp_filename, encoding_parameters, progress);

            File.Move(temp_filename, output_filename);

            if (lwav.Looping)
			{
				await MetaflacAsync($"--set-tag=LOOP_START={lwav.LoopStart} \"{output_filename}\"");
				await MetaflacAsync($"--set-tag=LOOP_END={lwav.LoopEnd} \"{output_filename}\"");
            }
        }

        public bool SupportsExtension(string extension)
        {
            if (extension.StartsWith(".")) extension = extension.Substring(1);
			if (extension.Equals("flac", StringComparison.InvariantCultureIgnoreCase)) return true;
			if (extension.Equals("lflac", StringComparison.InvariantCultureIgnoreCase)) return true;
			return false;
        }

		public bool SharesCodecsWith(IAudioExporter exporter) => false;

        private static readonly Regex MetaflacVorbisCommentRegex = new Regex(@"^    comment\[[0-9]+\]: ([^=]+)=(.+)");

        public async Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints = null, IProgress<double> progress = null)
        {
			var decoded = await effectEngine.ReadFileAsync(filename, hints, progress);

			var output = await MetaflacAsync($"--list \"{filename}\"");

			IEnumerable<(string, string)> getVorbisComments()
			{
				bool commentBlock = false;
				foreach (string line in output)
				{
					if (!line.StartsWith("  "))
						commentBlock = false;
					if (line.StartsWith("  type: 4 (VORBIS_COMMENT)"))
						commentBlock = true;

					if (commentBlock)
					{
						var match = MetaflacVorbisCommentRegex.Match(line);
						if (match.Success)
							yield return (match.Groups[1].Value, match.Groups[2].Value);
                    }
				}
			}

			int? getIntegerValue(params string[] names)
			{
				foreach ((string n, string v) in getVorbisComments())
					if (names.Contains(n) && int.TryParse(v, out int i))
						return i;
				return null;
			}

			var comments = getVorbisComments().ToDictionary(x => x.Item1, x => x.Item2);

			if (getIntegerValue("LOOP_START") is int s)
			{
				decoded.Looping = true;
				decoded.LoopStart = s;
			}

			if (getIntegerValue("LOOP_END") is int e)
			{
				decoded.LoopEnd = e;
			}

			return decoded;
        }
    }
}
