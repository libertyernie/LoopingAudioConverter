using DspAdpcm;
using DspAdpcm.Adpcm;
using DspAdpcm.Adpcm.Formats;
using DspAdpcm.Adpcm.Formats.Configuration;
using DspAdpcm.Adpcm.Formats.Structures;
using DspAdpcm.Pcm;
using DspAdpcm.Pcm.Formats;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
    public class RSTMExporter : IAudioExporter {
        private B_stmCodec encoding;

        /// <summary>
        /// Creates a new RSTMExporter instance that uses the given encoding when it has to re-encode a file.
        /// </summary>
        /// <param name="defaultEncoding">The encoding to use (ADPCM or PCM16)</param>
        public RSTMExporter(B_stmCodec defaultEncoding) {
            this.encoding = defaultEncoding;
        }

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
            LoopingTrackStream stream = null;

            try {
                switch (Path.GetExtension(lwav.OriginalFilePath ?? "").ToLowerInvariant()) {
                    case ".brstm":
                        stream = new Brstm(File.ReadAllBytes(lwav.OriginalFilePath)).AudioStream;
                        break;
                    case ".bcstm":
                        stream = new Bcstm(File.ReadAllBytes(lwav.OriginalFilePath)).AudioStream;
                        break;
                    case ".bfstm":
                        stream = new Bfstm(File.ReadAllBytes(lwav.OriginalFilePath)).AudioStream;
                        break;
                }
            } catch (Exception e) {
                Console.WriteLine(e.GetType().Name + ": " + e.Message);
            }

            stream = new Wave(lwav.Export()).AudioStream;

            LoopingTrackStream encoded = stream != null ? stream
                : stream is PcmStream && encoding == B_stmCodec.Adpcm ? Encode.PcmToAdpcmParallel(stream as PcmStream)
                : stream;

            if (lwav.Looping) {
                encoded.SetLoop(lwav.LoopStart, lwav.LoopEnd);
            } else {
                encoded.SetLoop(false);
            }

            using (Stream s = new FileStream(
                Path.Combine(output_dir, original_filename_no_ext + ".brstm"),
                FileMode.Create,
                FileAccess.Write))
            {
                var b = new Brstm(encoded);
                b.Configuration.TrackType = BrstmTrackType.Short;
                b.WriteFile(s);
            }
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			Task task = new Task(() => WriteFile(lwav, output_dir, original_filename_no_ext, progressTracker));
			task.Start();
			return task;
		}

		public string GetExporterName() {
            return "BRSTM (BrawlLib): " + encoding;
		}
	}
}
