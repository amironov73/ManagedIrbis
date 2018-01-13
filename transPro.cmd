@echo off

Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Classic\Libs\AM.Core\AM.Core.3.5.csproj"
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Classic\Libs\AM.Core\AM.Core.4.0.csproj"
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Classic\Libs\AM.Core\AM.Core.4.6.csproj"
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Droid\Libs\AM.Core\AM.Core.csproj"                ..\..\..\Classic\Libs\AM.Core\
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Universal\Libs\AM.Core\AM.Core.csproj"            ..\..\..\Classic\Libs\AM.Core\
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\WinMobile\AM.Core\AM.Core.csproj"                 ..\..\Classic\Libs\AM.Core\

Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Classic\Libs\AM.Core\AM.Core.3.5.csproj"
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Classic\Libs\AM.Core\AM.Core.4.0.csproj"
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Classic\Libs\AM.Core\AM.Core.4.6.csproj"
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Droid\Libs\ManagedIrbis\ManagedIrbis.csproj"      ..\..\..\Classic\Libs\ManagedIrbis\
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Universal\Libs\ManagedIrbis\ManagedIrbis.csproj"  ..\..\..\Classic\Libs\ManagedIrbis\
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\WinMobile\ManagedIrbis\ManagedIrbis.csproj"       ..\..\Classic\Libs\ManagedIrbis\


