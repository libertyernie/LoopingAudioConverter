using System;

namespace BrawlLib.LoopSelection
{
    public abstract class AudioBuffer : IDisposable
    {
        //Buffer will be valid for two seconds. The application MUST update/fill before then.
        //This is plenty of time, as timer updates should occur every 10 - 100 ms.
        internal const int DefaultBufferSpan = 2;

        internal AudioProvider _owner;
        public AudioProvider Owner { get { return _owner; } }

        internal IAudioStream _source;
        public IAudioStream Source { get { return _source; } }

        internal WaveFormatTag _format;
        public WaveFormatTag Format { get { return _format; } }

        internal int _frequency;
        public int Frequency { get { return _frequency; } }

        internal int _channels;
        public int Channels { get { return _channels; } }

        internal int _bitsPerSample;
        public int BitsPerSample { get { return _bitsPerSample; } }

        //Number of samples that can be stored inside the buffer.
        internal int _sampleLength;
        public int SampleLength { get { return _sampleLength; } }

        //Total byte length of the buffer.
        internal int _dataLength;
        public int DataLength { get { return _dataLength; } }

        //Number of bytes in each sample. (_bitsPerSample * _channels / 8)
        internal int _blockAlign;
        public int BlockAlign { get { return _blockAlign; } }

        //Byte offset within buffer in which to continue writing.
        //Read-only. It is the responsibility of the application to update the audio data in a timely manner.
        //As data is written, this is updated automatically.
        internal int _writeOffset;
        public int WriteOffset { get { return _writeOffset; } }

        //Byte offset within buffer in which reading is currently commencing.
        //The application must call Update (or Fill) to update this value.
        internal int _readOffset;
        public int ReadOffset { get { return _readOffset; } }

        //Cumulative sample position in which to continue writing.
        //This value is updated automatically when fill is called.
        internal int _writeSample;
        public int WriteSample { get { return _writeSample; } }

        //Cumulative sample position in which the buffer is currently reading.
        //This value is updated as Update is called.
        internal int _readSample;
        public int ReadSample { get { return _readSample; } }

        //Sets whether the buffer manages looping.
        //Use this with Source.
        internal bool _loop = false;
        public bool Loop { get { return _loop; } set { _loop = value; } }

        //internal bool _playing = false;
        //public bool IsPlaying { get { return _playing; } }

        //Byte offset within buffer in which playback is commencing.
        internal abstract int PlayCursor { get; set; }

        public abstract int Volume { get; set; }
        public abstract int Pan { get; set; }

        ~AudioBuffer() { Dispose(); }
        public virtual void Dispose()
        {
            if (_owner != null)
            {
                _owner._buffers.Remove(this);
                _owner = null;
            }
            GC.SuppressFinalize(this);
        }

        public abstract void Play();
        public abstract void Stop();
        public abstract BufferData Lock(int offset, int length);
        public abstract void Unlock(BufferData data);

        //Should only be used while playback is stopped
        public void Seek(int samplePos)
        {
            _readOffset = _writeOffset = PlayCursor;
            _readSample = _writeSample = samplePos;

            if (_source != null)
                _source.SamplePosition = samplePos;
        }
        public void Reset()
        {
            _readOffset = _writeOffset = PlayCursor;
        }

        public virtual void Update()
        {
            //Get current sample offset.
            int sampleOffset = PlayCursor / _blockAlign;
            //Get current byte offset
            int byteOffset = sampleOffset * _blockAlign;
            //Get sample difference since last update, taking into account circular wrapping.
            int sampleDifference = (((byteOffset < _readOffset) ? (byteOffset + _dataLength) : byteOffset) - _readOffset) / _blockAlign;
            //Get byte difference
            //int byteDifference = sampleDifference * _blockAlign;

            //If no change, why continue?
            if (sampleDifference == 0)
                return;

            //Set new read offset.
            _readOffset = byteOffset;

            //Update looping
            if (_source != null)
            {
                if ((_loop) && (_source.IsLooping))
                {
                    int start = _source.LoopStartSample;
                    int end = _source.LoopEndSample;
                    int newSample = _readSample + sampleDifference;

                    if ((newSample >= end) && (_writeSample < _readSample))
                        _readSample = start + ((newSample - start) % (end - start));
                    else
                        _readSample = Math.Min(newSample, _source.Samples);
                }
                else
                {
                    _readSample = Math.Min(_readSample + sampleDifference, _source.Samples);
                    //if (_readSample >= _source.Samples)
                    //    Stop();
                }
            }
            else
                _readSample += sampleDifference;
        }

        public virtual void Fill()
        {
            //This only works if a source has been assigned!
            if (_source == null)
                return;

            //Update read position
            Update();

            //Get number of samples available for writing. 
            int sampleCount = (((_readOffset <= _writeOffset) ? (_readOffset + _dataLength) : _readOffset) - _writeOffset) / _blockAlign / 8;

            //Fill samples
            Fill(_source, sampleCount, _loop);
        }
        public virtual void Fill(IAudioStream source, int samples, bool loop)
        {
            int byteCount = samples * _blockAlign;

            //Lock buffer and fill
            BufferData data = Lock(_writeOffset, byteCount);
            try { data.Fill(source, loop); }
            finally { Unlock(data); }

            //Advance offsets
            _writeOffset = (_writeOffset + byteCount) % _dataLength;
            _writeSample = source.SamplePosition;
        }
    }
}
