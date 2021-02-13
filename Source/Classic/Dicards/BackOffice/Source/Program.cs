// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

using AM.Logging;
using AM.Logging.NLog;

using BackOffice.Jobs;

using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;

using DicardsConfig;

using ManagedIrbis;

using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Logging;

#endregion

namespace BackOffice
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    internal sealed class Program
    {
        #region Private members

        /// <summary>
        /// Показывает ошибку и аварийно завершает программу.
        /// </summary>
        /// <param name="exception"></param>
        public static void ShowException
            (
                Exception exception
            )
        {
            XtraMessageBox.Show
                (
                    exception.ToString(),
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

            Environment.FailFast("Ошибка");
        }

        /// <summary>
        /// Конфигурация сервиса средствами Topshelf.
        /// </summary>
        private static void ConfigureService
            (
                HostConfigurator configurator
            )
        {
            configurator.ApplyCommandLine();

            var service = configurator.Service<DicardsService>();
            service.SetDescription("DICARDS reader importer service");
            service.SetDisplayName("BackOffice Service");
            service.SetServiceName("BackOffice");

            service.StartAutomaticallyDelayed();
            service.RunAsLocalSystem();
            service.EnableShutdown();

            service.UseNLog();

            // Необязательная настройка восстановления после сбоев
            service.EnableServiceRecovery(recovery =>
            {
                recovery.RestartService(1);
            });

            // Реакция на исключение
            service.OnException(exception =>
            {
                var log = HostLogger.Get<DicardsService>();
                log.ErrorFormat($"Exception {exception}");
            });
        }

        /// <summary>
        /// Однократный ручной запуск заданий.
        /// </summary>
        private static int RunJobsManually
            (
                string[] args
            )
        {
            string selectedJobName = null;

            if (args.Length > 1)
            {
                selectedJobName = args[1];
            }

            HostLogger.UseLogger
                (
                    new NLogLogWriterFactory.NLogHostLoggerConfigurator()
                );

            try
            {
                JobController.RunJobsManually(selectedJobName);
            }
            catch (Exception exception)
            {
                HostLogger.Get<Program>().ErrorFormat(exception.ToString());
                Console.WriteLine(exception.ToString());
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Конфигурирование и запуск сервиса.
        /// </summary>
        private static int ConfigureAndRunService
            (
                string[] args
            )
        {
            var result = HostFactory.Run(ConfigureService);

            return (int)result;
        }

        /// <summary>
        /// Заглушка на случай сбоев сертификатов.
        /// </summary>
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

        private static void ConfigureSecurityProtocol()
        {
            ServicePointManager.SecurityProtocol =
                // SecurityProtocolType.Ssl3 |
                // SecurityProtocolType.Tls |
                // SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls12;
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.ServerCertificateValidationCallback
                = _ServerCertificateValidationCallback;
        }

        private static int ConfigureSettings()
        {
            Log.Trace("Program::Configure: enter");

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                BonusSkins.Register();
                SkinManager.EnableFormSkins();
                WindowsFormsSettings.LoadApplicationSettings();

                ConfigurationDialog.Configure(null);
            }
            catch (Exception exception)
            {
                ShowException(exception);
                return 1;
            }

            Log.Trace("Program::Configure: exit");

            return 0;
        }

        private static void SetupLogging()
        {
            Log.SetLogger(new NLogger());
        }

        #endregion

        #region Program entry point

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            MarcRecord.TurnOffVerification();
            SetupLogging();
            ConfigureSecurityProtocol();

            if (args.Length != 0)
            {
                var command = args[0].ToLowerInvariant();

                if (command == "run")
                {
                    return RunJobsManually(args);
                }

                if (command == "configure" || command == "config"
                    || command == "-config" || command == "-configure")
                {
                    return ConfigureSettings();
                }
            }

            return ConfigureAndRunService(args);
        }

        #endregion
    }
}
