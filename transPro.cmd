@echo off

Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Classic\Libs\AM.Core\AM.Core.3.5.csproj"
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Classic\Libs\AM.Core\AM.Core.4.0.csproj"
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Classic\Libs\AM.Core\AM.Core.4.6.csproj"
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Droid\Libs\AM.Core\AM.Core.csproj"                 ..\..\..\Classic\Libs\AM.Core\
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\Universal\Libs\AM.Core\AM.Core.csproj"             ..\..\..\Classic\Libs\AM.Core\
Utils\TransProject.exe "Source\Classic\Libs\AM.Core\AM.Core.csproj"           "Source\WinMobile\AM.Core\AM.Core.csproj"                  ..\..\Classic\Libs\AM.Core\

Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Classic\Libs\AM.Core\AM.Core.3.5.csproj"
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Classic\Libs\AM.Core\AM.Core.4.0.csproj"
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Classic\Libs\AM.Core\AM.Core.4.6.csproj"
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Droid\Libs\ManagedIrbis\ManagedIrbis.csproj"       ..\..\..\Classic\Libs\ManagedIrbis\
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\Universal\Libs\ManagedIrbis\ManagedIrbis.csproj"   ..\..\..\Classic\Libs\ManagedIrbis\
Utils\TransProject.exe "Source\Classic\Libs\ManagedIrbis\ManagedIrbis.csproj" "Source\WinMobile\ManagedIrbis\ManagedIrbis.csproj"        ..\..\Classic\Libs\ManagedIrbis\

Utils\TransProject.exe "Source\Classic\Libs\AM.AOT\AM.AOT.csproj"             "Source\Classic\Libs\AM.AOT\AM.AOT.3.5.csproj"
Utils\TransProject.exe "Source\Classic\Libs\AM.AOT\AM.AOT.csproj"             "Source\Classic\Libs\AM.AOT\AM.AOT.4.0.csproj"
Utils\TransProject.exe "Source\Classic\Libs\AM.AOT\AM.AOT.csproj"             "Source\Classic\Libs\AM.AOT\AM.AOT.4.6.csproj"
Utils\TransProject.exe "Source\Classic\Libs\AM.AOT\AM.AOT.csproj"             "Source\Droid\Libs\AM.AOT\AM.AOT.csproj"                   ..\..\..\Classic\Libs\AM.AOT\
Utils\TransProject.exe "Source\Classic\Libs\AM.AOT\AM.AOT.csproj"             "Source\Universal\Libs\AM.AOT\AM.AOT.csproj"               ..\..\..\Classic\Libs\AM.AOT\
rem Utils\TransProject.exe "Source\Classic\Libs\AM.AOT\AM.AOT.csproj"             "Source\WinMobile\AM.AOT\AM.AOT.csproj"                   ..\..\Classic\Libs\AM.AOT\

Utils\TransProject.exe "Source\Classic\Libs\RestfulIrbis\RestfulIrbis.csproj" "Source\Classic\Libs\RestfulIrbis\RestfulIrbis.3.5.csproj"
Utils\TransProject.exe "Source\Classic\Libs\RestfulIrbis\RestfulIrbis.csproj" "Source\Classic\Libs\RestfulIrbis\RestfulIrbis.4.0.csproj"
Utils\TransProject.exe "Source\Classic\Libs\RestfulIrbis\RestfulIrbis.csproj" "Source\Classic\Libs\RestfulIrbis\RestfulIrbis.4.6.csproj"
Utils\TransProject.exe "Source\Classic\Libs\RestfulIrbis\RestfulIrbis.csproj" "Source\Droid\Libs\RestfulIrbis\RestfulIrbis.csproj"       ..\..\..\Classic\Libs\RestfulIrbis\
Utils\TransProject.exe "Source\Classic\Libs\RestfulIrbis\RestfulIrbis.csproj" "Source\Universal\Libs\RestfulIrbis\RestfulIrbis.csproj"   ..\..\..\Classic\Libs\RestfulIrbis\
