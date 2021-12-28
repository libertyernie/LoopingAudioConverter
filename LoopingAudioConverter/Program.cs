using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types.Audio;
using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.MP3;
using LoopingAudioConverter.MSF;
using LoopingAudioConverter.MSU1;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.QuickTime;
using LoopingAudioConverter.VGAudio;
using LoopingAudioConverter.Vorbis;
using LoopingAudioConverter.WAV;
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

namespace LoopingAudioConverter {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();

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
			if (o.ExporterType == ExporterType.FFmpeg_MP3 && (o.ExportPreLoop || o.ExportLoop)) {
				MessageBox.Show(owner, "MP3 encoding adds gaps at the start and end of each file, so the before-loop portion and the loop portion will not line up well.",
					"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			FFmpegEngine effectEngine = ConfigurationManager.AppSettings["ffmpeg_path"] is string ffmpeg_path
				? new FFmpegEngine(ffmpeg_path)
				: throw new AudioImporterException("Could not find SoX - please specify ffmpeg_path or sox_path in .config file");

			IEnumerable<IAudioImporter> BuildImporters() {
				yield return new WaveImporter();
				yield return new MP3Importer();
				yield return new VorbisConverter(effectEngine, "");
				yield return new VGAudioImporter();
				if (ConfigurationManager.AppSettings["vgmplay_path"] is string vgmplay_path)
					yield return new VGMImporter(vgmplay_path);
				yield return new MSU1Converter();
				yield return new MSFImporter();
				if (ConfigurationManager.AppSettings["vgmstream_path"] is string vgmstream_path)
					yield return new VGMStreamImporter(vgmstream_path);
				yield return effectEngine;
			}

			List<IAudioImporter> importers = BuildImporters().ToList();

			IAudioExporter getExporter() {
				switch (o.ExporterType) {
					case ExporterType.BRSTM:
						return new RSTMExporter(o.BxstmOptions?.Configuration);
					case ExporterType.BCSTM:
						return new CSTMExporter(o.BxstmOptions?.Configuration);
					case ExporterType.BFSTM:
						return new FSTMExporter(o.BxstmOptions?.Configuration);
					case ExporterType.DSP:
						return new DSPExporter();
					case ExporterType.IDSP:
						return new IDSPExporter();
					case ExporterType.HPS:
						return new HPSExporter();
					case ExporterType.HCA:
						return new HCAExporter(o.HcaOptions?.Configuration);
					case ExporterType.ADX:
						return new ADXExporter(o.AdxOptions?.Configuration);
					case ExporterType.BrawlLib_BRSTM_ADPCM:
						return new BrawlLibRSTMExporter(WaveEncoding.ADPCM, BrawlLibRSTMExporter.Container.RSTM);
					case ExporterType.BrawlLib_BRSTM_PCM16:
						return new BrawlLibRSTMExporter(WaveEncoding.PCM16, BrawlLibRSTMExporter.Container.RSTM);
					case ExporterType.BrawlLib_BCSTM:	
						return new BrawlLibRSTMExporter(WaveEncoding.ADPCM, BrawlLibRSTMExporter.Container.CSTM);
					case ExporterType.BrawlLib_BFSTM:
						return new BrawlLibRSTMExporter(WaveEncoding.ADPCM, BrawlLibRSTMExporter.Container.FSTM);
					case ExporterType.BrawlLib_BRWAV:
						return new BrawlLibRWAVExporter();
					case ExporterType.MSF_PCM16BE:
						return new MSFExporter(big_endian: true);
					case ExporterType.MSF_PCM16LE:
						return new MSFExporter(big_endian: false);
					case ExporterType.MSU1:
						return new MSU1Converter();
					case ExporterType.FLAC:
						return new FFmpegExporter(effectEngine, "", ".flac");
					case ExporterType.FFmpeg_MP3:
						return new FFmpegExporter(effectEngine, o.MP3FFmpegParameters, ".mp3");
					case ExporterType.AAC_M4A:
						return new AACExporter(ConfigurationManager.AppSettings["qaac_path"], o.AACEncodingParameters, adts: false);
					case ExporterType.FFmpeg_AAC_M4A:
						return new FFmpegExporter(effectEngine, o.AACFFmpegParameters, ".m4a");
					case ExporterType.AAC_ADTS:
						return new AACExporter(ConfigurationManager.AppSettings["qaac_path"], o.AACEncodingParameters, adts: true);
					case ExporterType.FFmpeg_AAC_ADTS:
						return new FFmpegExporter(effectEngine, o.AACFFmpegParameters, ".aac");
					case ExporterType.OggVorbis:
						return new VorbisConverter(effectEngine, o.OggVorbisEncodingParameters);
					case ExporterType.WAV:
						return new WaveExporter();
					default:
						throw new Exception("Could not create exporter type " + o.ExporterType);
				}
			}

			IAudioExporter exporter = getExporter();

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
					if (inputDir == o.InputDir) {
						outputDir = outputDir.Replace("*", "");
						break;
					}
					
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
				w = effectEngine.ApplyEffects(w,
					channels: o.Channels ?? w.Channels,
					rate: o.SampleRate ?? w.SampleRate,
					db: o.AmplifydB ?? 0M,
					amplitude: o.AmplifyRatio ?? 1M,
					pitch_semitones: o.PitchSemitones ?? 0,
					tempo_ratio: o.TempoRatio ?? 1,
					force: !o.ShortCircuit);
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
