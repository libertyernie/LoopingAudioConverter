using BrawlLib.LoopSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    public abstract class MSF
    {
        public MSFHeader Header;
        public byte[] Body { get; protected set; }

        public abstract short[] GetPCM16Samples();

        public abstract int LoopStartSample { get; set; }
        public abstract int LoopSampleCount { get; set; }

        protected MSF(MSFHeader header, byte[] body) {
            this.Header = header;
            this.Body = body;
        }

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
                        return new MSF_Unknown(header, body);
                }
            }
        }

        public unsafe static MSF FromAudioStream(IAudioStream stream)
        {
            MSFHeader header = MSFHeader.Create();
            header.codec = 0;
            header.channel_count = stream.Channels;
            header.data_size = stream.Samples * stream.Channels * sizeof(short);
            header.sample_rate = stream.Frequency;

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

            var msf = new MSF_PCM16BE(header, new byte[0]);
            msf.SetPCM16Samples(samples);
            if (stream.IsLooping)
            {
                msf.Header.flags.Flags |= MSFFlag.LoopMarker0;
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
