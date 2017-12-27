// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataflowBatchReader.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using AM.Collections;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Readers;

#endregion

namespace BiblioPolice
{
    [PublicAPI]
    public sealed class DataflowBatchReader
    {
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
        /// Output.
        /// </summary>
        [NotNull]
        public AbstractOutput Log { get; private set; }

        /// <summary>
        /// Loaded readers.
        /// </summary>
        [NotNull]
        public BlockingCollection<ReaderInfo> Readers
        {
            get;
            private set;
        }

        public BlockingCollection<DebtorInfo> Debtors
        {
            get;
            private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataflowBatchReader
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                [NotNull] AbstractOutput log
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(log, "log");

            Connection = connection;
            Database = database;
            Log = log;
            Readers = new BlockingCollection<ReaderInfo>();
            Debtors = new BlockingCollection<DebtorInfo>();
        }

        #endregion

        #region Private members

        private DebtorManager _debtorManager;

        [CanBeNull]
        private MarcRecord _ParseRecord
            (
                [CanBeNull] string line
            )
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            MarcRecord result = new MarcRecord
            {
                HostName = Connection.Host,
                Database = Database
            };

            result = ProtocolText.ParseResponseForAllFormat
                (
                    line,
                    result
                );

            if (!ReferenceEquals(result, null))
            {
                if (result.Deleted)
                {
                    return null;
                }
            }

            return result;
        }

        [CanBeNull]
        private ReaderInfo _ParseReader
            (
                [CanBeNull] MarcRecord record
            )
        {
            if (!ReferenceEquals(record, null))
            {
                ReaderInfo result = ReaderInfo.Parse(record);

                string status = result.Status;
                if (string.IsNullOrEmpty(status))
                {
                    result.Status = "0";
                }

                return result;
            }

            return null;
        }

        private void _AnalyzeReader
            (
                [CanBeNull] ReaderInfo reader
            )
        {
            if (!ReferenceEquals(reader, null))
            {
                Readers.Add(reader);

                if (reader.Status == "0")
                {
                    DebtorInfo debtor
                        = _debtorManager.GetDebtor(reader);
                    if (!ReferenceEquals(debtor, null))
                    {
                        Debtors.Add(debtor);
                    }
                }
            }
        }

        private string[] _ReadRawRecords
            (
                [NotNull] int[] mfnList
            )
        {
            if (mfnList.Length == 0)
            {
                return new string[0];
            }
            if (mfnList.Length == 1)
            {
                return new string[0]; // TODO FIXME!
            }

            List<object> arguments = new List<object>
                (
                    mfnList.Length + 3
                )
            {
                Database,
                IrbisFormat.All,
                mfnList.Length
            };
            foreach (int mfn in mfnList)
            {
                arguments.Add(mfn);
            }

            UniversalCommand command
                = Connection.CommandFactory.GetUniversalCommand
                    (
                        CommandCode.FormatRecord,
                        arguments.ToArray()
                    );
            command.AcceptAnyResponse = true;

            ServerResponse response 
                = Connection.ExecuteCommand(command);

            string[] result = response.RemainingUtfStrings()
                .Skip(1)
                .ToArray();

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load readers from database.
        /// </summary>
        public void LoadReaders()
        {
            int maxMfn = Connection.GetMaxMfn(Database);
            const int delta = 1000;

            DataflowLinkOptions linkOptions = new DataflowLinkOptions
            {
                PropagateCompletion = true
            };

            TransformBlock<string, MarcRecord> parseRecordBlock
                = new TransformBlock<string, MarcRecord>
                (
                    line => _ParseRecord(line),
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = 6,
                        SingleProducerConstrained = true
                    }
                );

            TransformBlock<MarcRecord, ReaderInfo> parseReaderBlock
                = new TransformBlock<MarcRecord, ReaderInfo>
                (
                    record => _ParseReader(record),
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = 4
                    }
                );

            ActionBlock<ReaderInfo> analyzeReaderBlock
                = new ActionBlock<ReaderInfo>
                (
                    reader => _AnalyzeReader(reader),
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = 4
                    }
                );

            parseRecordBlock.LinkTo
                (
                    parseReaderBlock,
                    linkOptions
                );

            parseReaderBlock.LinkTo
                (
                    analyzeReaderBlock,
                    linkOptions
                );

            DateTime today = DateTime.Today;

            _debtorManager
                = new DebtorManager(Connection)
                {
                    FromDate = today.AddYears(-1),
                    ToDate = today.AddMonths(-1)
                };
            _debtorManager.SetupDates();

            for (int offset = 1; offset < maxMfn; offset += delta)
            {
                WriteLine
                    (
                        "Загружается: {0} из {1}",
                        offset - 1,
                        maxMfn - 1
                    );

                int portion = Math.Min(delta, maxMfn - offset);

                int[] list = Enumerable.Range(offset, portion)
                    .ToArray();
                string[] lines = _ReadRawRecords(list);
                foreach (string line in lines)
                {
                    parseRecordBlock.Post(line);
                }
            }
            parseRecordBlock.Complete();

            analyzeReaderBlock.Completion.Wait();

            Readers.CompleteAdding();
            Debtors.CompleteAdding();
        }

        public void WriteDelimiter()
        {
            Log.WriteLine(new string('=', 60));
        }

        public void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            Log.WriteLine(format, args);
        }

        #endregion
    }
}
