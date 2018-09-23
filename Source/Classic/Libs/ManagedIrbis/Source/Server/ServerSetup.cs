// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerSetup.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ServerSetup
    {
        #region Properties

        /// <summary>
        /// Server INI-file.
        /// </summary>
        [NotNull]
        public ServerIniFile IniFile { get; private set; }

        /// <summary>
        /// Override for root path.
        /// </summary>
        [CanBeNull]
        public string RootPathOverride { get; set; }

        /// <summary>
        /// TCP port number override.
        /// </summary>
        public int PortNumberOverride { get; set; }

        /// <summary>
        /// Use TCP/IP v4 (enabled by default).
        /// </summary>
        public bool UseTcpIpV4 { get; set; }

        /// <summary>
        /// Use TCP/IP v6.
        /// </summary>
        public bool UseTcpIpV6 { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServerSetup
            (
                [NotNull] ServerIniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            IniFile = iniFile;
            UseTcpIpV4 = true;
        }

        #endregion
    }
}
