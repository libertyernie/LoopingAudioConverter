#pragma once
#include "../rstmcpp/pcm16.h"
#include "../rstmcpp/progresstracker.h"
#include "../rstmcpp/encoder.h"
#include <cstdint>

using namespace System;

namespace RSTMCPP {
	public interface class PCM16Source {
		property int32_t Channels;
		property int32_t SampleRate;
		property array<int16_t>^ Samples;
		property bool Looping;
		property int32_t LoopStart;
		property int32_t LoopEnd;
	};

	public enum class FileType : int32_t {
		RSTM = 0,
		CSTM = 1,
		CWAV = 2,
		BFSTM = 3
	};

	public ref class PCM16
	{
	private:
		rstmcpp::pcm16::PCM16* _pcm;
	public:
		PCM16(PCM16Source^ source) {
			pin_ptr<int16_t> sample_data = &source->Samples[0];
			_pcm = source->Looping
				? new rstmcpp::pcm16::PCM16(source->Channels, source->SampleRate, sample_data, source->Samples->Length)
				: new rstmcpp::pcm16::PCM16(source->Channels, source->SampleRate, sample_data, source->Samples->Length, source->LoopStart, source->LoopEnd);
		}

		array<uint8_t>^ Encode(FileType type) {
			rstmcpp::ProgressTracker progress;
			int size;
			char* data = rstmcpp::encoder::encode(this->_pcm, &progress, &size, (int)type);

			array<uint8_t>^ arr = gcnew array<uint8_t>(size);
			System::Runtime::InteropServices::Marshal::Copy(IntPtr((void*)data), arr, 0, size);

			free(data);
			return arr;
		}

		~PCM16() {
			this->!PCM16();
		}

		!PCM16() {
			if (_pcm != NULL) {
				delete _pcm;
				_pcm = NULL;
			}
		}
	};
}
