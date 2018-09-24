// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ServiceProcess;

using ManagedIrbis;

#endregion

// ReSharper disable LocalizableElement

namespace IrbisNetService
{
    class Program
    {
        private static void RunService()
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                new IrbisService()
            };
            ServiceBase.Run(servicesToRun);
        }

        // ====================================================================

        private static void InstallService()
        {
            ServiceInstallerUtility.Install();
        }

        // ====================================================================

        private static void UninstallService()
        {
            ServiceInstallerUtility.Uninstall();
        }

        // ====================================================================

        private static void StartService()
        {
            try
            {
                ServiceController controller
                    = new ServiceController(IrbisService.IrbisNet);
                controller.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // ====================================================================

        private static void StopService()
        {
            try
            {
                ServiceController controller
                    = new ServiceController(IrbisService.IrbisNet);
                controller.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // ====================================================================

        private static void ShowVersion()
        {
            Console.WriteLine
                (
                    "IrbisNetService - IBRIS64 compatible open source service\n"
                    + "version {0}",
                    IrbisConnection.ClientVersion
                );
        }

        // ====================================================================

        static void RunAsConsoleApplication()
        {
            Console.WriteLine("IrbisNetService version {0}", IrbisConnection.ClientVersion);
            Console.WriteLine("Running as console application");
            Console.WriteLine();

            // TODO implement
        }

        // ====================================================================

        private static void ShowHelp()
        {
            Console.WriteLine
                (
                    "IrbisNetService - IRBIS64 compatible open source network service\n\n"
                    + "\t-install \tinstall the service\n"
                    + "\t-uninstall \tuninstall the service\n"
                    + "\t-start \t\tstart the service\n"
                    + "\t-stop \t\tstop the service\n"
                    + "\t-console \trun the service as console application\n"
                    + "\t-version \tshow version and exit\n"
                    + "\t-help \tshow this screen and exit\n\n"
                );
        }

        // ====================================================================

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                if (Environment.UserInteractive)
                {
                    ShowHelp();
                }
                else
                {
                    RunService();
                }
            }
            else if (args.Length == 1)
            {
                switch (args[0].ToLowerInvariant())
                {
                    case "-install":
                    case "/install":
                    case "-i":
                    case "/i":
                        InstallService();
                        break;

                    case "-uninstall":
                    case "/uninstall":
                    case "-u":
                    case "/u":
                        UninstallService();
                        break;

                    case "-start":
                    case "/start":
                    case "-run":
                    case "/run":
                    case "-r":
                    case "/r":
                    case "/1":
                    case "-1":
                        StartService();
                        break;

                    case "-stop":
                    case "/stop":
                    case "-s":
                    case "/s":
                    case "-0":
                    case "/0":
                        StopService();
                        break;

                    case "-console":
                    case "/console":
                    case "-c":
                    case "/c":
                        RunAsConsoleApplication();
                        break;

                    case "-version":
                    case "/version":
                    case "-v":
                    case "/v":
                        ShowVersion();
                        break;

                    default:
                        ShowHelp();
                        break;
                }
            }
            else
            {
                ShowHelp();
            }
        }
    }
}
