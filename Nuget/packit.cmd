@echo off

SET BIN=..\Binaries
SET BUILD=Debug

DEL /q *.nupkg > nul 2> nul

IF not exist lib mkdir lib   > nul 2> nul
IF not exist tools mkdir tools > nul 2> nul

CALL :BUILD AM.Core
CALL :BUILD ManagedIrbis
CALL :BUILD AM.Drawing
CALL :BUILD AM.Rfid
CALL :BUILD AM.Windows.Forms
CALL :BUILD AM.Win32
CALL :BUILD IrbisUI

GOTO :END

:BUILD
ECHO BUILD %1

cd lib

IF not exist net35  mkdir net35  > nul 2> nul
IF not exist net40  mkdir net40  > nul 2> nul
IF not exist net45  mkdir net45  > nul 2> nul

del /s /q *.* > nul 2> nul
cd ..

copy %BIN%\%BUILD%35\%1.* lib\net35\  > nul
copy %BIN%\%BUILD%40\%1.* lib\net40\  > nul
copy %BIN%\%BUILD%45\%1.* lib\net45\  > nul

PatchNugetVersion.exe %BIN%\%BUILD%40\AM.Core.dll %1.nuspec

nuget.exe pack %1.nuspec

echo.

GOTO :END

:END

