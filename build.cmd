@echo Off
SET config=%1
if "%config%" == "" (
   SET config=Release
)

CALL "%~dp0setmsbuild.cmd"

call "%~dp0src\RestorePackages.cmd"
echo %msbuild% "%~dp0src\AutoTest.ArgumentNullException.sln" /nologo /verbosity:m /t:Rebuild /p:Configuration="%config%"
%msbuild% "%~dp0src\AutoTest.ArgumentNullException.sln" /nologo /verbosity:m /t:Rebuild /p:Configuration="%config%"
