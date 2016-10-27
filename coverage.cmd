@SETLOCAL
@SET config=%1
@IF ["%config%"] == [""] (
   SET config=Release
)

@FOR /r %%F IN (*OpenCover.Console.exe) DO @SET cover_exe=%%F
@IF NOT EXIST "%cover_exe%" (
   echo Unable to find OpenCover console.
   EXIT /B 2
)
::@echo %cover_exe%

@FOR /r %%F IN (*ReportGenerator.exe) DO @SET report_exe=%%F
@IF NOT EXIST "%report_exe%" (
   echo Unable to find ReportGenerator.
   EXIT /B 2
)
::@echo %report_exe%

@FOR /r %%F IN (*xunit.console.exe) DO @SET xunit_exe=%%F
@IF NOT EXIST "%xunit_exe%" (
   echo Unable to find xunit console runner.
   EXIT /B 2
)
::@echo %xunit_exe%

@SET results_path=%~dp0src\Tests\TestResults
@SET test_assemblies=%~dp0src\Tests\AutoTest.ArgumentNullException.Tests\bin\%config%\AutoTest.ArgumentNullException.Tests.dll
@SET test_assemblies=%test_assemblies% %~dp0src\Tests\AutoTest.ExampleLibrary.Tests\bin\%config%\AutoTest.ExampleLibrary.Tests.dll
@SET spec_results=%results_path%\Specifications.html
@SET xunit_results=%results_path%\Xunit.Tests.html
@SET coverage_filter=+[AutoTest.*]* -[*.Tests]* -[AutoTest.ExampleLibrary]* -[AutoTest.*]AutoTest.ArgNullEx.Framework.*
@SET coverage_results=%results_path%\Test.Coverage.xml

@IF NOT EXIST "%results_path%" MD "%results_path%"
::@echo "%xunit_exe%" %test_assemblies% -noshadow -html "%xunit_results%"
::@"%xunit_exe%" %test_assemblies% -noshadow -html "%xunit_results%"

@echo "%cover_exe%" -register:user "-target:%xunit_exe%" "-targetargs:%test_assemblies% -noshadow -html %xunit_results%" -returntargetcode -filter:^"%coverage_filter%^" "-output:%coverage_results%"
@"%cover_exe%" -register:user "-target:%xunit_exe%" "-targetargs:%test_assemblies% -noshadow -html %xunit_results%" -returntargetcode -filter:^"%coverage_filter%^" "-output:%coverage_results%"
@IF ERRORLEVEL 1 (
   echo Error executing the xunit tests
   EXIT /B 2
)

@echo "%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%" -reporttypes:XmlSummary
@"%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%" -reporttypes:XmlSummary

@echo "%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%\Report" -reporttypes:Html
@"%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%\Report" -reporttypes:Html
