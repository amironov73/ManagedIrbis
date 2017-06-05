// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.Istu.OldModel;
using AM.Windows.Forms;

using CodeJam;
using IrbisUI;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace CniInvent
{
    public partial class MainForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Connection to IRBIS-server.
        /// </summary>
        [NotNull]
        public Kladovka Kladovka { get; private set; }

        /// <summary>
        /// Idle manager.
        /// </summary>
        [NotNull]
        public IrbisIdleManager IdleManager { get; private set; }

        /// <summary>
        /// Log output.
        /// </summary>
        [NotNull]
        public TextBoxOutput Log { get; private set; }

        #endregion

        #region Construction

        public MainForm()
        {
            InitializeComponent();

            Log = new TextBoxOutput(_logBox);
            Kladovka = new Kladovka();
            IdleManager = new IrbisIdleManager
                (
                    Kladovka.Connection,
                    60 * 1000
                );
            IdleManager.Idle += IdleManager_Idle;
        }

        #endregion

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (!ReferenceEquals(Kladovka, null))
            {
                Kladovka.Dispose();
            }
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

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            WriteLine("Запуск программы");

            this.ShowVersionInfoInTitle();
            Log.PrintSystemInformation();

            WriteLine("Подключено к серверу");
        }

        private void _rfidBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (e.KeyCode == Keys.Enter)
            {
                string text = _rfidBox.Text.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    HandleRfid(text);
                }
                _rfidBox.Clear();
                e.SuppressKeyPress = true;
            }
        }

        private void CheckPodsob
            (
                [NotNull] PodsobRecord podsob
            )
        {
            Code.NotNull(podsob, "podsob");
            MarcRecord record = podsob.Record.ThrowIfNull("podsob.Record");

            SetStatus(true);
        }

        private void HandleRfid
            (
                [NotNull] string rfid
            )
        {
            SetStatus(new bool?());
            _descriptionBox.Clear();
            PodsobRecord podsob = Kladovka.FindPodsobByBarcode(rfid);
            if (!ReferenceEquals(podsob, null)
                && !ReferenceEquals(podsob.Record, null))
            {
                _descriptionBox.Text = podsob.Record.Description;
                CheckPodsob(podsob);
            }
            else
            {
                SetStatus(false);
            }
        }

        private void IdleManager_Idle
            (
                object sender,
                EventArgs e
            )
        {
            WriteLine("NO-OP");
        }

        public void SetStatus
            (
                bool? status
            )
        {
            if (!status.HasValue)
            {
                _indicatorPanel.BackColor = DefaultBackColor;
            }
            else
            {
                if (status.Value)
                {
                    _indicatorPanel.BackColor = Color.LimeGreen;
                }
                else
                {
                    _indicatorPanel.BackColor = Color.Red;
                }
            }
        }
    }
}
