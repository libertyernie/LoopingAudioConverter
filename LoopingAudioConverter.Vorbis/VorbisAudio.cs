using LoopingAudioConverter.PCM;
using System.Linq;

namespace LoopingAudioConverter.Vorbis {
	/// <summary>
	/// Represents 16-bit uncompressed PCM data sourced from an original Ogg Vorbis rendition.
	/// </summary>
	public class VorbisAudio : PCM16Audio {
		private readonly byte[] _originalData;

		public VorbisAudio(byte[] encoded, PCM16Audio decoded) : base(decoded.Channels, decoded.SampleRate, decoded.Samples) {
			_originalData = encoded;

			using (VorbisFile vorbisFile = new VorbisFile(encoded)) {
				VorbisComments c = vorbisFile.GetPageHeaders()
					.Select(p => p.GetCommentHeader())
					.Where(h => h != null)
					.Select(h => h.ExtractComments())
					.DefaultIfEmpty(new VorbisComments())
					.First();
				if (c.Comments.TryGetValue("LOOPSTART", out string loopStart)) {
					Looping = true;
					LoopStart = int.Parse(loopStart);
				}
				if (c.Comments.TryGetValue("LOOPLENGTH", out string loopLength)) {
					LoopEnd = int.Parse(loopStart) + int.Parse(loopLength);
				}
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
