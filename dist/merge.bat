SET FROM=..\LoopingAudioConverter\bin\Release
"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /targetplatform:"v4,C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /out:windows\LoopingAudioConverter.exe %FROM%\LoopingAudioConverter.exe %FROM%\VGAudio.dll %FROM%\LoopSelectionDialogLib.dll %FROM%\VorbisCommentSharp.dll
XCOPY /E /I ..\LoopingAudioConverter\tools_win32 windows\tools
XCOPY ..\LoopingAudioConverter\About.html windows
XCOPY windows\LoopingAudioConverter.exe linux32
XCOPY /E /I ..\LoopingAudioConverter\tools_linux-i686 linux32\tools
XCOPY ..\LoopingAudioConverter\About.html linux32
@PAUSE
