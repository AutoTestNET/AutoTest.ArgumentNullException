
@"%~dp0src\.nuget\NuGet.exe" push "%~dp0nuget\AutoTest.ArgumentNullException.0.2.0.nupkg"  -Source https://staging.nuget.org
@"%~dp0src\.nuget\NuGet.exe" push "%~dp0nuget\AutoTest.ArgumentNullException.Xunit.0.2.0.nupkg"  -Source https://staging.nuget.org

@pause
