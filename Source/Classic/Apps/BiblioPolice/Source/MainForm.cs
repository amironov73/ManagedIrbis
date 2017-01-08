/* MainForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace BiblioPolice
{
    public partial class MainForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Connection to IRBIS-server.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Readers.
        /// </summary>
        [NotNull]
        public BlockingCollection<ReaderInfo> Readers
        {
            get;
            private set;
        }

        /// <summary>
        /// Readers classified by status.
        /// </summary>
        [NotNull]
        public DictionaryList<string, ReaderInfo> ReadersByStatus
        {
            get;
            private set;
        }

        /// <summary>
        /// New readers with overdue loans.
        /// </summary>
        [NotNull]
        public BlockingCollection<DebtorInfo> Debtors
        {
            get;
            private set;
        }

        /// <summary>
        /// Log output.
        /// </summary>
        [NotNull]
        public TextBoxOutput Log { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            Log = new TextBoxOutput(_logBox);
            Readers = new BlockingCollection<ReaderInfo>();
            Debtors = new BlockingCollection<DebtorInfo>();
            ReadersByStatus 
                = new DictionaryList<string, ReaderInfo>();
            Connection = new IrbisConnection();
        }

        #endregion

        #region Private members

        private DebtorManager _debtorManager;

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            Connection.Dispose();
        }

        private void LoadReaders()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            WriteLine("Начало загрузки читателей");

            IEnumerable<MarcRecord> records 
                = BatchRecordReader.WholeDatabase
                (
                    Connection,
                    Connection.Database,
                    1000
                );
            BatchRecordReader batch 
                = records as BatchRecordReader;
            if (!ReferenceEquals(batch, null))
            {
                batch.BatchRead += Batch_BatchRead;
            }

            records.ProcessData(ParseAndAddReader);
            Readers.CompleteAdding();

            WriteDelimiter();
            WriteLine("Распределение читателей");
            string[] keys = ReadersByStatus.Keys;
            foreach (string key in keys)
            {
                WriteLine
                    (
                        "Статус {0}: {1} читателей",
                        key,
                        ReadersByStatus[key].Length
                    );
            }
            WriteDelimiter();

            DateTime today = DateTime.Today;

            _debtorManager
                = new DebtorManager(Connection)
                {
                    FromDate = today.AddYears(-1),
                    ToDate = today.AddMonths(-1)
                };
            _debtorManager.SetupDates();

            ReadersByStatus["0"].ProcessData(AnalyzeCandidate);
            Debtors.CompleteAdding();

            WriteLine
                (
                    "Кандидатов в должники: {0}", 
                    Debtors.Count
                );

            WriteLine("Окончание загрузки читателей");
            stopwatch.Stop();
            WriteLine
                (
                    "Загрузка заняла: {0}",
                    stopwatch.Elapsed.ToAutoString()
                );
            WriteLine("Загружено: {0}", Readers.Count);
            WriteDelimiter();
        }

        private void LoadReaders2()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            WriteDelimiter();
            WriteLine("Начало загрузки читателей");

            DataflowBatchReader batchReader
                = new DataflowBatchReader
                (
                    Connection,
                    "RDR",
                    Log
                );

            batchReader.LoadReaders();

            Readers = batchReader.Readers;
            Debtors = batchReader.Debtors;

            WriteDelimiter();

            foreach (ReaderInfo reader in Readers)
            {
                string status = reader.Status ?? "0";
                ReadersByStatus.Add(status, reader);
            }

            WriteLine("Распределение читателей");
            string[] keys = ReadersByStatus.Keys;
            foreach (string key in keys)
            {
                WriteLine
                    (
                        "Статус {0}: {1} читателей",
                        key,
                        ReadersByStatus[key].Length
                    );
            }
            WriteDelimiter();

            WriteLine
                (
                    "Кандидатов в должники: {0}",
                    Debtors.Count
                );

            WriteLine("Окончание загрузки читателей");
            stopwatch.Stop();
            WriteLine
                (
                    "Загрузка заняла: {0}",
                    stopwatch.Elapsed.ToAutoString()
                );
            WriteLine("Загружено: {0}", Readers.Count);
            WriteDelimiter();
        }

        private void AnalyzeCandidate
            (
                [NotNull] ReaderInfo reader
            )
        {
            DebtorInfo debtor = _debtorManager.GetDebtor(reader);
            if (!ReferenceEquals(debtor, null))
            {
                Debtors.Add(debtor);
            }
        }

        private void ParseAndAddReader
            (
                MarcRecord record
            )
        {
            ReaderInfo reader = ReaderInfo.Parse(record);
            string status = reader.Status;
            if (string.IsNullOrEmpty(status))
            {
                reader.Status = "0";
                status = "0";
            }
            Readers.Add(reader);
            ReadersByStatus.Add(status, reader);
        }

        private void Batch_BatchRead
            (
                object sender,
                EventArgs e
            )
        {
            BatchRecordReader batch = (BatchRecordReader) sender;

            WriteLine
                (
                    "Загружено: {0} из {1}",
                    batch.RecordsRead,
                    batch.TotalRecords
                );
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            WriteLine("Запуск программы");

            this.ShowVersionInfoInTitle();
            Log.PrintSystemInformation();

            string connectionString 
                = CM.AppSettings["connectionString"];
            Connection.ParseConnectionString(connectionString);
            Connection.Connect();
            WriteLine("Подключено к серверу");

            int count = Connection.GetMaxMfn() - 1;
            WriteLine
                (
                    "БД {0} содержит {1} записей",
                    Connection.Database,
                    count
                );

            Task.Factory.StartNew(LoadReaders2);
        }

        #endregion

        #region Public methods

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
