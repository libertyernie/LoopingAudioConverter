using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.BrawlLib {
    public class BrawlLibRSTMImporter : IAudioImporter {
        public bool SupportsExtension(string extension) {
            return new[] { ".brstm", ".bcstm", ".bfstm" }.Any(x => string.Equals(extension, x, StringComparison.InvariantCultureIgnoreCase));
        }

        public Task<PCM16Audio> ReadFileAsync(string filename) {
            PCM16Audio x = BrawlLibAudio.Create(File.ReadAllBytes(filename));
            return Task.FromResult(x);
        }
    }
}
