using BrawlLib.Wii.Audio;
using RSTMLib.WAV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stm_encode {
    class Program {
        static int usage() {
            Console.Error.WriteLine(@"stm-encode - encode and convert between RSTM and CSTM
Based on code from BrawlLib

This program is provided as-is without any warranty, implied or otherwise.
By using this program, the end user agrees to take full responsibility
regarding its proper and lawful use. The authors/hosts/distributors cannot be 
held responsible for any damage resulting in the use of this program, nor can 
they be held accountable for the manner in which it is used.

Usage:
stm-encode [-l<start-end>] <inputfile> <outputfile>

inputfile can be RSTM, CSTM, or WAV. outputfile can be RSTM or CSTM.");
            return 1;
        }

        unsafe static int Main(string[] args) {
            bool? looping = null;
            int? loopStart = null, loopEnd = null;
            string inputFile = null, outputFile = null;

            foreach (string s in args) {
                if (s == "/?" || s == "-h" || s == "--help") {
                    return usage();
                } else if (s.StartsWith("-l")) {
                    string[] split = s.Substring(2).Split('-');
                    if (split.Length == 0) {
                        looping = false;
                    } else {
                        looping = true;
                        loopStart = int.Parse(split[0]);
                        if (split.Length > 1) loopEnd = int.Parse(split[1]);
                    }
                } else if (inputFile == null) {
                    inputFile = s;
                } else if (outputFile == null) {
                    outputFile = s;
                } else {
                    Console.Error.WriteLine("Too many arguments: " + s);
                    return 1;
                }
            }

            if (inputFile == null) {
                Console.Error.WriteLine("No input file specified");
                return 1;
            }
            if (outputFile == null) {
                Console.Error.WriteLine("No output file specified");
                return 1;
            }

            if (!File.Exists(inputFile)) {
                Console.Error.WriteLine("Input file does not exist: " + inputFile);
                return 1;
            }

            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputFile));
            if (!Directory.Exists(outputDir)) {
                Console.Error.WriteLine("Output directory does not exist: " + outputDir);
                return 1;
            }

            byte[] inputarr = File.ReadAllBytes(inputFile);
            string tag;
            fixed (byte* input = inputarr)
            {
                tag = new string((sbyte*)input, 0, 4);
            }

            string ext = Path.GetExtension(outputFile).ToLowerInvariant();
            if (ext != ".brstm" && ext != ".bcstm") {
                Console.Error.WriteLine("Unsupported output format: " + ext);
                return 1;
            }

            byte[] rstm;
            switch (tag) {
                case "RIFF":
                    PCM16Audio wav = PCM16Factory.FromByteArray(inputarr);
                    rstm = RSTMConverter.EncodeToByteArray(new PCM16AudioStream(wav), new ConsoleProgressTracker());
                    break;
                case "RSTM":
                    rstm = inputarr;
                    break;
                case "CSTM":
                    rstm = CSTMConverter.ToRSTM(inputarr);
                    break;
                default:
                    Console.Error.WriteLine("Unknown file format: " + tag);
                    return 1;
            }

            byte[] outputarr =
                ext == ".bcstm" ? CSTMConverter.FromRSTM(rstm)
                : rstm;
            File.WriteAllBytes(outputFile, outputarr);

            return 0;
        }
    }
}
