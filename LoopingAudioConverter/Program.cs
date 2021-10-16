using BrawlLib.LoopSelection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VGAudio.Utilities;

namespace LoopingAudioConverter {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();

			List<string> errors = new List<string>(0);
			foreach (string s in new string[] { "sox_path", "vgmstream_path", "lame_path", "faad_path" }) {
				string v = ConfigurationManager.AppSettings[s];
				if (string.IsNullOrEmpty(v)) {
					errors.Add("The configuration setting " + s + " is missing");
				} else if (!File.Exists(ConfigurationManager.AppSettings[s])) {
					errors.Add($"Could not find {s} ({v})");
				}
			}

			if (errors.Any()) {
				MessageBox.Show("One or more programs could not be found; the program may not run properly:" + Environment.NewLine + string.Join(Environment.NewLine, errors));
			}

			string ini = null;
			List<string> initialInputFiles = new List<string>();
			bool auto = false;
			foreach (string arg in args) {
				if (arg == "--auto") {
					auto = true;
				} else if (Path.GetExtension(arg).ToLowerInvariant() == ".xml") {
					if (ini != null) {
						throw new Exception("You cannot specify more than one .xml file.");
					}
					ini = arg;
				} else {
					initialInputFiles.Add(arg);
				}
			}

			OptionsForm f = new OptionsForm();
			if (File.Exists(ini ?? "LoopingAudioConverter.xml")) {
				f.LoadOptions(ini ?? "LoopingAudioConverter.xml");
			}

			f.AddInputFiles(initialInputFiles);
			
			if (auto) {
				f.Auto = true;
				f.Shown += (o, e) => f.AcceptButton.PerformClick();
			} 

			{
				Application.Run(f);
				Task.WaitAll(f.RunningTasks.ToArray());
			}
		}

		/// <summary>
		/// Runs a batch conversion process.
		/// </summary>
		/// <param name="o">Options for the batch.</param>
		public static async Task RunAsync(Options o, bool showEndDialog = true, IWin32Window owner = null) {
			if (o.ExporterType == ExporterType.MP3 && (o.ExportPreLoop || o.ExportLoop)) {
				MessageBox.Show(owner, "MP3 encoding adds gaps at the start and end of each file, so the before-loop portion and the loop portion will not line up well.",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			SoX sox = new SoX(ConfigurationManager.AppSettings["sox_path"]);

			List<IAudioImporter> importers = new List<IAudioImporter> {
					new WAVImporter(),
					new MP3Importer(),
					new MP4Importer(ConfigurationManager.AppSettings["faad_path"]),
					new VGMImporter(ConfigurationManager.AppSettings["vgmplay_path"]),
					new MSU1(),
					new MSFImporter(),
					new VGMStreamImporter(ConfigurationManager.AppSettings["vgmstream_path"]),
					sox
				};
			if (o.VGAudioDecoder) {
				importers.Insert(1, new VGAudioImporter());
			}

			IAudioExporter exporter;
			switch (o.ExporterType) {
				case ExporterType.BRSTM:
					exporter = new RSTMExporter(o.BxstmOptions?.Configuration);
					break;
				case ExporterType.BCSTM:
					exporter = new CSTMExporter(o.BxstmOptions?.Configuration);
					break;
				case ExporterType.BFSTM:
					exporter = new FSTMExporter(o.BxstmOptions?.Configuration);
					break;
				case ExporterType.DSP:
					exporter = new DSPExporter();
					break;
				case ExporterType.IDSP:
					exporter = new IDSPExporter();
					break;
				case ExporterType.HPS:
					exporter = new HPSExporter();
					break;
				case ExporterType.HCA:
					exporter = new HCAExporter(o.HcaOptions?.Configuration);
					break;
				case ExporterType.ADX:
					exporter = new ADXExporter(o.AdxOptions?.Configuration);
					break;
				case ExporterType.MSF_PCM16BE:
					exporter = new MSFPCM16Exporter(big_endian: true);
					break;
				case ExporterType.MSF_PCM16LE:
					exporter = new MSFPCM16Exporter(big_endian: false);
					break;
				case ExporterType.MSU1:
					exporter = new MSU1();
					break;
				case ExporterType.FLAC:
					exporter = new FLACExporter(sox);
					break;
				case ExporterType.MP3:
					exporter = new MP3Exporter(ConfigurationManager.AppSettings["lame_path"], o.MP3EncodingParameters);
					break;
				case ExporterType.AAC_M4A:
					exporter = new AACExporter(ConfigurationManager.AppSettings["qaac_path"], o.AACEncodingParameters, adts: false);
					break;
				case ExporterType.AAC_ADTS:
					exporter = new AACExporter(ConfigurationManager.AppSettings["qaac_path"], o.AACEncodingParameters, adts: true);
					break;
				case ExporterType.OggVorbis:
					exporter = new OggVorbisExporter(sox, o.OggVorbisEncodingParameters);
					break;
				case ExporterType.WAV:
					exporter = new WAVExporter();
					break;
				default:
					throw new Exception("Could not create exporter type " + o.ExporterType);
			}

			List<Task> tasks = new List<Task>();
			SemaphoreSlim sem = new SemaphoreSlim(o.NumSimulTasks, o.NumSimulTasks);

			MultipleProgressWindow window = new MultipleProgressWindow();
			window.Show(owner);

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
				MessageBox.Show(owner, "No input files were selected.");
			}

			int i = 0;
			float maxProgress = o.InputFiles.Count() * 2;

			ConcurrentQueue<string> exported = new ConcurrentQueue<string>();
			DateTime start = DateTime.UtcNow;
			foreach (string inputFile in o.InputFiles) {
				await sem.WaitAsync();
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
						MessageBox.Show(owner, "Could not create output directory " + o.OutputDir + ": " + e.Message);
					}
				}

				string filename_no_ext = Path.GetFileNameWithoutExtension(inputFile);
				window.SetDecodingText(filename_no_ext);

				string extension = Path.GetExtension(inputFile);

				PCM16Audio w = null;
				List<AudioImporterException> exceptions = new List<AudioImporterException>();

				var importers_supported = importers.Where(im => im.SupportsExtension(extension));
				if (!importers_supported.Any()) {
					throw new Exception("No importers supported for file extension " + extension);
				}

				foreach (IAudioImporter importer in importers_supported) {
					try {
						if (importer is IRenderingAudioImporter) {
							((IRenderingAudioImporter)importer).SampleRate = o.SampleRate;
						}
						w = await importer.ReadFileAsync(inputFile);
						w.OriginalPath = inputFile;
						break;
					} catch (AudioImporterException e) {
						//Console.Error.WriteLine(importer.GetImporterName() + " could not read file " + inputFile + ": " + e.Message);
						exceptions.Add(e);
					}
				}

				if (loopOverrides.Any()) {
					if (loopOverrides.TryGetValue(Path.GetFileName(inputFile), out Tuple<int, int> val)) {
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
					DialogResult dr = MessageBox.Show(owner, "Could not read " + inputFile + ".", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
					if (dr == DialogResult.Cancel) {
						break;
					} else {
						continue;
					}
				}

				window.SetDecodingText(filename_no_ext + " (applying effects)");
				w = sox.ApplyEffects(w,
					channels: o.Channels ?? w.Channels,
					rate: o.SampleRate ?? w.SampleRate,
					db: o.AmplifydB ?? 0M,
					amplitude: o.AmplifyRatio ?? 1M,
					pitch_semitones: o.PitchSemitones ?? 0M,
					tempo_ratio: o.TempoRatio ?? 1M);
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
					int claims = 1;
					if (exporter is VGAudioExporter) {
						// VGAudio runs tasks in parallel for each channel, so let's consider that when deciding how many tasks to run.
						claims = Math.Min(n.Audio.Channels, o.NumSimulTasks);
					}
					for (int j = 0; j < claims; j++) {
						await sem.WaitAsync();
					}
					switch (o.UnknownLoopBehavior) {
						case UnknownLoopBehavior.ForceLoop:
							if (!toExport.Audio.Looping && !toExport.Audio.NonLooping) {
								toExport.Audio.Looping = true;
								toExport.Audio.LoopStart = 0;
								toExport.Audio.LoopEnd = toExport.Audio.Samples.Length / toExport.Audio.Channels;
							}
							break;
						case UnknownLoopBehavior.Ask:
						case UnknownLoopBehavior.AskAll:
							if (toExport.Audio.Looping || toExport.Audio.NonLooping) {
								if (o.UnknownLoopBehavior != UnknownLoopBehavior.AskAll) {
									break;
								}
							}
							PCM16LoopWrapper audioStream = new PCM16LoopWrapper(toExport.Audio);
							using (BrstmConverterDialog dialog = new BrstmConverterDialog(audioStream)) {
								dialog.AudioSource = n.Name;
								if (dialog.ShowDialog(owner) != DialogResult.OK) {
									toExport = null;
								}
							}
							break;
					}
					if (toExport == null) {
						i++;
						for (int j = 0; j < claims; j++) {
							sem.Release();
						}
						break;
					}

					if (!o.ShortCircuit) {
						toExport.Audio.OriginalPath = null;
						toExport.Audio.OriginalAudioData = null;
						toExport.Audio.OriginalMP3 = null;
					}
					if (!o.WriteLoopingMetadata) {
						toExport.Audio.Looping = false;
					}

					async Task Encode() {
						var row = window.AddEncodingRow(toExport.Name);
						try {
							await exporter.WriteFileAsync(toExport.Audio, outputDir, toExport.Name);
							exported.Enqueue(toExport.Name);
						} finally {
							for (int j = 0; j < claims; j++) {
								sem.Release();
							}
							row.Remove();
						}
					}

					tasks.Add(Encode());
				}
			}
			foreach (var t in tasks) {
				try {
					await t;
				} catch (Exception ex) {
					Console.Error.WriteLine($"{ex.GetType()}: {ex.Message}");
					Console.Error.WriteLine(ex.StackTrace);
					MessageBox.Show(owner, (ex.InnerException ?? ex).Message);
				}
			}
			DateTime end = DateTime.UtcNow;

			if (window.Visible) window.BeginInvoke(new Action(() => {
				window.AllowClose = true;
				window.Close();
			}));

			if (showEndDialog) {
				MessageBox.Show(owner, "Exported " + exported.Count + " file(s), total time: " + (end - start));
			}
		}
	}
}
