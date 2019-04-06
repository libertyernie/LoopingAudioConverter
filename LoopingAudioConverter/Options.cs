using LoopingAudioConverter.VGAudioOptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VGAudio.Containers.NintendoWare;

namespace LoopingAudioConverter {
	public enum ExporterType {
		BRSTM,
		BCSTM,
		BFSTM,
		BFSTM_LE,
		DSP,
		IDSP,
		HPS,
		HCA,
		ADX,
		MSF_PCM16LE,
		MSF_PCM16BE,
		MSU1,
		WAV,
		FLAC,
		MP3,
		AAC_M4A,
		AAC_ADTS,
		OggVorbis
	}

	public enum ChannelSplit {
		OneFile,
		Pairs,
		Each
	}

	public enum UnknownLoopBehavior {
		NoChange,
		ForceLoop,
		Ask,
		AskAll
	}

	public class Options {
		[XmlIgnore]
		public IEnumerable<string> InputFiles { get; set; }

		public string OutputDir { get; set; }
		public int? MaxChannels { get; set; }
		public int? MaxSampleRate { get; set; }
		public decimal? AmplifydB { get; set; }
		public decimal? AmplifyRatio { get; set; }
		public decimal? PitchSemitones { get; set; }
		public decimal? TempoRatio { get; set; }
		public ChannelSplit ChannelSplit { get; set; }
		public ExporterType ExporterType { get; set; }
		public string MP3EncodingParameters { get; set; }
		public string AACEncodingParameters { get; set; }
		public string OggVorbisEncodingParameters { get; set; }
		public AdxOptions AdxOptions { get; set; }
		public HcaOptions HcaOptions { get; set; }
		public NwCodec BxstmCodec { get; set; }
		public UnknownLoopBehavior UnknownLoopBehavior { get; set; }
		public bool ExportWholeSong { get; set; }
		public string WholeSongSuffix { get; set; }
		public int NumberOfLoops { get; set; }
		public decimal FadeOutSec { get; set; }
		public bool WriteLoopingMetadata { get; set; }
		public bool ExportPreLoop { get; set; }
		public string PreLoopSuffix { get; set; }
		public bool ExportLoop { get; set; }
		public string LoopSuffix { get; set; }
		public bool ShortCircuit { get; set; }
		public bool VGAudioDecoder { get; set; }
		public int NumSimulTasks { get; set; }
	}
}
