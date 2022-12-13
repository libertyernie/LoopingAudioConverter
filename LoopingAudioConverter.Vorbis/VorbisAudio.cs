using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.PCM;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Vorbis {
	public sealed class VorbisAudio : IAudio {
		private readonly byte[] _originalData;
		private readonly FFmpegEngine _effectEngine;

		public bool Looping { get; set; }
		public int LoopStart { get; set; }
		public int LoopEnd { get; set; }

		public VorbisAudio(byte[] encoded, FFmpegEngine effectEngine) {
			_originalData = encoded;
			_effectEngine = effectEngine;

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
					c.Comments["LOOPLENGTH"] = (LoopEnd - LoopStart).ToString();
				} else {
					c.Comments.Remove("LOOPSTART");
					c.Comments.Remove("LOOPLENGTH");
				}
				using (VorbisFile newFile = new VorbisFile(vorbisFile, c)) {
					return newFile.ToByteArray();
				}
			}
		}

		public async Task<PCM16Audio> DecodeAsync() {
			string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), ".ogg");
			var audio = await _effectEngine.ReadFileAsync(tempFile);
			File.Delete(tempFile);
			return audio;
		}

		public override string ToString() {
			return base.ToString() + " (Ogg Vorbis)";
		}

		void IDisposable.Dispose() { }
	}
}
