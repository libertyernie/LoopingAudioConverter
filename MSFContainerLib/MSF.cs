using BrawlLib.LoopSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
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
        /// Gets the audio data as raw 16-bit PCM (decoding if necessary.)
        /// </summary>
        /// <returns></returns>
        public abstract short[] GetPCM16Samples();

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
                if (value)
                {
                    _header.flags.Flags |= MSFFlag.LoopMarker0;
                    _header.flags.Flags &= ~MSFFlag.LoopMarker1;
                    _header.flags.Flags &= ~MSFFlag.LoopMarker2;
                    _header.flags.Flags &= ~MSFFlag.LoopMarker3;
                }
                else
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
        public abstract int LoopStartSample { get; set; }

        /// <summary>
        /// The sample at which the loop ends.
        /// </summary>
        public abstract int LoopSampleCount { get; set; }

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
                        throw new FormatException($"The codec {header.codec} is not supported.");
                }
            }
        }

        /// <summary>
        /// Creates a new MSF (with a 16-bit PCM codec) using an IAudioStream (BrawlLib) object as the source.
        /// </summary>
        /// <param name="stream">The IAudioStream object</param>
        /// <param name="big_endian">Whether to use big-endian (default is true)</param>
        /// <returns></returns>
        public unsafe static MSF FromAudioStream(IAudioStream stream, bool big_endian = true)
        {
            MSFHeader header = MSFHeader.Create();
            header.codec = big_endian ? 0 : 1;
            header.channel_count = stream.Channels;
            header.data_size = stream.Samples * stream.Channels * sizeof(short);
            header.sample_rate = stream.Frequency;
            if (stream.IsLooping)
                header.flags.Flags |= MSFFlag.LoopMarker0;

            short[] samples = new short[stream.Samples * stream.Channels];
            fixed (short* ptr = samples)
            {
                int pos = 0;
                do
                {
                    int read = stream.ReadSamples((IntPtr)(ptr + pos), (samples.Length - pos) / stream.Channels);
                    pos += read * stream.Channels;
                } while (pos < samples.Length);
            }

            MSF_PCM16 msf;
            if (big_endian)
            {
                BigEndianInt16[] samples_be = new BigEndianInt16[samples.Length];
                for (int i = 0; i < samples.Length; i++)
                {
                    samples_be[i] = samples[i];
                }
                byte[] data = new byte[samples.Length * sizeof(short)];
                fixed (BigEndianInt16* ptr = samples_be)
                {
                    Marshal.Copy((IntPtr)ptr, data, 0, data.Length);
                }
                msf = new MSF_PCM16BE(header, data);
            }
            else
            {
                byte[] data = new byte[samples.Length * sizeof(short)];
                fixed (short* ptr = samples)
                {
                    Marshal.Copy((IntPtr)ptr, data, 0, data.Length);
                }
                msf = new MSF_PCM16LE(header, data);
            }

            if (stream.IsLooping)
            {
                int start = stream.LoopStartSample;
                int end = stream.LoopEndSample - stream.LoopStartSample;
                msf.LoopStartSample = start;
                if (msf.LoopStartSample != start)
                    throw new Exception();
                msf.LoopSampleCount = end;
                if (msf.LoopSampleCount != end)
                    throw new Exception();
            }
            return msf;
        }

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
