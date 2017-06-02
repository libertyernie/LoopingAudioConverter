using VGAudio.Containers;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
	public class HPSExporter : VGAudioExporter {
        protected override byte[] GetData(AudioData audio) {
            return new HpsWriter().GetFile(audio);
        }

        protected override string GetExtension() {
            return ".hps";
        }
    }
}
