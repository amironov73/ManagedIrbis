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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Text.Output;
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
            _output = _logBox.Output;
        }

        #endregion

        #region Private members

        private readonly string _connectionString;

        private readonly AbstractOutput _output;

        #endregion

        private async void _pressMeButton_Click
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
                    _pressMeButton.Enabled = false;
                    try
                    {

                        AsyncClientSocket socket
                            = new AsyncClientSocket(connection);
                        connection.SetSocket(socket);

                        connection.ParseConnectionString
                        (
                            _connectionString
                        );

                        await connection.ConnectAsync();

                        _output.WriteLine("Connected");

                        int maxMfn 
                            = await connection.GetMaxMfnAsync();
                        _output.WriteLine
                        (
                            "Max MFN={0}",
                            maxMfn
                        );

                        await connection.DisconnectAsync();

                    }
                    finally
                    {
                        _pressMeButton.Enabled = true;
                    }
                }

                _output.WriteLine("Diconnected");
                _output.WriteLine(string.Empty);
            }
            catch (Exception exception)
            {
                _output.WriteLine
                (
                    exception.ToString()
                );
            }
        }

        private async void _press2Button_Click
            (
                object sender,
                EventArgs e
            )
        {
            using (TcpClient client = new TcpClient())
            {
                byte[] garbage = new byte[1000];

                Debug.WriteLine("Before connect");
                await client.ConnectAsync
                    (
                        IPAddress.Loopback,
                        6666
                    );
                Debug.WriteLine("After connect");
                Stream stream = client.GetStream();
                Debug.WriteLine("Before write");
                await stream.WriteAsync(garbage, 0, garbage.Length);
                Debug.WriteLine("After write");
                await stream.ReadAsync(garbage, 0, garbage.Length);
                Debug.WriteLine("After read");

                _logBox.AppendText("It works"
                                   + Environment.NewLine);

            }
        }
    }
}
