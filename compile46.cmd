@echo off


SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\Debug46   /property:TargetFrameworkVersion=v4.6 /p:DefineConstants="FW45;FW46"

"%MSBUILD%" ManagedIrbis\ManagedIrbis.csproj /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\Release46 /property:TargetFrameworkVersion=v4.6 /p:DefineConstants="FW45;FW46"
