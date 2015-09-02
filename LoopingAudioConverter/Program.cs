using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
    class Program {
        static void Main(string[] args) {
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = "..\\..\\tools\\vgmstream\\test.exe",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                Arguments = @"-L -p -l 2 ""C:\Brawl\sound\strm\S02.brstm"""
            };
            Process p = Process.Start(psi);
			LWAV w = null;
			try {
				w = LWAVFactory.FromStream(p.StandardOutput.BaseStream);
			} catch (Exception e) {
				Console.Error.WriteLine("Could not read .wav file: " + e.Message);
				return;
			}
			Console.WriteLine(w);
			File.WriteAllBytes(@"C:\Users\Owner\Downloads\out3.wav", w.Export());
        }
    }
}
