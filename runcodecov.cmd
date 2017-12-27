@echo off

cd Source\UnitTests\bin\Debug
C:\ProgramData\chocolatey\bin\OpenCover.Console.exe  -register:user -target:"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe" -targetargs:"/testcontainer:UnitTests.dll /runconfig:codecov.testsettings" -filter:"+[AM.Core]AM.* +[ManagedIrbis]*" -excludebyattribute:"*.ExcludeFromCodeCoverage*" -output:coverage.xml
C:\ProgramData\chocolatey\bin\codecov.exe -f coverage.xml  -t 4410b363-3e98-4950-81cc-f1a7ebd42395

cd ..\..\..\..

rem pause