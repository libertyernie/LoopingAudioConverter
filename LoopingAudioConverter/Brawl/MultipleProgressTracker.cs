using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopingAudioConverter.Brawl {
	public partial class MultipleProgressTracker : Form {
		public MultipleProgressTracker() {
			InitializeComponent();
		}

		public MultipleProgressTrackerRow Add(string text) {
			var row = new MultipleProgressTrackerRow(text);
			this.BeginInvoke(new Action(() => {
				flowLayoutPanel1.Controls.Add(row);
			}));
			return row;
		}
	}
}
