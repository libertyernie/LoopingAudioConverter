using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types.Audio;
using LoopingAudioConverter.BrawlLib;
using LoopingAudioConverter.Conversion;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.VGAudioOptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VGAudio.Containers.Adx;
using VGAudio.Containers.Hca;
using VGAudio.Containers.NintendoWare;

namespace LoopingAudioConverter {
	public partial class OptionsForm : Form, IConverterEnvironment, IProgress<double> {
		private class NVPair<T> {
			public string Name { get; set; }
			public T Value { get; set; }
			public NVPair(T value, string name) {
				this.Name = name;
				this.Value = value;
			}
		}

		private HashSet<Task> runningTasks;

		private class EncodingParameterCollection {
			public string
				MP3_FFmpeg = "",
				Vorbis = "",
				AAC_qaac = "",
				AAC_FFmpeg = "";
		}

		private EncodingParameterCollection encodingParameters = new EncodingParameterCollection();

		private HcaOptions hcaOptions = new HcaOptions();
		private AdxOptions adxOptions = new AdxOptions();
		private BxstmOptions bxstmOptions = new BxstmOptions();

		private WaveEncoding waveEncoding = 0;

		public IEnumerable<Task> RunningTasks {
			get {
				return runningTasks;
			}
		}

		public bool Auto { get; set; } = false;

		string IConverterEnvironment.FFmpegPath => ConfigurationManager.AppSettings["ffmpeg_path"];
		string IConverterEnvironment.QaacPath => ConfigurationManager.AppSettings["qaac_path"];
		string IConverterEnvironment.VGMPlayPath => ConfigurationManager.AppSettings["vgmplay_path"];
		string IConverterEnvironment.VGMStreamPath => ConfigurationManager.AppSettings["vgmstream_path"];
		string IConverterEnvironment.MetaflacPath => ConfigurationManager.AppSettings["metaflac_path"];

		bool IConverterEnvironment.Cancelled => !btnCancel.Enabled;

		public OptionsForm() {
			InitializeComponent();

			hcaOptions = new HcaOptions();
			adxOptions = new AdxOptions();

			var exporters = new[] {
				new NVPair<ExporterType>(ExporterType.VGAudio_BRSTM, "[VGAudio] BRSTM"),
				new NVPair<ExporterType>(ExporterType.VGAudio_BCSTM, "[VGAudio] BCSTM"),
				new NVPair<ExporterType>(ExporterType.VGAudio_BFSTM, "[VGAudio] BFSTM"),
				new NVPair<ExporterType>(ExporterType.VGAudio_DSP, "[VGAudio] DSP (Nintendo)"),
				new NVPair<ExporterType>(ExporterType.VGAudio_IDSP, "[VGAudio] IDSP (Nintendo)"),
				new NVPair<ExporterType>(ExporterType.VGAudio_HPS, "[VGAudio] HPS (HAL)"),
				new NVPair<ExporterType>(ExporterType.VGAudio_ADX, "[VGAudio] CRI ADX"),
				new NVPair<ExporterType>(ExporterType.VGAudio_HCA, "[VGAudio] CRI HCA"),
				new NVPair<ExporterType>(ExporterType.BrawlLib_BRSTM_ADPCM, "[BrawlLib] BRSTM (ADPCM)"),
				new NVPair<ExporterType>(ExporterType.BrawlLib_BRSTM_PCM16, "[BrawlLib] BRSTM (PCM16)"),
				new NVPair<ExporterType>(ExporterType.BrawlLib_BCSTM, "[BrawlLib] BCSTM (ADPCM)"),
				new NVPair<ExporterType>(ExporterType.BrawlLib_BFSTM, "[BrawlLib] BFSTM (ADPCM)"),
				new NVPair<ExporterType>(ExporterType.BrawlLib_BRWAV, "[BrawlLib] BRWAV (ADPCM)"),
				new NVPair<ExporterType>(ExporterType.MSF_PCM16BE, "MSF (PCM16, big-endian)"),
				new NVPair<ExporterType>(ExporterType.MSF_PCM16LE, "MSF (PCM16, little-endian)"),
				new NVPair<ExporterType>(ExporterType.MSU1, "MSU-1"),
				new NVPair<ExporterType>(ExporterType.WAV, "WAV"),
				new NVPair<ExporterType>(ExporterType.FLAC, "[FFmpeg] FLAC"),
				new NVPair<ExporterType>(ExporterType.MP3, "[FFmpeg] MP3"),
				new NVPair<ExporterType>(ExporterType.M4A, "[FFmpeg] AAC (.m4a)"),
				new NVPair<ExporterType>(ExporterType.AAC, "[FFmpeg] AAC (ADTS .aac)"),
				new NVPair<ExporterType>(ExporterType.OggVorbis, "[FFmpeg] Vorbis (.ogg)")
			}.ToList();
			if (ConfigurationManager.AppSettings["qaac_path"] != null) {
				exporters.Add(new NVPair<ExporterType>(ExporterType.QAAC_M4A, "[qaac] AAC (.m4a)"));
				exporters.Add(new NVPair<ExporterType>(ExporterType.QAAC_AAC, "[qaac] AAC (ADTS .aac)"));
			}
			comboBox1.DataSource = exporters;
			if (comboBox1.SelectedIndex < 0) comboBox1.SelectedIndex = 0;
			comboBox1.SelectedIndexChanged += (o, e) => {
				switch ((ExporterType)comboBox1.SelectedValue) {
					case ExporterType.VGAudio_BRSTM:
					case ExporterType.VGAudio_BCSTM:
					case ExporterType.VGAudio_BFSTM:
					case ExporterType.OggVorbis:
					case ExporterType.QAAC_M4A:
					case ExporterType.QAAC_AAC:
					case ExporterType.MP3:
					case ExporterType.M4A:
					case ExporterType.AAC:
					case ExporterType.VGAudio_HCA:
					case ExporterType.VGAudio_ADX:
						btnEncodingOptions.Visible = true;
						break;
					default:
						btnEncodingOptions.Visible = false;
						break;
				}
			};

			var unknownLoopBehaviors = new[] {
				new NVPair<InputLoopBehavior>(InputLoopBehavior.NoChange, "Keep as is"),
				new NVPair<InputLoopBehavior>(InputLoopBehavior.DiscardForAll, "Remove loop information if present"),
				new NVPair<InputLoopBehavior>(InputLoopBehavior.ForceLoop, "Add loop information if missing (start-to-end loop)"),
				new NVPair<InputLoopBehavior>(InputLoopBehavior.AskForNonLooping, "Ask for non-looping files"),
				new NVPair<InputLoopBehavior>(InputLoopBehavior.AskForAll, "Ask for all files")
			};
			ddlUnknownLoopBehavior.DataSource = unknownLoopBehaviors;
			if (ddlUnknownLoopBehavior.SelectedIndex < 0) ddlUnknownLoopBehavior.SelectedIndex = 0;

			runningTasks = new HashSet<Task>();

			Shown += (o, e) => {
				foreach (string str in new[] {
					"ffmpeg_path",
					"qaac_path",
					"vgmplay_path",
					"vgmstream_path",
					"metaflac_path"
				}) {
					string v = ConfigurationManager.AppSettings[str];
					if (v == null) {
						MessageBox.Show(this, $"The .config file setting {str} could not be found.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					if (!File.Exists(v)) {
						MessageBox.Show(this, $"The file {v} could not be found.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
			};
		}

		public void AddInputFiles(IEnumerable<string> inputFiles) {
			listBox1.Items.AddRange(inputFiles.ToArray());
		}

		public void LoadOptions(string filename) {
			Options o = GetOptions();
			try {
				OptionsSerialization.PopulateFromFile(filename, ref o);

				if (o.OutputDir != null)
					txtOutputDir.Text = o.OutputDir;
				chkMono.Checked = o.Channels == 1;
				chkSampleRate.Checked = o.SampleRate != null;
				if (o.SampleRate != null)
					numMaxSampleRate.Value = o.SampleRate.Value;
				chkAmplifydB.Checked = o.AmplifydB != null;
				if (o.AmplifydB != null)
					numAmplifydB.Value = o.AmplifydB.Value;
				chkAmplifyRatio.Checked = o.AmplifyRatio != null;
				if (o.AmplifyRatio != null)
					numAmplifyRatio.Value = o.AmplifyRatio.Value;
				chkPitch.Checked = o.PitchSemitones != null;
				if (o.PitchSemitones != null)
					numPitch.Value = (decimal)o.PitchSemitones.Value;
				chkTempo.Checked = o.TempoRatio != null;
				if (o.TempoRatio != null)
					numTempo.Value = (decimal)o.TempoRatio.Value;
				if (o.ChannelSplit == ChannelSplit.Pairs)
					radChannelsPairs.Checked = true;
				else if (o.ChannelSplit == ChannelSplit.Each)
					radChannelsSeparate.Checked = true;
				else if (o.ChannelSplit == ChannelSplit.OneFile)
					radChannelsTogether.Checked = true;
				comboBox1.SelectedValue = o.ExporterType;
				encodingParameters.AAC_qaac = o.AACEncodingParameters;
				encodingParameters.MP3_FFmpeg = o.MP3FFmpegParameters;
				encodingParameters.AAC_FFmpeg = o.AACFFmpegParameters;
				encodingParameters.Vorbis = o.OggVorbisEncodingParameters;
				hcaOptions = o.HcaOptions ?? new HcaOptions();
				adxOptions = o.AdxOptions ?? new AdxOptions();
				bxstmOptions = o.BxstmOptions ?? new BxstmOptions();
				waveEncoding = o.WaveEncoding ?? WaveEncoding.ADPCM;
				ddlUnknownLoopBehavior.SelectedValue = o.InputLoopBehavior;
				chk0End.Checked = o.ExportWholeSong;
				txt0EndFilenamePattern.Text = o.WholeSongSuffix;
				radNumberLoops.Checked = !o.WholeSongExportByDesiredDuration;
				numNumberLoops.Value = o.NumberOfLoops;
				radDesiredDuration.Checked = o.WholeSongExportByDesiredDuration;
				numDesiredDuration.Value = o.DesiredDuration;
				numFadeOutTime.Value = o.FadeOutSec;
				chk0Start.Checked = o.ExportPreLoop;
				txt0StartFilenamePattern.Text = o.PreLoopSuffix;
				chkStartEnd.Checked = o.ExportLoop;
				txtStartEndFilenamePattern.Text = o.LoopSuffix;
				chkEndFinal.Checked = o.ExportPostLoop;
				txtEndFinalFilenamePattern.Text = o.PostLoopSuffix;
				chkLastLap.Checked = o.ExportLastLap;
				txtLastLapFilenamePattern.Text = o.LastLapSuffix;
				chkNoEncode.Checked = o.BypassEncoding;
			} catch (Exception e) {
				MessageBox.Show(e.Message);
			}
		}

		public Options GetOptions() {
			List<string> filenames = new List<string>();
			foreach (object item in listBox1.Items) {
				filenames.Add(item.ToString());
			}
			return new Options {
				InputFiles = filenames,
				OutputDir = txtOutputDir.Text,
				InputDir = txtInputDir.Text,
				Channels = chkMono.Checked ? 1 : (int?)null,
				SampleRate = chkSampleRate.Checked ? (int)numMaxSampleRate.Value : (int?)null,
				AmplifydB = chkAmplifydB.Checked ? numAmplifydB.Value : (decimal?)null,
				AmplifyRatio = chkAmplifyRatio.Checked ? numAmplifyRatio.Value : (decimal?)null,
				PitchSemitones = chkPitch.Checked ? (double)numPitch.Value : (double?)null,
				TempoRatio = chkTempo.Checked ? (double)numTempo.Value : (double?)null,
				ChannelSplit = radChannelsPairs.Checked ? ChannelSplit.Pairs
					: radChannelsSeparate.Checked ? ChannelSplit.Each
					: ChannelSplit.OneFile,
				ExporterType = (ExporterType)comboBox1.SelectedValue,
				AACEncodingParameters = encodingParameters.AAC_qaac,
				MP3FFmpegParameters = encodingParameters.MP3_FFmpeg,
				AACFFmpegParameters = encodingParameters.AAC_FFmpeg,
				OggVorbisEncodingParameters = encodingParameters.Vorbis,
				HcaOptions = hcaOptions,
				AdxOptions = adxOptions,
				BxstmOptions = bxstmOptions,
				WaveEncoding = waveEncoding,
				InputLoopBehavior = (InputLoopBehavior)ddlUnknownLoopBehavior.SelectedValue,
				ExportWholeSong = chk0End.Checked,
				WholeSongSuffix = txt0EndFilenamePattern.Text,
				WholeSongExportByDesiredDuration = radDesiredDuration.Checked,
				NumberOfLoops = (int)numNumberLoops.Value,
				DesiredDuration = numDesiredDuration.Value,
				FadeOutSec = numFadeOutTime.Value,
				ExportPreLoop = chk0Start.Checked,
				PreLoopSuffix = txt0StartFilenamePattern.Text,
				ExportLoop = chkStartEnd.Checked,
				LoopSuffix = txtStartEndFilenamePattern.Text,
				ExportPostLoop = chkEndFinal.Checked,
				PostLoopSuffix = txtEndFinalFilenamePattern.Text,
				ExportLastLap = chkLastLap.Checked,
				LastLapSuffix = txtLastLapFilenamePattern.Text,
				BypassEncoding = chkNoEncode.Checked,
			};
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			string oldDir = Environment.CurrentDirectory;
			using (OpenFileDialog d = new OpenFileDialog()) {
				d.Multiselect = true;
				if (d.ShowDialog() == DialogResult.OK) {
					listBox1.Items.AddRange(d.FileNames);
				}
				// Reset directory (Windows XP?)
				Environment.CurrentDirectory = oldDir;
			}
		}

		private void btnAddDir_Click(object sender, EventArgs e) {
			using (FolderBrowserDialog d = new FolderBrowserDialog()) {
				if (d.ShowDialog() == DialogResult.OK) {
					txtInputDir.Text = d.SelectedPath;

					btnAddDir.Enabled = false;
					
					lblEnumerationStatus.Text = "Finding files...";
					Task<string[]> enumerateFiles = new Task<string[]>(() => {
						return Directory.EnumerateFiles(d.SelectedPath, "*.*", SearchOption.AllDirectories).ToArray();
					});
					enumerateFiles.ContinueWith(t => {
						if (t.IsFaulted) {
							MessageBox.Show("Could not enumerate files in this directory.");
						} else {
							this.BeginInvoke(new Action(() => {
								listBox1.Items.AddRange(t.Result);
							}));
						}
						this.BeginInvoke(new Action(() => {
							btnAddDir.Enabled = true;
							lblEnumerationStatus.Text = "";
						}));
					});
					enumerateFiles.Start();
				}
			}
		}

		private void btnRemove_Click(object sender, EventArgs e) {
			SortedSet<int> set = new SortedSet<int>();
			foreach (int index in listBox1.SelectedIndices) {
				set.Add(index);
			}
			foreach (int index in set.Reverse()) {
				listBox1.Items.RemoveAt(index);
			}
		}

		private void listBox1_DragEnter(object sender, DragEventArgs e) {
			string[] data = e.Data.GetData("FileDrop") as string[];
			if (data != null && data.Length != 0) {
				e.Effect = DragDropEffects.Link;
			}
		}

		private void listBox1_DragDrop(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.None;

			string[] data = e.Data.GetData("FileDrop") as string[];
			if (data != null) {
				foreach (string filepath in data) listBox1.Items.Add(filepath);
			}
		}

		private void chk0End_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = (CheckBox)sender;
			txt0EndFilenamePattern.Enabled = cb.Checked;
			numFadeOutTime.Enabled = numNumberLoops.Enabled = cb.Checked;
		}

		private void chk0Start_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = (CheckBox)sender;
			txt0StartFilenamePattern.Enabled = cb.Checked;
		}

		private void chkStartEnd_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = (CheckBox)sender;
			txtStartEndFilenamePattern.Enabled = cb.Checked;
		}

		private void chkEndFinal_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = (CheckBox)sender;
			txtEndFinalFilenamePattern.Enabled = cb.Checked;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			CheckBox cb = (CheckBox)sender;
			txtLastLapFilenamePattern.Enabled = cb.Checked;
		}

		private void chkMaxSampleRate_CheckedChanged(object sender, EventArgs e) {
			numMaxSampleRate.Enabled = chkSampleRate.Checked;
		}

		private void chkAmplifydB_CheckedChanged(object sender, EventArgs e) {
			numAmplifydB.Enabled = chkAmplifydB.Checked;
		}

		private void chkAmplifyRatio_CheckedChanged(object sender, EventArgs e) {
			numAmplifyRatio.Enabled = chkAmplifyRatio.Checked;
		}

		private void chkPitch_CheckedChanged(object sender, EventArgs e) {
			numPitch.Enabled = chkPitch.Checked;
		}

		private void chkTempo_CheckedChanged(object sender, EventArgs e) {
			numTempo.Enabled = chkTempo.Checked;
		}

		private void btnBrowse_Click(object sender, EventArgs e) {
			using (FolderBrowserDialog d = new FolderBrowserDialog()) {
				d.SelectedPath = txtOutputDir.Text;
				if (d.ShowDialog() == DialogResult.OK) {
					txtOutputDir.Text = d.SelectedPath;
				}
			}
		}

		private void btnOpenOutputDir_Click(object sender, EventArgs e) {
			try {
				string path = Path.GetFullPath(txtOutputDir.Text);
				if (Directory.Exists(path)) {
					Process.Start(path);
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void btnHelp_Click(object sender, EventArgs e) {
			Process.Start("About.html");
		}

		private async void btnOkay_Click(object sender, EventArgs e) {
			if (txtSuffixFilter.Text != "") {
				MessageBox.Show(this, "\"Copy files ending with\" has text in it, but you didn't click Filter. You might want to do this before continuing. If not, clear the text field and try again.");
				return;
			}
			Options o = this.GetOptions();
			if (o.ExporterType == ExporterType.QAAC_M4A || o.ExporterType == ExporterType.QAAC_AAC) {
				string qaac_path = ConfigurationManager.AppSettings["qaac_path"];
				if (qaac_path != null) {
					Process p = Process.Start(new ProcessStartInfo {
						FileName = qaac_path,
						UseShellExecute = false,
						CreateNoWindow = true,
						Arguments = "--check"
					});
					p.WaitForExit();
					if (p.ExitCode != 0) {
						MessageBox.Show($"AAC encoding is not supported: CoreAudioToolbox not found. Please intall iTunes.");
						return;
					}
				}
			}
			if (o.ExporterType == ExporterType.MSU1) {
				if (o.Channels != 2 || o.SampleRate != 44100) {
					var r = MessageBox.Show(this, "MSU-1 output must be 2 channels at a sample rate of 44100Hz. Is it OK to make this conversion?", Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
					if (r == DialogResult.OK) {
						o.Channels = 2;
						o.SampleRate = 44100;
					} else {
						return;
					}
				}
			}
			this.listBox1.Items.Clear();
			//Task t = Program.RunAsync(o, showEndDialog: !this.Auto, owner: this);
			btnCancel.Enabled = true;
			Task t = Converter.ConvertFilesAsync(this, o, o.InputFiles.ToList(), this);
			runningTasks.Add(t);
			UpdateTitle();
			try {
				await t;
			} catch (Exception ex) {
				Console.Error.WriteLine(ex);
				MessageBox.Show(this, "An error occurred.\r\nFor more details, run from cmd.exe and write a log file:\r\n\r\nLoopingAudioConverter.exe 2> log.txt", ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			runningTasks.Remove(t);
			UpdateTitle();
			if (this.Auto) {
				this.Close();
			}
		}

		private void UpdateTitle() {
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action(UpdateTitle));
				return;
			}
			string text = this.Text + ":";
			text = text.Substring(0, text.IndexOf(':'));
			splitContainer1.Enabled = panel1.Enabled = runningTasks.Count == 0;
			btnCancel.Enabled = runningTasks.Count != 0;
			switch (runningTasks.Count) {
				case 0:
					break;
				case 1:
					text += ": batch running";
					break;
				default:
					text += ": " + runningTasks.Count + " batches running";
					break;
			}
			this.Text = text;
		}

		private void btnSuffixFilter_Click(object sender, EventArgs e) {
			List<string> filenames = new List<string>();
			string suffix = txtSuffixFilter.Text;
			foreach (object item in listBox1.Items) {
				string s = item.ToString();
				if (s.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase)) filenames.Add(s);
			}
			listBox1.Items.Clear();
			listBox1.Items.AddRange(filenames.ToArray());
			txtSuffixFilter.Text = "";
		}

		private void btnLoadOptions_Click(object sender, EventArgs ea) {
			using (OpenFileDialog d = new OpenFileDialog()) {
				d.FileName = "LoopingAudioConverter.xml";
				if (d.ShowDialog() == DialogResult.OK) {
					LoadOptions(d.FileName);
				}
			}
		}

		private void btnSaveOptions_Click(object sender, EventArgs ea) {
			using (SaveFileDialog d = new SaveFileDialog()) {
				d.InitialDirectory = Environment.CurrentDirectory;
				d.FileName = "LoopingAudioConverter.xml";
				if (d.ShowDialog() == DialogResult.OK) {
					OptionsSerialization.WriteToFile(d.FileName, GetOptions());
				}
			}
		}

		private void btnEncodingOptions_Click(object sender, EventArgs e) {
			switch ((ExporterType)comboBox1.SelectedValue) {
				case ExporterType.MP3:
					using (var form = new MP3QualityForm(encodingParameters.MP3_FFmpeg)) {
						if (form.ShowDialog() != DialogResult.OK) return;
						encodingParameters.MP3_FFmpeg = form.FFmpegParameters;
					}
					break;
				case ExporterType.OggVorbis:
					using (var form = new QualityForm(encodingParameters.Vorbis)) {
						if (form.ShowDialog() != DialogResult.OK) return;
						encodingParameters.Vorbis = form.EncodingParameters;
					}
					break;
				case ExporterType.QAAC_M4A:
				case ExporterType.QAAC_AAC:
					using (var form = new AACQualityForm(encodingParameters.AAC_qaac)) {
						if (form.ShowDialog() != DialogResult.OK) return;
						encodingParameters.AAC_qaac = form.EncodingParameters;
					}
					break;
				case ExporterType.M4A:
				case ExporterType.AAC:
					using (var form = new QualityForm(encodingParameters.AAC_FFmpeg)) {
						if (form.ShowDialog() != DialogResult.OK) return;
						encodingParameters.AAC_FFmpeg = form.EncodingParameters;
					}
					break;
				case ExporterType.VGAudio_BRSTM:
				case ExporterType.VGAudio_BCSTM:
				case ExporterType.VGAudio_BFSTM:
					using (var f = new VGAudioOptionsForm<BxstmOptions, BxstmConfiguration>(bxstmOptions)) {
						if (f.ShowDialog(this) == DialogResult.OK) {
							bxstmOptions = f.SelectedObject;
						}
					};
					break;
				case ExporterType.VGAudio_HCA:
					using (var f = new VGAudioOptionsForm<HcaOptions, HcaConfiguration>(hcaOptions)) {
						if (f.ShowDialog(this) == DialogResult.OK) {
							hcaOptions = f.SelectedObject;
						}
					};
					break;
				case ExporterType.VGAudio_ADX:
					using (var f = new VGAudioOptionsForm<AdxOptions, AdxConfiguration>(adxOptions)) {
						if (f.ShowDialog(this) == DialogResult.OK) {
							adxOptions = f.SelectedObject;
						}
					};
					break;
				default:
					break;
			}
		}

		bool IConverterEnvironment.ShowLoopConversionDialog(NamedAudio file) {
			PCM16LoopWrapper audioStream = new PCM16LoopWrapper(file.Audio);
			using (BrstmConverterDialog dialog = new BrstmConverterDialog(audioStream)) {
				dialog.AudioSource = file.Name;
				return dialog.ShowDialog(this) == DialogResult.OK;
			}
		}

		void IConverterEnvironment.UpdateStatus(string filename, string message) {
			label6.Text = $"{filename}: {message}";
		}

		void IConverterEnvironment.ReportSuccess(string filename) {
			label6.Text = "";
		}

		void IConverterEnvironment.ReportFailure(string filename, string message) {
			MessageBox.Show(this, $"{filename}: {message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			label6.Text = "";
		}

		void IProgress<double>.Report(double value) {
			if (InvokeRequired) {
				BeginInvoke(new Action(() => {
					IProgress<double> pr = this;
					pr.Report(value);
				}));
				return;
			}
			progressBar1.Value = (int)(value * (progressBar1.Maximum - progressBar1.Minimum)) + progressBar1.Minimum;
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			btnCancel.Enabled = false;
		}

		private void chkNoEncode_CheckedChanged(object sender, EventArgs e) {
			panel2.Enabled = !chkNoEncode.Checked;
			pnlExportChannels.Enabled = !chkNoEncode.Checked;
			pnlExportSegments.Enabled = !chkNoEncode.Checked;
		}
	}
}
