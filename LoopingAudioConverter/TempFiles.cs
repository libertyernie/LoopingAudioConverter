using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoopingAudioConverter {
    public class TempFiles {
        private static TempFiles staticInstance = new TempFiles();
        private static Random r = new Random();
        private const string TEMPDIR = "tmp";

        //private List<string> filesCreated;

        private TempFiles() {
            if (!Directory.Exists(TEMPDIR)) {
                Directory.CreateDirectory(TEMPDIR);
            }
            //filesCreated = new List<string>();
        }

        ~TempFiles() {
            //foreach (string file in filesCreated) {
            //    try {
            //        File.Delete(file);
            //        Console.WriteLine("Deleted " + file);
            //    } catch { }
            //}
            try {
                Directory.Delete(TEMPDIR);
                Console.WriteLine("Deleted " + TEMPDIR);
            } catch { }
        }

        public static string Create(string extension) {
            extension = extension ?? "";
            if (!extension.StartsWith(".") && extension.Length > 0) extension = "." + extension;
            
            string tempFileName;
            do {
                tempFileName = Path.Combine(TEMPDIR, r.Next(0x10000).ToString("X4") + extension);
            } while (File.Exists(tempFileName));

            //staticInstance.filesCreated.Add(tempFileName);

            return tempFileName;
        }
    }
}
