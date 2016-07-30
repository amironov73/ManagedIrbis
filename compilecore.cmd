@echo off


SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

"%MSBUILD%" AM.Core\AM.Core.NetCore.xproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore  /p:DefineConstants="NETCORE"
"%MSBUILD%" ManagedIrbis\ManagedIrbis.NetCore.xproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore /p:DefineConstants="NETCORE"

"%MSBUILD%" AM.Core\AM.Core.NetCore.xproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore /p:DefineConstants="NETCORE"
"%MSBUILD%" ManagedIrbis\ManagedIrbis.NetCore.xproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore /p:DefineConstants="NETCORE"
