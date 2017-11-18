@echo off

cd Source\UnitTests\bin\Debug
C:\ProgramData\chocolatey\bin\OpenCover.Console.exe  -register:user -target:"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe" -targetargs:"/testcontainer:UnitTests.dll" -filter:"+[AM.Core.*]* +[ManagedIrbis]*" -output:coverage.xml
C:\ProgramData\chocolatey\bin\codecov.exe -f coverage.xml

pause