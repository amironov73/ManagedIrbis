// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReservPanel.cs -- 
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Configuration;
using AM.Data;
using AM.IO;
using AM.Json;
using AM.Logging;
using AM.Reflection;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Menus;
using ManagedIrbis.Readers;
using ManagedIrbis.Reservations;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

// ReSharper disable CoVariantArrayConversion

namespace ReservDesktop
{
    public partial class ReservPanel
        : UniversalCentralControl
    {
        #region Properties

        [NotNull]
        public BusyController Controller
        {
            get { return MainForm.ThrowIfNull().Controller; }
        }

        [CanBeNull]
        public string CurrentRoom
        {
            get
            {
                string result = null;
                MenuEntry current = _roomBox.SelectedItem as MenuEntry;
                if (!ReferenceEquals(current, null))
                {
                    result = current.Code;
                }
                return result;
            }
        }

        /// <summary>
        /// Current grid line.
        /// </summary>
        [CanBeNull]
        public ReservationInfo CurrentResource
        {
            get { return _bindingSource.Current as ReservationInfo; }
        }

        [NotNull]
        public string Prefix { get; set; }

        [NotNull]
        public IrbisProvider Provider
        {
            get { return MainForm.ThrowIfNull().GetIrbisProvider().ThrowIfNull(); }
        }

        [NotNull]
        public ReservationManager Manager
        {
            get
            {
                if (ReferenceEquals(_manager, null))
                {
                    _manager = new ReservationManager(Provider);
                }

                return _manager;
            }
        }

        [CanBeNull]
        public string Ticket
        {
            get { return _ticketBox.Text.Trim(); }
        }

        #endregion

        #region Construction

        // ReSharper disable NotNullMemberIsNotInitialized

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReservPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReservPanel
            (
                [NotNull] MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();

            _grid.AutoGenerateColumns = false;
            //_AtStartup();
        }

        // ReSharper restore NotNullMemberIsNotInitialized

        #endregion

        #region Private members

        private ReservationManager _manager;

        private void _AtStartup()
        {
            MenuEntry[] entries = null;
            Controller.Run
                (
                    () =>
                    {
                        entries = Manager.ListRooms();
                    }
                );
            _roomBox.Items.AddRange(entries);
            if (entries.Length != 0)
            {
                _roomBox.SelectedIndex = 0;
            }
        }

        #endregion

        #region Public methods


        #endregion

        private void _RefreshGrid()
        {
            _bindingSource.DataSource = null;
            string roomCode = CurrentRoom;
            if (!string.IsNullOrEmpty(roomCode))
            {
                ReservationInfo[] items = null;
                Controller.Run
                (
                    () =>
                    {
                        items = Manager.ListResources(roomCode);
                    }
                );
                _bindingSource.DataSource = items;
            }
        }

        private void _roomBox_SelectedIndexChanged
            (
                object sender,
                EventArgs e
            )
        {
            _RefreshGrid();
        }

        private void _connectButton_Click(object sender, EventArgs e)
        {
            _AtStartup();
        }

        private void _grid_RowPrePaint
            (
                object sender,
                DataGridViewRowPrePaintEventArgs e
            )
        {
            DataGridView grid = (DataGridView)sender;
            int index = e.RowIndex;
            if (index < 0 || index >= grid.RowCount)
            {
                return;
            }
            DataGridViewRow row = grid.Rows[index];
            ReservationInfo item = row.DataBoundItem as ReservationInfo;
            if (ReferenceEquals(item, null))
            {
                return;
            }
            if (item.Status != ReservationStatus.Free)
            {
                e.PaintParts &= ~DataGridViewPaintParts.Background;
                Graphics graphics = e.Graphics;
                Rectangle rectangle = e.RowBounds;
                Color color = index % 2 == 0
                    ? Color.LightPink
                    : Color.HotPink;
                if ((e.State & DataGridViewElementStates.Selected) != 0)
                {
                    color = Color.Red;
                }
                using (Brush brush = new SolidBrush(color))
                {
                    graphics.FillRectangle(brush, rectangle);
                }
            }
        }

        [CanBeNull]
        private string _GetReaderDescription
            (
                [NotNull] string ticket
            )
        {
            Code.NotNullNorEmpty(ticket, "ticket");

            string previousDatabase = Provider.Database;
            Provider.Database = ReaderUtility.DatabaseName;
            try
            {
                string expression = string.Format
                    (
                        "\"{0}{1}\"",
                        ReaderUtility.IdentifierPrefix,
                        ticket
                    );
                int[] found = Provider.Search(expression);
                if (found.Length == 0)
                {
                    WriteLine("Читатель с билетом {0} не найден", ticket);

                    return null;
                }
                if (found.Length != 1)
                {
                    WriteLine("Много читателей с билетом {0}", ticket);

                    return null;
                }
                int mfn = found[0];
                MarcRecord record = Provider.ReadRecord(mfn);
                if (ReferenceEquals(record, null))
                {
                    WriteLine("Ошибка при чтении записи MFN={0}");

                    return null;
                }
                string result = Provider.FormatRecord(record, "@brief");

                return result;
            }
            finally
            {
                Provider.Database = previousDatabase;
            }
        }

        private void _ticketBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (e.KeyData != Keys.Enter)
            {
                return;
            }
            _readerBox.Text = null;
            e.Handled = true;
            string ticket = Ticket;
            if (string.IsNullOrEmpty(ticket))
            {
                return;
            }
            string description = null;
            Controller.Run
                (
                    () =>
                    {
                        description = _GetReaderDescription(ticket);
                    }
                );
            _readerBox.Text = description;
        }

        private void _giveButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ReservationInfo resource = CurrentResource;
            if (ReferenceEquals(resource, null))
            {
                return;
            }
            if (resource.Status != ReservationStatus.Free)
            {
                return;
            }

            string ticket = Ticket;
            if (string.IsNullOrEmpty(ticket))
            {
                return;
            }

            Controller.Run
                (
                    () =>
                    {
                        int index = _bindingSource.Position;
                        resource = Manager.AssignResource(resource, ticket);
                        ReservationInfo[] array
                            = (ReservationInfo[])_bindingSource.DataSource;
                        array[index] = resource;
                    }
                );

            _bindingSource.ResetCurrentItem();
            Invalidate();
        }

        private void _returnButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ReservationInfo resource = CurrentResource;
            if (ReferenceEquals(resource, null))
            {
                return;
            }
            if (resource.Status == ReservationStatus.Free)
            {
                return;
            }

            Controller.Run
                (
                    () =>
                    {
                        int index = _bindingSource.Position;
                        resource = Manager.ReleaseResource(resource);
                        ReservationInfo[] array
                            = (ReservationInfo[])_bindingSource.DataSource;
                        array[index] = resource;
                    }
                );

            _bindingSource.ResetCurrentItem();
            Invalidate();
        }

        private void _refreshButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _RefreshGrid();
        }

        private void _refreshTimer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            _grid.Invalidate();
        }
    }
}
