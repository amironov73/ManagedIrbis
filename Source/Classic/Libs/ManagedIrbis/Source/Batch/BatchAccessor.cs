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
using System.Diagnostics;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

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
        /// Throw <see cref="IrbisNetworkException"/>
        /// when empty record received/decoded.
        /// </summary>
        public static bool ThrowOnEmptyRecord { get; set; }

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IIrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static BatchAccessor()
        {
            ThrowOnEmptyRecord = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BatchAccessor
            (
                [NotNull] IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Throw <see cref="IrbisNetworkException"/>
        /// if the record is empty.
        /// </summary>
        private static void _ThrowIfEmptyRecord
            (
                [NotNull] MarcRecord record,
                [NotNull] string line
            )
        {
            Code.NotNull(record, "record");
            Code.NotNull(line, "line");

            if (ThrowOnEmptyRecord
                && record.Fields.Count == 0
               )
            {
                Log.Error
                    (
                        "BatchAccessor::ThrowIfEmptyRecord: "
                        + "empty record detected"
                    );

                byte[] bytes = Encoding.UTF8.GetBytes(line);
                string dump = IrbisNetworkUtility.DumpBytes(bytes);
                string message = string.Format
                    (
                        "Empty record in BatchAccessor:{0}{1}",
                        Environment.NewLine,
                        dump
                    );

                IrbisNetworkException exception = new IrbisNetworkException(message);
                BinaryAttachment attachment = new BinaryAttachment
                    (
                        "response",
                        bytes
                    );
                exception.Attach(attachment);
                throw exception;
            }
        }


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
                    _ThrowIfEmptyRecord(result, line);

                    if (!result.Deleted)
                    {
                        result.Modified = false;
                        _records.Add(result);
                    }
                }
            }
        }

        private void _ParseRecord<T>
            (
                [NotNull] string line,
                [NotNull] string database,
                [NotNull] Func<MarcRecord, T> func,
                [NotNull] BlockingCollection<T> collection
            )
        {
            if (!string.IsNullOrEmpty(line))
            {
                MarcRecord record = new MarcRecord
                {
                    HostName = Connection.Host,
                    Database = database
                };

                record = ProtocolText.ParseResponseForAllFormat
                    (
                        line,
                        record
                    );

                if (!ReferenceEquals(record, null))
                {
                    if (!record.Deleted)
                    {
                        T result = func(record);

                        collection.Add(result);
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

            using (_records = new BlockingCollection<MarcRecord>(array.Length))
            {
                int[][] slices = array.Slice(1000).ToArray();

                foreach (int[] slice in slices)
                {
                    if (slice.Length == 1)
                    {
                        MarcRecord record = Connection.ReadRecord
                            (
                                database,
                                slice[0],
                                false,
                                null
                            );

                        _records.Add(record);
                    }
                    else
                    {
                        FormatCommand command
                            = Connection.CommandFactory.GetFormatCommand();
                        command.Database = database;
                        command.FormatSpecification = IrbisFormat.All;
                        command.MfnList.AddRange(slice);

                        Connection.ExecuteCommand(command);

                        string[] lines = command.FormatResult
                            .ThrowIfNullOrEmpty
                                (
                                    "command.FormatResult"
                                );

                        Debug.Assert
                            (
                                lines.Length == slice.Length,
                                "some records not retrieved"
                            );

                        Parallel.ForEach
                            (
                                lines,
                                line => _ParseRecord(line, database)
                            );
                    }
                }

                _records.CompleteAdding();

                return _records.ToArray();
            }

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

        /// <summary>
        /// Read and transform multiple records.
        /// </summary>
        [NotNull]
        public T[] ReadRecords<T>
            (
                [CanBeNull] string database,
                [NotNull] IEnumerable<int> mfnList,
                [NotNull] Func<MarcRecord, T> func
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
                return new T[0];
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

                T result1 = func(record);

                return new[] { result1 };
            }

#if FW4

            using (BlockingCollection<T> collection
                = new BlockingCollection<T>(array.Length))
            {

                int[][] slices = array.Slice(1000).ToArray();

                foreach (int[] slice in slices)
                {
                    if (slice.Length == 1)
                    {
                        MarcRecord record = Connection.ReadRecord
                            (
                                database,
                                slice[0],
                                false,
                                null
                            );

                        _records.Add(record);
                    }
                    else
                    {
                        FormatCommand command = Connection.CommandFactory
                                .GetFormatCommand();
                        command.Database = database;
                        command.FormatSpecification = IrbisFormat.All;
                        command.MfnList.AddRange(slice);

                        Connection.ExecuteCommand(command);

                        string[] lines = command.FormatResult
                            .ThrowIfNullOrEmpty
                            (
                                "command.FormatResult"
                            );

                        Debug.Assert
                            (
                                lines.Length == slice.Length,
                                "some records not retrieved"
                            );

                        Parallel.ForEach
                            (
                                lines,
                                line => _ParseRecord
                                    (
                                        line,
                                        database,
                                        func,
                                        collection
                                    )
                            );
                    }
                }

                collection.CompleteAdding();

                return collection.ToArray();
            }

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

            MarcRecord[] records = MarcRecordUtility.ParseAllFormat
                (
                    database,
                    Connection,
                    command.FormatResult
                        .ThrowIfNullOrEmpty("command.FormatResult")
                );
            Debug.Assert
                (
                    command.MfnList.Count == records.Length,
                    "some records not retrieved"
                );

            T[] result = records.Select
                (
                    // ReSharper disable ConvertClosureToMethodGroup
                    record => func(record)
                    // ReSharper restore ConvertClosureToMethodGroup
                )
                .ToArray();

            return result;

#endif
        }


        #endregion
    }
}
