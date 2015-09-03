using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoopingAudioConverter {
    class Program {
        static void Main(string[] args) {
            SoX sox = new SoX(@"..\..\tools\sox\sox.exe");

			IAudioImporter[] importers = {
				new LWAVImporter(),
				new VGMStreamImporter("..\\..\\tools\\vgmstream\\test.exe"),
                sox
			};

			string[] inputFiles = {
				@"C:\Brawl\sound\strm\S02.brstm",
				@"C:\Users\Owner\Desktop\test.wav",
				@"C:\Users\Owner\Desktop\frombrawl.wav"
			};
			foreach (string inputFile in inputFiles) {
				LWAV w = null;
				string extension = Path.GetExtension(inputFile);
				List<AudioImporterException> exceptions = new List<AudioImporterException>();

				var importers_supported = importers.Where(i => i.SupportsExtension(extension));
				if (!importers_supported.Any()) {
					throw new Exception("No importers supported for file extension " + extension);
				}

				foreach (IAudioImporter importer in importers_supported) {
					try {
						w = importer.ReadFile(inputFile);
						Console.WriteLine("Imported with " + importer.GetImporterName() + ": " + inputFile);
						break;
					} catch (AudioImporterException e) {
						Console.Error.WriteLine(importer.GetImporterName() + " could not read file " + inputFile + ": " + e.Message);
						exceptions.Add(e);
					}
				}

				if (w == null) {
					throw new AggregateException("Could not read " + inputFile, exceptions);
				}

				IAudioExporter exporter = new LWAVExporter();
				exporter.WriteFile(w, @"C:\Users\Owner\Downloads", inputFile);
			}

            /*ProcessStartInfo psi = new ProcessStartInfo {
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
			File.WriteAllBytes(@"C:\Users\Owner\Downloads\out3.wav", w.Export());*/
        }
    }
}
