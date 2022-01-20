using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.BrawlLib {
    public class BrawlLibImporter : IAudioImporter {
        public bool SupportsExtension(string extension) => true;

        public bool SharesCodecsWith(IAudioExporter exporter) => exporter is BrawlLibRSTMExporter;

        public async Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints, IProgress<double> progress) {
            await Task.Yield();

            try {
                return BrawlLibRSTMAudio.Create(File.ReadAllBytes(filename));
            } catch (Exception ex) {
                throw new AudioImporterException("Could not export to .wav using BrawlLib", ex);
            }
        }
    }
}
