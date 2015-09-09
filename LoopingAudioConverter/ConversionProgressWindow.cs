using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopingAudioConverter {
	public partial class ConversionProgressWindow : Form {
		public bool Cancelled { get; private set; }

		public int Maximum {
			set {
				progDecoding.Maximum = progEncoding.Maximum = value;
			}
		}

		private string decoding;
		private List<string> encoding;

		public ConversionProgressWindow() {
			InitializeComponent();
			Cancelled = false;
			encoding = new List<string>();
		}

		public void SetDecodingFilename(string filename) {
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action(() => {
					SetDecodingFilename(filename);
				}));
				return;
			}
			decoding = filename;
			lblDecoding.Text = "Decoding:\r\n    " + filename;
		}

		public void AddEncodingFilename(string filename) {
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action(() => {
					AddEncodingFilename(filename);
				}));
				return;
			}
			encoding.Add(filename);
			lblEncoding.Text = "Encoding:\r\n" + string.Join("\r\n", encoding.Select(s => "    " + s));
		}

		public void RemoveEncodingFilename(string filename) {
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action(() => {
					RemoveEncodingFilename(filename);
				}));
				return;
			}
			encoding.Remove(filename);
			lblEncoding.Text = "Encoding:\r\n" + string.Join("\r\n", encoding.Select(s => "    " + s));
		}

		public void IncrementDecodingBar() {
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action(IncrementDecodingBar));
				return;
			}
			progDecoding.Value++;
		}

		public void IncrementEncodingBar() {
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action(IncrementEncodingBar));
				return;
			}
			progEncoding.Value++;
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			btnCancel.Enabled = false;
			Cancelled = true;
		}
	}
}
