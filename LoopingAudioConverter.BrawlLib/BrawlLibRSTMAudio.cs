using BrawlLib.SSBB.ResourceNodes;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.BrawlLib {
    public unsafe sealed class BrawlLibRSTMAudio : IAudio {
        private readonly RSTMNode _node;

        public bool Looping => _node.IsLooped;
        public int LoopStart => _node.LoopStartSample;
        public int LoopEnd => _node.NumSamples;

		public unsafe BrawlLibRSTMAudio(RSTMNode node) {
            _node = node;
        }

        public void Export(string path) {
            _node.Export(path);
		}

        [Obsolete]
		public Task<PCM16Audio> DecodeAsync() {
			string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");
			var audio = Task.FromResult(WaveConverter.FromFile(file, true));
            File.Delete(file);
            return audio;
		}

		public override string ToString() {
			return base.ToString() + " (BrawlLib)";
        }

		public void Dispose() {
            _node.Dispose();
		}
	}
}
