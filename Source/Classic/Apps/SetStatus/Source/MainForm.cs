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
using AM.Threading;
using AM.Windows.Forms;
using IrbisUI;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.Menus;

#endregion

namespace SetStatus
{
    public partial class MainForm
        : Form
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private IrbisConnection GetConnection()
        {
            IrbisConnection result 
                = IrbisConnectionUtility.GetClientFromConfig();

            SlowSocket socket = new SlowSocket(result, result.Socket)
                {
                    Delay = 1000
                };
            result.SetSocket(socket);

            result.Busy.StateChanged += Busy_StateChanged;
            result.Disposing += Connection_Disposing;

            return result;
        }

        private void Busy_StateChanged
            (
                object sender,
                EventArgs e
            )
        {
            BusyState state = (BusyState) sender;

            this.InvokeIfRequired
                (
                    () =>
                    {
                        _busyStripe.Visible = state;
                        _busyStripe.Moving = state;
                    }
                );
        }

        private void Connection_Disposing
            (
                object sender,
                EventArgs e
            )
        {
            IrbisConnection connection = (IrbisConnection) sender;

            connection.Busy.StateChanged -= Busy_StateChanged;
            connection.Disposing -= Connection_Disposing;
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
        }

        #endregion

        private void _firstTimer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            _firstTimer.Enabled = false;

            _browser.Navigate("about:blank");
            while (_browser.IsBusy)
            {
                Application.DoEvents();
            }
            _browser.Navigate("about:blank");
            while (_browser.IsBusy)
            {
                Application.DoEvents();
            }

            using (IrbisConnection connection = GetConnection())
            {
                FileSpecification specification
                    = new FileSpecification
                        (
                            IrbisPath.MasterFile,
                            connection.Database,
                            "ste.mnu"
                        );
                MenuFile menu = MenuFile.ReadFromServer
                    (
                        connection,
                        specification
                    );
                MenuEntry[] entries
                    = menu.SortEntries(MenuSort.ByCode);

                if (entries.Length == 0)
                {
                    throw new Exception("Empty entries list");
                }

                _statusBox.Items.AddRange(entries);
                _statusBox.SelectedIndex = 0;
            }

            _numberBox.Focus();

        }

        private async void _checkButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            using (IrbisConnection client = GetConnection())
            {
                _browser.Navigate("about:blank");
                try
                {
                    string expression = string.Format
                        (
                            "\"IN={0}\" + \"INS={0}\"",
                            _numberBox.Text.Trim()
                        );

                    int[] records = await client.SearchAsync
                        (
                            expression
                        );
                    if (records.Length == 0)
                    {
                        _browser.DocumentText = "НЕ НАЙДЕНО";
                        return;
                    }
                    if (records.Length != 1)
                    {
                        _browser.DocumentText = "НАЙДЕНО МНОГО";
                        return;
                    }


                    string html = await client.FormatRecordAsync
                        (
                            "@",
                            records[0]
                        );
                    _browser.DocumentText = html;
                }
                catch (Exception ex)
                {
                    _browser.DocumentText = ex.ToString();
                }
            }

        }
    }
}
