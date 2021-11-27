using System.Windows.Forms;

namespace LoopingAudioConverter {
	public partial class QualityForm : Form {
		public string EncodingParameters {
			get {
				return txtParameters.Text;
			}
		}

		public QualityForm(string encodingParameters = null) {
			InitializeComponent();

			if (encodingParameters != null) {
				txtParameters.Text = encodingParameters;
			}
		}
	}
}
