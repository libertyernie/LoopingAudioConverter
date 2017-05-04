using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VGAudio.Containers.Bxstm;

namespace LoopingAudioConverter
{
    public static class OptionsSerialization
    {
        public static void PopulateFromFile(string filename, Options o)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    bool active = false;
                    while ((line = sr.ReadLine()) != null) {
                        if (line.StartsWith(";")) continue;
                        if (line.StartsWith("[")) {
                            active = line.Equals("[LoopingAudioConverter]", StringComparison.InvariantCultureIgnoreCase);
                            continue;
                        }

                        string[] split = line.Split('=');
                        if (split.Length == 2)
                        {
                            string v = split[1];
                            switch (split[0])
                            {
                                case "OutputDir":
                                    o.OutputDir = v;
                                    break;
                                case "MaxChannels":
                                    o.MaxChannels = int.Parse(v);
                                    break;
                                case "MaxSampleRate":
                                    o.MaxSampleRate = int.Parse(v);
                                    break;
                                case "AmplifydB":
                                    o.AmplifydB = decimal.Parse(v);
                                    break;
                                case "AmplifyRatio":
                                    o.AmplifyRatio = decimal.Parse(v);
                                    break;
                                case "ChannelSplit":
                                    o.ChannelSplit = (ChannelSplit)Enum.Parse(typeof(ChannelSplit), v, true);
                                    break;
                                case "ExporterType":
                                    o.ExporterType = (ExporterType)Enum.Parse(typeof(ExporterType), v, true);
                                    break;
                                case "MP3EncodingParameters":
                                    o.MP3EncodingParameters = v;
                                    break;
                                case "OggVorbisEncodingParameters":
                                    o.OggVorbisEncodingParameters = v;
                                    break;
                                case "BxstmCodec":
                                    o.BxstmCodec = (BxstmCodec)Enum.Parse(typeof(BxstmCodec), v, true);
                                    break;
                                case "ExportWholeSong":
                                    o.ExportWholeSong = bool.Parse(v);
                                    break;
                                case "WholeSongSuffix":
                                    o.WholeSongSuffix = v;
                                    break;
                                case "NumberOfLoops":
                                    o.NumberOfLoops = int.Parse(v);
                                    break;
                                case "FadeOutSec":
                                    o.FadeOutSec = decimal.Parse(v);
                                    break;
                                case "WriteLoopingMetadata":
                                    o.WriteLoopingMetadata = bool.Parse(v);
                                    break;
                                case "ExportPreLoop":
                                    o.ExportPreLoop = bool.Parse(v);
                                    break;
                                case "PreLoopSuffix":
                                    o.PreLoopSuffix = v;
                                    break;
                                case "ExportLoop":
                                    o.ExportLoop = bool.Parse(v);
                                    break;
                                case "LoopSuffix":
                                    o.LoopSuffix = v;
                                    break;
                                case "ShortCircuit":
                                    o.ShortCircuit = bool.Parse(v);
                                    break;
                                case "BrawlLibDecoder":
                                    o.BrawlLibDecoder = bool.Parse(v);
                                    break;
                                case "NumSimulTasks":
                                    o.NumSimulTasks = int.Parse(v);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static void WriteToFile(string filename, Options o)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("[LoopingAudioConverter]");
                    if (o.OutputDir != null) sw.WriteLine("OutputDir=" + o.OutputDir);
                    if (o.MaxChannels != null) sw.WriteLine("MaxChannels=" + o.MaxChannels);
                    if (o.MaxSampleRate != null) sw.WriteLine("MaxSampleRate=" + o.MaxSampleRate);
                    if (o.AmplifydB != null) sw.WriteLine("AmplifydB=" + o.AmplifydB);
                    if (o.AmplifyRatio != null) sw.WriteLine("AmplifyRatio=" + o.AmplifyRatio);
                    if (o.ChannelSplit != null) sw.WriteLine("ChannelSplit=" + o.ChannelSplit);
                    if (o.ExporterType != null) sw.WriteLine("ExporterType=" + o.ExporterType);
                    if (o.MP3EncodingParameters != null) sw.WriteLine("MP3EncodingParameters=" + o.MP3EncodingParameters);
                    if (o.OggVorbisEncodingParameters != null) sw.WriteLine("OggVorbisEncodingParameters=" + o.OggVorbisEncodingParameters);
                    if (o.BxstmCodec != null) sw.WriteLine("BxstmCodec=" + o.BxstmCodec);
                    if (o.ExportWholeSong != null) sw.WriteLine("ExportWholeSong=" + o.ExportWholeSong);
                    if (o.WholeSongSuffix != null) sw.WriteLine("WholeSongSuffix=" + o.WholeSongSuffix);
                    if (o.NumberOfLoops != null) sw.WriteLine("NumberOfLoops=" + o.NumberOfLoops);
                    if (o.FadeOutSec != null) sw.WriteLine("FadeOutSec=" + o.FadeOutSec);
                    if (o.WriteLoopingMetadata != null) sw.WriteLine("WriteLoopingMetadata=" + o.WriteLoopingMetadata);
                    if (o.ExportPreLoop != null) sw.WriteLine("ExportPreLoop=" + o.ExportPreLoop);
                    if (o.PreLoopSuffix != null) sw.WriteLine("PreLoopSuffix=" + o.PreLoopSuffix);
                    if (o.ExportLoop != null) sw.WriteLine("ExportLoop=" + o.ExportLoop);
                    if (o.LoopSuffix != null) sw.WriteLine("LoopSuffix=" + o.LoopSuffix);
                    if (o.ShortCircuit != null) sw.WriteLine("ShortCircuit=" + o.ShortCircuit);
                    if (o.BrawlLibDecoder != null) sw.WriteLine("BrawlLibDecoder=" + o.ShortCircuit);
                    if (o.NumSimulTasks != null) sw.WriteLine("NumSimulTasks=" + o.NumSimulTasks);
                }
            }
        }
    }
}
