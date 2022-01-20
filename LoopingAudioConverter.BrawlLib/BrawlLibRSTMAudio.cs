using BrawlLib.Internal.Audio;
using BrawlLib.SSBB.ResourceNodes;
using LoopingAudioConverter.Immutable;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.WAV;
using System;
using System.IO;
using System.Runtime.InteropServices;

using WX = BrawlLib.Internal.Audio.WAV;

namespace LoopingAudioConverter.BrawlLib {
    public unsafe class BrawlLibRSTMAudio : PCM16Audio, IDisposable {
        private readonly IntPtr _address;
        private readonly RSTMNode _node;

        private bool disposedValue;

        private BrawlLibRSTMAudio(PCM16Audio audio, IntPtr address, RSTMNode node)
            : base(audio.Audio, node.IsLooped ? LoopType.NewLooping(node.LoopStartSample, node.NumSamples) : LoopType.NonLooping)
        {
            _address = address;
            _node = node;
        }

        public static PCM16Audio Create(byte[] data) {
            int length = data.Length;
            IntPtr address = Marshal.AllocHGlobal(length);

            Marshal.Copy(data, 0, address, length);

            var node = NodeFactory.FromAddress(null, address, length);

            PCM16Audio audio = null;
            if (node is IAudioSource rstmNode) {
                string file = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");
                WX.ToFile(rstmNode.CreateStreams()[0], file);
                audio = WaveConverter.FromFile(file, delete: true);
            }

            if (node is RSTMNode r && audio != null)
                return new BrawlLibRSTMAudio(audio, address, r);

            if (node != null)
                node.Dispose();

            Marshal.FreeHGlobal(address);

            return audio ?? throw new Exception("Could not export to .wav using BrawlLib");
        }

        public byte[] ExportRSTM() {
            if (!OriginalLoop.Equals(Loop))
                throw new NotImplementedException("Cannot export RSTM with changed loop parameters");

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
