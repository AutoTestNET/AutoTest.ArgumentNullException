@call build
@src\.nuget\NuGet.exe pack .\nuget\AutoTest.ArgumentNullException.nuspec -OutputDirectory .\nuget
@src\.nuget\NuGet.exe pack .\nuget\AutoTest.ArgumentNullException.Xunit.nuspec -OutputDirectory .\nuget
