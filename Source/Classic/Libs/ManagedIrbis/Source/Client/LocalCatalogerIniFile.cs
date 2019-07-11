// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalCatalogerIniFile.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// INI-file for cataloger.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class LocalCatalogerIniFile
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// INI-file.
        /// </summary>
        [NotNull]
        public IniFile Ini { get; private set; }

        /// <summary>
        /// Context section.
        /// </summary>
        [NotNull]
        public ContextIniSection Context
        {
            get { return _contextIniSection; }
        }

        /// <summary>
        /// Desktop section.
        /// </summary>
        [NotNull]
        private DesktopIniSection Desktop
        {
            get { return _desktopIniSection; }
        }


        /// <summary>
        /// Magna section.
        /// </summary>
        [NotNull]
        public IniFile.Section MagnaSection
        {
            get
            {
                IniFile ini = Ini;
                IniFile.Section result = ini.GetOrCreateSection("Magna");

                return result;
            }
        }

        /// <summary>
        /// Main section.
        /// </summary>
        [NotNull]
        public IniFile.Section Main
        {
            get
            {
                IniFile ini = Ini;
                IniFile.Section result = ini.GetOrCreateSection("Main");

                return result;
            }
        }

        /// <summary>
        /// Организация, на которую куплен ИРБИС.
        /// </summary>
        public string Organization
        {
            get { return Main["User"]; }
        }

        /// <summary>
        /// IP адрес ИРБИС сервера.
        /// </summary>
        [NotNull]
        public string ServerIP
        {
            get
            {
                // coverity[dereference]
                return Main["ServerIP"] ?? "127.0.0.1";
            }
        }

        /// <summary>
        /// Port number of the IRBIS server.
        /// </summary>
        public int ServerPort
        {
            get
            {
                // coverity[dereference]
                int result = Convert.ToInt32
                    (
                        Main["ServerPort"] ?? "6666"
                    );

                return result;
            }
        }

        /// <summary>
        /// User login.
        /// </summary>
        [CanBeNull]
        public string UserName
        {
            get
            {
                const string Login = "UserName";
                return Context.UserName ?? MagnaSection[Login];
            }
        }

        /// <summary>
        /// User password.
        /// </summary>
        [CanBeNull]
        public string UserPassword
        {
            get
            {
                const string Password = "UserPassword";
                return Context.Password ?? MagnaSection[Password];
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalCatalogerIniFile
            (
                [NotNull] IniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            Ini = iniFile;
            _contextIniSection = new ContextIniSection(iniFile);
            _desktopIniSection = new DesktopIniSection(iniFile);
        }

        #endregion

        #region Private members

        private readonly  ContextIniSection _contextIniSection;
        private readonly DesktopIniSection _desktopIniSection;

        #endregion

        #region Public methods

        /// <summary>
        /// Build connection string.
        /// </summary>
        [NotNull]
        public string BuildConnectionString()
        {
            ConnectionSettings settings = new ConnectionSettings
            {
                Host = ServerIP,
                Port = ServerPort,
                Username = UserName.EmptyToNull(),
                Password = UserPassword.EmptyToNull()
            };

            return settings.ToString();
        }

        /// <summary>
        /// Get value.
        /// </summary>
        [CanBeNull]
        public string GetValue
            (
                [NotNull] string sectionName,
                [NotNull] string keyName,
                [CanBeNull] string defaultValue
            )
        {
            Code.NotNullNorEmpty(sectionName, "sectionName");
            Code.NotNullNorEmpty(keyName, "keyName");

            string result = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                );

            return result;
        }


        /// <summary>
        /// Load from specified file.
        /// </summary>
        [NotNull]
        public static LocalCatalogerIniFile Load
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            IniFile iniFile = new IniFile();
            iniFile.Read(fileName, IrbisEncoding.Ansi);
            LocalCatalogerIniFile result = new LocalCatalogerIniFile(iniFile);

            return result;
        }

        #endregion
    }
}
