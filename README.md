### ManagedIrbis
ManagedIrbis is a framework for client development for
popular russian library computer system IRBIS64 ("ИРБИС64").

### Supported environments

ManagedIrbis currently supports:

- classic desktop .NET 3.5/4.0/4.5/4.6 up to 4.6.2 (possibly 4.7);
- .NET Core 1.0.1 (standard library 1.6), 2.0.2 (.NET Standard 2.0);
- Mono 4.3;
- Compact Framework 3.5 (for WinMobile and Pocket PC);
- Silverlight 5;
- Xamarin Android.

### Components

- **AM.Core** - common classes and routines;
- **AM.AOT** - Porter stemmer, RusVectores.org client, Yandex MyStem wrapper;
- **AM.Drawing** - System.Drawing related stuff;
- **AM.Rfid** - RFID technology support;
- **AM.Suggestions** - suggestion control and clients (currently dadata.ru only);
- **AM.Windows.Forms** - Syste.Windows.Forms based visual components;
- **AM.Win32** - interop, Win32 API wrappers;
- **IrbisInterop** - interop with IRBIS64.DLL (Win32 only);
- **IrbisUI** - System.Windows.Forms based common UI components for clients;
- **ManagedIrbis** - common IRBIS client related classes;
- **ManagedIrbis.Office** - report driver for Excel file generation;
- **MoonIrbis** - Lua-based client scripting;
- **RestfulIrbis** - REST client and server for IRBIS;
- **SharpIrbis** - C#-based client scripting.

### Links

- [Source code repository](https://github.com/amironov73/ManagedIrbis);
- [ManagedIrbis home page](http://arsmagna.ru);
- [IRBIS64 home page](http://www.elnit.org/index.php?option=com_content&view=article&id=35&Itemid=108) (russian);
- [IRBIS64 online documentation](http://sntnarciss.ru/irbis.html) (russian);
- [IRBIS64 wiki](http://wiki.elnit.org/index.php/%D0%92%D0%B8%D0%BA%D0%B8-%D0%B4%D0%BE%D0%BA%D1%83%D0%BC%D0%B5%D0%BD%D1%82%D0%B0%D1%86%D0%B8%D1%8F_%D0%BF%D0%BE_%D1%81%D0%B8%D1%81%D1%82%D0%B5%D0%BC%D0%B5_%D0%B0%D0%B2%D1%82%D0%BE%D0%BC%D0%B0%D1%82%D0%B8%D0%B7%D0%B0%D1%86%D0%B8%D0%B8_%D0%B1%D0%B8%D0%B1%D0%BB%D0%B8%D0%BE%D1%82%D0%B5%D0%BA_%D0%98%D0%A0%D0%91%D0%98%D0%A1) (russian);
- [IRBIS64 support forum](http://irbis.gpntb.ru) (russian);
- [NuGet libraries](https://www.nuget.org/packages/ManagedIrbis/);
- [ManagedIrbis support topic](http://irbis.gpntb.ru/read.php?24,85009) (russian);
- [Builds with artifacts on AppVeyor](https://ci.appveyor.com/project/AlexeyMironov/managedclient-45/);
- [Builds on Travis](https://travis-ci.org/amironov73/ManagedIrbis).

### Build status

[![Issues](https://img.shields.io/github/issues/amironov73/ManagedIrbis.svg)](https://github.com/amironov73/ManagedIrbis/issues)
[![Release](https://img.shields.io/github/release/amironov73/ManagedIrbis.svg)](https://github.com/amironov73/ManagedIrbis/releases)
[![NuGet](https://img.shields.io/nuget/v/ManagedIrbis.svg)](https://www.nuget.org/packages/ManagedIrbis/)
[![Build status](https://img.shields.io/appveyor/ci/AlexeyMironov/managedclient-45.svg)](https://ci.appveyor.com/project/AlexeyMironov/managedclient-45/)
[![Build status](https://api.travis-ci.org/amironov73/ManagedIrbis.svg)](https://travis-ci.org/amironov73/ManagedIrbis/)
[![Coverity](https://img.shields.io/coverity/scan/11008.svg)](https://scan.coverity.com/projects/managedirbis)
[![Codecov](https://img.shields.io/codecov/c/github/amironov73/ManagedIrbis.svg)](https://codecov.io/gh/amironov73)

