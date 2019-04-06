using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers.Dsp;
using VGAudio.Formats;

namespace LoopingAudioConverter {
	public class DSPExporter : VGAudioExporter {
		protected override byte[] GetData(AudioData audio) {
			return new DspWriter().GetFile(audio);
		}

		protected override string GetExtension() {
			return ".dsp";
		}
	}
}
