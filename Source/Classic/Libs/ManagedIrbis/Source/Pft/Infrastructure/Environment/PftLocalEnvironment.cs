/* PftLocalEnvironment.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Environment
{
    /// <summary>
    /// Local operation mode.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftLocalEnvironment
        : PftEnvironmentAbstraction
    {
        #region Properties

        /// <summary>
        /// Data path.
        /// </summary>
        public string DataPath { get; set; }

        /// <summary>
        /// Root path.
        /// </summary>
        public string RootPath { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftLocalEnvironment()
        {
            RootPath = "C:/IRBIS64";
            DataPath = "C:/IRBIS64/DataI";
            Database = "IBIS";
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftLocalEnvironment
            (
                string rootPath
            )
            : this()
        {
            RootPath = rootPath;
            DataPath = rootPath + "/DataI";
        }

        #endregion

        #region Private members

        #endregion

        #region PftEnvironmentAbstraction members

        /// <inheritdoc/>
        public override string ReadFile
            (
                FileSpecification fileSpecification
            )
        {
            Code.NotNull(fileSpecification, "fileSpecification");

            string fileName = fileSpecification.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                throw new IrbisException("fileName");
            }

            string resultPath = null;
            string database = fileSpecification.Database ?? Database;

            switch (fileSpecification.Path)
            {
                case IrbisPath.MasterFile:
                    resultPath = Path.Combine
                        (
                            Path.Combine
                            (
                                DataPath,
                                database
                            ),
                            fileName
                        );
                    if (!File.Exists(resultPath))
                    {
                        resultPath = Path.Combine
                            (
                                Path.Combine
                                (
                                    DataPath,
                                    "Deposit"
                                ),
                                fileName
                            );
                    }
                    break;
            }

            if (string.IsNullOrEmpty(resultPath))
            {
                throw new IrbisException("filePath");
            }

            string result = File.ReadAllText
                (
                    resultPath,
                    IrbisEncoding.Ansi
                );

            return result;
        }

        #endregion
    }
}
