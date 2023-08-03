#pragma once

#include <vcclr.h>
#include <exception>
#include "mf.h"

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;
using namespace System::Threading::Tasks;
using namespace LoopingAudioConverter::PCM;
using namespace LoopingAudioConverter::WAV;

namespace LoopingAudioConverter {
	namespace MediaFoundation {
		public ref struct Converter : public LoopingAudioConverter::PCM::IAudioExporter {
			virtual Task^ WriteFileAsync(PCM16Audio^ lwav, String^ output_dir, String^ filename_no_ext, IProgress<double>^ progress) {
				String^ input_path = Path::GetTempFileName();
				File::WriteAllBytes(input_path, WaveConverter::Export(lwav));
				String^ output_path = Path::Combine(output_dir, filename_no_ext + ".flac");
				pin_ptr<const wchar_t> inStr = PtrToStringChars(input_path);
				pin_ptr<const wchar_t> outStr = PtrToStringChars(output_path);
				try {
					convertMediaFile(inStr, outStr);
					File::Delete(input_path);
					return Task::CompletedTask;
				}
				catch (std::exception ex) {
					const char* c = ex.what();
					String^ s = gcnew String(c);
					throw gcnew Exception(s);
				}
			}

			virtual bool TryWriteCompressedAudioToFile(System::Object^ audio, LoopingAudioConverter::PCM::ILoopPoints^ loopPoints, System::String^ output_dir, System::String^ original_filename_no_ext) {
				return false;
			}
		};
	}
}
