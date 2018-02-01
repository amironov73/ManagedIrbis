@echo off

rem SET MSBUILD=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
SET MSBUILD="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
SET BINARIES=Binaries
SET OUTPUT=..\..\..\..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m
SET NUGET=Nuget\nuget.exe
SET PATCH=Utils\PatchRuntimeVersion.exe

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
echo.
echo v3.5 patch
echo.
%PATCH% %BINARIES%\Debug35\*.exe.config   3.5
%PATCH% %BINARIES%\Release35\*.exe.config 3.5
echo.

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
echo.
echo v4.0 patch
echo.
%PATCH% %BINARIES%\Debug40\*.exe.config   4.0
%PATCH% %BINARIES%\Release40\*.exe.config 4.0
echo.

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
echo.
echo v4.5 patch
echo.
%PATCH% %BINARIES%\Debug45\*.exe.config   4.5
%PATCH% %BINARIES%\Release45\*.exe.config 4.5
echo.

goto :END

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
echo.
echo v4.6 patch
echo.
%PATCH% %BINARIES%\Debug46\*.exe.config   4.6
%PATCH% %BINARIES%\Release46\*.exe.config 4.6
echo.

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
echo.
echo v4.7 patch
echo.
%PATCH% %BINARIES%\Debug47\*.exe.config   4.7
%PATCH% %BINARIES%\Release47\*.exe.config 4.7
echo.

:END
