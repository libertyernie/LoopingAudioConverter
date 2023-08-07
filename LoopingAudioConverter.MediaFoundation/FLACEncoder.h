#pragma once

#include <atlstr.h>
#include <comdef.h>
#include <mfapi.h>
#include <mfidl.h>
#include <mfreadwrite.h>
#include <vcclr.h>

#pragma comment(lib, "mf.lib")
#pragma comment(lib, "mfplat.lib")
#pragma comment(lib, "mfreadwrite.lib")
#pragma comment(lib, "mfuuid.lib")

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;
using namespace LoopingAudioConverter::PCM;
using namespace LoopingAudioConverter::WAV;

// https://stackoverflow.com/a/48991319

inline void assert_success(HRESULT hr)
{
	if (FAILED(hr))
	{
		_com_error err(hr);
		String^ errMsg = gcnew String(err.ErrorMessage());
		throw gcnew AudioExporterException(errMsg);
	}
}

namespace LoopingAudioConverter {
	namespace MediaFoundation {
		public ref struct FLACEncoder {
			static void WriteFile(PCM16Audio^ lwav, String^ output_path) {
				array<uint8_t>^ wav = WaveConverter::Export(lwav);

				IStream* stream = NULL;
				IMFByteStream* byteStream = NULL;

				IMFCollection* pAvailableTypes = NULL;
				IMFMediaType* pOutputMediaType = NULL;

				try {
					// Initialize Media Foundation
					assert_success(MFStartup(MF_VERSION));

					// Set up to read from source
					IMFSourceReader* pSourceReader;
					pin_ptr<uint8_t> wavPtr = &wav[0];
					stream = SHCreateMemStream(wavPtr, wav->Length);
					MFCreateMFByteStreamOnStream(stream, &byteStream);
					assert_success(MFCreateSourceReaderFromByteStream(byteStream, NULL, &pSourceReader));

					IMFMediaType* pInputType;
					assert_success(pSourceReader->GetCurrentMediaType(MF_SOURCE_READER_FIRST_AUDIO_STREAM, &pInputType));

					// Set up to write to output file
					IMFSinkWriter* pSinkWriter;
					pin_ptr<const wchar_t> outStr = PtrToStringChars(output_path);
					assert_success(MFCreateSinkWriterFromURL(outStr, NULL, NULL, &pSinkWriter));

					// Get information on original input
					UINT32 desired_sample_rate;
					assert_success(pInputType->GetUINT32(MF_MT_AUDIO_SAMPLES_PER_SECOND, &desired_sample_rate));

					UINT32 desired_bit_depth;
					assert_success(pInputType->GetUINT32(MF_MT_AUDIO_BITS_PER_SAMPLE, &desired_bit_depth));

					UINT32 desired_channels;
					assert_success(pInputType->GetUINT32(MF_MT_AUDIO_NUM_CHANNELS, &desired_channels));

					// Look for codecs without field-of-use restrictions
					DWORD dwFlags = MFT_ENUM_FLAG_ALL;
					dwFlags &= ~MFT_ENUM_FLAG_FIELDOFUSE;

					// Sort the results
					dwFlags |= MFT_ENUM_FLAG_SORTANDFILTER;

					// Get the resulting list
					assert_success(MFTranscodeGetAudioOutputAvailableTypes(MFAudioFormat_FLAC, dwFlags, NULL, &pAvailableTypes));

					// Find out how many codecs are in the list
					DWORD dwMTCount;
					assert_success(pAvailableTypes->GetElementCount(&dwMTCount));

					// Look for a codec with a matching sample rate and channel count
					// Also check the bit depth (e.g. 16-bit for CD quality audio)
					for (DWORD i = 0; i < dwMTCount; i++) {
						IMFMediaType* pAudioType = NULL;
						assert_success(pAvailableTypes->GetElement(i, (IUnknown**)&pAudioType));

						// Make sure we got the right types
						GUID mediaType, audioFormat;
						assert_success(pAudioType->GetMajorType(&mediaType));
						assert_success(pAudioType->GetGUID(MF_MT_SUBTYPE, &audioFormat));
						if (mediaType != MFMediaType_Audio || audioFormat != MFAudioFormat_FLAC) {
							pAudioType->Release();
							continue;
						}

						UINT32 sample_rate = NULL;
						assert_success(pAudioType->GetUINT32(MF_MT_AUDIO_SAMPLES_PER_SECOND, &sample_rate));
						if (sample_rate != desired_sample_rate) {
							pAudioType->Release();
							continue;
						}

						UINT32 bit_depth = NULL;
						assert_success(pAudioType->GetUINT32(MF_MT_AUDIO_BITS_PER_SAMPLE, &bit_depth));
						if (bit_depth != desired_bit_depth) {
							pAudioType->Release();
							continue;
						}

						UINT32 channels = NULL;
						assert_success(pAudioType->GetUINT32(MF_MT_AUDIO_NUM_CHANNELS, &channels));
						if (channels != desired_channels) {
							pAudioType->Release();
							continue;
						}

						pOutputMediaType = pAudioType;

						break;
					}

					if (pOutputMediaType == NULL)
						throw gcnew AudioExporterException("Could not find a Media Foundation output codec with an appropriate sample rate, bit depth, and channel count.");

					// Add a new output stream with the appropriate media type
					DWORD dwWriterStreamIndex = -1;
					assert_success(pSinkWriter->AddStream(pOutputMediaType, &dwWriterStreamIndex));
					assert_success(pSinkWriter->SetInputMediaType(dwWriterStreamIndex, pInputType, NULL));

					// Start writing data
					assert_success(pSinkWriter->BeginWriting());
					while (true) {
						DWORD nStreamIndex, nStreamFlags;
						LONGLONG nTime;
						IMFSample* pSample;

						// Read from input stream
						assert_success(pSourceReader->ReadSample(
							MF_SOURCE_READER_FIRST_AUDIO_STREAM,
							0,
							&nStreamIndex,
							&nStreamFlags,
							&nTime,
							&pSample));

						// Write to input stream
						if (pSample)
							assert_success(pSinkWriter->WriteSample(dwWriterStreamIndex, pSample));

						// Check for end of input stream
						if (nStreamFlags & MF_SOURCE_READERF_ENDOFSTREAM)
							break;
					}
					assert_success(pSinkWriter->Finalize());
				}
				finally {
					if (pOutputMediaType)
						pOutputMediaType->Release();
					if (pAvailableTypes)
						pAvailableTypes->Release();
					if (byteStream)
						byteStream->Close();
					if (stream)
						stream->Release();
				}
			}
		};
	}
}
