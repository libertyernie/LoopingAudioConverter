using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LoopingAudioConverter {
	/// <summary>
	/// A class to use vgm2wav to render VGM/VGM files to WAV format.
	/// </summary>
	public class VGMImporter : IAudioImporter {
		private string ExePath;

		/// <summary>
		/// Initializes the VGM importer.
		/// </summary>
		/// <param name="exePath">Path to vgm2wav.exe (relative or absolute.)</param>
		public VGMImporter(string exePath) {
			ExePath = exePath;
		}

		/// <summary>
		/// Returns whether the extension matches that of a VGM file (vgm or vgz), ignoring any leading period.
		/// </summary>
		/// <param name="extension">Filename extension</param>
		/// <returns>true if vgm or vgz, false otherwise</returns>
		public bool SupportsExtension(string extension) {
			while (extension.StartsWith(".")) extension = extension.Substring(1);
			return string.Equals(extension, "vgm", StringComparison.InvariantCultureIgnoreCase)
				|| string.Equals(extension, "vgz", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Renders a file to WAV using vgm2wav and reads it into a PCM16Audio object.
		/// If the format is not supported, vgm2wav will write a message to the console and this function will throw an AudioImporterException.
		/// </summary>
		/// <param name="filename">The path of the file to read</param>
		/// <returns>An LWAV, which may or may not be looping</returns>
		public PCM16Audio ReadFile(string filename) {
			if (!File.Exists(ExePath)) {
				throw new AudioImporterException("vgm2wav.exe not found at path: " + ExePath);
			}
			if (filename.Contains('"')) {
				throw new AudioImporterException("File paths with double quote marks (\") are not supported");
			}

            string outfile = TempFiles.Create("wav");
			ProcessStartInfo psi = new ProcessStartInfo {
				FileName = ExePath,
				UseShellExecute = false,
				Arguments = "--loop-count 1 --fade-ms 500 \"" + filename + "\" " + outfile
			};
			Process p = Process.Start(psi);
			try {
                return PCM16Factory.FromFile(outfile, true);
			} catch (Exception e) {
				throw new AudioImporterException("Could not read output of test.exe: " + e.Message);
			}
		}

		public string GetImporterName() {
			return "VGMImporter";
		}
	}
}
