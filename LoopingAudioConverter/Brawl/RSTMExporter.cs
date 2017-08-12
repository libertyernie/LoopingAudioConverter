using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Audio;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public class RSTMExporter : IAudioExporter {
		private WaveEncoding encoding;

		/// <summary>
		/// Creates a new RSTMExporter instance that uses the given encoding when it has to re-encode a file.
		/// </summary>
		/// <param name="defaultEncoding">The encoding to use (ADPCM or PCM16)</param>
		public RSTMExporter(WaveEncoding defaultEncoding) {
			this.encoding = defaultEncoding;
		}

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IEncodingProgress progressTracker = null) {
			IProgressTracker pw = null;
			if (progressTracker != null) pw = new EncodingProgressWrapper(progressTracker);

			byte[] data = null;
			try {
				switch (Path.GetExtension(lwav.OriginalFilePath ?? "").ToLowerInvariant()) {
					case ".brstm":
						data = File.ReadAllBytes(lwav.OriginalFilePath);
						break;
					case ".bcstm":
						data = CSTMConverter.ToRSTM(File.ReadAllBytes(lwav.OriginalFilePath));
						break;
					case ".bfstm":
						data = FSTMConverter.ToRSTM(File.ReadAllBytes(lwav.OriginalFilePath));
						break;
				}
			} catch (Exception e) {
				Console.WriteLine(e.GetType().Name + ": " + e.Message);
			}

			if (data == null) {
				data = RSTMConverter.EncodeToByteArray(new PCM16AudioStream(lwav), pw, encoding);
				if (pw.Cancelled) throw new AudioExporterException("RSTM export cancelled");
			}
			File.WriteAllBytes(Path.Combine(output_dir, original_filename_no_ext + ".brstm"), data);
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
