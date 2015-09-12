using LoopingAudioConverter.Brawl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter {
    class Program {
		[STAThread]
        static void Main(string[] args) {
			Application.EnableVisualStyles();
			OptionsForm f = new OptionsForm();
			if (f.ShowDialog() != DialogResult.OK) {
				return;
			}
			Options o = f.GetOptions();

			int processors = Environment.ProcessorCount;

            SoX sox = new SoX(@"..\..\tools\sox\sox.exe");

			IAudioImporter[] importers = {
				new LWAVImporter(),
				new MP3Importer("..\\..\\tools\\madplay\\madplay.exe"),
				new VGMStreamImporter("..\\..\\tools\\vgmstream\\test.exe"),
                sox
			};

			IAudioExporter exporter;
			switch (o.ExporterType) {
				case ExporterType.BRSTM:
					exporter = new RSTMExporter();
					break;
				case ExporterType.FLAC:
					exporter = new FLACExporter(sox);
					break;
				case ExporterType.MP3:
					exporter = new MP3Exporter(@"..\..\tools\lame\lame.exe");
					break;
				case ExporterType.OggVorbis:
					exporter = new OggVorbisExporter(sox);
					break;
				case ExporterType.WAV:
					exporter = new LWAVExporter();
					break;
				default:
					throw new Exception("Could not create exporter type " + o.ExporterType);
			}

			List<Task> tasks = new List<Task>();
			Semaphore sem = new Semaphore(processors, processors);

			MultipleProgressWindow window = new MultipleProgressWindow();
			new Thread(new ThreadStart(() => {
				Application.EnableVisualStyles();
				window.ShowDialog();
			})).Start();

			Stopwatch s = new Stopwatch();
			s.Start();
			if (!o.InputFiles.Any()) {
				MessageBox.Show("No input files were selected.");
			}
			foreach (string inputFile in o.InputFiles) {
				sem.WaitOne();
				if (tasks.Any(t => t.IsFaulted)) break;
				if (window.DialogResult == DialogResult.Cancel) break;

				string filename_no_ext = Path.GetFileNameWithoutExtension(inputFile);
				window.SetDecodingText(filename_no_ext);

				LWAV w = null;
				string extension = Path.GetExtension(inputFile);
				List<AudioImporterException> exceptions = new List<AudioImporterException>();

				var importers_supported = importers.Where(i => i.SupportsExtension(extension));
				if (!importers_supported.Any()) {
					throw new Exception("No importers supported for file extension " + extension);
				}

				foreach (IAudioImporter importer in importers_supported) {
					try {
						Console.WriteLine("Decoding " + Path.GetFileName(inputFile) + " with " + importer.GetImporterName());
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

				window.SetDecodingText(filename_no_ext + " (applying effects)");
				w = sox.ApplyEffects(w, max_channels: 2);
				window.SetDecodingText("");

				Dictionary<LWAV, string> wavsToExport = new Dictionary<LWAV, string>(3);

				if (o.ExportWholeSong) wavsToExport.Add(w.PlayLoopAndFade(o.NumberOfLoops, o.FadeOutSec), filename_no_ext + o.WholeSongSuffix);
				if (o.ExportPreLoop) wavsToExport.Add(w.GetPreLoopSegment(), filename_no_ext + o.PreLoopSuffix);
				if (o.ExportLoop) wavsToExport.Add(w.GetLoopSegment(), filename_no_ext + o.LoopSuffix);

				sem.Release();

				foreach (KeyValuePair<LWAV, string> toExport in wavsToExport) {
					sem.WaitOne();
					MultipleProgressRow row = window.AddEncodingRow(toExport.Value);
					if (processors == 1) {
						exporter.WriteFile(toExport.Key, @"C:\Users\Owner\Downloads\b", toExport.Value, row);
						sem.Release();
						row.Remove();
					} else {
						Task task = exporter.WriteFileAsync(toExport.Key, @"C:\Users\Owner\Downloads\b", toExport.Value, row);
						task.ContinueWith(t => {
							sem.Release();
							row.Remove();
						});
						tasks.Add(task);
					}
				}
			}
			Task.WaitAll(tasks.ToArray());
			s.Stop();
			if (window.Visible) window.BeginInvoke(new Action(() => {
				window.Close();
			}));
			Console.WriteLine(s.Elapsed);
			if (tasks.Any(t => t.IsFaulted)) {
				throw new AggregateException(tasks.Where(t => t.IsFaulted).Select(t => t.Exception));
			}
        }
    }
}
