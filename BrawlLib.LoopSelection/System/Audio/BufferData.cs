using System;

namespace BrawlLib.LoopSelection
{
    public struct BufferData
    {
        internal int _sampleOffset;
        public int SampleOffset { get { return _sampleOffset; } }

        internal int _sampleLength;
        public int SampleLength { get { return _sampleLength; } }

        internal int _dataOffset;
        public int DataOffset { get { return _dataOffset; } }

        internal int _dataLength;
        public int DataLength { get { return _dataLength; } }

        internal IntPtr _part1Address;
        public IntPtr Part1Address { get { return _part1Address; } }
        internal int _part1Length;
        public int Part1Length { get { return _part1Length; } }
        internal int _part1Samples;
        public int Part1Samples { get { return _part1Samples; } }

        internal IntPtr _part2Address;
        public IntPtr Part2Address { get { return _part2Address; } }
        internal int _part2Length;
        public int Part2Length { get { return _part2Length; } }
        internal int _part2Samples;
        public int Part2Samples { get { return _part2Samples; } }

        public bool IsSplit { get { return _part2Length != 0; } }

        public void Fill(IAudioStream stream, bool loop)
        {
            int blockAlign = stream.BitsPerSample * stream.Channels / 8;
            int samplePos = stream.SamplePosition;
            int sampleCount = _sampleLength;
            int samplesRead;
            bool end = false;

            loop = loop && stream.IsLooping;
            int lastSample = loop ? stream.LoopEndSample : stream.Samples;

            IntPtr blockAddr = _part1Address;
            int blockRemaining = _part1Samples;

            while (sampleCount > 0)
            {
                //Get current block sample count
                int blockSamples = Math.Min(blockRemaining, sampleCount);

                //Fill zeros
                if (end)
                    Memory.Fill(blockAddr, (uint)(blockSamples * blockAlign), 0);
                else
                {
                    //Do we extend within last sample range?
                    if ((samplePos <= lastSample) && (lastSample < (samplePos + blockSamples)))
                    {
                        blockSamples = lastSample - samplePos;
                        end = true;
                    }

                    samplesRead = stream.ReadSamples(blockAddr, blockSamples);
                    samplePos += samplesRead;

                    if (samplesRead < blockSamples)
                    {
                        blockSamples = samplesRead;
                        end = true;
                    }
                    else if (loop && end)
                    {
                        stream.Wrap();
                        if (samplePos == stream.SamplePosition)
                        {
                            samplePos = -1;
                            break;
                        }
                        samplePos = stream.SamplePosition;
                        end = false;
                    }
                }

                blockAddr += blockSamples * blockAlign;
                blockRemaining -= blockSamples;

                //Wrap to second buffer
                if (blockRemaining <= 0)
                {
                    blockAddr = _part2Address;
                    blockRemaining = _part2Samples;
                }

                sampleCount -= blockSamples;
            }
        }
    }
}
