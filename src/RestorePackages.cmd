:: Using workaround 2 from:
:: http://blogs.msdn.com/b/dotnet/archive/2013/06/12/nuget-package-restore-issues.aspx

"%~dp0.nuget\NuGet.exe" install "%~dp0.nuget\packages.config" -OutputDirectory "%~dp0packages"
"%~dp0.nuget\NuGet.exe" install "%~dp0AutoTest.ArgumentNullException\packages.config" -OutputDirectory "%~dp0packages"
"%~dp0.nuget\NuGet.exe" install "%~dp0AutoTest.ArgumentNullException.Xunit\packages.config" -OutputDirectory "%~dp0packages"
"%~dp0.nuget\NuGet.exe" install "%~dp0Tests\AutoTest.ArgumentNullException.Tests\packages.config" -OutputDirectory "%~dp0packages"
"%~dp0.nuget\NuGet.exe" install "%~dp0Tests\AutoTest.ExampleLibrary.Tests\packages.config" -OutputDirectory "%~dp0packages"
