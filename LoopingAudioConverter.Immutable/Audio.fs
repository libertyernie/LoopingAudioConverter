namespace LoopingAudioConverter.Immutable

type Audio = {
    Channels: int
    SampleRate: int
    Samples: int16[]
} with
    member this.SamplesPerChannel = Array.length this.Samples / this.Channels
