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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Text.Output;
using AM.Windows.Forms;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure.Sockets;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace BookTerminator
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public AbstractOutput Output
        {
            get { return _logBox.Output; }
        }

        /// <summary>
        /// Current MFN.
        /// </summary>
        public int CurrentMfn { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            Connection = new IrbisConnection();
            _busyStripe.SubscribeTo(Connection);
        }

        #endregion

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            this.ShowVersionInfoInTitle();
            Output.PrintSystemInformation();

            //SlowSocket slowSocket = new SlowSocket
            //    (
            //        Connection,
            //        Connection.Socket
            //    );
            //Connection.SetSocket(slowSocket);

            RetryManager manager = new RetryManager
                (
                    5,
                    _Resolver
                );
            RetryClientSocket retrySocket
                = new RetryClientSocket
                    (
                        Connection,
                        Connection.Socket,
                        manager
                    );
            Connection.SetSocket(retrySocket);

            string connectionString
                = CM.AppSettings["connectionString"];
            Connection.ParseConnectionString
                (
                    connectionString
                );
            Connection.Connect();

            WriteLine
                (
                    "Connected to {0}, database {1}",
                    Connection.Host,
                    Connection.Database
                );
        }

        public bool _Resolver
            (
                Exception exception
            )
        {
            this.InvokeIfRequired
                (
                    () =>
                    {
                        WriteLine
                            (
                                "Exception: {0}",
                                exception.Message
                            );
                    }
                );

            return true;
        }

        public void WriteLine
            (
                string format,
                params object[] args
            )
        {
            Output.WriteLine(format, args);
        }

        public void WaitForBrowser()
        {
            Application.DoEvents();
            while (_browser.IsBusy)
            {
                Application.DoEvents();
                Thread.Sleep(20);
            }
            Application.DoEvents();
        }

        public void SetHtml
            (
                string text
            )
        {
            if (_browser.Document == null)
            {
                _browser.Navigate("about:blank");
                WaitForBrowser();
            }

            // One more time
            if (_browser.Document == null)
            {
                _browser.Navigate("about:blank");
                WaitForBrowser();
            }

            _browser.DocumentText = text;
            WaitForBrowser();
        }

        private void MainForm_FormClosing
            (
                object sender,
                FormClosingEventArgs e
            )
        {
            _busyStripe.UnsubscribeFrom(Connection);
            Connection.Dispose();
            Console.WriteLine("Disconnected");
        }

        private async void _findButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            SetHtml(string.Empty);

            string mfnText = _mfnBox.Text.Trim();
            if (string.IsNullOrEmpty(mfnText))
            {
                return;
            }

            int mfn = mfnText.SafeToInt32();
            if (mfn <= 0)
            {
                WriteLine("Bad MFN: {0}", mfnText);
            }

            try
            {
                string formatted = string.Empty;

                await Task.Run
                    (
                        () =>
                        {
                            string format 
                                = CM.AppSettings["format"];
                            formatted = Connection.FormatRecord
                                (
                                    format,
                                    mfn
                                );

                            MarcRecord record
                                = Connection.ReadRecord(mfn);
                            if (record.Deleted)
                            {
                                formatted 
                                    = "<strong>DELETED</strong><br/>"
                                    + formatted;
                            }
                        }
                    );

                SetHtml(formatted);

                WriteLine("MFN: {0}", mfn);
                CurrentMfn = mfn;
            }
            catch (Exception ex)
            {
                WriteLine("Searching for MFN: {0}", mfn);
                WriteLine("Exception: {0}", ex);
            }
        }

        private async void _deleteButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (CurrentMfn != 0)
            {
                WriteLine
                    (
                        "Delete MFN: {0}",
                        CurrentMfn
                    );

                await Task.Run
                    (
                        () =>
                        {
                            Connection.DeleteRecord(CurrentMfn);
                        }
                    );
            }

            _mfnBox.Clear();
            CurrentMfn = 0;
            SetHtml(string.Empty);
            _mfnBox.Focus();
        }

        private async void _timer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            if (Connection.Connected)
            {
                await Connection.NoOpAsync();
                WriteLine("NOP");
            }
        }

        private void _mfnBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (e.KeyData == Keys.Enter)
            {
                _findButton_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyData == Keys.F2)
            {
                _deleteButton_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
