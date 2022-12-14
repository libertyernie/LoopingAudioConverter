using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers.Wave;
using VGAudio.Formats;

namespace LoopingAudioConverter.VGAudio {
	public abstract class VGAudioExporter : IAudioExporter {
		protected abstract byte[] GetData(AudioData audio);
		protected abstract string GetExtension();

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			AudioData audio = new WaveReader().Read(lwav.Export());
			audio.SetLoop(lwav.Looping, lwav.LoopStart, lwav.LoopEnd);
			byte[] data = GetData(audio);
			File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + GetExtension()), data);
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext));
			task.Start();
			return task;
		}

		public void TryWriteFile(IAudio audio, ILoopPoints loopPoints, string output_dir, string original_filename_no_ext) {
			if (audio is VGAudioAudio a) {
				var d = a.AudioData;
				d.SetLoop(loopPoints.Looping, loopPoints.LoopStart, loopPoints.LoopEnd);
				byte[] data = GetData(d);
				File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + GetExtension()), data);
			}
		}
	}
}
