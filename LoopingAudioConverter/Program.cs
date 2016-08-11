using BrawlLib.Wii.Audio;
using LoopingAudioConverter.Brawl;
using RSTMLib.WAV;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();

			bool appsettingserror = false;
			foreach (string s in new string[] { "sox_path", "madplay_path", "vgmstream_path", "lame_path", "faad_path" }) {
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
			if (File.Exists("LoopingAudioConverter.ini")) {
				f.LoadOptions("LoopingAudioConverter.ini");
			}
			Application.Run(f);

			Task.WaitAll(f.RunningTasks.ToArray());
		}

        /// <summary>
        /// Runs a batch conversion process.
        /// </summary>
        /// <param name="o">Options for the batch.</param>
        /// <param name="audioFilter">If defined, NamedAudio data will be sent through this function after transformations are applied, before being converted and exported.</param>
		public static void Run(Options o, Func<NamedAudio, NamedAudio> audioDump = null) {
			if (o.ExporterType == ExporterType.MP3 && (o.ExportPreLoop || o.ExportLoop)) {
				MessageBox.Show("MP3 encoding adds gaps at the start and end of each file, so the before-loop portion and the loop portion will not line up well.",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			SoX sox = new SoX(ConfigurationManager.AppSettings["sox_path"]);

			List<IAudioImporter> importers = new List<IAudioImporter> {
					new WAVImporter(),
					new MP3Importer(ConfigurationManager.AppSettings["madplay_path"]),
					new MP4Importer(ConfigurationManager.AppSettings["faad_path"]),
					new VGMImporter(ConfigurationManager.AppSettings["vgm2wav_path"]),
					new VGMStreamImporter(ConfigurationManager.AppSettings["vgmstream_path"]),
					sox
				};
			if (o.BrawlLibDecoder)
			{
				importers.Insert(1, new RSTMImporter());
			}

			IAudioExporter exporter;
			switch (o.ExporterType) {
				case ExporterType.BRSTM:
					exporter = new RSTMExporter();
					break;
				case ExporterType.BCSTM:
					exporter = new CSTMExporter();
					break;
				case ExporterType.BFSTM:
					exporter = new FSTMExporter();
					break;
				case ExporterType.FLAC:
					exporter = new FLACExporter(sox);
					break;
				case ExporterType.MP3:
					exporter = new MP3Exporter(ConfigurationManager.AppSettings["lame_path"]);
					break;
				case ExporterType.OggVorbis:
					exporter = new OggVorbisExporter(sox);
					break;
				case ExporterType.WAV:
					exporter = new WAVExporter();
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

			Dictionary<string, Tuple<int, int>> loopOverrides = new Dictionary<string, Tuple<int, int>>();
			if (File.Exists("loop.txt")) {
				using (StreamReader sr = new StreamReader("loop.txt")) {
					string line;
					while ((line = sr.ReadLine()) != null) {
						line = Regex.Replace(line, "[ \t]+", " ");
						if (line.Length > 0 && line[0] != '#' && line.Contains(' ')) {
							try {
								int loopStart = int.Parse(line.Substring(0, line.IndexOf(" ")));
								line = line.Substring(line.IndexOf(" ") + 1);
								int loopEnd = int.Parse(line.Substring(0, line.IndexOf(" ")));
								line = line.Substring(line.IndexOf(" ") + 1);

								loopOverrides.Add(line, new Tuple<int, int>(loopStart, loopEnd));
							} catch (Exception e) {
								Console.Error.WriteLine("Could not parse line in loop.txt: " + line + " - " + e.Message);
							}
						}
					}
				}
			}

			if (!o.InputFiles.Any()) {
				MessageBox.Show("No input files were selected.");
			}

			int i = 0;
			float maxProgress = o.InputFiles.Count() * 2;
			if (o.InputFiles.Any()) window.ShowProgress();

			List<string> exported = new List<string>();
			foreach (string inputFile in o.InputFiles) {
				sem.WaitOne();
				if (tasks.Any(t => t.IsFaulted)) break;
				if (window.Canceled) break;

				string outputDir = o.OutputDir;
				string inputDir = Path.GetDirectoryName(inputFile);
				for (int x = 0; x < 100; x++) {
					int index = outputDir.LastIndexOf('*');
					if (index < 0) break;

					string replacement = Path.GetFileName(inputDir);
					outputDir = outputDir.Substring(0, index) + replacement + outputDir.Substring(index + 1);
					inputDir = Path.GetDirectoryName(inputDir);
				}

				if (!Directory.Exists(outputDir)) {
					try {
						Directory.CreateDirectory(outputDir);
					} catch (Exception e) {
						MessageBox.Show("Could not create output directory " + o.OutputDir + ": " + e.Message);
					}
				}

				string filename_no_ext = Path.GetFileNameWithoutExtension(inputFile);
				window.SetDecodingText(filename_no_ext);

				string extension = Path.GetExtension(inputFile);

				if (o.ShortCircuit) {
					if (o.ExporterType == ExporterType.BRSTM || o.ExporterType == ExporterType.BCSTM || o.ExporterType == ExporterType.BFSTM) {
						byte[] rstm = null;
						if (extension.Equals(".brstm", StringComparison.InvariantCultureIgnoreCase)) {
							rstm = File.ReadAllBytes(inputFile);
						} else if (extension.Equals(".bcstm", StringComparison.InvariantCultureIgnoreCase)) {
							byte[] cstm = File.ReadAllBytes(inputFile);
							rstm = CSTMConverter.ToRSTM(cstm);
						} else if (extension.Equals(".bfstm", StringComparison.InvariantCultureIgnoreCase)) {
							byte[] fstm = File.ReadAllBytes(inputFile);
							rstm = FSTMConverter.ToRSTM(fstm);
						}

						if (rstm != null) {
							string outputFile = null;
							if (o.ExporterType == ExporterType.BRSTM) {
								outputFile = filename_no_ext + ".brstm";
								File.WriteAllBytes(Path.Combine(outputDir, outputFile), rstm);
							} else if (o.ExporterType == ExporterType.BCSTM) {
								outputFile = filename_no_ext + ".bcstm";
								File.WriteAllBytes(Path.Combine(outputDir, outputFile), CSTMConverter.FromRSTM(rstm));
							} else if (o.ExporterType == ExporterType.BFSTM) {
								outputFile = filename_no_ext + ".bfstm";
								File.WriteAllBytes(Path.Combine(outputDir, outputFile), FSTMConverter.FromRSTM(rstm));
							}
							lock (exported) {
								exported.Add(outputFile);
							}
							sem.Release();
							continue;
						}
					}
				}

				PCM16Audio w = null;
				List<AudioImporterException> exceptions = new List<AudioImporterException>();

				var importers_supported = importers.Where(im => im.SupportsExtension(extension));
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

				if (loopOverrides.Any()) {
					Tuple<int, int> val;
					if (loopOverrides.TryGetValue(Path.GetFileName(inputFile), out val)) {
						if (val.Item1 < 0) {
							w.Looping = false;
						} else {
							w.Looping = true;
							w.LoopStart = val.Item1;
							w.LoopEnd = val.Item2;
						}
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

				List<NamedAudio> wavsToExport = new List<NamedAudio>();

				if (o.ExportWholeSong) wavsToExport.Add(new NamedAudio(w.PlayLoopAndFade(o.NumberOfLoops, o.FadeOutSec), filename_no_ext + o.WholeSongSuffix));
				if (o.ExportPreLoop) wavsToExport.Add(new NamedAudio(w.GetPreLoopSegment(), filename_no_ext + o.PreLoopSuffix));
				if (o.ExportLoop) wavsToExport.Add(new NamedAudio(w.GetLoopSegment(), filename_no_ext + o.LoopSuffix));

				if (o.ChannelSplit == ChannelSplit.Pairs) wavsToExport = wavsToExport.SelectMany(x => x.SplitMultiChannelToStereo()).ToList();
				if (o.ChannelSplit == ChannelSplit.Each) wavsToExport = wavsToExport.SelectMany(x => x.SplitMultiChannelToMono()).ToList();

				sem.Release();

				foreach (NamedAudio n in wavsToExport) {
                    NamedAudio toExport = n;
                    sem.WaitOne();
                    if (audioDump != null) {
                        toExport = audioDump(toExport);
                    }
                    MultipleProgressRow row = window.AddEncodingRow(toExport.Name);
                    if (o.NumSimulTasks == 1) {
                        exporter.WriteFile(toExport.LWAV, outputDir, toExport.Name, row);
                        lock (exported) {
                            exported.Add(toExport.Name);
                        }
                        window.Update(++i / maxProgress);
                        sem.Release();
                        row.Remove();
                    } else {
                        Task task = exporter.WriteFileAsync(toExport.LWAV, outputDir, toExport.Name, row);
                        tasks.Add(task.ContinueWith(t => {
                            lock (exported) {
                                exported.Add(toExport.Name);
                            }
                            window.Update(++i / maxProgress);
                            sem.Release();
                            row.Remove();
                        }));
                    }
				}

				window.Update(++i / maxProgress);
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
