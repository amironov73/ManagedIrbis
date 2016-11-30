// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalClient.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class LocalClient
        : AbstractClient
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
        public LocalClient()
        {
            RootPath = "C:/IRBIS64";
            DataPath = "C:/IRBIS64/DataI";
            Database = "IBIS";
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalClient
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

        #region Public methods

        #endregion

        #region AbstractClient members

        /// <inheritdoc/>
        public override int GetMaxMfn()
        {
            int result = 0;

            DirectReader64 reader = null;
            try
            {
                reader = _GetReader();
                if (reader != null)
                {
                    result = reader.GetMaxMfn();
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
        public override DatabaseInfo[] ListDatabases()
        {
            string fileName = Path.Combine
                (
                    DataPath,
                    "dbnam1.mnu"
                );

            string[] lines = File.ReadAllLines
                (
                    fileName,
                    IrbisEncoding.Ansi
                );

            DatabaseInfo[] result = DatabaseInfo.ParseMenu(lines);

            return result;
        }

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
        public override MarcRecord ReadRecordVersion
            (
                int mfn,
                int version
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
                    MarcRecord[] versions = reader.ReadAllRecordVersions(mfn);
                    int index = version;
                    if (version < 0)
                    {
                        index = versions.Length + version;
                    }
                    if (index >= 0 && index < versions.Length)
                    {
                        result = versions[index];
                    }
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

        #region IDisposable members

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
