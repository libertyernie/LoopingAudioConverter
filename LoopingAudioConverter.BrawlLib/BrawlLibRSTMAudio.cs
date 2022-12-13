using BrawlLib.SSBB.ResourceNodes;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoopingAudioConverter.BrawlLib {
    public unsafe sealed class BrawlLibRSTMAudio : IAudio {
        private readonly RSTMNode _node;

        public bool Looping { get; set; }
        public int LoopStart { get; set; }
        public int LoopEnd { get; set; }

        public unsafe BrawlLibRSTMAudio(RSTMNode node) {
            _node = node;

            Looping = _node.IsLooped;
            LoopStart = _node.LoopStartSample;
            LoopEnd = _node.NumSamples;
        }

        public bool LoopChanged => Looping != _node.IsLooped
            || LoopStart != _node.LoopStartSample
            || LoopEnd != _node.NumSamples;

        public void Export(string path) {
            if (LoopChanged)
                throw new Exception("Cannot export RSTM with changed loop parameters");

            _node.Export(path);
		}

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
