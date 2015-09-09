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

		private int IntValue {
			get {
				return (int)(1048576 * (CurrentValue - MinValue) / (MaxValue - MinValue));
			}
		}

		public EncodingProgressWrapper(IEncodingProgress t) {
			tracker = t;
		}

		public void Begin(float min, float max, float current) {
			MinValue = min;
			MaxValue = max;
			CurrentValue = current;

			tracker.Begin(IntValue, 1048576);
		}

		public void Cancel() {
			Cancelled = true;
		}

		public void Finish() {
			tracker.Finish();
		}

		public void Update(float value) {
			CurrentValue = value;

			tracker.Update(IntValue);
		}
	}
}
