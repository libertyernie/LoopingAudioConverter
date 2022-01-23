namespace LoopingAudioConverter

open System
open System.IO
open System.Text
open System.Threading.Tasks
open LoopingAudioConverter.Immutable
open LoopingAudioConverter.PCM

module MSU1 =
    let Importer = {
        new IAudioImporter with
            member __.SupportsExtension ext = ext.Equals (".pcm", StringComparison.InvariantCultureIgnoreCase)
            member __.SharesCodecsWith _ = false
            member __.ReadFileAsync (filename, _, _) =
                use fs = new FileStream (filename, FileMode.Open, FileAccess.Read)
                use br = new BinaryReader (fs)
                for c in "MSU1" do
                    let x = br.ReadByte()
                    if char x <> c then raise (AudioImporterException "This is not a valid MSU-1 .pcm file")
                let loopStart = br.ReadUInt32 ()
                let sampleData = [| while fs.Position < fs.Length do br.ReadInt16 () |]
                let audioData = {
                    channels = 2
                    sample_rate = 44100
                    samples = sampleData
                }
                let loop =
                    if loopStart = 0u
                    then NonLooping
                    else Looping (int loopStart, sampleData.Length / 2)
                Task.FromResult (new PCM16Audio (audioData, loop))
    }

    let Exporter = {
        new IAudioExporter with
            member __.WriteFileAsync (lwav, output_dir, original_filename_no_ext, progress) =
                if lwav.Channels <> 2 || lwav.SampleRate <> 44100 then
                    raise (AudioExporterException "MSU-1 output must be 2-channel audio at a sample rate of 44100Hz.")
                let (loopStart, loopEnd) =
                    match lwav.Loop with
                    | Looping (start, stop) -> (start, stop)
                    | NonLooping -> (0, Int32.MaxValue)
                let samples = Array.truncate (loopEnd * 2) lwav.Samples
                let data = [|
                    yield! Encoding.ASCII.GetBytes "MSU1"
                    yield! BitConverter.GetBytes loopStart
                    for sample in samples do
                        yield! BitConverter.GetBytes sample
                |]
                let output_filename = Path.Combine (output_dir, $"{original_filename_no_ext}.pcm")
                File.WriteAllBytes (output_filename, data)
                Task.CompletedTask
    }