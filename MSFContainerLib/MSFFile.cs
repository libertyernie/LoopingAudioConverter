using BrawlLib.LoopSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFEncoderLib
{
    public class MSFFile : IAudioStream
    {
        public readonly MSFHeader Header;
        public readonly Lazy<short[]> SampleData;

        public unsafe MSFFile(byte[] data)
        {
            fixed (byte* ptr = data)
            {
                Header = *(MSFHeader*)ptr;
            }
            int size = Math.Min(Header.data_size, data.Length - sizeof(MSFHeader));
            SampleData = new Lazy<short[]>(() =>
            {
                short[] samples;
                fixed (byte* ptr = data)
                {
                    switch (Header.codec)
                    {
                        case 0:
                            BigEndianInt16* be_samples = (BigEndianInt16*)(ptr + sizeof(MSFHeader));
                            samples = new short[size / sizeof(short)];
                            fixed (short* dest = samples)
                            {
                                for (int i = 0; i < samples.Length; i++)
                                {
                                    samples[i] = be_samples[i];
                                }
                            }
                            break;
                        case 1:
                            short* le_samples = (short*)(ptr + sizeof(MSFHeader));
                            samples = new short[size / sizeof(short)];
                            Marshal.Copy((IntPtr)le_samples, samples, 0, samples.Length);
                            break;
                        default:
                            throw new FormatException("This MSF file uses a codec other than 16-bit PCM.");
                    }
                }
                return samples;
            });
        }

        WaveFormatTag IAudioStream.Format => WaveFormatTag.WAVE_FORMAT_PCM;

        int IAudioStream.BitsPerSample => 16;

        int IAudioStream.Samples => SampleData.Value.Length / Header.channel_count;

        int IAudioStream.Channels => Header.channel_count;

        int IAudioStream.Frequency => Header.sample_rate;

        bool IAudioStream.IsLooping { get => Header.flags != -1 && (Header.flags & 3) != 0; set => throw new InvalidOperationException(); }
        int IAudioStream.LoopStartSample { get => Header.loop_start; set => throw new InvalidOperationException(); }
        int IAudioStream.LoopEndSample { get => Header.loop_end; set => throw new InvalidOperationException(); }
        int IAudioStream.SamplePosition { get; set; }

        void IDisposable.Dispose() { }

        int IAudioStream.ReadSamples(IntPtr destAddr, int numSamples)
        {
            IAudioStream s = this;
            if (s.SamplePosition + numSamples > s.Samples)
            {
                numSamples = s.Samples - s.SamplePosition;
            }

            Marshal.Copy(SampleData.Value, s.SamplePosition * s.Channels, destAddr, numSamples * s.Channels);
            s.SamplePosition += numSamples;
            return numSamples;
        }

        void IAudioStream.Wrap()
        {
            IAudioStream s = this;
            s.SamplePosition = s.LoopStartSample;
        }
    }
}
