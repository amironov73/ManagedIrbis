@echo off

SET BIN=..\Binaries
SET BUILD=Debug

del /q *.nupkg > nul

cd lib
del /s /q *.* > nul
cd ..

copy %BIN%\%BUILD%35 lib\net35\ > nul
copy %BIN%\%BUILD%40 lib\net40\ > nul
copy %BIN%\%BUILD%45 lib\net45\ > nul

cd lib
del /s /q MoonIrbis.* > nul
cd ..

PatchNugetVersion.exe %BIN%\%BUILD%40\AM.Core.dll ManagedClient.nuspec

nuget.exe pack