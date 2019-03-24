using BrawlLib.LoopSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    public class MSFAudioStream : IAudioStream
    {
        public readonly MSF MSF;
        public readonly short[] SampleData;

        public MSFAudioStream(MSF msf)
        {
            this.MSF = msf;
            this.SampleData = msf.GetPCM16Samples();
        }

        public WaveFormatTag Format => WaveFormatTag.WAVE_FORMAT_PCM;

        public int BitsPerSample => 16;

        public int Samples => SampleData.Length / MSF.Header.channel_count;

        public int Channels => MSF.Header.channel_count;

        public int Frequency => MSF.Header.sample_rate;

        public bool IsLooping {
            get => MSF.IsLooping;
            set => MSF.IsLooping = value;
        }
        public int LoopStartSample {
            get => MSF.LoopStartSample;
            set => MSF.LoopStartSample = value;
        }
        public int LoopEndSample {
            get => MSF.LoopStartSample + MSF.LoopSampleCount;
            set => MSF.LoopSampleCount = value - MSF.LoopStartSample;
        }

        public int SamplePosition { get; set; }

        public void Dispose() { }

        public int ReadSamples(IntPtr destAddr, int numSamples)
        {
            if (SamplePosition + numSamples > Samples)
            {
                numSamples = Samples - SamplePosition;
            }

            Marshal.Copy(SampleData, SamplePosition * Channels, destAddr, numSamples * Channels);
            SamplePosition += numSamples;
            return numSamples;
        }

        public void Wrap()
        {
            IAudioStream s = this;
            s.SamplePosition = s.LoopStartSample;
        }
    }
}
