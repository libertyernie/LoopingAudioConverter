using BrawlLib.SSBB.Types.Audio;
using LoopingAudioConverter.Conversion;
using LoopingAudioConverter.PCM;
using LoopingAudioConverter.VGAudioOptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using VGAudio.Containers.Adx;
using VGAudio.Containers.Hca;
using VGAudio.Containers.NintendoWare;

namespace LoopingAudioConverter {
	public class Options : IConverterOptions, ILoopExportParameters, IEncodingParameters {
		[XmlIgnore]
		public IEnumerable<string> InputFiles { get; set; }

		public string OutputDir { get; set; }
		public string InputDir { get; set; }
		public int? Channels { get; set; }
		public int? SampleRate { get; set; }
		public decimal? AmplifydB { get; set; }
		public decimal? AmplifyRatio { get; set; }
		public double? PitchSemitones { get; set; }
		public double? TempoRatio { get; set; }
		public TimeSpan DefaultInputDuration { get; set; } = TimeSpan.FromMinutes(20);
		public ChannelSplit ChannelSplit { get; set; }
		public ExporterType ExporterType { get; set; }
		public string AACEncodingParameters { get; set; }
		public string OggVorbisEncodingParameters { get; set; }
		public string MP3FFmpegParameters { get; set; }
		public string AACFFmpegParameters { get; set; }
		public AdxOptions AdxOptions { get; set; }
		public HcaOptions HcaOptions { get; set; }
		public BxstmOptions BxstmOptions { get; set; }
		public WaveEncoding? WaveEncoding { get; set; }
		public InputLoopBehavior InputLoopBehavior { get; set; }
		public bool ExportWholeSong { get; set; }
		public bool WholeSongExportByDesiredDuration { get; set; }
		public string WholeSongSuffix { get; set; }
		public int NumberOfLoops { get; set; }
		public decimal DesiredDuration { get; set; }
		public decimal FadeOutSec { get; set; }
		public bool ExportPreLoop { get; set; }
		public string PreLoopSuffix { get; set; }
		public bool ExportLoop { get; set; }
		public string LoopSuffix { get; set; }
		public bool ExportPostLoop { get; set; }
		public string PostLoopSuffix { get; set; }

		ILoopExportParameters IConverterOptions.LoopExportParameters => this;

		IEncodingParameters IConverterOptions.EncodingParameters => this;

		BxstmConfiguration IEncodingParameters.VGAudio_BXSTM => BxstmOptions.Configuration;

		HcaConfiguration IEncodingParameters.VGAudio_HCA => HcaOptions.Configuration;

		AdxConfiguration IEncodingParameters.VGAudio_ADX => AdxOptions.Configuration;

		string IEncodingParameters.QAAC => AACEncodingParameters;

		string IEncodingParameters.FFMpeg_MP3 => MP3FFmpegParameters;

		string IEncodingParameters.FFMpeg_AAC => AACFFmpegParameters;

		string IEncodingParameters.FFMpeg_Vorbis => OggVorbisEncodingParameters;

		WholeSongExportType ILoopExportParameters.WholeSongExportType => WholeSongExportByDesiredDuration ? WholeSongExportType.DesiredDuration : WholeSongExportType.NumberOfLoops;

		private class Hints : IRenderingHints {
			public int RenderingSampleRate { get; set; }
			public TimeSpan? RequiredDecodingDuration { get; set; }
		}

		IRenderingHints IConverterOptions.GetAudioHints(string filename) => new Hints {
			RenderingSampleRate = SampleRate ?? 44100,
			RequiredDecodingDuration = WholeSongExportByDesiredDuration ? TimeSpan.FromSeconds((double)(DesiredDuration + FadeOutSec))
				: GetLoopOverrides(filename) is LoopOverride l ? TimeSpan.FromSeconds(l.LoopEnd / (double)(SampleRate ?? 44100))
				: (TimeSpan?)null
		};

		public LoopOverride? GetLoopOverrides(string filename) {
			if (File.Exists("loop.txt")) {
				using (StreamReader sr = new StreamReader("loop.txt")) {
					string line;
					while ((line = sr.ReadLine()) != null) {
						line = Regex.Replace(line, "[ \t]+", " ");
						if (line.Length > 0 && line[0] != '#' && line.Contains(" ")) {
							try {
								int loopStart = int.Parse(line.Substring(0, line.IndexOf(" ")));
								line = line.Substring(line.IndexOf(" ") + 1);
								int loopEnd = int.Parse(line.Substring(0, line.IndexOf(" ")));
								line = line.Substring(line.IndexOf(" ") + 1);
								if (line == filename)
									return new LoopOverride { LoopStart = loopStart, LoopEnd = loopEnd };
							} catch (Exception e) {
								Console.Error.WriteLine("Could not parse line in loop.txt: " + line + " - " + e.Message);
							}
						}
					}
				}
			}
			return null;
		}
	}
}
