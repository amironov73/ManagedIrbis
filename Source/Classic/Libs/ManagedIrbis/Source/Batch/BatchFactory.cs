// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BatchFactory.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4 || ANDROID || UAP || NETCORE || PORTABLE

#region Using directives

using System.Collections.Generic;
using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BatchFactory
    {
        #region Public methods

        /// <summary>
        /// Get batch reader.
        /// </summary>
        [NotNull]
        public IEnumerable<MarcRecord> GetBatchReader
            (
                [NotNull] string kind,
                [NotNull] string connectionString,
                [NotNull] string database,
                [NotNull] IEnumerable<int> range
            )
        {
            Code.NotNullNorEmpty(kind, "kind");
            Code.NotNullNorEmpty(connectionString, "connectionString");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(range, "range");

            IEnumerable<MarcRecord> result;

            switch (kind)
            {
                case "parallel":
                    result = new ParallelRecordReader
                        (
                            -1,
                            connectionString,
                            range.ToArray()
                        );
                    break;

                default:
                    result = new BatchRecordReader
                        (
                            connectionString,
                            database,
                            1000,
                            true,
                            range
                        );
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get batch formatter.
        /// </summary>
        [NotNull]
        public IEnumerable<string> GetFormatter
            (
                [NotNull] string kind,
                [NotNull] string connectionString,
                [NotNull] string database,
                [NotNull] string format,
                [NotNull] IEnumerable<int> range
            )
        {
            Code.NotNullNorEmpty(kind, "kind");
            Code.NotNullNorEmpty(connectionString, "connectionString");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNullNorEmpty(format, "format");
            Code.NotNull(range, "range");

            IEnumerable<string> result;

            switch (kind)
            {
                case "parallel":
                    result = new ParallelRecordFormatter
                    (
                        -1,
                        connectionString,
                        range.ToArray(),
                        format
                    );
                    break;

                default:
                    result = new BatchRecordFormatter
                    (
                        connectionString,
                        database,
                        format,
                        1000,
                        range
                    );
                    break;
            }

            return result;
        }

        #endregion
    }
}

#endif
