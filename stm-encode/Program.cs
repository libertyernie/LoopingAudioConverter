using BrawlLib.Wii.Audio;
using System;
using System.Audio;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stm_encode {
    class Program {
        static int usage() {
            Console.Error.WriteLine(@"stm-encode - encode, decode, and convert between RSTM, CSTM, and FSTM
Based on code from BrawlLib
https://github.com/libertyernie/LoopingAudioConverter/stm-encode

This program is provided as-is without any warranty, implied or otherwise.
By using this program, the end user agrees to take full responsibility
regarding its proper and lawful use. The authors/hosts/distributors cannot be 
held responsible for any damage resulting in the use of this program, nor can 
they be held accountable for the manner in which it is used.

Usage:
stm-encode [options] <inputfile> <outputfile>

Supported formats are .brstm, .bcstm, .bfstm, or .wav.

Options (WAV input):
    -l             Loop from start of file until end of file
    -l<start>      Loop from sample <start> until end of file
    -l<start-end>  Loop from sample <start> until sample <end>
    -noloop        Do not loop (ignore looping info in input file if any)

Options (WAV output):
    -L             Include looping information in smpl chunk");
            return 1;
        }

        unsafe static int Main(string[] args) {
            if (args.Length == 0) {
                return usage();
            }

            bool? looping = null;
            int? loopStart = null, loopEnd = null;
            string inputFile = null, outputFile = null;
            bool includeSmpl = false;

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
                } else if (s == "-L") {
                    includeSmpl = true;
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
            if (ext != ".brstm" && ext != ".bcstm" && ext != ".bfstm" && ext != ".wav") {
                Console.Error.WriteLine("Unsupported output format: " + ext);
                return 1;
            }

            byte[] rstm;
            switch (tag) {
                case "RIFF":
                    IAudioStream wav = new PCMStream(inputarr);
                    wav.IsLooping = looping ?? wav.IsLooping;
                    wav.LoopStartSample = loopStart ?? wav.LoopStartSample;
                    wav.LoopEndSample = loopEnd ?? wav.LoopEndSample;
                    rstm = RSTMConverter.EncodeToByteArray(wav, new ConsoleProgressTracker());
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
                : ext == ".wav" ? WAV.ToByteArray(RSTMConverter.CreateStreams(rstm)[0], appendSmplChunk: includeSmpl)
                : rstm;
            File.WriteAllBytes(outputFile, outputarr);

            return 0;
        }
    }
}
