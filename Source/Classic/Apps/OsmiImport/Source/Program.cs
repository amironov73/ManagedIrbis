// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- entry point
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;

using AM.Logging;
using AM.Logging.NLog;

using ManagedIrbis;

#endregion

// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

namespace OsmiImport
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        private static void RunService()
        {
            Log.Trace("Program::RunService: enter");

            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                new ImportService()
            };
            ServiceBase.Run(servicesToRun);

            Log.Trace("Program::RunService: exit");
        }

        // ====================================================================

        private static void InstallService()
        {
            Log.Trace("Program::InstallService: enter");

            ServiceInstallerUtility.Install();

            Log.Trace("Program::InstallService: exit");
        }

        // ====================================================================

        private static void UninstallService()
        {
            Log.Trace("Program::UninstallService: enter");

            ServiceInstallerUtility.Uninstall();

            Log.Trace("Program::UninstallService: exit");
        }

        // ====================================================================

        private static void StartService()
        {
            Log.Trace("Program::StartService: enter");

            try
            {
                ServiceController controller
                    = new ServiceController(ImportService.ImportDaemon);
                controller.Start();
            }
            catch (Exception exception)
            {
                Log.TraceException("Log::StartService", exception);
            }

            Log.Trace("Program::StartService: exit");
        }

        // ====================================================================

        private static void StopService()
        {
            Log.Trace("Program::StopService: enter");

            try
            {
                ServiceController controller
                    = new ServiceController(ImportService.ImportDaemon);
                controller.Stop();
            }
            catch (Exception exception)
            {
                Log.TraceException("Program::StopService", exception);
            }

            Log.Trace("Program::StopService: exit");
        }

        // ====================================================================

        private static void ShowVersion()
        {
            Console.WriteLine
                (
                    "OsmiImport - ImportDaemon for IBRIS64 and DiCARDS\n"
                    + "version {0}",
                    IrbisConnection.ClientVersion
                );
        }

        // ====================================================================

        /// <summary>
        /// Запускаемся как простое консольное приложение.
        /// Выполняем импорт однократно.
        /// </summary>
        static void RunAsConsoleApplication(string[] args)
        {
            Console.WriteLine("OsmiImport version {0}", IrbisConnection.ClientVersion);
            Console.WriteLine("Running as console application");
            Console.WriteLine();

            Importer.LoadConfiguration(args);
            Importer.DoWork();

            Console.WriteLine();
            Console.WriteLine("Stopped");
            Console.WriteLine();
        }

        // ====================================================================

        private static void ShowHelp()
        {
            Console.WriteLine
                (
                    "OsmiImport - Import daemon for IRBIS64 and DiCARDS\n\n"
                    + "\t-install \tinstall the service\n"
                    + "\t-uninstall \tuninstall the service\n"
                    + "\t-start \t\tstart the service\n"
                    + "\t-stop \t\tstop the service\n"
                    + "\t-console \trun the service as console application\n"
                    + "\t-version \tshow version and exit\n"
                    + "\t-help \t\tshow this screen and exit\n\n"
                );
        }

        // ====================================================================

        static void SetupLogging()
        {
            Log.SetLogger(new NLogger());
        }

        // ====================================================================

        static void DoCommandLine
            (
                string[] args
            )
        {
            if (args.Length == 0)
            {
                if (Debugger.IsAttached)
                {
                    RunAsConsoleApplication(args);
                }
                else if (Environment.UserInteractive)
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
                        RunAsConsoleApplication(args);
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

        // ====================================================================

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args"></param>
        static int Main
            (
                string[] args
            )
        {
            ServicePointManager.SecurityProtocol =
                // SecurityProtocolType.Ssl3 |
                // SecurityProtocolType.Tls |
                // SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls12;
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.ServerCertificateValidationCallback
                = _ServerCertificateValidationCallback;

            int returnCode = 0;

            SetupLogging();

            Log.Trace("Program starts");

            try
            {
                DoCommandLine(args);
            }
            catch (Exception exception)
            {
                Log.TraceException("Global exception", exception);
                returnCode = 1;
            }

            Log.Info("Return code=" + returnCode);
            Log.Trace("Program exits");

            return returnCode;
        }

        private static bool _ServerCertificateValidationCallback
            (
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslpolicyerrors
            )
        {
            return true;
        }
    }
}
