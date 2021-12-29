using BrawlLib.SSBB.ResourceNodes;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter.BrawlLib {
    public unsafe class BrawlLibAudio : PCM16Audio, IDisposable {
        private readonly IntPtr _address;
        private readonly int _length;
        private readonly RSTMNode _node;
        private bool disposedValue;

        private BrawlLibAudio(IntPtr address, int length, PCM16Audio decoded) : base(decoded.Channels, decoded.SampleRate, decoded.Samples) {
            _address = address;
            _length = length;
            _node = NodeFactory.FromAddress(null, _address, _length) as RSTMNode;

            Looping = _node.IsLooped;
            LoopStart = _node.LoopStartSample;
            LoopEnd = _node.NumSamples;
        }

        public static BrawlLibAudio Create(byte[] data) {
            int length = data.Length;
            IntPtr address = Marshal.AllocHGlobal(length);
            Marshal.Copy(data, 0, address, length);

            string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");
            using (var node = NodeFactory.FromAddress(null, address, length)) {
                node.Export(file);
            }

            var audio = WaveConverter.FromFile(file, true);
            return new BrawlLibAudio(address, length, audio);
        }

        public bool LoopChanged => Looping != _node.IsLooped
            || LoopStart != _node.LoopStartSample
            || LoopEnd != _node.NumSamples;

        public byte[] ExportRSTM() {
            if (LoopChanged)
                throw new Exception("Cannot export RSTM with changed loop parameters");

            string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".brstm");
            _node.Export(file);
            byte[] data = File.ReadAllBytes(file);
            File.Delete(file);
            return data;
        }

        public override string ToString() {
			return base.ToString() + " (BrawlLib)";
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    _node.Dispose();
                }

                Marshal.FreeHGlobal(_address);
                disposedValue = true;
            }
        }

        ~BrawlLibAudio() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
