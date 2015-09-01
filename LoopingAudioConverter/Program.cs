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
                Arguments = @"-L -p -l 2 ""C:\Users\ischemm\Downloads\Metal City.brstm"""
            };
            Process p = Process.Start(psi);
            WaveData w = WaveData.FromStream(p.StandardOutput.BaseStream);
            Console.WriteLine(w);
            File.WriteAllBytes(@"C:\Users\ischemm\Downloads\out3.wav", w.Export());
        }
    }
}
