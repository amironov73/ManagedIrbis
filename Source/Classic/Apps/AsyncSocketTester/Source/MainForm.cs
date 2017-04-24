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
using AM.Windows.Forms;
using ManagedIrbis;
using ManagedIrbis.Infrastructure;

#endregion

namespace AsyncSocketTester
{
    public partial class MainForm 
        : Form
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainForm
            (
                string connectionString
            )
        {
            _connectionString = connectionString;

            InitializeComponent();
            _console = new ConsoleControl();
            Controls.Add(_console);
        }

        #endregion

        #region Private members

        private readonly string _connectionString;

        private readonly ConsoleControl _console;

        #endregion

        private void _pressMeButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                using (IrbisConnection connection 
                    = new IrbisConnection())
                {
                    AsyncClientSocket socket
                        = new AsyncClientSocket(connection);
                    connection.SetSocket(socket);

                    connection.ParseConnectionString
                        (
                            _connectionString
                        );

                    connection.Connect();

                    _console.WriteLine("Connected");

                    int maxMfn = connection.GetMaxMfn();
                    _console.WriteLine
                        (
                            string.Format
                                (
                                    "Max MFN={0}",
                                    maxMfn
                                )
                        );
                }

                _console.WriteLine("Diconnected");
                _console.WriteLine();
            }
            catch (Exception exception)
            {
                _console.WriteLine
                    (
                        Color.Red,
                        exception.Message
                    );
            }
        }
    }
}
