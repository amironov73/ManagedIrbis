@echo off

SET MSBUILD=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
SET OUTPUT=..\Binaries

%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35
%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35

%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40
%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40

%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45
%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45


%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=%OUTPUT%\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"

%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=%OUTPUT%\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"

%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=%OUTPUT%\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
