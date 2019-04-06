using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGAudio.Containers;

namespace LoopingAudioConverter.VGAudioOptions {
	public abstract class VGAudioOptionsBase<T> where T : Configuration {
		[Browsable(false)]
		public abstract T Configuration { get; }

		public bool TrimFile { get; set; }
	}
}
