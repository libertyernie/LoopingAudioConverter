using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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
			IAudioExporter exporter = new MP3Exporter(@"..\..\tools\lame\lame.exe");

			string[] inputFiles = Directory.EnumerateFiles(@"C:\melee\audio", "*.hps").ToArray();

			List<Task> tasks = new List<Task>();
			Semaphore sem = new Semaphore(4, 4);

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
						break;
					} catch (AudioImporterException e) {
						//Console.Error.WriteLine(importer.GetImporterName() + " could not read file " + inputFile + ": " + e.Message);
						exceptions.Add(e);
					}
				}

				if (w == null) {
					throw new AggregateException("Could not read " + inputFile, exceptions);
				}
				sem.WaitOne();
				Task task = exporter.WriteFileAsync(w, @"C:\Users\Owner\Downloads", Path.GetFileNameWithoutExtension(inputFile));
				task.ContinueWith(t => {
					if (t.Exception != null) Console.WriteLine(t.Exception.Message);
					sem.Release();
				});
				tasks.Add(task);
				GC.Collect();
			}
			Task.WaitAll(tasks.ToArray());
			s.Stop();
			Console.WriteLine(s.Elapsed);
        }
    }
}
