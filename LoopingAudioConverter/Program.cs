using LoopingAudioConverter.Brawl;
using System;
using System.Collections.Generic;
using System.Configuration;
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

			bool appsettingserror = false;
			foreach (string s in new string[] { "sox_path", "madplay_path", "vgmstream_path", "lame_path" }) {
				string v = ConfigurationManager.AppSettings[s];
				if (string.IsNullOrEmpty(v)) {
					appsettingserror = true;
					Console.Error.WriteLine("The configuration setting " + s + " is missing.");
				} else if (!File.Exists(ConfigurationManager.AppSettings[s])) {
					appsettingserror = true;
					Console.Error.WriteLine("Could not find " + s + ": " + v);
				}
			}
			if (appsettingserror) {
				MessageBox.Show("One or more programs could not be found; the program may not run properly. See the console for details.");
			}

			OptionsForm f = new OptionsForm();
			if (f.ShowDialog() != DialogResult.OK) {
				return;
			}
			Options o = f.GetOptions();

			if (o.ExporterType == ExporterType.MP3 && (o.ExportPreLoop || o.ExportLoop)) {
				MessageBox.Show("MP3 encoding adds gaps at the start and end of each file, so the before-loop portion and the loop portion will not line up well.",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			if (!Directory.Exists(o.OutputDir)) {
				try {
					Directory.CreateDirectory(o.OutputDir);
				} catch (Exception e) {
					MessageBox.Show("Could not create output directory " + o.OutputDir + ": " + e.Message);
				}
			}

			SoX sox = new SoX(ConfigurationManager.AppSettings["sox_path"]);

			IAudioImporter[] importers = {
				new LWAVImporter(),
				new MP3Importer(ConfigurationManager.AppSettings["madplay_path"]),
				new VGMStreamImporter(ConfigurationManager.AppSettings["vgmstream_path"]),
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
			Semaphore sem = new Semaphore(o.NumSimulTasks, o.NumSimulTasks);

			MultipleProgressWindow window = new MultipleProgressWindow();
			new Thread(new ThreadStart(() => {
				Application.EnableVisualStyles();
				window.ShowDialog();
			})).Start();

			if (!o.InputFiles.Any()) {
				MessageBox.Show("No input files were selected.");
			}

			List<string> exported = new List<string>();
			foreach (string inputFile in o.InputFiles) {
				sem.WaitOne();
				if (tasks.Any(t => t.IsFaulted)) break;
				if (window.Canceled) break;

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
					window.SetDecodingText("");
					DialogResult dr = MessageBox.Show("Could not read " + inputFile + ".", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
					if (dr == DialogResult.Cancel) {
						break;
					} else {
						continue;
					}
				}

				window.SetDecodingText(filename_no_ext + " (applying effects)");
				w = sox.ApplyEffects(w,
					max_channels: o.MaxChannels ?? int.MaxValue,
					max_rate: o.MaxSampleRate ?? int.MaxValue,
					db: o.AmplifydB ?? 0M,
					amplitude: o.AmplifyRatio ?? 1M);
				window.SetDecodingText("");

				List<NamedLWAV> wavsToExport = new List<NamedLWAV>();

				if (o.ExportWholeSong) wavsToExport.Add(new NamedLWAV(w.PlayLoopAndFade(o.NumberOfLoops, o.FadeOutSec), filename_no_ext + o.WholeSongSuffix));
				if (o.ExportPreLoop) wavsToExport.Add(new NamedLWAV(w.GetPreLoopSegment(), filename_no_ext + o.PreLoopSuffix));
				if (o.ExportLoop) wavsToExport.Add(new NamedLWAV(w.GetLoopSegment(), filename_no_ext + o.LoopSuffix));

				if (o.ChannelSplit == ChannelSplit.Pairs) wavsToExport = wavsToExport.SelectMany(x => x.SplitMultiChannelToStereo()).ToList();
				if (o.ChannelSplit == ChannelSplit.Each) wavsToExport = wavsToExport.SelectMany(x => x.SplitMultiChannelToMono()).ToList();

				sem.Release();

				foreach (NamedLWAV toExport in wavsToExport) {
					sem.WaitOne();
					MultipleProgressRow row = window.AddEncodingRow(toExport.Name);
					if (o.NumSimulTasks == 1) {
						exporter.WriteFile(toExport.LWAV, o.OutputDir, toExport.Name, row);
						lock (exported) {
							exported.Add(toExport.Name);
						}
						sem.Release();
						row.Remove();
					} else {
						Task task = exporter.WriteFileAsync(toExport.LWAV, o.OutputDir, toExport.Name, row);
						task.ContinueWith(t => {
							lock (exported) {
								exported.Add(toExport.Name);
							}
							sem.Release();
							row.Remove();
						});
						tasks.Add(task);
					}
				}
			}
			Task.WaitAll(tasks.ToArray());
			
			if (window.Visible) window.BeginInvoke(new Action(() => {
				window.AllowClose = true;
				window.Close();
			}));
			
			if (tasks.Any(t => t.IsFaulted)) {
				throw new AggregateException(tasks.Where(t => t.IsFaulted).Select(t => t.Exception));
			}

			MessageBox.Show("Exported " + exported.Count + " file(s).");
		}
	}
}
