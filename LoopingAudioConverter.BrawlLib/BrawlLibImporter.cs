using BrawlLib.Internal.Audio;
using BrawlLib.SSBB.ResourceNodes;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using System;
using System.IO;
using System.Threading.Tasks;

using WX = BrawlLib.Internal.Audio.WAV;

namespace LoopingAudioConverter.BrawlLib {
    public class BrawlLibImporter : IAudioImporter {
        public bool SupportsExtension(string extension) => true;

        public async Task<PCM16Audio> ReadFileAsync(string filename, IRenderingHints hints, IProgress<double> progress) {
            await Task.Yield();

            try {
                string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");
                using (var node = NodeFactory.FromFile(null, filename)) {
                    if (node is IAudioSource rstmNode) {
                        WX.ToFile(rstmNode.CreateStreams()[0], file);
                        return WaveConverter.FromFile(file, true);
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
