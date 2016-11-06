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

using ManagedIrbis.Direct;
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

        private MstFile64 _GetMst()
        {
            string fileName = Path.Combine
                (
                    Path.Combine
                        (
                            DataPath,
                            Database
                        ),
                    Database + ".mst"
                );

            MstFile64 result = new MstFile64(fileName);

            return result;
        }

        private DirectReader64 _GetReader()
        {
            string fileName = Path.Combine
                (
                    Path.Combine
                        (
                            DataPath,
                            Database
                        ),
                    Database + ".mst"
                );

            DirectReader64 result
                = new DirectReader64(fileName, false);

            return result;
        }

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

            if (!File.Exists(resultPath))
            {
                return null;
            }

            string result = File.ReadAllText
                (
                    resultPath,
                    IrbisEncoding.Ansi
                );

            return result;
        }

        /// <inheritdoc/>
        public override MarcRecord ReadRecord
            (
                int mfn
            )
        {
            if (mfn <= 0)
            {
                return null;
            }

            MarcRecord result = null;
            DirectReader64 reader = null;
            try
            {
                reader = _GetReader();
                if (reader != null)
                {
                    result = reader.ReadRecord(mfn);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Nothing to do actually
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public override int[] Search
            (
                string expression
            )
        {
            int[] result = new int[0];

            if (string.IsNullOrEmpty(expression))
            {
                return result;
            }

            DirectReader64 reader = null;
            try
            {
                reader = _GetReader();
                if (reader != null)
                {
                    result = reader.SearchSimple(expression);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Nothing to do actually
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            return result;
        }

        #endregion
    }
}

