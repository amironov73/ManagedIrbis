@echo off

cd Source/NetCore2

SET OUTPUT=../../../Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

dotnet restore

dotnet msbuild /target:ReBuild /property:Configuration=Debug   /property:OutputPath=%OUTPUT%\DebugCore2   %PARAMS%
dotnet msbuild /target:ReBuild /property:Configuration=Release /property:OutputPath=%OUTPUT%\ReleaseCore2 %PARAMS%

cd ../..