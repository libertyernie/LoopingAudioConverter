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
            Console.Error.WriteLine(@"stm-encode - encode and convert between RSTM, CSTM, and FSTM
Based on code from BrawlLib
https://github.com/libertyernie/LoopingAudioConverter/stm-encode

This program is provided as-is without any warranty, implied or otherwise.
By using this program, the end user agrees to take full responsibility
regarding its proper and lawful use. The authors/hosts/distributors cannot be 
held responsible for any damage resulting in the use of this program, nor can 
they be held accountable for the manner in which it is used.

Usage:
stm-encode [options] <inputfile> <outputfile>

inputfile can be .brstm, .bcstm, .bfstm, or .wav.
outputfile can be .brstm, .bcstm, or .bfstm.

Options (WAV input only):
    -l             Loop from start of file until end of file
    -l<start>      Loop from sample <start> until end of file
    -l<start-end>  Loop from sample <start> until sample <end>
    -noloop        Do not loop (ignore smpl chunk in WAV file if one exists)");
            return 1;
        }

        unsafe static int Main(string[] args) {
            if (args.Length == 0) {
                return usage();
            }

            bool? looping = null;
            int? loopStart = null, loopEnd = null;
            string inputFile = null, outputFile = null;

            foreach (string s in args) {
                if (s == "/?" || s == "-h" || s == "--help") {
                    return usage();
                } else if (s.StartsWith("-l")) {
                    looping = true;
                    string[] split = s.Substring(2).Split('-');
                    if (split.Length > 0) {
                        loopStart = int.Parse(split[0]);
                        if (split.Length > 1) {
                            loopEnd = int.Parse(split[1]);
                        }
                    }
                } else if (s == "-noloop") {
                    looping = false;
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
            if (ext != ".brstm" && ext != ".bcstm" && ext != ".bfstm") {
                Console.Error.WriteLine("Unsupported output format: " + ext);
                return 1;
            }

            byte[] rstm;
            switch (tag) {
                case "RIFF":
                    PCM16Audio wav = PCM16Factory.FromByteArray(inputarr);
                    wav.Looping = looping ?? wav.Looping;
                    wav.LoopStart = loopStart ?? wav.LoopStart;
                    wav.LoopEnd = loopEnd ?? wav.LoopEnd;
                    rstm = RSTMConverter.EncodeToByteArray(new PCM16AudioStream(wav), new ConsoleProgressTracker());
                    break;
                case "RSTM":
                    rstm = inputarr;
                    break;
                case "CSTM":
                    rstm = CSTMConverter.ToRSTM(inputarr);
                    break;
                case "FSTM":
                    rstm = FSTMConverter.ToRSTM(inputarr);
                    break;
                default:
                    Console.Error.WriteLine("Unknown file format: " + tag);
                    return 1;
            }

            byte[] outputarr =
                ext == ".bcstm" ? CSTMConverter.FromRSTM(rstm)
                : ext == ".bfstm" ? FSTMConverter.FromRSTM(rstm)
                : ext == ".wav" ? PCM16Factory.FromAudioStream(RSTMConverter.CreateStreams(rstm)[0]).Export()
                : rstm;
            File.WriteAllBytes(outputFile, outputarr);

            return 0;
        }
    }
}
