using System;
using System.Runtime.InteropServices;

namespace BrawlLib.LoopSelection
{
    internal static partial class Win32
    {
        internal static unsafe partial class DirectSound
        {
            public delegate bool DSEnumCallback(Guid* lpGuid, sbyte* lpcstrDescription, sbyte* lpcstrModule, IntPtr lpContext);

            public static readonly Guid DefaultPlaybackGuid = new Guid("DEF00000-9C6D-47ED-AAF1-4DDA8F2B5C03");
            public static readonly Guid DefaultVoicePlaybackGuid = new Guid("DEF00002-9C6D-47ED-AAF1-4DDA8F2B5C03");

            [DllImport("DSound.dll", PreserveSig = false)]
            public static extern void GetDeviceID(ref Guid pGuidSrc, out Guid pGuidDest);
            [DllImport("DSound.dll", PreserveSig = false)]
            public static extern void DirectSoundEnumerate([MarshalAs(UnmanagedType.FunctionPtr)] DSEnumCallback lpDSEnumCallback, IntPtr lpContext);
            [DllImport("DSound.dll", PreserveSig = false)]
            public static extern void DirectSoundCreate8(Guid* lpcGuidDevice, out IDirectSound8 ppDS8, IntPtr pUnkOuter);

            #region Structs
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct DSCaps
            {
                public uint dwSize;
                public DSCapsFlags dwFlags;
                public uint dwMinSecondarySampleRate;
                public uint dwMaxSecondarySampleRate;
                public uint dwPrimaryBuffers;
                public uint dwMaxHwMixingAllBuffers;
                public uint dwMaxHwMixingStaticBuffers;
                public uint dwMaxHwMixingStreamingBuffers;
                public uint dwFreeHwMixingAllBuffers;
                public uint dwFreeHwMixingStaticBuffers;
                public uint dwFreeHwMixingStreamingBuffers;
                public uint dwMaxHw3DAllBuffers;
                public uint dwMaxHw3DStaticBuffers;
                public uint dwMaxHw3DStreamingBuffers;
                public uint dwFreeHw3DAllBuffers;
                public uint dwFreeHw3DStaticBuffers;
                public uint dwFreeHw3DStreamingBuffers;
                public uint dwTotalHwMemBytes;
                public uint dwFreeHwMemBytes;
                public uint dwMaxContigFreeHwMemBytes;
                public uint dwUnlockTransferRateHwBuffers;
                public uint dwPlayCpuOverheadSwBuffers;
                public uint dwReserved1;
                public uint dwReserved2;
            }
            #endregion

            #region Enums
            [Flags]
            public enum DSCapsFlags : uint
            {
                PrimaryMono = 0x00000001,
                PrimaryStereo = 0x00000002,
                Primary8Bit = 0x00000004,
                Primary16Bit = 0x00000008,
                ContinuousRate = 0x00000010,
                EmulDriver = 0x00000020,
                Certified = 0x00000040,
                SecondaryMono = 0x00000100,
                SecondaryStereo = 0x00000200,
                Secondary8Bit = 0x00000400,
                Secondary16Bit = 0x00000800
            }

            public enum DSCooperativeLevel : uint
            {
                Normal = 0x00000001,
                Priority = 0x00000002,
                Exclusive = 0x00000003,
                WritePrimary = 0x00000004
            }

            [Flags]
            public enum DSSpeakerConfig : uint
            {
                DirectOut = 0x00000000,
                Headphone = 0x00000001,
                Mono = 0x00000002,
                Quad = 0x00000003,
                Stereo = 0x00000004,
                Surround = 0x00000005,
                Back5Point1 = 0x00000006,
                Wide7Point1 = 0x00000007,
                Surround7Point1 = 0x00000008,
                Surround5Point1 = 0x00000009,

                StereoGeometryMin = 0x00050000,  //   5 degrees
                GeometryNarrow = 0x000A0000,  //  10 degrees
                GeometryWide = 0x00140000,  //  20 degrees
                GeometryMax = 0x00B40000  // 180 degrees
            }
            #endregion

            #region Interfaces

            [Guid("C50A7E93-F395-4834-9EF6-7FA99DE50966"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public unsafe interface IDirectSound8
            {
                void CreateSoundBuffer(ref DSBufferDesc pcDSBufferDesc, out IDirectSoundBuffer8 ppDSBuffer, IntPtr pUnkOuter);
                void GetCaps(ref DSCaps pDSCaps);
                void DuplicateSoundBuffer(IDirectSoundBuffer8 pDSBufferOriginal, out IDirectSoundBuffer8 ppDSBufferDuplicate);
                void SetCooperativeLevel(IntPtr hwnd, DSCooperativeLevel dwLevel);
                void Compact();
                void GetSpeakerConfig(out DSSpeakerConfig pdwSpeakerConfig);
                void SetSpeakerConfig(DSSpeakerConfig dwSpeakerConfig);
                void Initialize(ref Guid pcGuidDevice);
                void VerifyCertification(out uint pdwCertified);
            }

            #endregion
        }
    }
}
