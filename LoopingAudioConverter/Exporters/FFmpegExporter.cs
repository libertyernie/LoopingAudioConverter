using MSFContainerLib;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VorbisCommentSharp;

namespace LoopingAudioConverter {
	public class FFmpegExporter : IAudioExporter {
		private readonly FFmpeg effectEngine;
		private readonly string encoding_parameters;
		private readonly string output_extension;

		public FFmpegExporter(FFmpeg effectEngine, string encoding_parameters, string output_extension) {
			this.effectEngine = effectEngine;
			this.encoding_parameters = encoding_parameters;
			this.output_extension = output_extension;
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + output_extension);

			if (Path.GetExtension(lwav.OriginalPath ?? "").Equals(".msf", StringComparison.InvariantCultureIgnoreCase) && output_extension == ".mp3") {
				byte[] data = File.ReadAllBytes(lwav.OriginalPath);
				IPcmAudioSource<short> msf = MSF.Parse(data);
				if (msf is MSF_MP3 mp3) {
					File.WriteAllBytes(output_filename, mp3.Body);
					return;
				}
			}

			if (lwav.OriginalPath != null && output_extension.Equals(Path.GetExtension(lwav.OriginalPath), StringComparison.InvariantCultureIgnoreCase)) {
				File.Copy(lwav.OriginalPath, output_filename, true);
			} else {
				await effectEngine.WriteFileAsync(lwav, output_filename, encoding_parameters);
			}

			if (new string[] { ".ogg", ".logg" }.Contains(output_extension)) {
				using (VorbisFile file = new VorbisFile(File.ReadAllBytes(output_filename))) {
					var commentHeader = file.GetPageHeaders().Select(p => p.GetCommentHeader()).Where(h => h != null).FirstOrDefault();
					var comments = commentHeader?.ExtractComments() ?? new VorbisComments();
					if (lwav.Looping) {
						if (commentHeader == null) throw new Exception("No comment header found in Ogg Vorbis file - cannot edit it to make it loop.");

						comments.Comments.TryGetValue("LOOPSTART", out string loopStart);
						comments.Comments.TryGetValue("LOOPLENGTH", out string loopLength);

						if (loopStart != lwav.LoopStart.ToString() || loopLength != lwav.LoopLength.ToString()) {
							comments.Comments["LOOPSTART"] = lwav.LoopStart.ToString();
							comments.Comments["LOOPLENGTH"] = lwav.LoopLength.ToString();
							using (VorbisFile newFile = new VorbisFile(file, comments)) {
								File.WriteAllBytes(output_filename, newFile.ToByteArray());
							}
						}
					} else {
						if (comments.Comments.ContainsKey("LOOPSTART") || comments.Comments.ContainsKey("LOOPLENGTH")) {
							comments.Comments.Remove("LOOPSTART");
							comments.Comments.Remove("LOOPLENGTH");
							using (VorbisFile newFile = new VorbisFile(file, comments)) {
								File.WriteAllBytes(output_filename, newFile.ToByteArray());
							}
						}
					}
				}
			}
		}
	}
}
