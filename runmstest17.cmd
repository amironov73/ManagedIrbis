@echo off

cd Source\UnitTests\bin\Debug

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\MSTest.exe" /runconfig:codecov.testsettings /testcontainer:UnitTests.dll

cd ..\..\..\..

rem pause