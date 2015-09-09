using LoopingAudioConverter.Brawl;
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
			int processors = Environment.ProcessorCount;

            SoX sox = new SoX(@"..\..\tools\sox\sox.exe");

			IAudioImporter[] importers = {
				new LWAVImporter(),
				new MP3Importer("..\\..\\tools\\madplay\\madplay.exe"),
				new VGMStreamImporter("..\\..\\tools\\vgmstream\\test.exe"),
                sox
			};
			IAudioExporter exporter = new RSTMExporter();

			string[] inputFiles = Directory.EnumerateFiles(@"C:\Brawl\sound\strm", "*.brstm").Skip(5).Take(5).ToArray();

			List<Task> tasks = new List<Task>();
			Semaphore sem = new Semaphore(processors, processors);

			MultipleProgressWindow window = new MultipleProgressWindow();
			new Thread(new ThreadStart(() => {
				System.Windows.Forms.Application.EnableVisualStyles();
				window.ShowDialog();
			})).Start();

			Stopwatch s = new Stopwatch();
			s.Start();
			foreach (string inputFile in inputFiles) {
				sem.WaitOne();
				if (tasks.Any(t => t.IsFaulted)) break;

				window.SetDecodingText(Path.GetFileNameWithoutExtension(inputFile));

				LWAV w = null;
				string extension = Path.GetExtension(inputFile);
				List<AudioImporterException> exceptions = new List<AudioImporterException>();

				var importers_supported = importers.Where(i => i.SupportsExtension(extension));
				if (!importers_supported.Any()) {
					throw new Exception("No importers supported for file extension " + extension);
				}

				foreach (IAudioImporter importer in importers_supported) {
					try {
						Console.WriteLine("Trying to decode " + Path.GetFileName(inputFile) + " with " + importer.GetImporterName());
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

				window.SetDecodingText("To mono");
				w = sox.ApplyEffects(w, 1);

				window.SetDecodingText("");
				MultipleProgressRow row = window.AddEncodingRow(inputFile);
				if (processors == 1) {
					exporter.WriteFile(w, @"C:\Users\Owner\Downloads\a", Path.GetFileNameWithoutExtension(inputFile), row);
					sem.Release();
					row.Remove();
				} else {
					Task task = exporter.WriteFileAsync(w, @"C:\Users\Owner\Downloads\a", Path.GetFileNameWithoutExtension(inputFile), row);
					task.ContinueWith(t => {
						sem.Release();
						row.Remove();
					});
					tasks.Add(task);
				}
			}
			Task.WaitAll(tasks.ToArray());
			s.Stop();
			window.BeginInvoke(new Action(() => {
				window.Close();
			}));
			Console.WriteLine(s.Elapsed);
			if (tasks.Any(t => t.IsFaulted)) {
				throw new AggregateException(tasks.Where(t => t.IsFaulted).Select(t => t.Exception));
			}
        }
    }
}
