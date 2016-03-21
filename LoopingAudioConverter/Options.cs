using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoopingAudioConverter {
	public enum ExporterType {
		BRSTM,
		BCSTM,
		BFSTM,
		WAV,
		FLAC,
		MP3,
		OggVorbis
	}

	public enum ChannelSplit {
		OneFile,
		Pairs,
		Each
	}

	public class Options {
		public IEnumerable<string> InputFiles { get; set; }
		public string OutputDir { get; set; }
		public int? MaxChannels { get; set; }
		public int? MaxSampleRate { get; set; }
		public decimal? AmplifydB { get; set; }
		public decimal? AmplifyRatio { get; set; }
		public ChannelSplit ChannelSplit { get; set; }
		public ExporterType ExporterType { get; set; }
		public bool ExportWholeSong { get; set; }
		public string WholeSongSuffix { get; set; }
		public int NumberOfLoops { get; set; }
		public decimal FadeOutSec { get; set; }
		public bool ExportPreLoop { get; set; }
		public string PreLoopSuffix { get; set; }
		public bool ExportLoop { get; set; }
		public string LoopSuffix { get; set; }
		public bool ShortCircuit { get; set; }
		public int NumSimulTasks { get; set; }
	}
}
