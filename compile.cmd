@echo off

SET MSBUILD=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
rem SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

FOR %%P IN (AM.Core,ManagedIrbis,AM.Rfid,AM.Drawing,IrbisUI) DO (
  FOR %%B IN (Debug,Release) DO (
    CALL :BUILD %%P %%B 35  v3.5   "FW35"
    CALL :BUILD %%P %%B 40  v4.0   "FW40"
    CALL :BUILD %%P %%B 45  v4.5   "FW40;FW45"
    CALL :BUILD %%P %%B 451 v4.5.1 "FW40;FW45;FW451"
    CALL :BUILD %%P %%B 46  v4.6   "FW40;FW45;FW46"
    CALL :BUILD %%P %%B 461 v4.6.1 "FW40;FW45;FW46;FW461"
  )
)

EXIT

:BUILD
ECHO BUILD %1 %2 %4
"%MSBUILD%" Sources\%1\%1.csproj /p:Configuration=%2 /p:TargetFrameworkVersion=%4  /p:OutputPath=%OUTPUT%\%2%3 %PARAMS% /p:DefineConstants=%5 /t:Rebuild
GOTO :END

:END