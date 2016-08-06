@echo off

SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

dotnet restore Sources\NetCore\AM.Core\project.json
dotnet restore Sources\NetCore\ManagedIrbis\project.json

"%MSBUILD%" Sources\NetCore\AM.Core\AM.Core.xproj           /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore /p:DefineConstants="DEBUG;NETCORE" %PARAMS%
"%MSBUILD%" Sources\NetCore\ManagedIrbis\ManagedIrbis.xproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore /p:DefineConstants="DEBUG;NETCORE" %PARAMS%

"%MSBUILD%" Sources\NetCore\AM.Core\AM.Core.xproj           /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore /p:DefineConstants="NETCORE"     %PARAMS%
"%MSBUILD%" Sources\NetCore\ManagedIrbis\ManagedIrbis.xproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore /p:DefineConstants="NETCORE"     %PARAMS%
