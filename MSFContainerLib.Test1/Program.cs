using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MSFContainerLib.Test1
{
    class DemoAudioSource : IPcmAudioSource<short>
    {
        public IEnumerable<short> SampleData => new short[]
        {
            0x1234,
            0x5678,
            0x0000,
            0x1111,
            0x2222,
            0x3333,
            0x4444,
            0x5555,
            0x6666,
            0x7777,
        };

        public int Channels => 2;

        public int SampleRate => 44100;

        public bool IsLooping => true;
        public int LoopStartSample => 2;
        public int LoopSampleCount => 10;
    }

    class Program
    {
        static unsafe void Main(string[] args)
        {
            var md5 = MD5.Create();

            short[] test = new[] { (short)0x1234 };
            fixed (short* ptr = test)
            {
                if (*(BigEndianInt16*)ptr == 0x1234)
                {
                    Console.WriteLine("Your endian: big");
                }
                else if (*(LittleEndianInt16*)ptr == 0x1234)
                {
                    Console.WriteLine("Your endian: little");
                }
            }

            Console.WriteLine("PCM test");
            {
                MSF msf_le = MSF.FromAudioSource(new DemoAudioSource(), big_endian: false);
                byte[] data_le = msf_le.Export();
                byte[] hash_le = md5.ComputeHash(data_le);
                Console.WriteLine($"Little endian: new byte[] {{ {string.Join(",", hash_le.Select(x => "0x" + ((int)x).ToString("X2")))} }}");
                if (hash_le.SequenceEqual(new byte[] { 0x26, 0x81, 0x01, 0xBF, 0x82, 0x4E, 0x92, 0xF6, 0x4E, 0xC8, 0xD3, 0xD3, 0x6A, 0xE4, 0x36, 0x57 }))
                    Console.WriteLine("    OK");
                else
                    Console.WriteLine("    error");
            }
            {
                MSF msf_be = MSF.FromAudioSource(new DemoAudioSource(), big_endian: true);
                byte[] data_be = msf_be.Export();
                byte[] hash_be = md5.ComputeHash(data_be);
                Console.WriteLine($"Big endian: new byte[] {{ {string.Join(",", hash_be.Select(x => "0x" + ((int)x).ToString("X2")))} }}");
                if (hash_be.SequenceEqual(new byte[] { 0x1A, 0x47, 0xEA, 0xA0, 0xFB, 0xFB, 0xFF, 0x4B, 0xF7, 0x84, 0x04, 0x46, 0x0B, 0xDC, 0x22, 0x35 }))
                    Console.WriteLine("    OK");
                else
                    Console.WriteLine("    error");
            }
            Console.WriteLine();

            Console.WriteLine("MP3 test");
            // Get an MP3 MSF for testing
            byte[] mp3data = File.ReadAllBytes("stage_Sonic.msf");
            var h = MSFHeader.Create();
            h.channel_count = 2;
            h.codec = 7;
            h.data_size = mp3data.Length;
            h.flags = new MSFFlags
            {
                Flags = MSFFlag.MP3JointStereo | MSFFlag.Resample
            };
            h.sample_rate = 44100;
            var msf = new MSF_MP3(h, mp3data);
            // Check MD5
            {
                byte[] hash = md5.ComputeHash(msf.Export());
                Console.WriteLine($"Checksum for sample MP3 MSF: new byte[] {{ {string.Join(",", hash.Select(x => "0x" + ((int)x).ToString("X2")))} }}");
                if (hash.SequenceEqual(new byte[] { 0x35, 0xB0, 0x90, 0x9D, 0x1A, 0x9A, 0xD3, 0x6C, 0x60, 0xCF, 0x03, 0x86, 0x7D, 0xB4, 0xB9, 0x03 }))
                    Console.WriteLine("    OK");
                else
                    Console.WriteLine("    error");
            }
            // Decode by converting to a PCM16 MSF
            try
            {
                var pcm_version = MSF.FromAudioSource(msf);
                // Check MD5
                byte[] hash = md5.ComputeHash(pcm_version.Export());
                Console.WriteLine($"Checksum of decoded PCM data: new byte[] {{ {string.Join(",", hash.Select(x => "0x" + ((int)x).ToString("X2")))} }}");
                if (hash.SequenceEqual(new byte[] { 0xD9, 0xD5, 0x11, 0x98, 0x69, 0xB3, 0xFA, 0x46, 0xD9, 0x71, 0x7B, 0xBF, 0x94, 0x64, 0x93, 0xE5 }))
                    Console.WriteLine("    OK");
                else
                    Console.WriteLine("    error");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine($"Checksum of decoded PCM data: N/A (not supported)");
            }
        }
    }
}
