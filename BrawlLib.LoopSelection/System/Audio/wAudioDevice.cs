using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BrawlLib.LoopSelection
{
    unsafe class wAudioDevice : AudioDevice
    {
        internal Guid _guid;

        private wAudioDevice() { }
        private wAudioDevice(Guid guid, string desc, string driver)
        {
            _guid = guid;
            _description = desc;
            _driver = driver;
        }

        internal static AudioDevice[] PlaybackDevices
        {
            get
            {
                List<AudioDevice> list = new List<AudioDevice>();
                GCHandle handle = GCHandle.Alloc(list);
                try { Win32.DirectSound.DirectSoundEnumerate(EnumCallback, (IntPtr)handle); }
                finally { handle.Free(); }
                return list.ToArray();
            }
        }
        internal static AudioDevice DefaultPlaybackDevice
        {
            get
            {
                Guid g1 = Win32.DirectSound.DefaultPlaybackGuid, g2;
                Win32.DirectSound.GetDeviceID(ref g1, out g2);
                wAudioDevice dev = new wAudioDevice() { _guid = g2 };

                GCHandle handle = GCHandle.Alloc(dev);
                try { Win32.DirectSound.DirectSoundEnumerate(EnumCallback, (IntPtr)handle); }
                finally { handle.Free(); }
                return dev;
            }
        }
        public static AudioDevice DefaultVoicePlaybackDevice
        {
            get
            {
                Guid g1 = Win32.DirectSound.DefaultVoicePlaybackGuid, g2;
                Win32.DirectSound.GetDeviceID(ref g1, out g2);
                wAudioDevice dev = new wAudioDevice() { _guid = g2 };

                GCHandle handle = GCHandle.Alloc(dev);
                try { Win32.DirectSound.DirectSoundEnumerate(EnumCallback, (IntPtr)handle); }
                finally { handle.Free(); }
                return dev;
            }
        }

        private static bool EnumCallback(Guid* guid, sbyte* desc, sbyte* module, IntPtr context)
        {
            if (guid == null)
                return true;

            object ctx = ((GCHandle)context).Target;
            if (ctx is List<AudioDevice>)
            {
                ((List<AudioDevice>)ctx).Add(new wAudioDevice(*guid, new String(desc), new String(module)));
                return true;
            }
            else if (ctx is wAudioDevice)
            {
                wAudioDevice dev = ctx as wAudioDevice;
                if (*guid == dev._guid)
                {
                    dev._description = new String(desc);
                    dev._driver = new String(module);
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
