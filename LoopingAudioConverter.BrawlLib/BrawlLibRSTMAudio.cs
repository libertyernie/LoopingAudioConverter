using BrawlLib.SSBB.ResourceNodes;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter.BrawlLib {
    public unsafe class BrawlLibRSTMAudio : PCM16Audio, IDisposable {
        private readonly IntPtr _address;
        private readonly int _length;
        private readonly RSTMNode _node;
        private bool disposedValue;

        public BrawlLibRSTMAudio(byte[] data, PCM16Audio decoded) : base(decoded.Channels, decoded.SampleRate, decoded.Samples) {
            _length = data.Length;
            _address = Marshal.AllocHGlobal(_length);

            Marshal.Copy(data, 0, _address, _length);

            _node = NodeFactory.FromAddress(null, _address, _length) as RSTMNode;

            Looping = _node.IsLooped;
            LoopStart = _node.LoopStartSample;
            LoopEnd = _node.NumSamples;
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

        ~BrawlLibRSTMAudio() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
