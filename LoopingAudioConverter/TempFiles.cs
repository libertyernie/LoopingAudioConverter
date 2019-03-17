using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoopingAudioConverter {
	public static class TempFiles {
		private const string TEMPDIR = "tmp";

		private static readonly List<string> _files = new List<string>();

		public static string Create(string extension) {
			if (!Directory.Exists(TEMPDIR)) {
				Directory.CreateDirectory(TEMPDIR);
			}

			extension = extension ?? "";
			if (!extension.StartsWith(".") && extension.Length > 0) extension = "." + extension;

			string tempFileName;
			do {
				tempFileName = Path.Combine(TEMPDIR, Guid.NewGuid().ToString() + extension);
			} while (File.Exists(tempFileName));

			_files.Add(tempFileName);
			return tempFileName;
		}

		public static void DeleteAll() {
			/*foreach (string s in _files) {
				if (File.Exists(s)) {
					File.Delete(s);
				}
			}
			_files.Clear();*/
		}
	}
}
