@echo off

cd Source\Windows81

SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m
SET NUGET=..\..\Nuget\nuget.exe

%NUGET% restore

"%MSBUILD%" AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugWindows81 /p:DefineConstants="DEBUG;WIN81" %PARAMS%
"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugWindows81 /p:DefineConstants="DEBUG;WIN81" %PARAMS%

"%MSBUILD%" AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseWindows81 /p:DefineConstants="WIN81"     %PARAMS%
"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseWindows81 /p:DefineConstants="WIN81"     %PARAMS%

cd ..\..
