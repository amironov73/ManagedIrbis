// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServiceInstallerUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Configuration.Install;
using System.Reflection;

#endregion

namespace CatBoy
{
    static class ServiceInstallerUtility
    {
        private static readonly string ExecutablePath
            = Assembly.GetExecutingAssembly().Location;

        // ====================================================================

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

        // ====================================================================

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
    }
}
