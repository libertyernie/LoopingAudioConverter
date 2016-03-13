using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopingAudioConverter {
	public partial class OptionsForm : Form {
		private class NVPair {
			public string Name { get; set; }
			public ExporterType Value { get; set; }
			public NVPair(ExporterType value, string name) {
				this.Name = name;
				this.Value = value;
			}
		}

		private HashSet<Task> runningTasks;

		public IEnumerable<Task> RunningTasks {
			get {
				return runningTasks;
			}
		}

		public OptionsForm() {
			InitializeComponent();
			comboBox1.DataSource = new List<NVPair>() {
				new NVPair(ExporterType.BRSTM, "BRSTM"),
				new NVPair(ExporterType.BCSTM, "BCSTM"),
				new NVPair(ExporterType.WAV, "WAV (+smpl)"),
				new NVPair(ExporterType.FLAC, "FLAC"),
				new NVPair(ExporterType.MP3, "MP3"),
				new NVPair(ExporterType.OggVorbis, "Ogg Vorbis")
			};
			if (comboBox1.SelectedIndex < 0) comboBox1.SelectedIndex = 0;
            numSimulTasks.Value = Math.Min(Environment.ProcessorCount, numSimulTasks.Maximum);

			runningTasks = new HashSet<Task>();
		}

		public Options GetOptions() {
			List<string> filenames = new List<string>();
			foreach (object item in listBox1.Items) {
				filenames.Add(item.ToString());
			}
			return new Options {
				InputFiles = filenames,
				OutputDir = txtOutputDir.Text,
				MaxChannels = chkMono.Checked ? 1 : (int?)null,
				MaxSampleRate = chkMaxSampleRate.Checked ? (int)numMaxSampleRate.Value : (int?)null,
				AmplifydB = chkAmplifydB.Checked ? numAmplifydB.Value : (decimal?)null,
				AmplifyRatio = chkAmplifyRatio.Checked ? numAmplifyRatio.Value : (decimal?)null,
                ChannelSplit = radChannelsPairs.Checked ? ChannelSplit.Pairs
                    : radChannelsSeparate.Checked ? ChannelSplit.Each
                    : ChannelSplit.OneFile,
				ExporterType = (ExporterType)comboBox1.SelectedValue,
				ExportWholeSong = chk0End.Checked,
				WholeSongSuffix = txt0EndFilenamePattern.Text,
				NumberOfLoops = (int)numNumberLoops.Value,
				FadeOutSec = numFadeOutTime.Value,
				ExportPreLoop = chk0Start.Checked,
				PreLoopSuffix = txt0StartFilenamePattern.Text,
				ExportLoop = chkStartEnd.Checked,
				LoopSuffix = txtStartEndFilenamePattern.Text,
                NumSimulTasks = (int)numSimulTasks.Value,
                ShortCircuit = chkShortCircuit.Checked
			};
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			using (OpenFileDialog d = new OpenFileDialog()) {
				d.Multiselect = true;
				if (d.ShowDialog() == DialogResult.OK) {
					listBox1.Items.AddRange(d.FileNames);
				}
			}
		}

		private void btnAddDir_Click(object sender, EventArgs e) {
			using (FolderBrowserDialog d = new FolderBrowserDialog()) {
				if (d.ShowDialog() == DialogResult.OK) {
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

		private void chkMaxSampleRate_CheckedChanged(object sender, EventArgs e) {
			numMaxSampleRate.Enabled = chkMaxSampleRate.Checked;
		}

		private void chkAmplifydB_CheckedChanged(object sender, EventArgs e) {
			numAmplifydB.Enabled = chkAmplifydB.Checked;
		}

		private void chkAmplifyRatio_CheckedChanged(object sender, EventArgs e) {
			numAmplifyRatio.Enabled = chkAmplifyRatio.Checked;
		}

		private void btnBrowse_Click(object sender, EventArgs e) {
			using (FolderBrowserDialog d = new FolderBrowserDialog()) {
				d.SelectedPath = txtOutputDir.Text;
				if (d.ShowDialog() == DialogResult.OK) {
					txtOutputDir.Text = d.SelectedPath;
				}
			}
		}

		private void btnHelp_Click(object sender, EventArgs e) {
			Process.Start("About.html");
		}

		private void btnOkay_Click(object sender, EventArgs e) {
			if (txtSuffixFilter.Text != "") {
				MessageBox.Show(this, "\"Copy files ending with\" has text in it, but you didn't click Filter. You might want to do this before continuing. If not, clear the text field and try again.");
				return;
			}
			Options o = this.GetOptions();
			this.listBox1.Items.Clear();
			Task t = new Task(() => Program.Run(o));
			runningTasks.Add(t);
			UpdateTitle();
			t.Start();
			t.ContinueWith(x => {
				runningTasks.Remove(x);
				UpdateTitle();
			});
		}

		private void UpdateTitle() {
			if (this.InvokeRequired) {
				this.BeginInvoke(new Action(UpdateTitle));
				return;
			}
			string text = this.Text + ":";
			text = text.Substring(0, text.IndexOf(':'));
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
	}
}
