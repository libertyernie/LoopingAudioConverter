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
        private readonly Lazy<short[]> SampleData;
        private readonly Lazy<long> Bitrate;

        public override int LoopStartSample {
            get {
                long x = Header.loop_start;
                x *= Header.sample_rate;
                x *= sizeof(byte);
                x /= Bitrate.Value;
                return checked((int)x);
            }
            set {
                long x = value;
                x *= Bitrate.Value;
                x /= sizeof(byte);
                x /= Header.sample_rate;
                Header.loop_start = checked((int)x);
            }
        }
        public override int LoopEndSample {
            get {
                long x = Header.loop_length;
                x += Header.loop_start;
                x *= Header.sample_rate;
                x *= sizeof(byte);
                x /= Bitrate.Value;
                return checked((int)x);
            }
            set {
                long x = value;
                x *= Bitrate.Value;
                x /= sizeof(byte);
                x /= Header.sample_rate;
                x -= Header.loop_start;
                Header.loop_length = checked((int)x);
            }
        }

        public MSF_MP3(MSFHeader header, byte[] body) : base(header, body) {
            if (Header.codec != 7)
            {
                throw new FormatException("The codec of this MSF file is not MP3");
            }
            SampleData = new Lazy<short[]>(() => Decode());
            Bitrate = new Lazy<long>(() =>
            {
                double sampleCount = SampleData.Value.Length / Header.channel_count;
                double seconds = sampleCount / Header.sample_rate;
                double bytes_per_second = body.Length / seconds;
                return (long)Math.Round(bytes_per_second);
            });
        }

        private unsafe short[] Decode()
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
