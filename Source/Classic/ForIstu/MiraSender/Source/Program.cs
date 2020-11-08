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

using AM.Logging;
using AM.Logging.NLog;

#endregion

namespace MiraSender
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    class Program
    {
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

        private static void SetupLogging()
        {
            Log.SetLogger(new NLogger());
        }

        static void Main(string[] args)
        {
            SetupLogging();
            ConfigureSecurityProtocol();

            try
            {
                if (args.Length != 0)
                {
                    Reminder.SetPreselected(args);
                }

                Reminder.LoadConfiguration();
                Reminder.DoWork();
            }
            catch (Exception exception)
            {
                Log.TraceException("Main: exception:", exception);
            }

        }
    }
}
