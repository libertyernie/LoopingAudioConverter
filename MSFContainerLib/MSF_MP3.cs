using MP3Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    public class MSF_MP3 : MSF
    {
        public override int LoopStartSample { get; set;  }
        public override int LoopEndSample { get; set; }

        private short[] SampleData { get; set; }

        private MSF_MP3(MSFHeader header, byte[] body, short[] samples, int bitrate) : base(header, body) {
            if (Header.codec != 7)
            {
                throw new FormatException("The codec of this MSF file is not MP3");
            }
            LoopStartSample = checked((int)((long)Header.loop_start * Header.sample_rate * sizeof(byte) / bitrate));
            LoopEndSample = LoopStartSample + checked((int)((long)Header.loop_length * Header.sample_rate * sizeof(byte) / bitrate));
        }

        public static unsafe MSF_MP3 Decode(MSFHeader header, byte[] body)
        {
            using (var output = new MemoryStream())
            {
                int bitrate;

                using (var input = new MemoryStream(body, false))
                using (var mp3 = new MP3Stream(input))
                {
                    mp3.CopyTo(output);

                    double mp3BytesLength = input.Position;
                    double uncompressedLength = output.Position;
                    double samples = uncompressedLength / header.channel_count / sizeof(short);
                    double seconds = samples / header.sample_rate;
                    double bytes_per_second = mp3BytesLength / seconds;
                    bitrate = (int)Math.Round(bytes_per_second);
                }

                byte[] array = output.ToArray();
                fixed (byte* ptr = array)
                {
                    short* ptr16 = (short*)ptr;
                    short[] array16 = new short[array.Length / sizeof(short)];
                    Marshal.Copy((IntPtr)ptr16, array16, 0, array16.Length);
                    return new MSF_MP3(header, body, array16, bitrate);
                }
            }
        }

        public unsafe override short[] GetPCM16Samples()
        {
            using (var output = new MemoryStream())
            {
                using (var input = new MemoryStream(Body, false))
                using (var mp3 = new MP3Stream(input))
                {
                    mp3.CopyTo(output);
                }

                byte[] array = output.ToArray();
                fixed (byte* ptr = array)
                {
                    short* ptr16 = (short*)ptr;
                    short[] array16 = new short[array.Length / sizeof(short)];
                    Marshal.Copy((IntPtr)ptr16, array16, 0, array16.Length);
                    return array16;
                }
            }
        }

        public unsafe override void SetPCM16Samples(short[] samples)
        {
            throw new NotImplementedException();
        }
    }
}
