using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib
{
    public interface IPcmAudioSource<T> where T : struct
    {
        IEnumerable<T> SampleData { get; }
        int Channels { get; }
        int SampleRate { get; }
        bool IsLooping { get; }
        int LoopStartSample { get; }
        int LoopSampleCount { get; }
    }
}
