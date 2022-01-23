using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types.Audio;
using LoopingAudioConverter.BrawlLib;
using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.MP3;
using LoopingAudioConverter.MSF;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.QuickTime;
using LoopingAudioConverter.VGAudio;
using LoopingAudioConverter.VGMStream;
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
	}
}
