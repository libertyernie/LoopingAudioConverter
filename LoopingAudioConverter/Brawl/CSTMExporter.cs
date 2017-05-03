using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers;

namespace LoopingAudioConverter.Brawl {
	public class CSTMExporter : IAudioExporter {
		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
            byte[] wav = lwav.Export();
            AudioWithConfig audio = new WaveReader().ReadWithConfig(wav);
            var newAudio = audio.Audio;
            newAudio.SetLoop(lwav.Looping, lwav.LoopStart, lwav.LoopEnd);
            byte[] data = new BrstmWriter().GetFile(newAudio, audio.Configuration);
            File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + ".bcstm"), data);
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext, progressTracker));
			task.Start();
			return task;
		}

		public string GetExporterName() {
			return "BCSTM (VGAudio)";
		}
	}
}
