::C:\git\AutoTest.ArgumentNullException\src\packages\OpenCover.4.5.1403\OpenCover.Console.exe
::-register:user
::"-target:C:\git\AutoTest.ArgumentNullException\src\packages\xunit.runners.1.9.1\tools\xunit.console.clr4.exe"
::"-targetargs:C:\git\AutoTest.ArgumentNullException\src\Tests\Tests.xunit"
::"-filter:+[AutoTest.ArgumentNullException]* +[AutoTest.ArgumentNullException.Xunit]* +[AutoTest.ExampleLibrary]* -[AutoTest.ArgumentNullException.Xunit]System.Threading.Tasks.*"
::"-output:C:\git\AutoTest.ArgumentNullException\src\Tests\CoverageResults.xml"

C:\git\AutoTest.ArgumentNullException\src\packages\OpenCover.4.5.1403\OpenCover.Console.exe -register:user "-target:C:\git\AutoTest.ArgumentNullException\src\packages\xunit.runners.1.9.1\tools\xunit.console.clr4.exe" "-targetargs:C:\git\AutoTest.ArgumentNullException\src\Tests\Tests.xunit" "-filter:+[AutoTest.ArgumentNullException]* +[AutoTest.ArgumentNullException.Xunit]* +[AutoTest.ExampleLibrary]* -[AutoTest.ArgumentNullException.Xunit]System.Threading.Tasks.*" "-output:C:\git\AutoTest.ArgumentNullException\src\Tests\CoverageResults.xml"

::C:\git\AutoTest.ArgumentNullException\src\packages\ReportGenerator.1.8.1.0\ReportGenerator.exe
::"C:\git\AutoTest.ArgumentNullException\src\Tests\CoverageResults.xml"
::"C:\git\AutoTest.ArgumentNullException\src\Tests\Report"
::Html

C:\git\AutoTest.ArgumentNullException\src\packages\ReportGenerator.1.8.1.0\ReportGenerator.exe "C:\git\AutoTest.ArgumentNullException\src\Tests\CoverageResults.xml" "C:\git\AutoTest.ArgumentNullException\src\Tests\Report" Html

@pause
