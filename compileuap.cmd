@echo off

cd Source\Universal

SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m
SET NUGET=..\..\Nuget\nuget.exe

%NUGET% restore

"%MSBUILD%" Libs\AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugUniversal /p:DefineConstants="DEBUG;UAP" %PARAMS%
"%MSBUILD%" Libs\ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugUniversal /p:DefineConstants="DEBUG;UAP" %PARAMS%
"%MSBUILD%" Libs\AM.AOT\AM.AOT.csproj             /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugUniversal /p:DefineConstants="DEBUG;UAP" %PARAMS%
"%MSBUILD%" Libs\RestfulIrbis\RestfulIrbis.csproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugUniversal /p:DefineConstants="DEBUG;UAP" %PARAMS%

"%MSBUILD%" Libs\AM.Core\AM.Core.csproj           /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseUniversal /p:DefineConstants="UAP"     %PARAMS%
"%MSBUILD%" Libs\ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseUniversal /p:DefineConstants="UAP"     %PARAMS%
"%MSBUILD%" Libs\AM.AOT\AM.AOT.csproj             /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseUniversal /p:DefineConstants="UAP"     %PARAMS%
"%MSBUILD%" Libs\RestfulIrbis\RestfulIrbis.csproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseUniversal /p:DefineConstants="UAP"     %PARAMS%

cd ..\..
