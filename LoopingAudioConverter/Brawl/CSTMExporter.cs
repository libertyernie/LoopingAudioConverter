using System;
using System.IO;
using System.Threading.Tasks;
using VGAudio.Containers;
using VGAudio.Containers.Bxstm;
using VGAudio.Containers.Wave;
using VGAudio.Formats;

namespace LoopingAudioConverter.Brawl {
	public class CSTMExporter : IAudioExporter {
        private BxstmCodec encoding;

        /// <summary>
        /// Creates a new CSTMExporter instance that uses the given encoding when it has to re-encode a file.
        /// </summary>
        /// <param name="defaultEncoding">The encoding to use</param>
        public CSTMExporter(BxstmCodec defaultEncoding) {
            this.encoding = defaultEncoding;
        }

        public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
            AudioData audio = lwav.OriginalAudioData ?? new WaveReader().Read(lwav.Export());
            audio.SetLoop(lwav.Looping, lwav.LoopStart, lwav.LoopEnd);
            byte[] data = new BcstmWriter().GetFile(audio, new BcstmConfiguration { Codec = this.encoding });
            File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + ".bcstm"), data);
        }

        public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
            Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext, progressTracker));
            task.Start();
            return task;
        }

        public string GetExporterName() {
            return "BCSTM (VGAudio): " + encoding;
        }
    }
}
