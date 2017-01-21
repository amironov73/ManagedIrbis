// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServiceInstallerUtility.cs -- service installation helper
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Configuration.Install;
using System.Reflection;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Service installation helper.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    static class ServiceInstallerUtility
    {
        #region Private members

        private static readonly string ExecutablePath
            = Assembly.GetExecutingAssembly().Location;

        #endregion

        #region Public methods

        // ================================================

        /// <summary>
        /// Install services the executable contains.
        /// </summary>
        /// <returns></returns>
        public static bool Install()
        {
            try
            {
                ManagedInstallerClass.InstallHelper
                    (
                        new[] { ExecutablePath }
                    );
            }
            catch
            {
                return false;
            }
            return true;
        }

        // ================================================

        /// <summary>
        /// Uninstall services.
        /// </summary>
        public static bool Uninstall()
        {
            try
            {
                ManagedInstallerClass.InstallHelper
                    (
                        new[] { "/u", ExecutablePath }
                    );
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
