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
        /// Reads an MSF file from a byte array.
        /// </summary>
        /// <param name="data">The complete data of the MSF file (including the header)</param>
        /// <returns>An MSF object</returns>
        public unsafe static MSF Parse(byte[] data)
        {
            fixed (byte* ptr = data)
            {
                MSFHeader header = *(MSFHeader*)ptr;
                int size = Math.Min(header.data_size, data.Length - sizeof(MSFHeader));
                byte[] body = new byte[size];
                Marshal.Copy((IntPtr)(ptr + sizeof(MSFHeader)), body, 0, body.Length);
                switch (header.codec)
                {
                    case 0:
                        return new MSF_PCM16BE(header, body);
                    case 1:
                        return new MSF_PCM16LE(header, body);
                    case 7:
                        return new MSF_MP3(header, body);
                    default:
                        throw new NotSupportedException($"The codec {header.codec} is not supported.");
                }
            }
		}

		/// <summary>
		/// Reads the data of the MSF file.
		/// </summary>
		/// <returns>An IAudio object with the header and audio data</returns>
		public abstract IAudio Read();

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
