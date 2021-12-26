using BrawlLib.SSBB.Types.Audio;
using LoopingAudioConverter.VGAudioOptions;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LoopingAudioConverter {
	public enum ExporterType {
		BRSTM,
		BCSTM,
		BFSTM,
		DSP,
		IDSP,
		HPS,
		HCA,
		ADX,
		BrawlLib_BRSTM_ADPCM,
		BrawlLib_BRSTM_PCM16,
		BrawlLib_BCSTM,
		BrawlLib_BFSTM,
		BrawlLib_BRWAV,
		MSF_PCM16LE,
		MSF_PCM16BE,
		MSU1,
		WAV,
		FLAC,
		MP3,
		AAC_M4A,
		AAC_ADTS,
		OggVorbis,
		FFmpeg_MP3,
		FFmpeg_AAC_M4A,
		FFmpeg_AAC_ADTS
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
		public string InputDir { get; set; }
		public int? Channels { get; set; }
		public int? SampleRate { get; set; }
		public decimal? AmplifydB { get; set; }
		public decimal? AmplifyRatio { get; set; }
		public double? PitchSemitones { get; set; }
		public double? TempoRatio { get; set; }
		public ChannelSplit ChannelSplit { get; set; }
		public ExporterType ExporterType { get; set; }
		public string MP3EncodingParameters { get; set; }
		public string AACEncodingParameters { get; set; }
		public string OggVorbisEncodingParameters { get; set; }
		public string MP3FFmpegParameters { get; set; }
		public string AACFFmpegParameters { get; set; }
		public AdxOptions AdxOptions { get; set; }
		public HcaOptions HcaOptions { get; set; }
		public BxstmOptions BxstmOptions { get; set; }
		public WaveEncoding? WaveEncoding { get; set; }
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
		public int NumSimulTasks { get; set; }
	}
}
