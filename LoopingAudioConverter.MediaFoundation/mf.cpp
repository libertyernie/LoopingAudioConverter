// https://stackoverflow.com/a/48991319

#include <windows.h>
#include <windowsx.h>

#include <atlstr.h>
#include <comdef.h>
#include <exception>

#include <mfapi.h>
#include <mfplay.h>
#include <mfreadwrite.h>
#include <mmdeviceapi.h>
#include <Audioclient.h>
#include <mferror.h>
#include <Wmcodecdsp.h>

#pragma comment(lib, "mf.lib")
#pragma comment(lib, "mfplat.lib")
#pragma comment(lib, "mfplay.lib")
#pragma comment(lib, "mfreadwrite.lib")
#pragma comment(lib, "mfuuid.lib")
#pragma comment(lib, "wmcodecdspuuid")

inline void ThrowIfFailed(HRESULT hr)
{
    if (FAILED(hr))
    {
        // Get the error message
        _com_error err(hr);
        LPCTSTR errMsg = err.ErrorMessage();

        OutputDebugString(L"################################## ERROR ##################################\n");
        OutputDebugString(errMsg);
        OutputDebugString(L"\n################################## ----- ##################################\n");

        CStringA sb(errMsg);
        // Set a breakpoint on this line to catch DirectX API errors
        throw std::exception(sb);
    }
}

template <class T> void SafeRelease(T** ppT)
{
    if (*ppT)
    {
        (*ppT)->Release();
        *ppT = nullptr;
    }
}

HRESULT GetOutputMediaTypes(
    GUID cAudioFormat,
    UINT32 cSampleRate,
    UINT32 cBitPerSample,
    UINT32 cChannels,
    IMFMediaType** ppType
)
{
    // Enumerate all codecs except for codecs with field-of-use restrictions.
    // Sort the results.
    DWORD dwFlags =
        (MFT_ENUM_FLAG_ALL & (~MFT_ENUM_FLAG_FIELDOFUSE)) |
        MFT_ENUM_FLAG_SORTANDFILTER;

    IMFCollection* pAvailableTypes = NULL;    // List of audio media types.
    IMFMediaType* pAudioType = NULL;         // Corresponding codec.

    HRESULT hr = MFTranscodeGetAudioOutputAvailableTypes(
        cAudioFormat,
        dwFlags,
        NULL,
        &pAvailableTypes
    );

    // Get the element count.
    DWORD dwMTCount;
    hr = pAvailableTypes->GetElementCount(&dwMTCount);

    // Iterate through the results and check for the corresponding codec.
    for (DWORD i = 0; i < dwMTCount; i++)
    {
        hr = pAvailableTypes->GetElement(i, (IUnknown**)&pAudioType);

        GUID majorType;
        hr = pAudioType->GetMajorType(&majorType);

        GUID subType;
        hr = pAudioType->GetGUID(MF_MT_SUBTYPE, &subType);

        if (majorType != MFMediaType_Audio || subType != MFAudioFormat_FLAC)
        {
            continue;
        }

        UINT32 sampleRate = NULL;
        hr = pAudioType->GetUINT32(
            MF_MT_AUDIO_SAMPLES_PER_SECOND,
            &sampleRate
        );

        UINT32 bitRate = NULL;
        hr = pAudioType->GetUINT32(
            MF_MT_AUDIO_BITS_PER_SAMPLE,
            &bitRate
        );

        UINT32 channels = NULL;
        hr = pAudioType->GetUINT32(
            MF_MT_AUDIO_NUM_CHANNELS,
            &channels
        );

        if (sampleRate == cSampleRate
            && bitRate == cBitPerSample
            && channels == cChannels)
        {
            // Found the codec.
            // Jump out!
            break;
        }
    }

    // Add the media type to the caller
    *ppType = pAudioType;
    (*ppType)->AddRef();
    SafeRelease(&pAudioType);

    return hr;
}

void convertMediaFile(LPCWSTR inputPath, LPCWSTR outputPath)
{
    HRESULT hr = S_OK;

    //// Initialize com interface
    //ThrowIfFailed(
    //    CoInitializeEx(0, COINIT_MULTITHREADED)
    //);

    // Start media foundation
    ThrowIfFailed(
        MFStartup(MF_VERSION)
    );

    IMFMediaType* pInputType = NULL;
    IMFSourceReader* pSourceReader = NULL;
    IMFMediaType* pOuputMediaType = NULL;
    IMFSinkWriter* pSinkWriter = NULL;

    // Create source reader
    hr = MFCreateSourceReaderFromURL(
        inputPath,
        NULL,
        &pSourceReader
    );

    // Create sink writer
    hr = MFCreateSinkWriterFromURL(
        outputPath,
        NULL,
        NULL,
        &pSinkWriter
    );

    // Get media type from source reader
    hr = pSourceReader->GetCurrentMediaType(
        MF_SOURCE_READER_FIRST_AUDIO_STREAM,
        &pInputType
    );

    // Get sample rate, bit rate and channels
    UINT32 sampleRate = NULL;
    hr = pInputType->GetUINT32(
        MF_MT_AUDIO_SAMPLES_PER_SECOND,
        &sampleRate
    );

    UINT32 bitRate = NULL;
    hr = pInputType->GetUINT32(
        MF_MT_AUDIO_BITS_PER_SAMPLE,
        &bitRate
    );

    UINT32 channels = NULL;
    hr = pInputType->GetUINT32(
        MF_MT_AUDIO_NUM_CHANNELS,
        &channels
    );

    // Try to find a media type that is fitting.
    hr = GetOutputMediaTypes(
        MFAudioFormat_FLAC,
        sampleRate,
        bitRate,
        channels,
        &pOuputMediaType);

    DWORD dwWriterStreamIndex = -1;

    // Add the stream
    hr = pSinkWriter->AddStream(
        pOuputMediaType,
        &dwWriterStreamIndex
    );

    // Set input media type
    hr = pSinkWriter->SetInputMediaType(
        dwWriterStreamIndex,
        pInputType,
        NULL
    );

    // Tell the sink writer to accept data
    hr = pSinkWriter->BeginWriting();

    // Forever alone loop
    for (;;)
    {
        DWORD nStreamIndex, nStreamFlags;
        LONGLONG nTime;
        IMFSample* pSample;

        // Read through the samples until...
        hr = pSourceReader->ReadSample(
            MF_SOURCE_READER_FIRST_AUDIO_STREAM,
            0,
            &nStreamIndex,
            &nStreamFlags,
            &nTime,
            &pSample);

        if (pSample)
        {
            OutputDebugString(L"Write sample...\n");

            hr = pSinkWriter->WriteSample(
                dwWriterStreamIndex,
                pSample
            );
        }

        // ... we are at the end of the stream...
        if (nStreamFlags & MF_SOURCE_READERF_ENDOFSTREAM)
        {
            // ... and jump out.
            break;
        }
    }

    // Call finalize to finish writing.
    hr = pSinkWriter->Finalize();
}
