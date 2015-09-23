using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopingAudioConverter {
	public partial class MultipleProgressWindow : Form {
		public bool Canceled { get; private set; }
		public bool AllowClose { get; set; }

		public MultipleProgressWindow() {
			InitializeComponent();
		}

		public void SetDecodingText(string text) {
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action(() => {
					SetDecodingText(text);
				}));
				return;
			}
			lblDecoding.Text = text;
		}

		public MultipleProgressRow AddEncodingRow(string text) {
			if (this.InvokeRequired) {
				return (MultipleProgressRow)this.Invoke(new Func<MultipleProgressRow>(() => {
					return AddEncodingRow(text);
				}));
			}
			MultipleProgressRow row = new MultipleProgressRow(text);
			row.Dock = DockStyle.Bottom;
			pnlEncoding.Controls.Add(row);
			return row;
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			Canceled = true;
			btnCancel.Enabled = false;
		}

		private void MultipleProgressWindow_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.CloseReason == CloseReason.UserClosing && !AllowClose) {
				Canceled = true;
				btnCancel.Enabled = false;
				e.Cancel = true;
			}
		}
	}
}
