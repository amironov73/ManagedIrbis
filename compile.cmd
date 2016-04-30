@echo off

SET MSBUILD=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
SET OUTPUT=..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

FOR %%P IN (AM.Core,ManagedClient,IrbisUI,MoonIrbis) DO (
  FOR %%T IN (v3.5,v4.0,v4.5) DO (
     FOR %%B IN (Debug,Release) DO (
       CALL :CLEAN %%P %%B %%T
     )
  )
)

FOR %%P IN (AM.Core,ManagedClient,IrbisUI,MoonIrbis) DO (
  FOR %%B IN (Debug,Release) DO (
    CALL :BUILD %%P %%B v3.5 "FW461;FW46;FW452;FW451;FW45;FW40;FW35"
    CALL :BUILD %%P %%B v4.0 "FW461;FW46;FW452;FW451;FW45;FW40"
    CALL :BUILD %%P %%B v4.5 "FW461;FW46;FW452;FW451;FW45"
  )
)

EXIT

:CLEAN

ECHO CLEAN %1 %2 %3
%MSBUILD% %1\%1.csproj /t:Clean /p:Configuration=%2 /p:TargetFrameworkVersion=%3 /p:OutputPath=%OUTPUT%\%2 %PARAMS%
GOTO :END

:BUILD
ECHO BUILD %1 %2 %3
%MSBUILD% %1\%1.csproj /p:Configuration=%2 /p:TargetFrameworkVersion=%3  /p:OutputPath=%OUTPUT%\%2 %PARAMS% /p:DefineConstants=%4
GOTO :END

:END