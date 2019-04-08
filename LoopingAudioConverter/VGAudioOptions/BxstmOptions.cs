using VGAudio.Containers.NintendoWare;
using VGAudio.Utilities;
using VGAudio.Containers.NintendoWare.Structures;
using System.ComponentModel;

namespace LoopingAudioConverter.VGAudioOptions {
	public class BxstmOptions : VGAudioOptionsBase<BxstmConfiguration> {
		public struct BxstmVersion {
			public bool UseDefault;
			public byte Major, Minor, Micro, Revision;
		}

		/// <summary>
		/// If <c>true</c>, rebuilds the seek table when building the file.
		/// If <c>false</c>, reuses the seek table read from the imported file,
		/// if available.
		/// Default is <c>true</c>.
		/// </summary>
		[Description("Whether to rebuild the seek table instead of reading from the imported file (if any).")]
		public bool RecalculateSeekTable { get; set; } = true;

		/// <summary>
		/// If <c>true</c>, recalculates the loop context when building the file.
		/// If <c>false</c>, reuses the loop context read from the imported file,
		/// if available.
		/// Default is <c>true</c>.
		/// </summary>
		[Description("Whether to recalculate the loop context instead of reading from the imported file (if any).")]
		public bool RecalculateLoopContext { get; set; } = true;

		/// <summary>
		/// The number of samples in each block when interleaving
		/// the audio data in the audio file.
		/// Must be divisible by 14.
		/// Default is 14,336 (0x3800).
		/// </summary>
		[Description("The number of samples in each block when interleaving the audio data in the audio file. Must be divisible by 14.")]
		public int SamplesPerInterleave { get; set; } = 0x3800;

		/// <summary>
		/// The number of samples per entry in the seek table. Used when
		/// building the audio file.
		/// Default is 14,336 (0x3800).
		/// </summary>
		[Description("The number of samples per entry in the seek table.")]
		public int SamplesPerSeekTableEntry { get; set; } = 0x3800;

		/// <summary>
		/// When building the audio file, the loop points and audio will
		/// be adjusted so that the start loop point is a multiple of
		/// this number. Default is 14,336 (0x3800).
		/// </summary>
		[Description("When building the audio file, the loop points and audio will be adjusted so that the start loop point is a multiple of this number.")]
		public int LoopPointAlignment { get; set; } = 0x3800;

		[Description("The audio codec to use.")]
		public NwCodec Codec { get; set; } = NwCodec.GcAdpcm;
		[Description("The endianness. Wii and Wii U use big endian, while 3DS and Switch use little endian.")]
		public Endianness? Endianness { get; set; }
		[Description("A version number. Set useDefault to let VGAudio pick a number based on the container format.")]
		public BxstmVersion Version { get; set; } = new BxstmVersion { UseDefault = true };

		/// <summary>
		/// The type of track description to be used when building the 
		/// BRSTM header.
		/// Default is <see cref="BrstmTrackType.Standard"/>.
		/// Used only in BRSTM files.
		/// </summary>
		[Description("The type of track description to be used when building the header. Used only in BRSTM files.")]
		public BrstmTrackType TrackType { get; set; } = BrstmTrackType.Standard;

		/// <summary>
		/// The type of seek table to use when building the BRSTM
		/// ADPC block.
		/// Default is <see cref="BrstmSeekTableType.Standard"/>.
		/// Used only in BRSTM files.
		/// </summary>
		[Description("The type of seek table to use when building the BRSTM ADPC block. Used only in BRSTM files.")]
		public BrstmSeekTableType SeekTableType { get; set; } = BrstmSeekTableType.Standard;

		public override BxstmConfiguration Configuration => new BxstmConfiguration {
			Codec = Codec,
			Endianness = Endianness,
			LoopPointAlignment = LoopPointAlignment,
			RecalculateLoopContext = RecalculateLoopContext,
			RecalculateSeekTable = RecalculateSeekTable,
			SamplesPerInterleave = SamplesPerInterleave,
			SamplesPerSeekTableEntry = SamplesPerSeekTableEntry,
			SeekTableType = SeekTableType,
			TrackType = TrackType,
			TrimFile = TrimFile,
			Version = Version.UseDefault
				? null
				: new NwVersion(
					Version.Major,
					Version.Minor,
					Version.Micro,
					Version.Revision)
		};
	}
}
