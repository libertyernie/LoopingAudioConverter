using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public partial class MultipleProgressTrackerRow : UserControl, IProgressTracker {
		public float MaxValue { get; set; }
		public float MinValue { get; set; }
		public float CurrentValue { get; set; }
		public bool Cancelled { get; set; }

		public MultipleProgressTrackerRow(string text) {
			InitializeComponent();
			lblName.Text = text;
			panel1.Paint += panel1_Paint;
		}

		void panel1_Paint(object sender, PaintEventArgs e) {
			float w = panel1.Width * (CurrentValue - MinValue) / (MaxValue - MinValue);
			if (w < 0) w = 0;
			if (w > panel1.Width) w = panel1.Width;
			e.Graphics.FillRectangle(SystemBrushes.Highlight, 0, 0, w, panel1.Height);
		}

		public void Begin(float min, float max, float current) {
			MinValue = min;
			MaxValue = max;
			CurrentValue = current;
		}

		public void Cancel() {
			Cancelled = true;
			this.Parent.Controls.Remove(this);
		}

		public void Finish() {
			this.Parent.Controls.Remove(this);
		}

		public void Update(float value) {
			CurrentValue = value;
			panel1.Invalidate();
		}
	}
}
