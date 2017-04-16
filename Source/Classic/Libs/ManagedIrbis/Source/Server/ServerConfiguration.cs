// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerConfiguration.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

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
    public sealed class ServerConfiguration
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Path for AlphabetTable (without extension).
        /// </summary>
        public string AlphabetTablePath { get; set; }

        /// <summary>
        /// Data path.
        /// </summary>
        public string DataPath { get; set; }

        /// <summary>
        /// System path.
        /// </summary>
        public string SystemPath { get; set; }

        /// <summary>
        /// Path for UpperCaseTable (without extension).
        /// </summary>
        public string UpperCaseTable { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Create server configuration from INI-file.
        /// </summary>
        [NotNull]
        public static ServerConfiguration FromIniFile
            (
                [NotNull] ServerIniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            ServerConfiguration result = new ServerConfiguration
            {
                SystemPath = iniFile.SystemPath,
                DataPath = iniFile.DataPath,
                AlphabetTablePath = iniFile.AlphabetTablePath,
                UpperCaseTable = iniFile.UpperCaseTable
            };

            return result;
        }

        /// <summary>
        /// Create server configuration from INI file.
        /// </summary>
        [NotNull]
        public static ServerConfiguration FromIniFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            IniFile iniFile = new IniFile
                (
                    fileName,
                    IrbisEncoding.Ansi,
                    false
                );
            ServerIniFile serverIni = new ServerIniFile
                (
                    iniFile
                );
            ServerConfiguration result = FromIniFile(serverIni);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify(bool throwOnError)
        {
            Verifier<ServerConfiguration> verifier
                = new Verifier<ServerConfiguration>(this, throwOnError);

            verifier
                .DirectoryExist(SystemPath, "SystemPath")
                .DirectoryExist(DataPath, "DataPath")
                .NotNullNorEmpty(AlphabetTablePath, "AlphabetTablePath")
                .NotNullNorEmpty(UpperCaseTable, "UpperCaseTable");

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
