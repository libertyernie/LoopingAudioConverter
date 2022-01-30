namespace LoopingAudioConverter.Immutable

type Loop = Loop of LoopStart: int * LoopEnd: int | NonLooping
with
    member this.Multiply ratio =
        match this with
        | Loop (s, e) -> Loop ((ratio * double s) |> round |> int, (ratio * double e) |> round |> int)
        | NonLooping -> NonLooping