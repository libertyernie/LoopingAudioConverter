using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGAudio.Codecs.CriHca;
using VGAudio.Containers.Hca;

namespace LoopingAudioConverter.VGAudioOptions {
	public class HcaOptions : VGAudioOptionsBase<HcaConfiguration> {
		public CriHcaQuality Quality { get; set; }

		public bool LimitBitrate { get; set; }

		public int Bitrate { get; set; }

		public ulong? KeyCode { get; set; }

		public override HcaConfiguration Configuration => new HcaConfiguration {
			Bitrate = Bitrate,
			EncryptionKey = KeyCode is ulong u ? new CriHcaKey(u) : null,
			LimitBitrate = LimitBitrate,
			TrimFile = TrimFile,
			Quality = Quality
		};
	}
}
