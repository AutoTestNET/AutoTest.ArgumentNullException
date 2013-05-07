@call "%~dp0build"
@"%~dp0src\.nuget\NuGet.exe" pack "%~dp0nuget\AutoTest.ArgumentNullException.nuspec" -OutputDirectory "%~dp0nuget"
@"%~dp0src\.nuget\NuGet.exe" pack "%~dp0nuget\AutoTest.ArgumentNullException.Xunit.nuspec" -OutputDirectory "%~dp0nuget"
