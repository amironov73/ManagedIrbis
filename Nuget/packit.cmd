@echo off

SET BIN=..\Binaries
SET BUILD=Debug

DEL /q *.nupkg > nul 2> nul

IF not exist lib     mkdir lib     > nul 2> nul

CALL :BUILD AM.Core            core
CALL :BUILD ManagedIrbis       core
CALL :BUILD AM.Drawing         no
CALL :BUILD AM.Rfid            no
CALL :BUILD AM.Windows.Forms   no
CALL :BUILD AM.Win32           no
CALL :BUILD IrbisUI            no

GOTO :END

:BUILD
ECHO BUILD %1

cd lib

del /s /q *.*   > nul 2> nul
rmdir /s /q *.* > nul 2> nul

IF not exist net35 mkdir net35 > nul 2> nul
IF not exist net40 mkdir net40 > nul 2> nul
IF not exist net45 mkdir net45 > nul 2> nul

IF %2==core mkdir netstandard1.0  > nul 2> nul
IF %2==no   rmdir netstandard1.0  > nul 2> nul

cd ..

copy %BIN%\%BUILD%35\%1.* lib\net35\  > nul
copy %BIN%\%BUILD%40\%1.* lib\net40\  > nul
copy %BIN%\%BUILD%45\%1.* lib\net45\  > nul

copy %BIN%\%BUILD%35\System.Threading.* lib\net35\ > nul

IF %2==core copy %BIN%\%BUILD%Core\%1\bin\%BUILD%\netstandard1.6\%1.* lib\netstandard1.0 > nul
DEL lib\netstandard1.0\*.json > nul 2> nul

PatchNugetVersion.exe %BIN%\%BUILD%40\AM.Core.dll %1.nuspec

nuget.exe pack %1.nuspec

echo.

GOTO :END

:END

