using System;

namespace BrawlLib.LoopSelection
{
    public interface IAudioStream: IDisposable
    {
        WaveFormatTag Format { get; }
        int BitsPerSample { get; }
        int Samples { get; }
        int Channels { get; }
        int Frequency { get; }
        bool IsLooping { get; set; }
        int LoopStartSample { get; set; }
        int LoopEndSample { get; set; }
        int SamplePosition { get; set; }
        
        /// <summary>
        /// Reads numSamples audio samples into the address specified by destAddr.
        /// This method does not observe loop points and does not loop automatically.
        /// </summary>
        /// <param name="destAddr">The address at which to start writing samples.</param>
        /// <param name="numSamples">The maximum number of samples to read.</param>
        /// <returns>The actual number of samples read (per channel).</returns>
        int ReadSamples(IntPtr destAddr, int numSamples);

        //Wraps the stream to the loop context.
        //Must be used manually in order to track stream state. (Just good coding practice)
        void Wrap();
    }
}
