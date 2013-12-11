@echo Off

SET config=%1

IF ["%1"] == [""] (
   SET config=Release
)

call "%~dp0build.cmd" %config%

@echo "%~dp0src\.nuget\NuGet.exe" pack "%~dp0src\AutoTest.ArgumentNullException\AutoTest.ArgumentNullException.csproj" -Properties Configuration=%config% -NonInteractive -Symbols -OutputDirectory "%~dp0src\AutoTest.ArgumentNullException\bin\%config%"
"%~dp0src\.nuget\NuGet.exe" pack "%~dp0src\AutoTest.ArgumentNullException\AutoTest.ArgumentNullException.csproj" -Properties Configuration=%config% -NonInteractive -Symbols -OutputDirectory "%~dp0src\AutoTest.ArgumentNullException\bin\%config%"

@echo "%~dp0src\.nuget\NuGet.exe" pack "%~dp0src\AutoTest.ArgumentNullException.Xunit\AutoTest.ArgumentNullException.Xunit.csproj" -Properties Configuration=%config% -NonInteractive -Symbols -OutputDirectory "%~dp0src\AutoTest.ArgumentNullException.Xunit\bin\%config%"
"%~dp0src\.nuget\NuGet.exe" pack "%~dp0src\AutoTest.ArgumentNullException.Xunit\AutoTest.ArgumentNullException.Xunit.csproj" -Properties Configuration=%config% -NonInteractive -Symbols -OutputDirectory "%~dp0src\AutoTest.ArgumentNullException.Xunit\bin\%config%"
