SET FROM=..\LoopingAudioConverter\bin\Release
XCOPY /E /I %FROM%\*.exe windows
XCOPY /E /I %FROM%\*.dll windows
XCOPY /E /I ..\LoopingAudioConverter\tools_win32 windows\tools
XCOPY ..\LoopingAudioConverter\About.html windows
XCOPY /E /I %FROM%\*.exe linux32
XCOPY /E /I %FROM%\*.dll linux32
XCOPY /E /I ..\LoopingAudioConverter\tools_linux-i686 linux32\tools
XCOPY ..\LoopingAudioConverter\About.html linux32
XCOPY /E /I %FROM%\*.exe linux64
XCOPY /E /I %FROM%\*.dll linux64
XCOPY /E /I ..\LoopingAudioConverter\tools_linux-amd64 linux64\tools
XCOPY ..\LoopingAudioConverter\About.html linux64
@PAUSE
