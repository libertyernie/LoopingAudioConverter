using LoopingAudioConverter.PCM;

namespace LoopingAudioConverter.Conversion {
    public interface IConverterEnvironment {
        string FFmpegPath { get; }
        string QaacPath { get; }
        string VGMPlayPath { get; }
        string VGMStreamPath { get; }

        bool Cancelled { get; }

        bool ShowLoopConversionDialog(NamedAudio file);

        void UpdateStatus(string filename, string message);
        void ReportSuccess(string filename);
        void ReportFailure(string filename, string message);
    }
}
