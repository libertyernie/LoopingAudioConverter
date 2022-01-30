using LoopingAudioConverter.Immutable;
using LoopingAudioConverter.PCM;
using System.Linq;

namespace LoopingAudioConverter.Vorbis {
	/// <summary>
	/// Represents 16-bit uncompressed PCM data sourced from an original Ogg Vorbis rendition.
	/// </summary>
	public class VorbisAudio : PCM16Audio {
		private readonly byte[] _originalData;

		private VorbisAudio(byte[] encoded, Audio decoded, Loop loop) : base(decoded, loop) {
			_originalData = encoded;
		}

		public static VorbisAudio Create(byte[] encoded, Audio decoded) {
			using (VorbisFile vorbisFile = new VorbisFile(encoded)) {
				VorbisComments c = vorbisFile.GetPageHeaders()
					.Select(p => p.GetCommentHeader())
					.Where(h => h != null)
					.Select(h => h.ExtractComments())
					.DefaultIfEmpty(new VorbisComments())
					.First();
				Loop GetLoop() {
					if (c.Comments.TryGetValue("LOOPSTART", out string s1) && int.TryParse(s1, out int s2)) {
						if (c.Comments.TryGetValue("LOOPLENGTH", out string l1) && int.TryParse(s1, out int l2)) {
							return Loop.NewLoop(s2, l2);
						} else {
							return Loop.NewLoop(s2, decoded.SamplesPerChannel);
						}
					} else {
						return Loop.NonLooping;
					}
				}
				return new VorbisAudio(encoded, decoded, GetLoop());
			}
		}

		public byte[] Export() {
			using (VorbisFile vorbisFile = new VorbisFile(_originalData)) {
				VorbisComments c = vorbisFile.GetPageHeaders()
					.Select(p => p.GetCommentHeader())
					.Where(h => h != null)
					.Select(h => h.ExtractComments())
					.DefaultIfEmpty(new VorbisComments())
					.First();
				if (Looping) {
					c.Comments["LOOPSTART"] = LoopStart.ToString();
					c.Comments["LOOPLENGTH"] = LoopLength.ToString();
				} else {
					c.Comments.Remove("LOOPSTART");
					c.Comments.Remove("LOOPLENGTH");
				}
				using (VorbisFile newFile = new VorbisFile(vorbisFile, c)) {
					return newFile.ToByteArray();
				}
			}
		}

		public override string ToString() {
			return base.ToString() + " (Ogg Vorbis)";
		}
    }
}
