/* GlobalCorrector.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status:poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Gbl;
using ManagedIrbis.Network;
using ManagedIrbis.Network.Commands;
using ManagedIrbis.Network.Sockets;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Обёртка для облегчения выполнения глобальной корректировки
    /// порциями (например, по 100 записей за раз).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GlobalCorrector
    {
        #region Events

        /// <summary>
        /// Вызывается после обработки очередной порции записей
        /// и в конце общей обработки.
        /// </summary>
        public event EventHandler PortionProcessed;

        #endregion

        #region Properties

        [NotNull]
        public IrbisConnection Connection { get; private set; }

        [NotNull]
        public string Database { get; private set; }

        public int ChunkSize { get; private set; }

        /// <summary>
        /// Актуализировать ли словарь. По умолчанию <c>true</c>.
        /// </summary>
        public bool Actualize { get; set; }

        /// <summary>
        /// Выполнять ли autoin.gbl.
        /// По умолчанию <c>false</c>.
        /// </summary>
        public bool Autoin { get; set; }

        /// <summary>
        /// Выполнять ли формально-логический контроль.
        /// По умолчанию <c>false</c>.
        /// </summary>
        public bool FormalControl { get; set; }

        public bool Cancel { get; set; }

        [NotNull]
        public GblResult Result { get; private set; }

        #endregion

        #region Construction

        public GlobalCorrector
            (
                [NotNull] IrbisConnection connection
            )
            : this
            (
                connection,
                connection.ThrowIfNull("connection").Database,
                100
            )
        {
        }

        public GlobalCorrector
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                int chunkSize
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(database, "database");

            if (chunkSize < 1)
            {
                throw new ArgumentOutOfRangeException("chunkSize");
            }

            Connection = connection;
            Database = database;
            ChunkSize = chunkSize;
            Actualize = true;
            Result = GblResult.GetEmptyResult();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Обработать интервал записей.
        /// </summary>
        [NotNull]
        public GblResult ProcessInterval
            (
                int minMfn,
                int maxMfn,
                [NotNull] GblStatement[] statements
            )
        {
            Code.NotNull(statements, "statements");
            if (minMfn <= 0)
            {
                throw new ArgumentOutOfRangeException("minMfn");
            }

            int limit = Connection.GetMaxMfn() - 1;
            if (minMfn > limit)
            {
                throw new ArgumentOutOfRangeException("minMfn");
            }
            maxMfn = Math.Min(maxMfn, limit);
            if (minMfn > maxMfn)
            {
                throw new ArgumentOutOfRangeException("minMfn");
            }

            if (statements.Length == 0)
            {
                Result = GblResult.GetEmptyResult();
                return Result;
            }

            Result = GblResult.GetEmptyResult();
            Result.RecordsSupposed = maxMfn - minMfn + 1;

            int startMfn = minMfn;

            while (startMfn <= maxMfn)
            {
                int amount = Math.Min
                    (
                        maxMfn - startMfn + 1,
                        ChunkSize
                    );
                int endMfn = startMfn + amount - 1;

                try
                {
                    GblResult intermediateResult
                        = Connection.GlobalCorrection
                        (
                            null,
                            0,
                            0,
                            startMfn,
                            endMfn,
                            null,
                            Actualize,
                            FormalControl,
                            Autoin,
                            statements
                        );
                    Result.MergeResult(intermediateResult);

                    Result.TimeElapsed = DateTime.Now
                        - Result.TimeStarted;

                    PortionProcessed.Raise(this);

                    if (Cancel || Result.Canceled)
                    {
                        Result.Canceled = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Result.Exception = ex;
                    break;
                }

                startMfn = endMfn + 1;
            }

            Result.TimeElapsed = DateTime.Now
                - Result.TimeStarted;

            return Result;
        }


        /// <summary>
        /// Обработать явно (вручную) заданное множество записей.
        /// </summary>
        [NotNull]
        public GblResult ProcessRecordset
            (
                [NotNull] IEnumerable<int> recordset,
                [NotNull] GblStatement[] statements
            )
        {
            Code.NotNull(recordset, "recordset");
            Code.NotNull(statements, "statements");

            if (statements.Length == 0)
            {
                return GblResult.GetEmptyResult();
            }

            List<int> list = recordset.ToList();
            if (list.Count == 0)
            {
                return GblResult.GetEmptyResult();
            }

            Result = GblResult.GetEmptyResult();
            Result.RecordsSupposed = list.Count;

            while (list.Count > 0)
            {
                int[] portion = list.Take(ChunkSize).ToArray();
                list = list.Skip(ChunkSize).ToList();
                try
                {
                    GblResult intermediateResult
                        = Connection.GlobalCorrection
                        (
                            null,
                            0,
                            0,
                            0,
                            0,
                            portion,
                            Actualize,
                            FormalControl,
                            Autoin,
                            statements
                        );
                    Result.MergeResult(intermediateResult);

                    Result.TimeElapsed = DateTime.Now
                        - Result.TimeStarted;

                    PortionProcessed.Raise(this);

                    if (Cancel || Result.Canceled)
                    {
                        Result.Canceled = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Result.Exception = ex;
                    break;
                }
            }

            Result.TimeElapsed = DateTime.Now
                - Result.TimeStarted;

            return Result;
        }

        /// <summary>
        /// Обработать результат поиска.
        /// </summary>
        [NotNull]
        public GblResult ProcessSearchResult
            (
            [NotNull] string searchExpression,
            [NotNull] GblStatement[] statements
            )
        {
            Code.NotNullNorEmpty(searchExpression, "searchExpression");
            Code.NotNull(statements, "statements");

            if (statements.Length == 0)
            {
                Result = GblResult.GetEmptyResult();
                return Result;
            }

            int[] found = Connection.Search(searchExpression);
            if (found.Length == 0)
            {
                Result = GblResult.GetEmptyResult();
                return Result;
            }

            return ProcessRecordset
                (
                    found,
                    statements
                );
        }

        /// <summary>
        /// Обработать базу данных в целом.
        /// </summary>
        [NotNull]
        public GblResult ProcessWholeDatabase
            (
                [NotNull] GblStatement[] statements
            )
        {
            Code.NotNull(statements, "statements");
            if (statements.Length == 0)
            {
                Result = GblResult.GetEmptyResult();
                return Result;
            }

            int maxMfn = Connection.GetMaxMfn() - 1;
            return ProcessInterval
                (
                    1,
                    maxMfn,
                    statements
                );
        }


        #endregion
    }
}
