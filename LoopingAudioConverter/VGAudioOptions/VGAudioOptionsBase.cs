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

		/// <summary>
		/// If <c>true</c>, trims the output file length to the set LoopEnd.
		/// If <c>false</c> or if the <see cref="IAudioFormat"/> does not loop,
		/// the output file is not trimmed.
		/// Default is <c>true</c>.
		/// </summary>
		[Description("If true, trims the output file length to the set LoopEnd. If false or if the IAudioFormat does not loop, the output file is not trimmed.")]
		public bool TrimFile { get; set; } = true;
	}
}
