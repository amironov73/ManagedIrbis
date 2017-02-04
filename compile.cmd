@echo off

SET MSBUILD=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
rem SET MSBUILD=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET OUTPUT=..\..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

FOR %%P IN (AM.Core,ManagedIrbis,AM.Rfid,AM.Drawing,AM.Windows.Forms,,AM.Win32,IrbisUI,AM.Suggestions,AM.AOT,AM.Ocr,RestfulIrbis) DO (
  FOR %%B IN (Debug,Release) DO (
    CALL :BUILDLIB %%P %%B 35  v3.5   "CLASSIC;DESKTOP;FW35"
    CALL :BUILDLIB %%P %%B 40  v4.0   "CLASSIC;DESKTOP;FW4;FW40"
    CALL :BUILDLIB %%P %%B 45  v4.5   "CLASSIC;DESKTOP;FW4;FW45"
rem CALL :BUILDLIB %%P %%B 451 v4.5.1 "CLASSIC;DESKTOP;FW4;FW40;FW45;FW451"
    CALL :BUILDLIB %%P %%B 46  v4.6   "CLASSIC;DESKTOP;FW4;FW46"
rem CALL :BUILDLIB %%P %%B 461 v4.6.1 "CLASSIC;DESKTOP;FW4;FW46;FW461"
rem CALL :BUILDLIB %%P %%B 461 v4.6.2 "CLASSIC;DESKTOP;FW4;FW46;FW462"
  )
)

FOR %%P IN (mx64,PftBench) DO (
  FOR %%B IN (Debug,Release) DO (
    CALL :BUILDAPP %%P %%B 40  v4.0   "CLASSIC;DESKTOP;FW4;FW40"
    CALL :BUILDAPP %%P %%B 45  v4.5   "CLASSIC;DESKTOP;FW4;FW45"
    CALL :BUILDAPP %%P %%B 46  v4.6   "CLASSIC;DESKTOP;FW4;FW46"
  )
)

EXIT

:BUILDLIB
ECHO BUILD %1 %2 %4
"%MSBUILD%" Source\Classic\Libs\%1\%1.csproj /p:Configuration=%2 /p:TargetFrameworkVersion=%4  /p:OutputPath=%OUTPUT%\%2%3 %PARAMS% /p:DefineConstants=%5 /t:Rebuild
GOTO :END

:BUILDAPP
ECHO BUILD %1 %2 %4
"%MSBUILD%" Source\Classic\Apps\%1\%1.csproj /p:Configuration=%2 /p:TargetFrameworkVersion=%4  /p:OutputPath=%OUTPUT%\%2%3 %PARAMS% /p:DefineConstants=%5 /t:Rebuild
GOTO :END

:END
