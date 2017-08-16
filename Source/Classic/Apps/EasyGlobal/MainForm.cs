// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MainForm.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Text.Output;
using AM.Windows.Forms;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Search;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace EasyGlobal
{
    public partial class MainForm 
        : Form
    {
        #region Properties

        public AbstractOutput Output { get; private set; }

        public IrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        public MainForm()
        {
            InitializeComponent();

            _console.Clear();

            _log = new FileOutput("log.txt");

            Output = new TeeOutput
                (
                    _log,
                    new ConsoleControlOutput(_console)
                );
            Connection = new IrbisConnection();
        }

        #endregion

        #region Private members

        private FileOutput _log;

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                string connectionString = CM.AppSettings["connectionString"];
                Connection.ParseConnectionString(connectionString);
                Connection.Connect();
                Output.WriteLine("Connected");
                _timer.Enabled = true;

                DatabaseInfo[] databases = Connection.ListDatabases();
                DatabaseInfo selectedDb = databases
                    .FirstOrDefault
                    (
                        db => db.Name.SameString(Connection.Database)
                    );

                _dbBox.Items.AddRange(databases);
                if (!ReferenceEquals(selectedDb, null))
                {
                    _dbBox.SelectedIndex = Array.IndexOf(databases, selectedDb);
                }
            }
            catch (Exception ex)
            {
                Output.WriteLine("Exception: {0}", ex);
            }
        }

        private void _timer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            Connection.NoOp();
        }

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            Connection.Dispose();
            Output.WriteLine("Disconnected");
            _log.Close();
        }

        #endregion

        private void _Printer
            (
                PftContext context,
                PftNode node,
                PftNode[] arguments
            )
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                string text = context.GetStringArgument(arguments, i);
                if (!string.IsNullOrEmpty(text))
                {
                    context.Write(node, text);
                }
            }
        }

        private void _goButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _console.Clear();

            string searchExpression = _searchBox.Text.Trim();
            if (string.IsNullOrEmpty(searchExpression))
            {
                return;
            }

            try
            {
                int counter = 0;

                string programText = _programBox.Text;
                IrbisProvider provider
                    = new ConnectedClient(Connection);
                PftFormatter formatter = new PftFormatter();
                formatter.SetProvider(provider);
                formatter.ParseProgram(programText);

                formatter.Context.Functions.Add("print", _Printer);

                DatabaseInfo database = (DatabaseInfo) _dbBox.SelectedItem;
                string databaseName = database.Name.ThrowIfNull();
                SearchParameters parameters = new SearchParameters
                {
                    Database = databaseName,
                    SearchExpression = searchExpression
                };

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                int[] found = Connection.SequentialSearch(parameters);
                Output.WriteLine("Found: {0}", found.Length);

                BatchRecordReader batch = new BatchRecordReader
                    (
                        Connection,
                        databaseName,
                        500,
                        found
                    );
                using (BatchRecordWriter buffer = new BatchRecordWriter
                    (
                        Connection,
                        databaseName,
                        100
                    ))
                {
                    foreach (MarcRecord record in batch)
                    {
                        record.Modified = false;
                        formatter.Context.ClearAll();
                        string text = formatter.FormatRecord(record);
                        if (!string.IsNullOrEmpty(text))
                        {
                            Output.WriteLine(text);
                        }

                        if (record.Modified)
                        {
                            counter++;
                            Output.WriteLine
                                (
                                    "[{0}] modified",
                                    record.Mfn
                                );
                            buffer.Append(record);
                        }

                        Application.DoEvents();
                    }
                }

                stopwatch.Stop();

                Output.WriteLine(string.Empty);
                Output.WriteLine("Done: {0}", stopwatch.Elapsed);
                Output.WriteLine("Records modified: {0}", counter);
                Output.WriteLine(string.Empty);

            }
            catch (Exception ex)
            {
                Output.WriteLine("Exception: {0}", ex);
            }
        }
    }
}
