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

        public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".ogg");
			await effectEngine.WriteFileAsync(lwav, output_filename, encoding_parameters, progress);
            using (VorbisFile vorbisFile = new VorbisFile(File.ReadAllBytes(output_filename)))
            {
                VorbisComments c = vorbisFile.GetPageHeaders()
                    .Select(p => p.GetCommentHeader())
                    .Where(h => h != null)
                    .Select(h => h.ExtractComments())
                    .DefaultIfEmpty(new VorbisComments())
                    .First();
                if (lwav.Looping)
                {
                    c.Comments["LOOPSTART"] = lwav.LoopStart.ToString();
                    c.Comments["LOOPLENGTH"] = lwav.LoopLength.ToString();
                }
                else
                {
                    c.Comments.Remove("LOOPSTART");
                    c.Comments.Remove("LOOPLENGTH");
                }
                using (VorbisFile newFile = new VorbisFile(vorbisFile, c))
                {
                    File.WriteAllBytes(output_filename, newFile.ToByteArray());
                }
            }
		}
    }
}
