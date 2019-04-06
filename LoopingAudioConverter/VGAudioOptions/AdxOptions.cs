using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGAudio.Codecs.CriAdx;
using VGAudio.Containers.Adx;

namespace LoopingAudioConverter.VGAudioOptions {
	public class AdxOptions : VGAudioOptionsBase<AdxConfiguration> {
		public enum AdxEncryptionType {
			None = 0,
			KeyString = 8,
			KeyCode = 9
		}

		private int _filter = 2;

		public int Version { get; set; } = 4;

		public int FrameSize { get; set; } = 18;

		public int Filter {
			get {
				return _filter;
			}
			set {
				_filter = (byte)value % 4;
			}
		}

		public CriAdxType Type { get; set; } = CriAdxType.Linear;

		public AdxEncryptionType EncryptionType { get; set; }

		public string KeyString { get; set; }

		public ulong? KeyCode { get; set; }

		public override AdxConfiguration Configuration => new AdxConfiguration {
			EncryptionKey =
				EncryptionType == AdxEncryptionType.KeyString && !string.IsNullOrEmpty(KeyString) ? new CriAdxKey(KeyString)
				: EncryptionType == AdxEncryptionType.KeyCode && KeyCode is ulong k ? new CriAdxKey(k)
				: null,
			EncryptionType = (int)EncryptionType,
			Filter = Filter,
			FrameSize = FrameSize,
			TrimFile = TrimFile,
			Type = Type,
			Version = Version
		};
	}
}
