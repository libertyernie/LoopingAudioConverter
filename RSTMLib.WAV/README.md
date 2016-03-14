# RSTMLib.WAV

RSTMLib is a subset of BrawlLib that contains only the code necessary to
encode RSTM and CSTM streams, and convert between the two. This library, which
is used by LoopingAudioConverter, contains code for reading, writing, and
representing WAV files with loop information, as well as an implementation
of BrawlLib's IAudioStream that wraps around WAV data.

This library does not decode RSTM or CSTM streams; it's recommended to use
vgmstream for that.
