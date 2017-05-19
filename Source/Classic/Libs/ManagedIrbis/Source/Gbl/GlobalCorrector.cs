// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GlobalCorrector.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status:poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

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

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [NotNull]
        public string Database { get; private set; }

        /// <summary>
        /// Chunk size.
        /// </summary>
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

        /// <summary>
        /// Was cancelled?
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Result.
        /// </summary>
        [NotNull]
        public GblResult Result { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
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

        /// <summary>
        /// Constructor.
        /// </summary>
        public GlobalCorrector
            (
                [NotNull] IrbisConnection connection,
                string database
            )
            : this
            (
                connection,
                database.ThrowIfNull("database"),
                100
            )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
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
                Log.Error
                    (
                        "GlobalCorrector::Constructor: "
                        + "chunkSize="
                        + chunkSize
                    );

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
        /// Create the <see cref="GlobalCorrector"/>
        /// from <see cref="GblSettings"/>.
        /// </summary>
        [NotNull]
        public static GlobalCorrector FromSettings
            (
                [NotNull] IrbisConnection connection,
                [NotNull] GblSettings settings
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(settings, "settings");

            GlobalCorrector result
                = new GlobalCorrector(connection)
            {
                Database = settings.Database ?? connection.Database,
                Actualize = settings.Actualize,
                Autoin = settings.Autoin,
                FormalControl = settings.FormalControl
            };

            return result;
        }

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
                Log.Error
                    (
                        "GlobalCorrector::ProcessInterval: "
                        + "minMfn="
                        + minMfn
                    );

                throw new ArgumentOutOfRangeException("minMfn");
            }

            int limit = Connection.GetMaxMfn() - 1;
            if (minMfn > limit)
            {
                Log.Error
                    (
                        "GlobalCorrector::ProcessInterval: "
                        + "minMfn="
                        + minMfn
                        + ", limit="
                        + limit
                    );

                throw new ArgumentOutOfRangeException("minMfn");
            }
            maxMfn = Math.Min(maxMfn, limit);
            if (minMfn > maxMfn)
            {
                Log.Error
                    (
                        "GlobalCorrector::ProcessInterval: "
                        + "minMfn="
                        + minMfn
                        + ", maxMfn="
                        + maxMfn
                    );

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
                    GblSettings settings = ToSettings(statements);
                    settings.MinMfn = startMfn;
                    settings.MaxMfn = endMfn;

                    GblResult intermediateResult
                        = Connection.GlobalCorrection (settings);
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
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "GlobalCorrector::ProcessInterval",
                            exception
                        );

                    Result.Exception = exception;
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
                    GblSettings settings = ToSettings(statements);
                    settings.FirstRecord = 0;
                    settings.NumberOfRecords = 0;
                    settings.MfnList = portion;

                    GblResult intermediateResult
                        = Connection.GlobalCorrection(settings);
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
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "GlobalCorrector::ProcessRecordset",
                            exception
                        );

                    Result.Exception = exception;
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

        /// <summary>
        /// Convert <see cref="GlobalCorrector"/>
        /// to <see cref="GblSettings"/>.
        /// </summary>
        [NotNull]
        public GblSettings ToSettings
            (
                [NotNull] IEnumerable<GblStatement> statements
            )
        {
            Code.NotNull(statements, "statements");

            GblSettings result = new GblSettings
                (
                    Connection,
                    statements
                )
            {
                Actualize = Actualize,
                Autoin = Autoin,
                Database = Database,
                FormalControl = FormalControl
            };

            return result;
        }

        #endregion
    }
}
