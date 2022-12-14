using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Vorbis {
    public class VorbisExporter : IAudioExporter {
		private readonly FFmpegEngine effectEngine;
		private readonly string encoding_parameters;

		public VorbisExporter(FFmpegEngine effectEngine, string encoding_parameters) {
			this.effectEngine = effectEngine;
			this.encoding_parameters = encoding_parameters;
		}

		private void SetLoop(string path, ILoopPoints loopPoints) {
			using (VorbisFile vorbisFile = new VorbisFile(File.ReadAllBytes(path))) {
				VorbisComments c = vorbisFile.GetPageHeaders()
					.Select(p => p.GetCommentHeader())
					.Where(h => h != null)
					.Select(h => h.ExtractComments())
					.DefaultIfEmpty(new VorbisComments())
					.First();
				if (loopPoints.Looping) {
					c.Comments["LOOPSTART"] = loopPoints.LoopStart.ToString();
					c.Comments["LOOPLENGTH"] = (loopPoints.LoopEnd - loopPoints.LoopStart).ToString();
				} else {
					c.Comments.Remove("LOOPSTART");
					c.Comments.Remove("LOOPLENGTH");
				}
				using (VorbisFile newFile = new VorbisFile(vorbisFile, c)) {
					File.WriteAllBytes(path, newFile.ToByteArray());
				}
			}
		}

		public bool TryWriteCompressedAudioToFile(object audio, ILoopPoints loopPoints, string output_dir, string original_filename_no_ext) {
			if (audio is VorbisAudio v) {
				string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".ogg");
				File.WriteAllBytes(output_filename, v.Data);
				SetLoop(output_filename, loopPoints);
				return true;
			}
			return false;
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".ogg");
			await effectEngine.WriteFileAsync(lwav, output_filename, encoding_parameters, progress);
			SetLoop(output_filename, lwav);
		}
    }
}
