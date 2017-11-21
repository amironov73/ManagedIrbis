@echo off

cd Source\UnitTests\bin\Debug

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\MSTest.exe" /testcontainer:UnitTests.dll /runconfig:codecov.testsettings

cd ..\..\..\..

rem pause