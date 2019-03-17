using LoopingAudioConverter.Vorbis;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class OggVorbisExporter : IAudioExporter {
		private readonly SoX sox;
		private readonly string encodingParameters;

		public OggVorbisExporter(SoX sox, string encodingParameters = null) {
			this.sox = sox;
			this.encodingParameters = encodingParameters;
		}

		public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + ".ogg");

			// Don't re-encode if the original input file was also Ogg Vorbis
			if (lwav.OriginalPath != null && new string[] { ".ogg", ".logg" }.Contains(Path.GetExtension(lwav.OriginalPath), StringComparer.InvariantCultureIgnoreCase)) {
				File.Copy(lwav.OriginalPath, output_filename, true);
			} else {
				await sox.WriteFileAsync(lwav, output_filename, encodingParameters);
			}

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

		public string GetExporterName() {
			return "Ogg Vorbis (SoX)";
		}
	}
}
