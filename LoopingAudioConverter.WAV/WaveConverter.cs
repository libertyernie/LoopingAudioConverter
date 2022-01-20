﻿using LoopingAudioConverter.Immutable;
using LoopingAudioConverter.PCM;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LoopingAudioConverter.WAV {
	public class WaveConverterException : Exception {
		public WaveConverterException(string message) : base(message) { }
	}

	/// <summary>
	/// Convert between RIFF WAVE format (the .wav files used by Microsoft) and this application's internal PCM16Audio class.
	/// Input data must have 16 bits per sample and be in one of the following formats:
	/// * WAVE_FORMAT_PCM (0x0001)
	/// * WAVE_FORMAT_EXTENSIBLE (0xFFFE) with a subformat of KSDATAFORMAT_SUBTYPE_PCM (00000001-0000-0010-8000-00aa00389b71)
	/// This program will read the "fmt ", "data", and (if present) "smpl" chunks; any other chunks in the file will be ignored.
	/// Output data will use WAVE_FORMAT_PCM and have "fmt " and "data" chunks, along with a "smpl" chunk if it is a looping track.
	/// </summary>
	public static class WaveConverter {
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct fmt {
			public ushort format;
			public short channels;
			public int sampleRate;
			public int byteRate;
			public short blockAlign;
			public short bitsPerSample;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct fmt_extensible {
			[FieldOffset(0)]
			public fmt fmt;
			[FieldOffset(16)]
			public short cbSize;
			[FieldOffset(18)]
			public short validBitsPerSample;
			[FieldOffset(18)]
			public short samplesPerBlock;
			[FieldOffset(18)]
			public short reserved;
			[FieldOffset(20)]
			public int channelMask;
			[FieldOffset(24)]
			public Guid subFormat;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct smpl {
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

		/// <summary>
		/// Reads RIFF WAVE data from a stream.
		/// If the size of the "data" chunk is incorrect or negative, but the "data" chunk is known to be the last chunk in the file, set the assumeDataIsLastChunk parameter to true.
		/// </summary>
		/// <param name="stream">Stream to read from (no data will be written to the stream)</param>
		/// <returns></returns>
		public unsafe static PCM16Audio FromStream(Stream stream) {
			byte[] buffer = new byte[12];
			int r = stream.Read(buffer, 0, 12);
			if (r == 0) {
				throw new WaveConverterException("No data in stream");
			} else if (r < 12) {
				throw new WaveConverterException("Unexpected end of stream in first 12 bytes");
			}
			fixed (byte* bptr = buffer) {
				if (*(int*)bptr != tag("RIFF")) {
					throw new WaveConverterException("RIFF header not found");
				}
				if (*(int*)(bptr + 8) != tag("WAVE")) {
					throw new WaveConverterException("WAVE header not found");
				}
			}

			int channels = 0;
			int sampleRate = 0;

			short[] sample_data = null;
			bool convert_from_8_bit = false;

			int? loopStart = null;
			int? loopEnd = null;

			// Keep reading chunk headers into a buffer of 8 bytes
			while ((r = stream.Read(buffer, 0, 8)) > 0) {
				if (r < 8) {
					throw new WaveConverterException("Unexpected end of stream in chunk header");
				} else {
					fixed (byte* ptr1 = buffer) {
						// Four ASCII characters
						string id = Marshal.PtrToStringAnsi((IntPtr)ptr1, 4);

						int chunklength = *(int*)(ptr1 + 4);

						byte[] buffer2;
						if (id == "data" && chunklength == -1) {
							// Chunk length is not specified properly in file - assume the chunk lasts until the end of the file.
							using (MemoryStream ms = new MemoryStream()) {
								stream.CopyTo(ms);
								buffer2 = ms.ToArray();
							}
						} else {
							// Look at the length of the chunk and read that many bytes into a byte array.
							buffer2 = new byte[chunklength];
							int total = 0;
							while (total < buffer2.Length) {
								total += (r = stream.Read(buffer2, total, buffer2.Length - total));
								if (r == 0) throw new WaveConverterException("Unexpected end of data in \"" + Marshal.PtrToStringAnsi((IntPtr)ptr1, 4) + "\" chunk: expected " + buffer2.Length + " bytes, got " + total + " bytes");
							}
						}

						fixed (byte* ptr2 = buffer2) {
							if (id == "fmt ") {
								// Format chunk
								fmt* fmt = (fmt*)ptr2;
								if (fmt->format != 1) {
									if (fmt->format == 65534) {
										// WAVEFORMATEXTENSIBLE
										fmt_extensible* ext = (fmt_extensible*)fmt;
										if (ext->subFormat == new Guid("00000001-0000-0010-8000-00aa00389b71")) {
											// KSDATAFORMAT_SUBTYPE_PCM
										} else {
											throw new WaveConverterException("Only uncompressed PCM suppported - found WAVEFORMATEXTENSIBLE with subformat " + ext->subFormat);
										}
									} else {
										throw new WaveConverterException("Only uncompressed PCM suppported - found format " + fmt->format);
									}
								} else if (fmt->bitsPerSample != 16) {
									if (fmt->bitsPerSample == 8) {
										convert_from_8_bit = true;
									} else {
										throw new WaveConverterException("Only 16-bit wave files supported");
									}
								}

								channels = fmt->channels;
								sampleRate = fmt->sampleRate;
							} else if (id == "data") {
								// Data chunk - contains samples
								sample_data = new short[buffer2.Length / 2];
								Marshal.Copy((IntPtr)ptr2, sample_data, 0, sample_data.Length);
							} else if (id == "smpl") {
								// sampler chunk
								smpl* smpl = (smpl*)ptr2;
								if (smpl->sampleLoopCount > 1) {
									throw new WaveConverterException("Cannot read looping .wav file with more than one loop");
								} else if (smpl->sampleLoopCount == 1) {
									// There is one loop - we only care about start and end points
									smpl_loop* loop = (smpl_loop*)(smpl + 1);
									if (loop->type != 0) {
										throw new WaveConverterException("Cannot read looping .wav file with loop of type " + loop->type);
									}
									loopStart = loop->start;
									loopEnd = loop->end;
								}
							} else {
								Console.Error.WriteLine("Ignoring unknown chunk " + id);
							}
						}
					}
				}
			}

			if (sampleRate == 0) {
				throw new WaveConverterException("Format chunk not found");
			}
			if (sample_data == null) {
				throw new WaveConverterException("Data chunk not found");
			}

			if (convert_from_8_bit) {
				short[] new_sample_data = new short[sample_data.Length * 2];
				fixed (short* short_ptr = sample_data) {
					byte* ptr = (byte*)short_ptr;
					for (int i = 0; i < new_sample_data.Length; i++) {
						new_sample_data[i] = (short)((ptr[i] - 0x80) << 8);
					}
				}
				sample_data = new_sample_data;
			}

			PCM16Audio wav = new PCM16Audio(
				new PCMData(channels, sampleRate, sample_data),
				loopStart is int s && loopEnd is int e ? LoopType.NewLooping(s, e) : LoopType.NonLooping);
			return wav;
		}

		/// <summary>
		/// Reads RIFF WAVE data from a byte array.
		/// This method wraps a read-only MemoryStream around the byte array and sends the stream to the FromStream method.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static PCM16Audio FromByteArray(byte[] p) {
			using (MemoryStream stream = new MemoryStream(p, false)) {
				return FromStream(stream);
			}
		}

		/// <summary>
		/// Reads RIFF WAVE data from a file.
		/// This method opens a read-only FileStream and sends the stream to the FromStream method.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="delete">Whether to try to delete the file afterwards (no exception will be thrown if unsuccessful)</param>
		/// <returns></returns>
		public static PCM16Audio FromFile(string filename, bool delete) {
			PCM16Audio w;
			using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
				w = FromStream(stream);
			}
			try {
				if (delete) File.Delete(filename);
			} catch { }
			return w;
		}

		/// <summary>
		/// Exports data to a byte array in RIFF WAVE (.wav) format.
		/// Output data will use WAVE_FORMAT_PCM and have "fmt " and "data" chunks, along with a "smpl" chunk if it is a looping track.
		/// Note that even files with more than 2 channels will use WAVE_FORMAT_PCM as the format, even though doing this is invalid according to the spec.
		/// </summary>
		/// <param name="lwav"></param>
		/// <returns></returns>
		public static unsafe byte[] Export(this PCM16Audio lwav) {
			int length = 12 + 8 + sizeof(fmt) + 8 + (lwav.Samples.Length * 2);
			if (lwav.Looping) {
				length += 8 + sizeof(smpl) + sizeof(smpl_loop);
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

				*(int*)ptr = tag("fmt ");
				ptr += 4;
				*(int*)ptr = sizeof(fmt);
				ptr += 4;

				fmt* fmt = (fmt*)ptr;
				fmt->format = 1;
				fmt->channels = checked((short)lwav.Channels);
				fmt->sampleRate = lwav.SampleRate;
				fmt->byteRate = lwav.SampleRate * lwav.Channels * 2;
				fmt->blockAlign = (short)(lwav.Channels * 2);
				fmt->bitsPerSample = 16;
				ptr += sizeof(fmt);

				*(int*)ptr = tag("data");
				ptr += 4;
				*(int*)ptr = lwav.Samples.Length * 2;
				ptr += 4;

				Marshal.Copy(lwav.Samples, 0, (IntPtr)ptr, lwav.Samples.Length);
				ptr += lwav.Samples.Length * 2;

				if (lwav.Looping) {
					*(int*)ptr = tag("smpl");
					ptr += 4;
					*(int*)ptr = sizeof(smpl) + sizeof(smpl_loop);
					ptr += 4;

					smpl* smpl = (smpl*)ptr;
					smpl->sampleLoopCount = 1;
					ptr += sizeof(smpl);

					smpl_loop* loop = (smpl_loop*)ptr;
					loop->loopID = 0;
					loop->type = 0;
					loop->start = lwav.LoopStart;
					loop->end = lwav.LoopEnd;
					loop->fraction = 0;
					loop->playCount = 0;
					ptr += sizeof(smpl_loop);
				}
				return data;
			}
		}
	}
}
