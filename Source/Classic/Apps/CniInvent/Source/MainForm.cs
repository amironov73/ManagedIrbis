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

        private StatusRecord _goodRecord;

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
                _goodRecord = null;
                e.SuppressKeyPress = true;

                string text = _placeBox.Text.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("Не указано место хранения!");
                    return;
                }

                text = _rfidBox.Text.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    HandleRfid(text);
                }
                _rfidBox.Clear();
            }
        }

        private bool CheckPodsob
            (
                [NotNull] string rfid,
                [NotNull] PodsobRecord podsob
            )
        {
            Code.NotNull(podsob, "podsob");

            bool usePodsob = podsob.Inventory != 0;

            string place = _placeBox.Text.Trim();
            if (string.IsNullOrEmpty(place))
            {
                WriteLine("Не задано место хранения!");

                return false;
            }

            if (usePodsob && !string.IsNullOrEmpty(podsob.Ticket)
                && !podsob.Ticket.SameString(place))
            {
                WriteLine
                    (
                        "Место хранение не совпадает: {0}",
                        podsob.Ticket.ToVisibleString()
                    );

                return false;
            }

            if (usePodsob && !string.IsNullOrEmpty(podsob.OnHand))
            {
                WriteLine
                    (
                        "Книга числится за {0}",
                        podsob.OnHand.ToVisibleString()
                    );

                return false;
            }

            if (usePodsob && !string.IsNullOrEmpty(podsob.Sigla)
                && !place.SameString(podsob.Sigla))
            {
                WriteLine
                    (
                        "Не совпадает сигла: {0}",
                        podsob.Sigla.ToVisibleString()
                    );

                return false;
            }

            MarcRecord record = podsob.Record.ThrowIfNull("podsob.Record");
            RecordField field = record.Fields
                .FirstOrDefault
                (
                    f => rfid.SameString(f.GetFirstSubFieldValue('h'))
                        || rfid.SameString(f.GetFirstSubFieldValue('t'))
                );
            if (ReferenceEquals(field, null))
            {
                WriteLine("Не найден экземпляр: {0}", rfid);

                return false;
            }

            string status = field.GetFirstSubFieldValue('a');
            if (!status.SameString("0"))
            {
                WriteLine
                    (
                        "Неожиданный статус: {0}",
                        status.ToVisibleString()
                    );

                return false;
            }

            string inventory = field.GetFirstSubFieldValue('b');
            if (string.IsNullOrEmpty(inventory))
            {
                WriteLine
                    (
                        "Инвентарный номер: {0}",
                        inventory.ToVisibleString()
                    );

                return false;
            }
            if (usePodsob 
                && !inventory.SameString(podsob.Inventory.ToInvariantString()))
            {
                WriteLine
                    (
                        "Инвентарный номер не совпадает: {0} и {1}",
                        podsob.Inventory,
                        inventory
                    );

                return false;
            }

            string fieldPlace = field.GetFirstSubFieldValue('d');
            if (!place.SameString(fieldPlace))
            {
                WriteLine
                    (
                        "Не совпадает место хранения: {0}",
                        fieldPlace.ToVisibleString()
                    );

                return false;
            }

            _goodRecord = new StatusRecord
            {
                Inventory = podsob.Inventory,
                Record = record,
                Description = record.Description,
                Field = field,
                Place = place
            };

            return true;
        }

        private void HandleRfid
            (
                [NotNull] string rfid
            )
        {
            _goodRecord = null;
            SetStatus(new bool?());
            _descriptionBox.Clear();
            PodsobRecord podsob = Kladovka.FindPodsobByBarcode(rfid);
            if (!ReferenceEquals(podsob, null)
                && !ReferenceEquals(podsob.Record, null))
            {
                _descriptionBox.Text = podsob.Record.Description;

                bool status = CheckPodsob(rfid, podsob);
                SetStatus(status);
                if (status)
                {
                    
                }
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

        private void _confirmButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            StatusRecord status = _goodRecord;
            if (ReferenceEquals(status, null))
            {
                return;
            }

            WriteLine("Помечаем: {0}", status.Inventory);
            RecordField field = status.Field;
            field.SetSubField('s', IrbisDate.TodayText);
            field.SetSubField('!', status.Place);
            Kladovka.Connection.WriteRecord(status.Record);
        }
    }
}
