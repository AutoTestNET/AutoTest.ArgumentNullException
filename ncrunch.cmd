@call "%~dp0RestorePackages.cmd"
@echo "%ProgramFiles(x86)%\Remco Software\NCrunch Console Tool\ncrunch.exe" "%~dp0src\AutoTest.ArgumentNullException.sln" /o "%~dp0src\TestResults"
@"%ProgramFiles(x86)%\Remco Software\NCrunch Console Tool\ncrunch.exe" "%~dp0src\AutoTest.ArgumentNullException.sln" /o "%~dp0src\TestResults"
