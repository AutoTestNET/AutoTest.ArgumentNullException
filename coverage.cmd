@echo Off

SET msbuild="%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

if "%1" == "" (
   call "%~dp0build.cmd" Debug
)

echo %msbuild% "%~dp0src\Build\ExecuteUnitTests.proj" /nologo /verbosity:m /t:Rebuild
%msbuild% "%~dp0src\Build\ExecuteUnitTests.proj" /nologo /verbosity:m /t:Rebuild
