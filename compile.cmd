@echo off

rem SET MSBUILD=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
SET MSBUILD="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
SET OUTPUT=..\..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m
SET NUGET=Nuget\nuget.exe

echo v3.5 restore
echo.
%NUGET% restore Source\ManagedIrbis.3.5.sln
echo.
echo v3.5 DEBUG
echo.
%MSBUILD% Source\ManagedIrbis.3.5.sln /p:Configuration=Debug    /t:Rebuild %PARAMS%

echo.
echo v3.5 RELEASE
echo.
%MSBUILD% Source\ManagedIrbis.3.5.sln /p:Configuration=Release  /t:Rebuild %PARAMS%

echo v4.0 restore
echo.
%NUGET% restore Source\ManagedIrbis.4.0.sln
echo.
echo v4.0 DEBUG
echo.
%MSBUILD% Source\ManagedIrbis.4.0.sln /p:Configuration=Debug    /t:Rebuild %PARAMS%

echo.
echo v4.0 RELEASE
echo.
%MSBUILD% Source\ManagedIrbis.4.0.sln /p:Configuration=Release  /t:Rebuild %PARAMS%

echo v4.5 restore
echo.
%NUGET% restore Source\ManagedIrbis.4.5.sln
echo.
echo v4.5 DEBUG
echo.
%MSBUILD% Source\ManagedIrbis.4.5.sln /p:Configuration=Debug    /t:Rebuild /p:OutputPath=%OUTPUT%\Debug45 %PARAMS%

echo.
echo v4.5 RELEASE
echo.
%MSBUILD% Source\ManagedIrbis.4.5.sln /p:Configuration=Release  /t:Rebuild /p:OutputPath=%OUTPUT%\Release45 %PARAMS%

echo v4.6 restore
echo.
%NUGET% restore Source\ManagedIrbis.4.6.sln
echo.
echo v4.6 DEBUG
echo.
%MSBUILD% Source\ManagedIrbis.4.6.sln /p:Configuration=Debug    /t:Rebuild %PARAMS%

echo.
echo v4.6 RELEASE
echo.
%MSBUILD% Source\ManagedIrbis.4.6.sln /p:Configuration=Release  /t:Rebuild %PARAMS%

echo v4.7 restore
echo.
%NUGET% restore Source\ManagedIrbis.4.6.sln
echo.
echo v4.7 DEBUG
echo.
%MSBUILD% Source\ManagedIrbis.4.7.sln /p:Configuration=Debug    /t:Rebuild %PARAMS%

echo.
echo v4.7 RELEASE
echo.
%MSBUILD% Source\ManagedIrbis.4.7.sln /p:Configuration=Release  /t:Rebuild %PARAMS%
