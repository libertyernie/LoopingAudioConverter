using BrawlLib.SSBB.Types.Audio;
using LoopingAudioConverter.BrawlLib;
using LoopingAudioConverter.FFmpeg;
using LoopingAudioConverter.MP3;
using LoopingAudioConverter.MSF;
using LoopingAudioConverter.MSU1;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.QuickTime;
using LoopingAudioConverter.VGAudio;
using LoopingAudioConverter.VGM;
using LoopingAudioConverter.VGMStream;
using LoopingAudioConverter.Vorbis;
using LoopingAudioConverter.WAV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoopingAudioConverter.Conversion {
    public static class Converter {
        public static async Task ConvertFilesAsync(IConverterEnvironment env, IConverterOptions o, IReadOnlyList<string> inputFiles, IProgress<double> progress = null) {
            FFmpegEngine effectEngine = File.Exists(env.FFmpegPath)
                ? new FFmpegEngine(env.FFmpegPath)
                : throw new Exception("Could not find ffmpeg - please specify ffmpeg_path in .config file");

            IAudioExporter getExporter() {
                switch (o.ExporterType) {
                    case ExporterType.VGAudio_BRSTM:
                        return new RSTMExporter(o.EncodingParameters.VGAudio_BXSTM);
                    case ExporterType.VGAudio_BCSTM:
                        return new CSTMExporter(o.EncodingParameters.VGAudio_BXSTM);
                    case ExporterType.VGAudio_BFSTM:
                        return new FSTMExporter(o.EncodingParameters.VGAudio_BXSTM);
                    case ExporterType.VGAudio_DSP:
                        return new DSPExporter();
                    case ExporterType.VGAudio_IDSP:
                        return new IDSPExporter();
                    case ExporterType.VGAudio_HPS:
                        return new HPSExporter();
                    case ExporterType.VGAudio_HCA:
                        return new HCAExporter(o.EncodingParameters.VGAudio_HCA);
                    case ExporterType.VGAudio_ADX:
                        return new ADXExporter(o.EncodingParameters.VGAudio_ADX);
                    case ExporterType.BrawlLib_BRSTM_ADPCM:
                        return new BrawlLibRSTMExporter(WaveEncoding.ADPCM, BrawlLibRSTMExporter.Container.RSTM);
                    case ExporterType.BrawlLib_BRSTM_PCM16:
                        return new BrawlLibRSTMExporter(WaveEncoding.PCM16, BrawlLibRSTMExporter.Container.RSTM);
                    case ExporterType.BrawlLib_BCSTM:
                        return new BrawlLibRSTMExporter(WaveEncoding.ADPCM, BrawlLibRSTMExporter.Container.CSTM);
                    case ExporterType.BrawlLib_BFSTM:
                        return new BrawlLibRSTMExporter(WaveEncoding.ADPCM, BrawlLibRSTMExporter.Container.FSTM);
                    case ExporterType.BrawlLib_BRWAV:
                        return new BrawlLibRWAVExporter();
                    case ExporterType.MSF_PCM16BE:
                        return new MSFExporter(big_endian: true);
                    case ExporterType.MSF_PCM16LE:
                        return new MSFExporter(big_endian: false);
                    case ExporterType.MSU1:
                        return new MSU1Converter();
                    case ExporterType.FLAC:
                        return new FFmpegExporter(effectEngine, "", ".flac");
                    case ExporterType.MP3:
                        return new MP3Exporter(effectEngine, o.EncodingParameters.FFMpeg_MP3);
                    case ExporterType.QAAC_M4A:
                        return new AACExporter(env.QaacPath, o.EncodingParameters.FFMpeg_AAC, adts: false);
                    case ExporterType.M4A:
                        return new FFmpegExporter(effectEngine, o.EncodingParameters.QAAC, ".m4a");
                    case ExporterType.QAAC_AAC:
                        return new AACExporter(env.QaacPath, o.EncodingParameters.FFMpeg_AAC, adts: true);
                    case ExporterType.AAC:
                        return new FFmpegExporter(effectEngine, o.EncodingParameters.QAAC, ".aac");
                    case ExporterType.OggVorbis:
                        return new VorbisExporter(effectEngine, o.EncodingParameters.FFMpeg_Vorbis);
                    case ExporterType.WAV:
                        return new WaveExporter();
                    default:
                        throw new Exception("Could not create exporter type " + o.ExporterType);
                }
            }

            IAudioExporter exporter = getExporter();

            IEnumerable<IAudioImporter> BuildImporters() {
                yield return new WaveImporter();
                yield return new MP3Importer();
                yield return new VorbisImporter(effectEngine);
                if (env.VGMPlayPath is string vgmplay_path)
                    yield return new VGMImporter(vgmplay_path);
                yield return new MSU1Converter();
                yield return new MSFImporter();
                if (env.VGMStreamPath is string vgmstream_path)
                    yield return new VGMStreamImporter(vgmstream_path);
                yield return new VGAudioImporter();
                yield return new BrawlLibImporter();
                yield return new VGMImporter(effectEngine);
                yield return effectEngine;
            }

            List<IAudioImporter> importers = BuildImporters().OrderBy(y => y.SharesCodecsWith(exporter) ? 1 : 2).ToList();

            if (!inputFiles.Any()) {
                throw new Exception("No input files were selected.");
            }

            double x = 1.0 / inputFiles.Count;
            int i = 0;
            foreach (string inputFile in inputFiles) {
                var pr = new ProgressSubset(progress, x * i, x * (i + 1));
                if (!env.Cancelled)
                    await ConvertFileAsync(env, o, importers, effectEngine, exporter, inputFile, pr);
                pr.Report(1.0);
                i++;
            }
        }

        public static async Task ConvertFileAsync(
            IConverterEnvironment env,
            IConverterOptions o,
            IEnumerable<IAudioImporter> importers,
            FFmpegEngine effectEngine,
            IAudioExporter exporter,
            string inputFile,
            IProgress<double> progress = null
        ) {
            progress?.Report(0.0);

            string outputDir = o.OutputDir;
            string inputDir = Path.GetDirectoryName(inputFile);
            for (int x = 0; x < 100; x++) {
                if (inputDir == o.InputDir) {
                    outputDir = outputDir.Replace("*", "");
                    break;
                }

                int index = outputDir.LastIndexOf('*');
                if (index < 0) break;

                string replacement = Path.GetFileName(inputDir);
                outputDir = outputDir.Substring(0, index) + replacement + outputDir.Substring(index + 1);
                inputDir = Path.GetDirectoryName(inputDir);
            }

            if (!Directory.Exists(outputDir)) {
                try {
                    Directory.CreateDirectory(outputDir);
                } catch (Exception e) {
                    throw new Exception("Could not create output directory " + o.OutputDir, e);
                }
            }

            string filename_no_ext = Path.GetFileNameWithoutExtension(inputFile);
            env.UpdateStatus(filename_no_ext, "Reading");

            string extension = Path.GetExtension(inputFile);

            PCM16Audio w = null;

            var importers_supported = importers.Where(im => im.SupportsExtension(extension));
            if (!importers_supported.Any()) {
                throw new Exception("No importers supported for file extension " + extension);
            }

            foreach (IAudioImporter importer in importers_supported) {
                try {
                    env.UpdateStatus(filename_no_ext, $"Decoding ({importer.GetType().Name})");
                    w = await importer.ReadFileAsync(inputFile, o.GetAudioHints(inputFile), new ProgressSubset(progress, 0.0, 0.5));
                    progress?.Report(0.5);
                    break;
                } catch (AudioImporterException e) {
                    env.UpdateStatus(filename_no_ext, $"Could not decode ({importer.GetType().Name}) - {e.Message}");
                }
            }

            if (o.GetLoopOverrides(Path.GetFileName(inputFile)) is LoopOverride loopOverride) {
                if (loopOverride.LoopStart < 0) {
                    w.Looping = false;
                } else {
                    w.Looping = true;
                    w.LoopStart = loopOverride.LoopStart;
                    w.LoopEnd = loopOverride.LoopEnd;
                }
            }

            if (w == null) {
                env.ReportFailure(filename_no_ext, "Could not read " + inputFile);
            } else {
                env.UpdateStatus(filename_no_ext, "Applying effects");
                w = await effectEngine.ApplyEffectsAsync(w,
                    channels: o.Channels ?? w.Channels,
                    rate: o.SampleRate ?? w.SampleRate,
                    db: o.AmplifydB ?? 0M,
                    amplitude: o.AmplifyRatio ?? 1M,
                    pitch_semitones: o.PitchSemitones ?? 0,
                    tempo_ratio: o.TempoRatio ?? 1,
                    force: !o.BypassEncodingWhenPossible);
                env.UpdateStatus(filename_no_ext, "Applied effects");

                List<NamedAudio> wavsToExport = new List<NamedAudio>();

                if (!env.Cancelled) {
                    var l = o.LoopExportParameters;
                    if (l.ExportWholeSong)
                        if (l.WholeSongExportType == WholeSongExportType.NumberOfLoops)
                            wavsToExport.Add(new NamedAudio(w.PlayLoopAndFade(l.NumberOfLoops, l.FadeOutSec), filename_no_ext + l.WholeSongSuffix));
                        else if (l.WholeSongExportType == WholeSongExportType.DesiredDuration)
                            wavsToExport.Add(new NamedAudio(w.PlayAndFade(l.DesiredDuration, l.FadeOutSec), filename_no_ext + l.WholeSongSuffix));
                    if (l.ExportPreLoop)
                        wavsToExport.Add(new NamedAudio(w.GetPreLoopSegment(), filename_no_ext + l.PreLoopSuffix));
                    if (l.ExportLoop)
                        wavsToExport.Add(new NamedAudio(w.GetLoopSegment(), filename_no_ext + l.LoopSuffix));

                    if (l.ExportPostLoop) {
                        PCM16Audio segment = w.GetPostLoopSegment();
                        if (segment.Samples.Any())
                            wavsToExport.Add(new NamedAudio(segment, filename_no_ext + l.PostLoopSuffix));
                    }

                    if (l.ExportLastLap) {
                        var lastLap = w = await effectEngine.ApplyEffectsAsync(w,
					        pitch_semitones: 1,
					        tempo_ratio: 1.1);
						wavsToExport.Add(new NamedAudio(lastLap, filename_no_ext + l.LastLapSuffix));
					}

                    if (o.ChannelSplit == ChannelSplit.Pairs) wavsToExport = wavsToExport.SelectMany(x => x.SplitMultiChannelToStereo()).ToList();
                    if (o.ChannelSplit == ChannelSplit.Each) wavsToExport = wavsToExport.SelectMany(x => x.SplitMultiChannelToMono()).ToList();
                }

                try {
                    foreach (NamedAudio n in wavsToExport) {
                        NamedAudio toExport = n;
                        switch (o.InputLoopBehavior) {
                            case InputLoopBehavior.ForceLoop:
                                if (!toExport.Audio.Looping) {
                                    toExport.Audio.Looping = true;
                                    toExport.Audio.LoopStart = 0;
                                    toExport.Audio.LoopEnd = toExport.Audio.Samples.Length / toExport.Audio.Channels;
                                }
                                break;
                            case InputLoopBehavior.AskForNonLooping:
                            case InputLoopBehavior.AskForAll:
                                if (o.InputLoopBehavior == InputLoopBehavior.AskForNonLooping && toExport.Audio.Looping) {
                                    break;
                                }
                                if (!env.ShowLoopConversionDialog(toExport)) {
                                    toExport = null;
                                }
                                break;
                            case InputLoopBehavior.DiscardForAll:
                                toExport.Audio.Looping = false;
                                break;
                        }

                        if (toExport != null) {
                            env.UpdateStatus(filename_no_ext, $"Encoding ({exporter.GetType().Name})");
                            await exporter.WriteFileAsync(toExport.Audio, outputDir, toExport.Name, new ProgressSubset(progress, 0.5, 1.0));
                        }
                    }

                    env.ReportSuccess(filename_no_ext);
                } catch (Exception ex) {
                    env.ReportFailure(filename_no_ext, $"Could not encode ({exporter.GetType().Name}) - {ex.Message}");
                }
            }

            progress?.Report(1.0);
        }
    }
}
