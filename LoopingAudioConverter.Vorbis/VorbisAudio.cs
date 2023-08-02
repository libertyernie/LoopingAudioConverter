namespace LoopingAudioConverter.Vorbis {
	public sealed class VorbisAudio {
		public readonly byte[] Data;

		public VorbisAudio(byte[] encoded) {
			Data = encoded;
		}

		public override string ToString() {
			return base.ToString() + " (Ogg Vorbis)";
		}
	}
}
