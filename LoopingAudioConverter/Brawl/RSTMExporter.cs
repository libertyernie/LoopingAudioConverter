using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers;
using VGAudio.Containers.Bxstm;

namespace LoopingAudioConverter.Brawl {
    public class RSTMExporter : IAudioExporter {
        private BxstmCodec encoding;

        /// <summary>
        /// Creates a new RSTMExporter instance that uses the given encoding when it has to re-encode a file.
        /// </summary>
        /// <param name="defaultEncoding">The encoding to use (ADPCM or PCM16)</param>
        public RSTMExporter(BxstmCodec defaultEncoding) {
            this.encoding = defaultEncoding;
        }

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
            byte[] wav = lwav.Export();
            AudioWithConfig audio = new WaveReader().ReadWithConfig(wav);
            var newAudio = audio.Audio;
            newAudio.SetLoop(lwav.Looping, lwav.LoopStart, lwav.LoopEnd);
            var writer = new BrstmWriter();
            writer.Configuration.Codec = this.encoding;
            byte[] data = writer.GetFile(newAudio, audio.Configuration);
            File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + ".brstm"), data);
        }

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext, progressTracker));
			task.Start();
			return task;
		}

		public string GetExporterName() {
            return "BRSTM (VGAudio): " + encoding;
		}
	}
}
