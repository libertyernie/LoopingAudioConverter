using LoopingAudioConverter.PCM;
using VGAudio.Containers.Adx;
using VGAudio.Containers.Hca;
using VGAudio.Containers.NintendoWare;

namespace LoopingAudioConverter.Conversion {
    public enum ExporterType {
        VGAudio_BRSTM,
        VGAudio_BCSTM,
        VGAudio_BFSTM,
        VGAudio_DSP,
        VGAudio_IDSP,
        VGAudio_HPS,
        VGAudio_HCA,
        VGAudio_ADX,
        BrawlLib_BRSTM_ADPCM,
        BrawlLib_BRSTM_PCM16,
        BrawlLib_BCSTM,
        BrawlLib_BFSTM,
        BrawlLib_BRWAV,
        QAAC_M4A,
        QAAC_AAC,
        MSF_PCM16BE,
        MSF_PCM16LE,
        MSU1,
        FLAC,
        MP3,
        M4A,
        AAC,
        OggVorbis,
        WAV
    }

    public enum ChannelSplit {
        OneFile,
        Pairs,
        Each
    }

    public enum InputLoopBehavior {
        NoChange,
        ForceLoop,
        AskForNonLooping,
        AskForAll,
        DiscardForAll
    }

    public interface IEncodingParameters {
        BxstmConfiguration VGAudio_BXSTM { get; }
        HcaConfiguration VGAudio_HCA { get; }
        AdxConfiguration VGAudio_ADX { get; }
        string QAAC { get; }
        string FFMpeg_MP3 { get; }
        string FFMpeg_AAC { get; }
        string FFMpeg_Vorbis { get; }
    }

    public enum WholeSongExportType {
        NumberOfLoops, DesiredDuration
    }

    public interface ILoopExportParameters {
        bool ExportPreLoop { get; }
        string PreLoopSuffix { get; }
        bool ExportLoop { get; }
        string LoopSuffix { get; }
        bool ExportPostLoop { get; }
        string PostLoopSuffix { get; }
        bool ExportWholeSong { get; }
        WholeSongExportType WholeSongExportType { get; }
        string WholeSongSuffix { get; }

        int NumberOfLoops { get; }
        decimal DesiredDuration { get; }
        decimal FadeOutSec { get; }
    }

    public struct LoopOverride {
        public int LoopStart, LoopEnd;
    }

    public interface IConverterOptions {
        string InputDir { get; }
        string OutputDir { get; }

        IRenderingHints GetAudioHints(string filename);

        ChannelSplit ChannelSplit { get; }
        InputLoopBehavior InputLoopBehavior { get; }
        ILoopExportParameters LoopExportParameters { get; }
        LoopOverride? GetLoopOverrides(string filename);

        int? Channels { get; }
        int? SampleRate { get; }
        decimal? AmplifydB { get; }
        decimal? AmplifyRatio { get; }
        double? PitchSemitones { get; }
        double? TempoRatio { get; }

        ExporterType ExporterType { get; }
        IEncodingParameters EncodingParameters { get; }
    }
}
