using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	/// <summary>
	/// A class to use vgmstream's test.exe to convert formats it supports to LWAV format.
	/// </summary>
	public class VGMStreamImporter : IAudioImporter {
		private string TestExePath;

		/// <summary>
		/// Initializes the vgmstream importer.
		/// </summary>
		/// <param name="exePath">Path to test.exe (relative or absolute.)</param>
		public VGMStreamImporter(string test_exe_path) {
			TestExePath = test_exe_path;
		}

		/// <summary>
		/// Will always return true. vgmstream supports many file formats, and it should be tried before any importers that don't support looping audio.
		/// </summary>
		/// <param name="extension">Filename extension</param>
		/// <returns>true</returns>
		public bool SupportsExtension(string extension) {
			return true;
		}

		/// <summary>
		/// Converts a file to WAV using test.exe and reads it into an LWAV object.
		/// If the format is not supported, test.exe will write a message to the console and this function will throw an AudioImporterException.
		/// </summary>
		/// <param name="filename">The path of the file to read</param>
		/// <returns>An LWAV, which may or may not be looping</returns>
		public PCM16Audio ReadFile(string filename) {
			if (!File.Exists(TestExePath)) {
				throw new AudioImporterException("test.exe not found at path: " + TestExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

            if (!Directory.Exists("tmp")) {
                Directory.CreateDirectory("tmp");
            }

			ProcessStartInfo psi = new ProcessStartInfo {
                WorkingDirectory = "tmp",
				FileName = TestExePath,
				UseShellExecute = false,
				CreateNoWindow = true,
				Arguments = "-L -l 1 -f 0 \"" + filename + "\""
			};
			Process p = Process.Start(psi);
            p.WaitForExit();

			try {
                return PCM16Factory.FromFile("tmp/dump.wav", true);
			} catch (Exception e) {
				throw new AudioImporterException("Could not read output of test.exe: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "VGMStreamImporter";
		}
	}
}
