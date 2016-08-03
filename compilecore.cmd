@echo off

SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

dotnet restore NetCore\AM.Core\project.json
dotnet restore NetCore\ManagedIrbis\project.json

"%MSBUILD%" NetCore\AM.Core\AM.Core.xproj           /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore /p:DefineConstants="DEBUG;NETCORE"
"%MSBUILD%" NetCore\ManagedIrbis\ManagedIrbis.xproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore /p:DefineConstants="DEBUG;NETCORE"

"%MSBUILD%" NetCore\AM.Core\AM.Core.xproj           /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore /p:DefineConstants="NETCORE"
"%MSBUILD%" NetCore\ManagedIrbis\ManagedIrbis.xproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore /p:DefineConstants="NETCORE"
