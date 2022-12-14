using BrawlLib.Internal.Audio;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types.Audio;
using BrawlLib.Wii.Audio;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static OpenTK.Graphics.OpenGL.GL;

namespace LoopingAudioConverter.BrawlLib {
	public class BrawlLibRSTMExporter : IAudioExporter {
		public enum Container { RSTM, CSTM, FSTM }

		private class ProgressTracker : IProgressTracker {
			private readonly IProgress<double> _progress;

			private float _min, _max, _current;

            public ProgressTracker(IProgress<double> progress) {
                _progress = progress;
            }

            public bool Cancelled { get; set; }

            float IProgressTracker.MinValue { get => _min; set { _min = value; UpdateInternal(); } }
			float IProgressTracker.MaxValue { get => _max; set { _max = value; UpdateInternal(); } }
			float IProgressTracker.CurrentValue { get => _current; set { _current = value; UpdateInternal(); } }

            public void Cancel() {
				Cancelled = true;
            }

            void IProgressTracker.Begin(float min, float max, float current) {
				_min = min;
				_max = max;
				_current = current;
				UpdateInternal();
            }

            void IProgressTracker.Finish() {
				_current = _max;
				UpdateInternal();
            }

            void IProgressTracker.Update(float value) {
				_current = value;
				UpdateInternal();
            }

			private void UpdateInternal() {
				if (_max == _min) {
					_progress?.Report(0);
				} else {
					double ratio = (_current - _min) / (_max - _min);
					if (ratio < 0) ratio = 0;
					if (ratio > 1) ratio = 1;
					_progress?.Report(ratio);
				}
            }
        }

		private readonly WaveEncoding _waveEncoding;
		private readonly Container _container;

		public BrawlLibRSTMExporter(WaveEncoding waveEncoding, Container container) {
			_waveEncoding = waveEncoding;
			_container = container;
		}

		private unsafe byte[] Encode(PCM16Audio lwav, IProgressTracker progressTracker) {
			using (var ms = new MemoryStream()) {
				var wrapper = new PCM16LoopWrapper(lwav);

				using (var fileMap = RSTMConverter.Encode(wrapper, progressTracker, _waveEncoding))
				using (var inputStream = new UnmanagedMemoryStream((byte*)fileMap.Address.address, fileMap.Length)) {
					inputStream.CopyTo(ms);
				}

				return ms.ToArray();
			}
		}

		public void WriteFile(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgressTracker progressTracker) {
			byte[] data = Encode(lwav, progressTracker);

			string ext = ".brstm";
			if (_container == Container.CSTM) {
				data = CSTMConverter.FromRSTM(data);
				ext = ".bcstm";
			}
			if (_container == Container.FSTM) {
				data = FSTMConverter.FromRSTM(data);
				ext = ".bfstm";
			}

			File.WriteAllBytes(
				Path.Combine(output_dir, original_filename_no_ext + ext),
				data);
		}

        public async Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext, IProgress<double> progress = null) {
			await Task.Yield();
			await Task.Run(() => WriteFile(lwav, output_dir, original_filename_no_ext, new ProgressTracker(progress)));
		}

		public bool TryWriteFile(object audio, ILoopPoints loopPoints, string output_dir, string original_filename_no_ext) {
			string ext = _container == Container.RSTM ? ".brstm"
				: _container == Container.CSTM ? ".bcstm"
				: _container == Container.FSTM ? ".bfstm"
				: throw new NotImplementedException();

			string outfile = Path.Combine(output_dir, original_filename_no_ext + ext);

			if (audio is RSTMNode r) {
				if (loopPoints.Looping != r.IsLooped) return false;
				if (loopPoints.LoopStart != r.LoopStartSample) return false;
				if (loopPoints.LoopEnd != r.NumSamples) return false;

				r.Export(outfile);
				return true;
			}

			return false;
		}
	}
}
