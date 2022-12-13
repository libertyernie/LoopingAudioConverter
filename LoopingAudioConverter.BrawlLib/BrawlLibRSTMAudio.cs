using BrawlLib.SSBB.ResourceNodes;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Runtime.InteropServices;

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

        public override string ToString() {
			return base.ToString() + " (BrawlLib)";
        }

		public void Dispose() {
            _node.Dispose();
		}
	}
}
