using System.Windows.Forms;

namespace LoopingAudioConverter {
	public partial class ErrorForm : Form {
		public string Message {
			get {
				return textBox1.Text;
			}
			set {
				textBox1.Text = value;
			}
		}

		public ErrorForm() {
			InitializeComponent();
		}
	}
}
