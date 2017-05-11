@echo off

SET BIN=..\Binaries
SET BUILD=Debug

DEL /q *.nupkg > nul 2> nul

IF not exist lib     mkdir lib     > nul 2> nul

CALL :BUILD AM.Core            core sl droid uap win81 pcl
CALL :BUILD ManagedIrbis       core sl droid uap win81 pcl
CALL :BUILD AM.Drawing         no   no no    no  no    no
CALL :BUILD AM.Rfid            no   no no    no  no    no
CALL :BUILD AM.Windows.Forms   no   no no    no  no    no
CALL :BUILD AM.Win32           no   no no    no  no    no
CALL :BUILD IrbisUI            no   no no    no  no    no
CALL :BUILD AM.Suggestions     no   no no    no  no    no
CALL :BUILD AM.AOT             core no droid uap no    pcl
CALL :BUILD AM.Ocr             no   no no    no  no    no
CALL :BUILD RestfulIrbis       core no droid uap no    pcl
CALL :BUILD IrbisInterop       no   no no    no  no    no
CALL :BUILD ManagedIrbis.Isis  no   no no    no  no    no

GOTO :END

:BUILD
ECHO BUILD %1

cd lib

del /s /q *.*   > nul 2> nul
rmdir /s /q *.* > nul 2> nul

IF not exist net35 mkdir net35 > nul 2> nul
IF not exist net40 mkdir net40 > nul 2> nul
IF not exist net45 mkdir net45 > nul 2> nul

IF %2==core  mkdir netstandard1.6        > nul 2> nul
IF %2==no    rmdir netstandard1.6        > nul 2> nul

IF %3==sl    mkdir sl50                  > nul 2> nul
IF %3==no    rmdir sl50                  > nul 2> nul

IF %4==droid mkdir MonoAndroid           > nul 2> nul
IF %4==no    rmdir MonoAndroid           > nul 2> nul

IF %5==uap   mkdir uap                   > nul 2> nul
IF %5==no    rmdir uap                   > nul 2> nul

IF %6==win81 mkdir win81                 > nul 2> nul
IF %6==no    rmdir win81                 > nul 2> nul

IF %7==pcl   mkdir "portable-net45+win8" > nul 2> nul
IF %7==no    rmdir "portable-net45+win8" > nul 2> nul

cd ..

copy %BIN%\%BUILD%35\%1.* lib\net35\  > nul
copy %BIN%\%BUILD%40\%1.* lib\net40\  > nul
copy %BIN%\%BUILD%45\%1.* lib\net45\  > nul

RMDIR lib\net35\ru > nul 2> nul
RMDIR lib\net40\ru > nul 2> nul
RMDIR lib\net45\ru > nul 2> nul

IF EXIST %BIN%\%BUILD%35\ru\%1.resources.dll (
MKDIR lib\net35\ru > nul 2> nul
copy %BIN%\%BUILD%35\ru\%1.resources.dll lib\net35\ru > nul 2> nul
)
IF EXIST %BIN%\%BUILD%40\ru\%1.resources.dll (
MKDIR lib\net40\ru > nul 2> nul
copy %BIN%\%BUILD%40\ru\%1.resources.dll lib\net40\ru > nul 2> nul
)
IF EXIST %BIN%\%BUILD%45\ru\%1.resources.dll (
MKDIR lib\net45\ru > nul 2> nul
copy %BIN%\%BUILD%45\ru\%1.resources.dll lib\net45\ru > nul 2> nul
)

copy %BIN%\%BUILD%35\System.Threading.* lib\net35\ > nul

IF %2==core  copy %BIN%\%BUILD%Core\%1.*                               lib\netstandard1.6        > nul
IF %3==sl    copy %BIN%\%BUILD%SL50\%1.*                               lib\sl50                  > nul
IF %4==droid copy %BIN%\%BUILD%Droid\%1.*                              lib\MonoAndroid           > nul
IF %5==uap   copy %BIN%\%BUILD%Universal\%1.*                          lib\uap                   > nul
IF %6==win81 copy %BIN%\%BUILD%Windows81\%1.*                          lib\win81                 > nul
IF %7==pcl   copy %BIN%\%BUILD%Portable\%1.*                           "lib\portable-net45+win8" > nul

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


IF %1==IrbisInterop (
copy ..\Source\Delphi\Irbis65\Irbis65.dll     lib\net35 > nul
copy ..\Source\Delphi\Irbis65\Irbis65.dll     lib\net40 > nul
copy ..\Source\Delphi\Irbis65\Irbis65.dll     lib\net45 > nul
)

IF %1==ManagedIrbis.Isis (
copy %BIN%\%BUILD%35\ISIS32.DLL               lib\net35 > nul
copy %BIN%\%BUILD%40\ISIS32.DLL               lib\net40 > nul
copy %BIN%\%BUILD%45\ISIS32.DLL               lib\net45 > nul
)


PatchNugetVersion.exe %BIN%\%BUILD%40\AM.Core.dll %1.nuspec

nuget.exe pack %1.nuspec

echo.

GOTO :END

:END

