using LoopingAudioConverter.PCM;
using System;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter.MSF
{
    /// <summary>
    /// The header and data of an MSF file.
    /// MSF is an audio format used in PlayStation 3 software.
    /// </summary>
    public abstract class MSF
    {
        protected MSFHeader _header;
        protected byte[] _body;

        /// <summary>
        /// The MSF header.
        /// </summary>
        public MSFHeader Header => _header;

        /// <summary>
        /// The data after the header.
        /// </summary>
        public byte[] Body => _body;

        /// <summary>
        /// Whether the stream is looping.
        /// </summary>
        public bool IsLooping {
            get {
                return Header.flags.Flags.HasFlag(MSFFlag.LoopMarker0)
                    || Header.flags.Flags.HasFlag(MSFFlag.LoopMarker1)
                    || Header.flags.Flags.HasFlag(MSFFlag.LoopMarker2)
                    || Header.flags.Flags.HasFlag(MSFFlag.LoopMarker3);
            }
            set {
                if (value && !IsLooping)
                {
                    _header.flags.Flags |= MSFFlag.LoopMarker0;
                    _header.flags.Flags &= ~MSFFlag.LoopMarker1;
                    _header.flags.Flags &= ~MSFFlag.LoopMarker2;
                    _header.flags.Flags &= ~MSFFlag.LoopMarker3;
                }
                else if (!value && IsLooping)
                {
                    _header.flags.Flags &= ~MSFFlag.LoopMarker0;
                    _header.flags.Flags &= ~MSFFlag.LoopMarker1;
                    _header.flags.Flags &= ~MSFFlag.LoopMarker2;
                    _header.flags.Flags &= ~MSFFlag.LoopMarker3;
                }
            }
        }

        /// <summary>
        /// The sample at which the loop starts.
        /// </summary>
        public abstract int GetLoopStartSample();

        /// <summary>
        /// The number of samples in the loop.
        /// </summary>
        public abstract int GetLoopSampleCount();

        /// <summary>
        /// Creates a new MSF file.
        /// </summary>
        /// <param name="header">The header</param>
        /// <param name="body">The rest of the data</param>
        protected MSF(MSFHeader header, byte[] body) {
            _header = header;
            _body = body;
        }

        /// <summary>
        /// Decodes the data of the MSF file.
        /// </summary>
        /// <returns>A PCM16Audio object with the header and uncompressed audio data</returns>
        public abstract PCM16Audio Decode();

        /// <summary>
        /// Exports the data of the MSF file.
        /// </summary>
        /// <returns>A byte array with the header and audio data</returns>
        public unsafe byte[] Export()
        {
            byte[] arr = new byte[sizeof(MSFHeader) + Body.Length];
            fixed (byte* ptr = arr)
            {
                *(MSFHeader*)ptr = Header;
            }
            Array.Copy(Body, 0, arr, sizeof(MSFHeader), Body.Length);
            return arr;
        }
    }
}
