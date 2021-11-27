using System.IO;

namespace LoopingAudioConverter {
    public static class OptionsSerialization {
        public static void PopulateFromFile(string filename, ref Options o) {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
				var writer = new System.Xml.Serialization.XmlSerializer(typeof(Options));
				o = (Options)writer.Deserialize(fs);
            }
        }

        public static void WriteToFile(string filename, Options o) {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write)) {
				var writer = new System.Xml.Serialization.XmlSerializer(typeof(Options));
				writer.Serialize(fs, o);
            }
        }
    }
}
