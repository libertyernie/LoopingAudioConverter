#pragma once
#include <cstdint>
#include "../rstmcpp/pcm16.h"
#include "../rstmcpp/progresstracker.h"
#include "../rstmcpp/encoder.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace rstmcpp;
using namespace rstmcpp::pcm16;
using namespace rstmcpp::encoder;

namespace RSTMCPP {
	public interface class IPCM16Source {
		property int32_t Channels { int32_t get(); }
		property int32_t SampleRate { int32_t get(); }
		property array<int16_t>^ Samples { array<int16_t>^ get(); }
		property bool Looping { bool get(); }
		property int32_t LoopStart { int32_t get(); }
		property int32_t LoopEnd { int32_t get(); }
	};

	public enum class FileType : int32_t {
		RSTM = 0,
		CSTM = 1,
		CWAV = 2,
		BFSTM = 3
	};

	public ref class PCM16Source
	{
	private:
		rstmcpp::pcm16::PCM16* _pcm;
	public:
		PCM16Source(IPCM16Source^ source) {
			array<int16_t>^ samples = source->Samples;
			pin_ptr<int16_t> sample_data = &samples[0];
			_pcm = source->Looping
				? new PCM16(source->Channels, source->SampleRate, sample_data, source->Samples->Length, source->LoopStart, source->LoopEnd)
				: new PCM16(source->Channels, source->SampleRate, sample_data, source->Samples->Length);
		}

		array<uint8_t>^ Encode(FileType type) {
			ProgressTracker progress;
			int size;
			char* data = encode(this->_pcm, &progress, &size, (int)type);

			if (data == NULL)
				throw gcnew Exception("Could not convert to the given format");

			array<uint8_t>^ arr = gcnew array<uint8_t>(size);
			Marshal::Copy(IntPtr((void*)data), arr, 0, size);

			free(data);
			return arr;
		}

		~PCM16Source() {
			this->!PCM16Source();
		}

		!PCM16Source() {
			if (_pcm != NULL) {
				delete _pcm;
				_pcm = NULL;
			}
		}
	};
}
