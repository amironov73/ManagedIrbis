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
using AM.Text.Output;
using AM.Windows.Forms;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using CodeJam;

using IrbisUI;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace SpisanoLi
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
            //Kladovka = new Kladovka(Log);
            //IdleManager = new IrbisIdleManager
            //(
            //    Kladovka.Connection,
            //    60 * 1000
            //);
            //IdleManager.Idle += IdleManager_Idle;
        }

        #endregion

        #region Private members

        private void _findButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            FindInventory(_inputBox.Text);
            _inputBox.Text = string.Empty;
            _inputBox.Focus();
        }

        private void _inputBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (e.KeyData == Keys.Enter)
            {
                _findButton_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            this.ShowVersionInfoInTitle();
            Log.PrintSystemInformation();
        }

        #endregion

        #region Public methods

        public void FindInventory
            (
                string number
            )
        {
            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            number = number.Trim();
            if (string.IsNullOrEmpty(number))
            {
                return;
            }
            using (DbManager db = new DbManager("Cards"))
            {
                Table<WrittenOff> writtenOff = db.GetTable<WrittenOff>();
                WrittenOff found = writtenOff.FirstOrDefault
                    (
                        item => item.Number == number
                    );

                WriteLine
                    (
                        ReferenceEquals(found, null)
                            ? "Номер {0} НЕ списан"
                            : "Номер {0} списан",
                        number
                    );
            }
        }

        public void Write
            (
                [NotNull] string text
            )
        {
            Log.Write(text);
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
