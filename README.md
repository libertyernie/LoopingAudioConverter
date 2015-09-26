# LoopingAudioConverter

This application acts as a frontend to other programs and libraries, and allows conversion between the Wii .brstm format and a variety of other formats.

Supported input formats:

    WAV (with or without "smpl" chunk to denote looping audio)
    MP3, using madplay
    VGM/VGZ, using vgm2wav
    Any audio format supported by SoX
    Any audio format supported by vgmstream

Supported output formats:

    WAV (with "smpl" chunk if the audio should loop; vgmstream can read these loops)
    MP3, using lame
    FLAC or Ogg Vorbis, using SoX
    RSTM (.brstm), using RSTMLib, a subset of BrawlLib

See LoopingAudioConverter/About.html for more information.
