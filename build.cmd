@echo Off
SET config=%1
if "%config%" == "" (
   SET config=Release
)

SET msbuild="%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

%msbuild% "%~dp0src\AutoTest.ArgumentNullException.sln" /t:Rebuild /p:Configuration="%config%"
