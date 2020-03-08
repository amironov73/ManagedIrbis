// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ApplicationInstaller.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
//using System.Configuration.Install;
//using System.ServiceProcess;

using JetBrains.Annotations;

using ManagedIrbis;

#endregion

namespace IrbisBot
{
//    /// <summary>
//    /// Нужно для инфраструктуры installutil.
//    /// </summary>
//    [PublicAPI]
//    [RunInstaller(true)]
//    public class ApplicationInstaller
//        : Installer
//    {
//        #region Construction

//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        public ApplicationInstaller()
//        {
//            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller
//            {
//                Account = ServiceAccount.NetworkService
//            };

//            string version = IrbisConnection.ClientVersion.ToString();
//            ServiceInstaller serviceInstaller = new ServiceInstaller
//            {
//                Description = "Telegram bot for IRBIS64",
//                DisplayName = BotService.IrbisBot + " v" + version,
//                ServiceName = BotService.IrbisBot,
//                StartType = ServiceStartMode.Automatic,
//#if FW4
//                DelayedAutoStart = true
//#endif
//            };

//            Installers.Add(processInstaller);
//            Installers.Add(serviceInstaller);
//        }

//        #endregion
//    }
}
