using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using VGAudio.Containers;

namespace LoopingAudioConverter.VGAudioOptions {
	public partial class VGAudioOptionsForm<T, U> : Form where T : VGAudioOptionsBase<U> where U : Configuration {
		public VGAudioOptionsForm(T options) {
			InitializeComponent();
			using (MemoryStream stream = new MemoryStream()) {
				XmlSerializer xs = new XmlSerializer(typeof(T));
				xs.Serialize(stream, options);
				stream.Position = 0;
				propertyGrid1.SelectedObject = (T)xs.Deserialize(stream);
			}
		}

		public T SelectedObject => propertyGrid1.SelectedObject as T;
	}
}
