@echo off

SET MSBUILD=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
SET OUTPUT=..\Binaries
SET PARAMS=/consoleloggerparameters:ErrorsOnly /m

%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    %PARAMS%
%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  %PARAMS%

%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    %PARAMS%
%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  %PARAMS%

%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    %PARAMS%
%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  %PARAMS%


%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35" %PARAMS%
%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35" %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35" %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35" %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35" %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35" %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35" %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35" %PARAMS%

%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"      %PARAMS%
%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"      %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"      %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"      %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"      %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"      %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"      %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"      %PARAMS%

%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"           %PARAMS%
%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"           %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"           %PARAMS%
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"           %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"           %PARAMS%
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"           %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"           %PARAMS%
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"           %PARAMS%
