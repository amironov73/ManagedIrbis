// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BatchAccessor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Gbl;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#if FW4

using System.Collections.Concurrent;
using System.Threading.Tasks;

#endif

#endregion

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BatchAccessor
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BatchAccessor
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Private members

#if FW4

        private BlockingCollection<MarcRecord> _records;

        private void _ParseRecord
            (
                [NotNull] string line,
                [NotNull] string database
            )
        {
            if (!string.IsNullOrEmpty(line))
            {
                MarcRecord result = new MarcRecord
                {
                    HostName = Connection.Host,
                    Database = database
                };

                result = ProtocolText.ParseResponseForAllFormat
                (
                    line,
                    result
                );

                if (!ReferenceEquals(result, null))
                {
                    if (!result.Deleted)
                    {
                        _records.Add(result);
                    }
                }
            }
        }

#endif

        #endregion

        #region Public methods

        /// <summary>
        /// Read multiple records.
        /// </summary>
        [NotNull]
        public MarcRecord[] ReadRecords
            (
                [CanBeNull] string database,
                [NotNull] IEnumerable<int> mfnList
            )
        {
            Code.NotNull(mfnList, "mfnList");

            if (string.IsNullOrEmpty(database))
            {
                database = Connection.Database;
            }

            database.ThrowIfNull("database");

            int[] array = mfnList.ToArray();

            if (array.Length == 0)
            {
                return new MarcRecord[0];
            }

            if (array.Length == 1)
            {
                int mfn = array[0];

                MarcRecord record = Connection.ReadRecord
                    (
                        database,
                        mfn,
                        false,
                        null
                    );

                return new[] { record };
            }

#if FW4

            _records = new BlockingCollection<MarcRecord>
                (
                    array.Length
                );

            int[][] slices = array.Slice(1000).ToArray();

            foreach (int[] slice in slices)
            {
                FormatCommand command
                    = Connection.CommandFactory.GetFormatCommand();
                command.Database = database;
                command.FormatSpecification = IrbisFormat.All;
                command.MfnList.AddRange(slice);

                Connection.ExecuteCommand(command);

                string[] lines = command.FormatResult
                    .ThrowIfNullOrEmpty("command.FormatResult");

                Debug.Assert
                    (
                        lines.Length == slice.Length,
                        "some records not retrieved"
                    );

                Parallel.ForEach
                    (
                        lines,
                        line => _ParseRecord (line, database)
                    );
            }

            _records.CompleteAdding();

            return _records.ToArray();

#else

            FormatCommand command 
                = Connection.CommandFactory.GetFormatCommand();
            command.Database = database;
            command.FormatSpecification = IrbisFormat.All;
            command.MfnList.AddRange(array);

            if (array.Length > IrbisConstants.MaxPostings)
            {
                throw new ArgumentException();
            }

            Connection.ExecuteCommand(command);

            MarcRecord[] result = MarcRecordUtility.ParseAllFormat
                (
                    database,
                    Connection,
                    command.FormatResult
                        .ThrowIfNullOrEmpty("command.FormatResult")
                );
            Debug.Assert
                (
                    command.MfnList.Count == result.Length,
                    "some records not retrieved"
                );

            return result;


#endif
        }

        #endregion
    }
}
