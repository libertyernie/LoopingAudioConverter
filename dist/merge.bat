SET FROM=..\LoopingAudioConverter\bin\Release
"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /targetplatform:"v4,C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0" /out:LoopingAudioConverter.exe %FROM%\LoopingAudioConverter.exe %FROM%\RSTMLib.dll %FROM%\RSTMLib.wav.dll
XCOPY /E /I ..\LoopingAudioConverter\tools_win32 tools
XCOPY ..\LoopingAudioConverter\About.html .
@PAUSE
