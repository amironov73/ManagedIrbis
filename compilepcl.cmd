@echo off

cd Source\Portable

SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m
SET NUGET=..\..\Nuget\nuget.exe

%NUGET% restore

"%MSBUILD%" AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugDroid /p:DefineConstants="DEBUG;PORTABLE" %PARAMS%
"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugDroid /p:DefineConstants="DEBUG;PORTABLE" %PARAMS%

"%MSBUILD%" AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseDroid /p:DefineConstants="PORTABLE"     %PARAMS%
"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseDroid /p:DefineConstants="PORTABLE"     %PARAMS%

cd ..\..
