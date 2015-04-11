@echo Off

CALL "%~dp0setmsbuild.cmd"

if "%1" == "" (
   call "%~dp0build.cmd" Debug
)

echo %msbuild% "%~dp0src\Build\ExecuteUnitTests.proj" /nologo /verbosity:m /t:Rebuild /p:ExcludeXUnitTests=True
%msbuild% "%~dp0src\Build\ExecuteUnitTests.proj" /nologo /verbosity:m /t:Rebuild /p:ExcludeXUnitTests=True
