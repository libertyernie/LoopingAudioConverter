using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
    class Program {
        static void Main(string[] args) {
            SoX sox = new SoX(@"..\..\tools\sox\sox.exe");

			IAudioImporter[] importers = {
				new LWAVImporter(),
				new MP3Importer("..\\..\\tools\\madplay\\madplay.exe"),
				new VGMStreamImporter("..\\..\\tools\\vgmstream\\test.exe"),
                sox
			};

			string[] inputFiles = {
				@"C:\Brawl\sound\strm\S03.brstm",
				@"C:\Users\Owner\Desktop\test.wav",
				@"C:\Users\Owner\Desktop\frombrawl.wav",
				@"C:\Users\Owner\Music\iTunes\iTunes Media\Music\Bowman\Comfortable Bugs\03 Bad Dudes.mp3"
			};
			Stopwatch s = new Stopwatch();
			s.Start();
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
						//Console.Error.WriteLine(importer.GetImporterName() + " could not read file " + inputFile + ": " + e.Message);
						exceptions.Add(e);
					}
				}

				if (w == null) {
					throw new AggregateException("Could not read " + inputFile, exceptions);
				}

				IAudioExporter exporter = new MP3Exporter(@"..\..\tools\lame\lame.exe");
				exporter.WriteFile(w, @"C:\Users\Owner\Downloads", Path.GetFileNameWithoutExtension(inputFile));
				
			}
			s.Stop();
			Console.WriteLine(s.Elapsed);
        }
    }
}
