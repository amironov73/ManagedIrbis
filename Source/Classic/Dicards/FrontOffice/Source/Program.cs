// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.XtraEditors;

using AM;
using AM.Logging;
using AM.Logging.NLog;

using DicardsConfig;

#endregion

namespace FrontOffice
{
    static class Program
    {
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
        /// Вызывается при возникновении ошибки в потоке.
        /// </summary>
        static void Application_ThreadException
            (
                object sender,
                System.Threading.ThreadExceptionEventArgs e
            )
        {
            ShowException(e.Exception);
        }

        private static void SetupLogging()
        {
            Log.SetLogger(new NLogger());
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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main
            (
                string[] args
            )
        {
            SetupLogging();
            ConfigureSecurityProtocol();

            Application.ThreadException += Application_ThreadException;

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                BonusSkins.Register();
                SkinManager.EnableFormSkins();
                WindowsFormsSettings.LoadApplicationSettings();

                if (args.Length != 0)
                {
                    if (args[0].SameString("config"))
                    {
                        ConfigurationDialog.Configure(null);
                    }

                    return;
                }

                Application.Run(new MainForm());
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
    }
}
