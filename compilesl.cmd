@echo off

cd Source\Silverlight

SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m
SET NUGET=..\..\Nuget\nuget.exe

%NUGET% restore

"%MSBUILD%" AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugSL50 /p:DefineConstants="DEBUG;SILVERLIGHT" %PARAMS%
"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugSL50 /p:DefineConstants="DEBUG;SILVERLIGHT" %PARAMS%

"%MSBUILD%" AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseSL50 /p:DefineConstants="SILVERLIGHT"     %PARAMS%
"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseSL50 /p:DefineConstants="SILVERLIGHT"     %PARAMS%

cd ..\..
