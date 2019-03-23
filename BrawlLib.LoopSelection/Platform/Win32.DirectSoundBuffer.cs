using System.Runtime.InteropServices;
using System;

namespace BrawlLib.LoopSelection
{
    static partial class Win32
    {
        internal static partial class DirectSound
        {
            #region Structs

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            internal unsafe struct DSBufferDesc
            {
                public const uint Size = 36;

                public uint dwSize;
                public DSBufferCapsFlags dwFlags;
                public uint dwBufferBytes;
                public uint dwReserved;
                public WaveFormatEx* lpwfxFormat;
                public Guid guid3DAlgorithm;

                public DSBufferDesc(uint bufferSize, DSBufferCapsFlags bufferCaps) : this(bufferSize, bufferCaps, null, Guid.Empty) { }
                public DSBufferDesc(uint bufferSize, DSBufferCapsFlags bufferCaps, WaveFormatEx* format, Guid algorithm)
                {
                    dwSize = Size;
                    dwFlags = bufferCaps;
                    dwBufferBytes = bufferSize;
                    dwReserved = 0;
                    lpwfxFormat = format;
                    guid3DAlgorithm = algorithm;
                }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            internal struct DSBufferCaps
            {
                public const uint Size = 20;

                public uint dwSize;
                public DSBufferCapsFlags dwFlags;
                public uint dwBufferBytes;
                public uint dwUnlockTransferRate;
                public uint dwPlayCpuOverhead;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            internal struct DSEffectDesc
            {
                public uint dwSize;
                public DSEffectDescFlags dwFlags;
                public Guid guidDSFXClass;
                public IntPtr dwReserved1;
                public IntPtr dwReserved2;
            }
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            internal struct DSBufferPositionNotify
            {
                public uint dwOffset;
                public IntPtr hEventNotify;
            }

            #endregion

            #region Flags

            [Flags]
            internal enum DSBufferCapsFlags : uint
            {
                PrimaryBuffer = 0x00000001,
                Static = 0x00000002,
                LocHardware = 0x00000004,
                LocSoftware = 0x00000008,
                Ctrl3D = 0x00000010,
                CtrlFrequency = 0x00000020,
                CtrlPan = 0x00000040,
                CtrlVolume = 0x00000080,
                CtrlPositionNotify = 0x00000100,
                CtrlFX = 0x00000200,
                StickyFocus = 0x00004000,
                GlobalFocus = 0x00008000,
                GetCurrentPosition2 = 0x00010000,
                Mute3dAtMaxDistance = 0x00020000,
                LocDefer = 0x00040000,
                TruePlayPosition = 0x00080000
            }

            [Flags]
            internal enum DSBufferStatus : uint
            {
                Playing = 0x00000001,
                BufferLost = 0x00000002,
                Looping = 0x00000004,
                LocHardware = 0x00000008,
                LocSoftware = 0x00000010,
                Terminated = 0x00000020
            }

            [Flags]
            internal enum DSBufferPlayFlags : uint
            {
                None = 0,
                Looping = 0x00000001,
                LocHardware = 0x00000002,
                LocSoftware = 0x00000004,
                TerminateByTime = 0x00000008,
                TerminateByDistance = 0x000000010,
                TerminateByPriority = 0x000000020
            }

            [Flags]
            internal enum DSLockFlags : uint
            {
                None = 0,
                FromWriteCursor = 0x00000001,
                EntireBuffer = 0x00000002
            }

            internal enum DSEffectDescFlags : uint
            {
                None = 0x0000000,
                LocHardware = 0x00000001,
                LocSoftware = 0x00000002
            }

            internal enum DSEffectResult : uint
            {
                Present = 0,
                LocHardware = 1,
                LocSoftware = 2,
                Unallocated = 3,
                Failed = 4,
                Unknown = 5,
                SendLoop = 6
            }

            #endregion

            #region COM Interfaces
            [Guid("279AFA85-4981-11CE-A521-0020AF0BE560"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            internal unsafe interface IDirectSoundBuffer8
            {
                void GetCaps(ref DSBufferCaps pDSBufferCaps);
                void GetCurrentPosition(uint* pdwCurrentPlayCursor, uint* pdwCurrentWriteCursor);
                void GetFormat(WaveFormatEx* pwfxFormat, uint dwSizeAllocated, uint* pdwSizeWritten);
                void GetVolume(out int plVolume);
                void GetPan(out int plPan);
                void GetFrequency(out uint pdwFrequency);
                void GetStatus(out DSBufferStatus pdwStatus);
                void Initialize(IDirectSound8 pDirectSound, ref DSBufferDesc pcDSBufferDesc);
                void Lock(uint dwOffset, uint dwBytes, out IntPtr ppvAudioPtr1, out uint pdwAudioBytes1, out IntPtr ppvAudioPtr2, out uint pdwAudioBytes2, DSLockFlags dwFlags);
                void Play(uint dwReserved1, uint dwPriority, DSBufferPlayFlags dwFlags);
                void SetCurrentPosition(uint dwNewPosition);
                void SetFormat(WaveFormatEx* pcfxFormat);
                void SetVolume(int lVolume);
                void SetPan(int lPan);
                void SetFrequency(uint dwFrequency);
                void Stop();
                void Unlock(IntPtr pvAudioPtr1, uint dwAudioBytes1, IntPtr pvAudioPtr2, uint dwAudioBytes2);
                void Restore();
                void SetFX(uint dwEffectsCount, [MarshalAs(UnmanagedType.LPArray)] DSEffectDesc[] pDSFXDesc, [MarshalAs(UnmanagedType.LPArray)] DSEffectResult[] pdwResultCodes);
                void AcquireResources(DSBufferPlayFlags dwFlags, uint dwEffectsCount, [MarshalAs(UnmanagedType.LPArray)] DSEffectResult[] pdwResultCodes);
                void GetObjectInPath(ref Guid rguidObject, uint dwIndex, ref Guid rguidInterface, out IntPtr ppObject);
            }

            [Guid("B0210783-89CD-11D0-AF08-00A0C925CD16"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            internal unsafe interface IDirectSound8Notify
            {
                void SetNotificationPositions(uint dwPositionNotifies, [MarshalAs(UnmanagedType.LPArray)] DSBufferPositionNotify[] pcPositionNotifies);
            }
            #endregion
        }
    }
}
