@echo off

SET BIN=..\Binaries
SET BUILD=Debug

del /q *.nupkg > nul 2> nul

if not exist lib mkdir lib   > nul 2> nul
if not exist tools mkdir tools > nul 2> nul

cd lib

if not exist net35 mkdir net35 > nul 2> nul
if not exist net40 mkdir net40 > nul 2> nul
if not exist net45 mkdir net45 > nul 2> nul

del /s /q *.* > nul 2> nul
cd ..

copy %BIN%\%BUILD%35 lib\net35\ > nul
copy %BIN%\%BUILD%40 lib\net40\ > nul
copy %BIN%\%BUILD%45 lib\net45\ > nul

cd lib
del /s /q MoonIrbis.* > nul
cd ..

PatchNugetVersion.exe %BIN%\%BUILD%40\AM.Core.dll ManagedClient.nuspec

nuget.exe pack