using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
    public class WaveDataException : Exception {
        public WaveDataException(string message) : base(message) { }
    }

    public class WaveData {
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        private struct fmt {
            public int id;
            public int size;
            public short format;
            public short channels;
            public int sampleRate;
            public int byteRate;
            public short blockAlign;
            public short bitsPerSample;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct smpl {
            public int id;
			public int size;
			public uint manufacturer;
			public uint product;
			public uint samplePeriod;
			public uint midiUnityNote;
			public uint midiPitchFraction;
			public uint smpteFormat;
			public sbyte smpteOffsetHours;
			public byte smpteOffsetMinutes;
			public byte smpteOffsetSeconds;
			public byte smpteOffsetFrames;
			public uint sampleLoopCount;
			public uint samplerDataCount;
		}
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct smpl_loop {
			public uint loopID;
			public int type;
			public int start;
			public int end;
			public uint fraction;
			public int playCount;
        }

        private static unsafe int tag(string s) {
            if (s.Length != 4) throw new ArgumentException("String must be 4 characters long");
            int i = 0;
            byte* ptr = (byte*)&i;
            ptr[0] = (byte)s[0];
            ptr[1] = (byte)s[1];
            ptr[2] = (byte)s[2];
            ptr[3] = (byte)s[3];
            return i;
        }

        public unsafe static WaveData FromStream(Stream stream) {
            int length;

            byte[] buffer = new byte[8];
            if (stream.Read(buffer, 0, 8) < 8) {
                throw new WaveDataException("Unexpected end of stream in first 8 bytes");
            }
            fixed (byte* bptr = buffer) {
                if (*(int*)bptr != tag("RIFF")) {
                    throw new WaveDataException("RIFF header not found");
                }
                length = *(int*)(bptr + 4);
            }

            byte[] buffer2 = new byte[length + 8];
            Array.Copy(buffer, buffer2, 8);

            int total = 0;
            while (total < length) {
                int r = stream.Read(buffer2, 8 + total, length - total);
                if (r == 0) throw new WaveDataException("Unexpected end of stream when trying to read " + length + " bytes");
                total += r;
            }

            return FromByteArray(buffer2);
        }

        public unsafe static WaveData FromByteArray(byte[] data) {
            int channels = 0;
            int sampleRate = 0;
            short[] samples = null;

            bool looping = false;
            int loopStart = 0;
            int loopEnd = 0;

            fixed (byte* bptr = data) {
                // Verify RIFF format
                if (*(int*)bptr != tag("RIFF")) {
                    throw new WaveDataException("RIFF header not found");
                }

                // Verify WAVE format
                if (*(int*)(bptr + 8) != tag("WAVE")) {
                    throw new WaveDataException("WAVE header not found");
                }

                // Look for chunks until end of byte array
                byte* end = bptr + data.Length;
                for (byte* ptr = bptr + 12; ptr < end; ptr += 8 + *(int*)(ptr + 4)) {
                    // Four ASCII characters - stored here as int32
                    int id = *(int*)ptr;
                    if (id == tag("fmt ")) {
                        // Format chunk
                        fmt* fmt = (fmt*)ptr;
                        if (fmt->format != 1) {
                            throw new WaveDataException("Only uncompressed wave files suppported");
                        } else if (fmt->bitsPerSample != 16) {
                            throw new WaveDataException("Only 16-bit wave files supported");
                        }

                        channels = fmt->channels;
                        sampleRate = fmt->sampleRate;
                    } else if (id == tag("data")) {
                        // Data chunk - contains samples
                        int bytelen = *((int*)(ptr + 4));
                        samples = new short[bytelen / 2];
                        Marshal.Copy((IntPtr)(ptr + 8), samples, 0, samples.Length);
                    } else if (id == tag("smpl")) {
                        // sampler chunk
                        smpl* smpl = (smpl*)ptr;
                        if (smpl->sampleLoopCount > 1) {
                            throw new WaveDataException("Cannot read looping .wav file with more than one loop");
                        } else if (smpl->sampleLoopCount == 1) {
                            // There is one loop - we only care about start and end points
                            smpl_loop* loop = (smpl_loop*)(smpl + 1);
                            if (loop->type != 0) {
                                throw new WaveDataException("Cannot read looping .wav file with loop of type " + loop->type);
                            }
                            looping = true;
                            loopStart = loop->start;
                            loopEnd = loop->end;
                        }
                    } else {
                        Console.Error.WriteLine("Ignoring unknown chunk " + id);
                    }
                }
            }

            if (sampleRate == 0) {
                throw new WaveDataException("Format chunk not found");
            }
            if (samples == null) {
                throw new WaveDataException("Data chunk not found");
            }

            WaveData wav = new WaveData(channels, sampleRate, samples);
            if (looping) {
                wav.Looping = true;
                wav.LoopStart = loopStart;
                wav.LoopEnd = loopEnd;
            }
            return wav;
        }

        public short Channels;
        public int SampleRate;
        private short[] Samples;

        public short this[int v] {
            get {
                return Samples[v];
            }
        }

        public bool Looping;
        public int LoopStart;
        public int LoopEnd;

        /// <summary>
        /// Creates a WAV with the given metadata and audio data.
        /// </summary>
        /// <param name="channels">Number of channels</param>
        /// <param name="sampleRate">Sample rate</param>
        /// <param name="samples">Audio data (array will not be modified)</param>
        public WaveData(int channels, int sampleRate, short[] samples) {
            if (channels > short.MaxValue) throw new ArgumentException("Streams of more than " + short.MaxValue + " channels not supported");
            if (channels <= 0) throw new ArgumentException("Number of channels must be a positive integer");
            if (sampleRate <= 0) throw new ArgumentException("Sample rate must be a positive integer");

            this.Channels = (short)channels;
            this.SampleRate = sampleRate;

            this.Samples = new short[samples.Length];
            Array.Copy(samples, this.Samples, samples.Length);

            Looping = false;
            LoopStart = 0;
            LoopEnd = Samples.Length;
        }

        public WaveData Amplify(int dB) {
            double ratio = Math.Pow(10.0, dB / 20.0);

            // Replace samples
            short[] nSamples = new short[Samples.Length];
            for (int i = 0; i < nSamples.Length; i++) {
                double nSample = Math.Round(this.Samples[i] * ratio);
                nSamples[i] =
                    nSample > short.MaxValue ? short.MaxValue
                    : nSample < short.MinValue ? short.MinValue
                    : (short)nSample;
            }

            WaveData nWav = new WaveData(this.Channels, this.SampleRate, nSamples);
            nWav.Looping = this.Looping;
            nWav.LoopStart = this.LoopStart;
            nWav.LoopEnd = this.LoopEnd;
            return nWav;
        }

        public unsafe byte[] Export() {
            int length = 12 + sizeof(fmt) + 8 + (Samples.Length * 2);
            if (Looping) {
                length += sizeof(smpl) + sizeof(smpl_loop);
            }
            byte[] data = new byte[length];
            fixed (byte* start = data) {
                byte* ptr = start;
                *(int*)ptr = tag("RIFF");
                ptr += 4;
                *(int*)ptr = length - 8;
                ptr += 4;
                *(int*)ptr = tag("WAVE");
                ptr += 4;

                fmt* fmt = (fmt*)ptr;
                fmt->id = tag("fmt ");
                fmt->size = sizeof(fmt) - 8;
                fmt->format = 1;
                fmt->channels = Channels;
                fmt->sampleRate = SampleRate;
                fmt->byteRate = SampleRate * Channels * 2;
                fmt->blockAlign = (short)(Channels * 2);
                fmt->bitsPerSample = 16;
                ptr += sizeof(fmt);

                *(int*)ptr = tag("data");
                ptr += 4;
                *(int*)ptr = Samples.Length * 2;
                ptr += 4;

                Marshal.Copy(Samples, 0, (IntPtr)ptr, Samples.Length);
                ptr += Samples.Length * 2;

                if (Looping) {
                    smpl* smpl = (smpl*)ptr;
                    smpl->id = tag("smpl");
                    smpl->size = sizeof(smpl) + sizeof(smpl_loop) - 8;
                    smpl->sampleLoopCount = 1;
                    ptr += sizeof(smpl);

                    smpl_loop* loop = (smpl_loop*)ptr;
                    loop->loopID = 0;
                    loop->type = 0;
                    loop->start = LoopStart;
                    loop->end = LoopEnd;
                    loop->fraction = 0;
                    loop->playCount = 0;
                    ptr += sizeof(smpl_loop);
                }
                return data;
            }
        }

        public override string ToString() {
            return SampleRate + "Hz " + Channels + " channels: " + Samples.Length + " (" + TimeSpan.FromSeconds(Samples.Length / (SampleRate * Channels)) + ")"
                + (Looping ? (" loop " + LoopStart + "-" + LoopEnd) : "");
        }
    }
}
