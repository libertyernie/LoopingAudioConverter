using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	/// <summary>
	/// Tracks the progress of multiple operations by using a single line on the console to show only the oldest and newest operations, along with the number of operations currently running.
	/// The header and main rows will be written to the console when the first item is started; afterwards, the main row will be written over, with the cursor being left at the beginning of the row each time.
	/// </summary>
	public class ConsoleProgressTracker {
		public class TrackedItem : IProgressTracker {
			public string Name { get; set; }
			public bool Cancelled { get; set; }
			public float MinValue { get; set; }
			public float MaxValue { get; set; }
			public float CurrentValue { get; set; }

			private ConsoleProgressTracker parent;

			public TrackedItem(ConsoleProgressTracker parent) {
				this.parent = parent;
			}

			public string BarText {
				get {
					StringBuilder sb = new StringBuilder();
					sb.Append((Name + "        ").Substring(0, 8));
					sb.Append(' ');
					sb.Append('[');

					int numAsterisks = (int)(27 * (CurrentValue - MinValue) / (MaxValue - MinValue));
					if (numAsterisks > 27) numAsterisks = 27;
					for (int i = 0; i < numAsterisks; i++) {
						sb.Append('*');
					}
					for (int i = numAsterisks; i < 27; i++) {
						sb.Append(' ');
					}
					sb.Append(']');
					return sb.ToString();
				}
			}

			public void Begin(float min, float max, float current) {
				MinValue = min;
				MaxValue = max;
				parent.TrackedItems.AddLast(this);
				Update(current);
			}

			public void Cancel() {
				Cancelled = true;
				parent.TrackedItems.Remove(this);
				parent.Update();
			}

			public void Finish() {
				parent.TrackedItems.Remove(this);
				parent.Update();
			}

			public void Update(float value) {
				CurrentValue = value;
				parent.Update();
			}
		}

		private LinkedList<TrackedItem> TrackedItems = new LinkedList<TrackedItem>();
		private string lastString;
		private bool wroteHeader;

		public ConsoleProgressTracker() {
			TrackedItems = new LinkedList<TrackedItem>();
		}

		public void Update() {
			StringBuilder sb = new StringBuilder();
			sb.Append('\r');
			sb.Append((char)(TrackedItems.Count + '0'));
			sb.Append(' ');
			switch (TrackedItems.Count) {
				case 0:
					for (int i = 0; i < 77; i++) sb.Append(' ');
					break;
				case 1:
					sb.Append(TrackedItems.First.Value.BarText);
					for (int i = 0; i < 39; i++) sb.Append(' ');
					break;
				default:
					sb.Append(TrackedItems.First.Value.BarText);
					sb.Append(' ');
					sb.Append(TrackedItems.Last.Value.BarText);
					break;
			}
			if (lastString != sb.ToString()) {
				lastString = sb.ToString();
				Console.Write(lastString);
			}
		}

		public TrackedItem Add(string name) {
			if (!wroteHeader) {
				Console.WriteLine("  Longest-running:                       Most recently started:                ");
				wroteHeader = true;
			}
			return new TrackedItem(this) { Name = name };
		}
	}
}
