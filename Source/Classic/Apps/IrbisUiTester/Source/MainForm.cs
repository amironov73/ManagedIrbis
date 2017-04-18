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
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Text.Output;

using IrbisUI;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure.Sockets;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace IrbisUiTester
{
    public partial class MainForm 
        : Form
    {
        #region Properties

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public AbstractOutput Output
        { get { return _logBox.Output; } }

        #endregion

        #region Construction

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private IrbisConnection GetConnection()
        {
            string connectionString
                = CM.AppSettings["connectionString"];

            IrbisConnection result
                = new IrbisConnection(connectionString);

            if (_slowBox.CheckBox.Checked)
            {
                SlowSocket slow = new SlowSocket
                    (
                        result,
                        result.Socket
                    )
                {
                    Delay = 1000
                };
                result.SetSocket(slow);
            }

            if (_brokenBox.CheckBox.Checked)
            {
                BrokenSocket broken = new BrokenSocket
                    (
                        result,
                        result.Socket
                    )
                {
                    Probability = 0.5
                };
                result.SetSocket(broken);
            }

            if (_busyBox.CheckBox.Checked)
            {
                _busyStripe.SubscribeTo(result);
            }

            if (_retryBox.CheckBox.Checked)
            {
                RetryManager manager =
                    result.SetupRetryForm();
                manager.ExceptionOccurs += ExceptionOccurs;
                manager.Resolved += ExceptionResolved;
                result.Disposing += ConnectionDisposing;
            }

            return result;
        }

        private void ConnectionDisposing
            (
                object sender,
                EventArgs eventArgs
            )
        {
            IrbisConnection connection = sender as IrbisConnection;
            if (!ReferenceEquals(connection, null))
            {
                RetryClientSocket socket = connection.Socket
                    as RetryClientSocket;
                if (!ReferenceEquals(socket, null))
                {
                    RetryManager manager = socket.RetryManager;
                    manager.ExceptionOccurs -= ExceptionOccurs;
                    manager.Resolved -= ExceptionResolved;
                }
                connection.Disposing -= ConnectionDisposing;
            }

            WriteLine("Disconnected");
        }

        private void ExceptionResolved
            (
                object sender,
                EventArgs eventArgs
            )
        {
            WriteLine("Exception resolved");
        }

        private void ExceptionOccurs
            (
                object sender,
                ExceptionEventArgs eventArgs
            )
        {
            WriteLine
                (
                    "Exception: {0}",
                    eventArgs.Exception.Message
                );
        }

        private void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Output.WriteLine(format, args);
        }

        #endregion

        private async void _pingItem_Click
            (
                object sender,
                EventArgs e
            )
        {
            using (IrbisConnection connection = GetConnection())
            {
                WriteLine("Ping start");
                await connection.NoOpAsync();
                WriteLine("Ping done");
            }
        }
    }
}
