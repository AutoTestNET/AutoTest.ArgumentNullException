@call "%~dp0setmsbuild.cmd"

@SETLOCAL

@SET CACHED_NUGET="%USERPROFILE%\.nuget\NuGet.exe"
@SET LOCAL_NUGET="%~dp0src\.nuget\NuGet.exe"

@IF EXIST %CACHED_NUGET% goto copynuget
@echo Downloading latest version of NuGet.exe...
@IF NOT EXIST "%USERPROFILE%\.nuget" md "%USERPROFILE%\.nuget"
powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'http://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile %CACHED_NUGET:"='%"

:copynuget
@IF EXIST %LOCAL_NUGET% goto restore
@IF NOT EXIST "%~dp0src\.nuget" md "%~dp0src\.nuget"
copy %CACHED_NUGET% %LOCAL_NUGET% > nul

:restore
%LOCAL_NUGET% restore "%~dp0src\AutoTest.ArgumentNullException.sln"
