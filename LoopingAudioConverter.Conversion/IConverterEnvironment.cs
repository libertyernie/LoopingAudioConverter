using LoopingAudioConverter.PCM;

namespace LoopingAudioConverter.Conversion {
    public interface IConverterEnvironment {
        string FFmpegPath { get; }
        string QaacPath { get; }
        string VGMStreamPath { get; }
        string MetaflacPath { get; }

        bool Cancelled { get; }

        void UpdateStatus(string filename, string message);
        void ReportSuccess(string filename);
        void ReportFailure(string filename, string message);
    }
}
