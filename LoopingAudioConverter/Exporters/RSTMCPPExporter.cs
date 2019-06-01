using RSTMCPP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopingAudioConverter {
	public class RSTMCPPExporter : IAudioExporter {
		private readonly FileType _type;
		private readonly string _ext;

		public RSTMCPPExporter(FileType type) {
			_type = type;
			_ext =
				_type == FileType.BFSTM ? "bfstm"
				: _type == FileType.CSTM ? "bcstm"
				: _type == FileType.CWAV ? "bcwav"
				: _type == FileType.RSTM ? "brstm"
				: throw new Exception("Unknown type");
		}

		public Task WriteFileAsync(PCM16Audio lwav, string output_dir, string original_filename_no_ext) {
			var p = new PCM16Source(lwav);
			byte[] data = p.Encode(_type);
			string output_filename = Path.Combine(output_dir, original_filename_no_ext + "." + _ext);
			File.WriteAllBytes(output_filename, data);
			return Task.FromResult(0);
		}
	}
}
