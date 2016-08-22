@echo off

SET BIN=..\Binaries
SET BUILD=Debug

FOR %%P IN (JetBrains.Annotations,MoonSharp.Interpreter,Newtonsoft.Json) DO (
  FOR %%B IN (35,40,45,451,46,461) DO (
    DEL %BIN%\%BUILD%%%B\%%P.* > nul 2> nul
  )
)

DEL %BIN%\%BUILD%35\System.Threading.* > nul 2> nul

del /q *.nupkg > nul 2> nul

if not exist lib mkdir lib   > nul 2> nul
if not exist tools mkdir tools > nul 2> nul

cd lib

if not exist net35  mkdir net35  > nul 2> nul
if not exist net40  mkdir net40  > nul 2> nul
if not exist net45  mkdir net45  > nul 2> nul
if not exist net451 mkdir net451 > nul 2> nul
if not exist net46  mkdir net46  > nul 2> nul
if not exist net461 mkdir net461 > nul 2> nul

del /s /q *.* > nul 2> nul
cd ..

copy %BIN%\%BUILD%35 lib\net35\  > nul
copy %BIN%\%BUILD%40 lib\net40\  > nul
copy %BIN%\%BUILD%45 lib\net45\  > nul
copy %BIN%\%BUILD%45 lib\net451\ > nul
copy %BIN%\%BUILD%45 lib\net46\  > nul
copy %BIN%\%BUILD%45 lib\net461\ > nul

PatchNugetVersion.exe %BIN%\%BUILD%40\AM.Core.dll ManagedIrbis.nuspec

nuget.exe pack