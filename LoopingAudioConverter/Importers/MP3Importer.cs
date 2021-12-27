using LoopingAudioConverter.PCM;
using MP3Sharp;
using RunProcessAsTask;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class MP3Importer : IAudioImporter {
		public bool SupportsExtension(string extension) {
			if (extension.StartsWith(".")) extension = extension.Substring(1);
			return extension.Equals("mp3", StringComparison.InvariantCultureIgnoreCase);
		}

		private unsafe static short[] ToUInt16Array(byte[] array) {
			fixed (byte* ptr = array) {
				short* ptr16 = (short*)ptr;
				short[] array16 = new short[array.Length / sizeof(short)];
				Marshal.Copy((IntPtr)ptr16, array16, 0, array16.Length);
				return array16;
			}
		}

		public async Task<PCM16Audio> ReadFileAsync(string filename) {
			byte[] mp3data = File.ReadAllBytes(filename);
			using (var output = new MemoryStream())
			using (var input = new MemoryStream(mp3data, false))
			using (var mp3 = new MP3Stream(input)) {
				await mp3.CopyToAsync(output);

				byte[] array = output.ToArray();
				short[] samples = ToUInt16Array(array);
				return new PCM16Audio(mp3.ChannelCount, mp3.Frequency, samples);
			}
		}
	}
}
