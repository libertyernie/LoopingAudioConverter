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
        }
    }
}
