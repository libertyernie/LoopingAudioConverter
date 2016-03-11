using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public class EncodingProgressWrapper : IProgressTracker {
		private IEncodingProgress tracker;

		public float MinValue { get; set; }
		public float MaxValue { get; set; }
		public float CurrentValue { get; set; }
		public bool Cancelled { get; set; }

		private float Ratio {
			get {
				return (CurrentValue - MinValue) / (MaxValue - MinValue);
			}
		}

		public EncodingProgressWrapper(IEncodingProgress t) {
			tracker = t;
		}

		public void Begin(float min, float max, float current) {
			MinValue = min;
			MaxValue = max;
			CurrentValue = current;

			tracker.ShowProgress();
			tracker.Update(Ratio);
		}

		public void Cancel() {
			Cancelled = true;
		}

		public void Finish() {
			tracker.Remove();
		}

		public void Update(float value) {
			CurrentValue = value;

			tracker.Update(Ratio);
		}
	}
}
