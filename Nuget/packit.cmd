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
IF %1==AM.Rfid (
copy %BIN%\%BUILD%35\FeCom.dll                lib\net35 > nul
copy %BIN%\%BUILD%35\FedmIscCoreVC80.dll      lib\net35 > nul
copy %BIN%\%BUILD%35\FedmIscMyAxxessVC80.dll  lib\net35 > nul
copy %BIN%\%BUILD%35\fefu.dll                 lib\net35 > nul
copy %BIN%\%BUILD%35\FeIsc.dll                lib\net35 > nul
copy %BIN%\%BUILD%35\fetcl.dll                lib\net35 > nul
copy %BIN%\%BUILD%35\fetcp.dll                lib\net35 > nul
copy %BIN%\%BUILD%35\feusb.dll                lib\net35 > nul
copy %BIN%\%BUILD%35\OBIDISC4NET.dll          lib\net35 > nul
copy %BIN%\%BUILD%35\OBIDISC4NETnative.dll    lib\net35 > nul
copy %BIN%\%BUILD%35\pcsc-sharp.dll           lib\net35 > nul
copy %BIN%\%BUILD%35\pcsc-sharp.pdb           lib\net35 > nul
copy %BIN%\%BUILD%35\pcsc-sharp.xml           lib\net35 > nul

copy %BIN%\%BUILD%40\FeCom.dll                lib\net40 > nul
copy %BIN%\%BUILD%40\FedmIscCoreVC100.dll     lib\net40 > nul
copy %BIN%\%BUILD%40\FedmIscMyAxxessVC100.dll lib\net40 > nul
copy %BIN%\%BUILD%40\fefu.dll                 lib\net40 > nul
copy %BIN%\%BUILD%40\FeIsc.dll                lib\net40 > nul
copy %BIN%\%BUILD%40\fetcl.dll                lib\net40 > nul
copy %BIN%\%BUILD%40\fetcp.dll                lib\net40 > nul
copy %BIN%\%BUILD%40\feusb.dll                lib\net40 > nul
copy %BIN%\%BUILD%40\OBIDISC4NET.dll          lib\net40 > nul
copy %BIN%\%BUILD%40\OBIDISC4NETnative.dll    lib\net40 > nul
copy %BIN%\%BUILD%40\pcsc-sharp.dll           lib\net40 > nul
copy %BIN%\%BUILD%40\pcsc-sharp.pdb           lib\net40 > nul
copy %BIN%\%BUILD%40\pcsc-sharp.xml           lib\net40 > nul

copy %BIN%\%BUILD%45\FeCom.dll                lib\net45 > nul
copy %BIN%\%BUILD%45\FedmIscCoreVC100.dll     lib\net45 > nul
copy %BIN%\%BUILD%45\FedmIscMyAxxessVC100.dll lib\net45 > nul
copy %BIN%\%BUILD%45\fefu.dll                 lib\net45 > nul
copy %BIN%\%BUILD%45\FeIsc.dll                lib\net45 > nul
copy %BIN%\%BUILD%45\fetcl.dll                lib\net45 > nul
copy %BIN%\%BUILD%45\fetcp.dll                lib\net45 > nul
copy %BIN%\%BUILD%45\feusb.dll                lib\net45 > nul
copy %BIN%\%BUILD%45\OBIDISC4NET.dll          lib\net45 > nul
copy %BIN%\%BUILD%45\OBIDISC4NETnative.dll    lib\net45 > nul
copy %BIN%\%BUILD%45\pcsc-sharp.dll           lib\net45 > nul
copy %BIN%\%BUILD%45\pcsc-sharp.pdb           lib\net45 > nul
copy %BIN%\%BUILD%45\pcsc-sharp.xml           lib\net45 > nul
)

PatchNugetVersion.exe %BIN%\%BUILD%40\AM.Core.dll %1.nuspec

nuget.exe pack %1.nuspec

echo.

GOTO :END

:END

