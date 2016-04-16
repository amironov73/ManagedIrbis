@echo off

SET MSBUILD=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Debug35
%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Release35
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Debug35
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Release35
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Debug35
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Release35
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Debug35
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Release35

%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Debug40
%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Release40
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Debug40
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Release40
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Debug40
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Release40
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Debug40
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Release40

%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Debug45
%MSBUILD% AM.Core\AM.Core.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Release45
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Debug45
%MSBUILD% ManagedClient\ManagedClient.csproj /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Release45
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Debug45
%MSBUILD% IrbisUI\IrbisUI.csproj             /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Release45
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Debug45
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /t:Clean   /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Release45


%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Debug   /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Debug35    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Release /p:TargetFrameworkVersion=v3.5   /p:OutputPath=bin\Release35  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40;FW35"

%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Debug40    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Release /p:TargetFrameworkVersion=v4.0   /p:OutputPath=bin\Release40  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45;FW40"

%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% AM.Core\AM.Core.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% ManagedClient\ManagedClient.csproj /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% IrbisUI\IrbisUI.csproj             /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Debug   /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Debug45    /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
%MSBUILD% MoonIrbis\MoonIrbis.csproj         /p:Configuration=Release /p:TargetFrameworkVersion=v4.5   /p:OutputPath=bin\Release45  /p:DefineConstants="FW461;FW46;FW452;FW451;FW45"
