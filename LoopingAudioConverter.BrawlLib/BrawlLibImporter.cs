using BrawlLib.Internal.Audio;
using BrawlLib.SSBB.ResourceNodes;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using System;
using System.IO;
using System.Threading.Tasks;

using WX = BrawlLib.Internal.Audio.WAV;

namespace LoopingAudioConverter.BrawlLib {
    public class BrawlLibImporter : IOpinionatedAudioImporter {
        public bool SupportsExtension(string extension) => true;

        public bool SharesCodecsWith(IAudioExporter exporter) => exporter is BrawlLibRSTMExporter;

        public async Task<PCM16Audio> ReadFileAsync(string filename) {
            await Task.Yield();

            try {
                string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");
                using (var node = NodeFactory.FromFile(null, filename)) {
                    if (node is IAudioSource rstmNode) {
                        WX.ToFile(rstmNode.CreateStreams()[0], file);
                        var decoded = WaveConverter.FromFile(file, true);
                        return node is RSTMNode
                            ? new BrawlLibRSTMAudio(File.ReadAllBytes(filename), decoded)
                            : decoded;
                    } else {
                        throw new AudioImporterException("Could not export to .wav using BrawlLib");
                    }
                }

            } catch (Exception ex) {
                throw new AudioImporterException("Could not export to .wav using BrawlLib", ex);
            }
        }
    }
}
