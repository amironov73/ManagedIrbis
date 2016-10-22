@echo off

cd Source\Universal

SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m
SET NUGET=..\..\Nuget\nuget.exe

%NUGET% restore

"%MSBUILD%" AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugUniversal /p:DefineConstants="DEBUG;UAP" %PARAMS%
"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugUniversal /p:DefineConstants="DEBUG;UAP" %PARAMS%

"%MSBUILD%" AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseUniversal /p:DefineConstants="UAP"     %PARAMS%
"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseUniversal /p:DefineConstants="UAP"     %PARAMS%

cd ..\..
