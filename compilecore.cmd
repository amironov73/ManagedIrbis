@echo off

cd Source/NetCore

SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=../../../Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

dotnet restore

dotnet msbuild /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore   %PARAMS%
dotnet msbuild /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore %PARAMS%

cd ../..