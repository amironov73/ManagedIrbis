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

using AM.Windows.Forms;

using ManagedIrbis;
using ManagedIrbis.Statistics;

#endregion

namespace PingMonitor
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

        private IrbisConnection _connection;

        private IrbisPing _pinger;

        public IrbisConnection GetConnection()
        {
            IrbisConnection result
                = IrbisConnectionUtility.GetClientFromConfig();

            return result;
        }


        #endregion

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            this.ShowVersionInfoInTitle();

            _connection = GetConnection();
            _connection.Connect();

            this.Text += ": " + _connection.Host;

            _pinger = new IrbisPing(_connection)
            {
                Active = true
            };
            _pinger.StatisticsUpdated += _pinger_StatisticsUpdated;
            _plotter.Statistics = _pinger.Statistics;
        }

        private void _pinger_StatisticsUpdated
            (
                object sender,
                EventArgs e
            )
        {
            PingStatistics statistics = _pinger.Statistics;
            if (statistics.Data.Count > 1500)
            {
                while (statistics.Data.Count > 1000)
                {
                    statistics.Data.Dequeue();
                }
            }

            _plotter.Invalidate();
        }

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            _pinger.Active = false;
            _pinger.Dispose();
            _connection.Dispose();
        }
    }
}
