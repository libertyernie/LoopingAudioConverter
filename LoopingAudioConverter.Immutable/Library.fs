namespace LoopingAudioConverter.Immutable

type LoopType =
| Looping of start: int * stop: int
| NonLooping
with
    member this.LoopStart = match this with Looping (a, _) -> a | NonLooping -> failwith "Cannot get loop point of a non-looping file"
    member this.LoopEnd = match this with Looping (_, b) -> b | NonLooping -> failwith "Cannot get loop point of a non-looping file"

type PCMData = {
    channels: int
    sample_rate: int
    samples: int16[]
} with
    member this.SamplesPerChannel = Array.length this.samples / this.channels
