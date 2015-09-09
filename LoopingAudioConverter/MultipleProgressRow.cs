using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopingAudioConverter {
	public partial class MultipleProgressRow : UserControl, IEncodingProgress {
		public int Value { get; private set; }
		public int Maximum { get; private set; }

		public string Text {
			get {
				if (this.InvokeRequired) {
					return (string)this.Invoke(new Func<string>(() => {
						return label1.Text;
					}));
				} else {
					return label1.Text;
				}
			}
			set {
				if (this.InvokeRequired) {
					this.BeginInvoke(new Action(() => {
						label1.Text = value;
					}));
				} else {
					label1.Text = value;
				}
			}
		}

		public MultipleProgressRow() {
			InitializeComponent();
		}

		public MultipleProgressRow(string text) {
			InitializeComponent();
			label1.Text = text;
			pnlProgress.Paint += pnlProgress_Paint;
		}

		private void pnlProgress_Paint(object sender, PaintEventArgs e) {
			float width = pnlProgress.Width * Value / Maximum;
			e.Graphics.FillRectangle(SystemBrushes.Highlight, 0, 0, width, pnlProgress.Height);
		}

		public void Begin(int value, int maximum) {
			Maximum = maximum;
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action(() => {
					pnlProgress.Visible = true;
				}));
			} else {
				pnlProgress.Visible = true;
			}
			Update(value);
		}

		public void Finish() {
			if (this.Parent != null) {
				if (this.Parent.InvokeRequired) {
					this.Parent.BeginInvoke(new Action(Finish));
					return;
				}
				this.Parent.Controls.Remove(this);
			}
		}

		public void Update(int value) {
			Value = value;
			pnlProgress.Invalidate();
		}
	}
}
