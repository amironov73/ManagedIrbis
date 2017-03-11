@echo off

SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

dotnet restore Source\NetCore\AM.Core\project.json
dotnet restore Source\NetCore\ManagedIrbis\project.json
dotnet restore Source\NetCore\RestfulIrbis\project.json

"%MSBUILD%" Source\NetCore\AM.Core\AM.Core.xproj           /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore /p:DefineConstants="DEBUG;NETCORE" %PARAMS%
"%MSBUILD%" Source\NetCore\ManagedIrbis\ManagedIrbis.xproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore /p:DefineConstants="DEBUG;NETCORE" %PARAMS%
"%MSBUILD%" Source\NetCore\RestfulIrbis\RestfulIrbis.xproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore /p:DefineConstants="DEBUG;NETCORE" %PARAMS%

"%MSBUILD%" Source\NetCore\AM.Core\AM.Core.xproj           /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore /p:DefineConstants="NETCORE"     %PARAMS%
"%MSBUILD%" Source\NetCore\ManagedIrbis\ManagedIrbis.xproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore /p:DefineConstants="NETCORE"     %PARAMS%
"%MSBUILD%" Source\NetCore\RestfulIrbis\RestfulIrbis.xproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore /p:DefineConstants="NETCORE"     %PARAMS%
