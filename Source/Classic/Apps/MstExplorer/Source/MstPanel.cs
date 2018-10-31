// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstPanel.cs --
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
using AM.UI;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Direct;
using ManagedIrbis.Fields;
using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace MstExplorer
{
    public partial class MstPanel
        : UniversalCentralControl
    {
        #region Properties



        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MstPanel()
            : base(null)
        {
            InitializeComponent();
            CreateToolboxItems();
            OpenMstFile();
        }

        public MstPanel
            (
                [NotNull] MainForm mainForm
            )
            : base(mainForm)
        {
            InitializeComponent();
            CreateToolboxItems();
            OpenMstFile();
        }


        #endregion

        #region Private members

        private FoundRecord[] _foundRecords;
        private MstFile64 _mstFile;

        private void CreateToolboxItems()
        {
            ToolStripItem openItem = MainForm.ToolStrip.Items.Add("Open...");
            openItem.Click += _OpenItem_Click;
        }

        private void _OpenItem_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                OpenMstFile(_openFileDialog.FileName);
            }
        }

        #endregion

        #region Public methods

        public void OpenMstFile()
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                OpenMstFile(args[1]);
            }
        }

        public void OpenMstFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            if (!ReferenceEquals(_mstFile, null))
            {
                _mstFile.Dispose();
                _mstFile = null;
            }


            WriteLine("Finding records...");
            Run(() =>
            {
                _foundRecords = MstRecover64.FindRecords(fileName);
            });
            WriteLine("Records found: {0}", _foundRecords.Length);

            _mstFile = new MstFile64(fileName, DirectAccessMode.ReadOnly);
            _bindingSource.DataSource = _foundRecords;
        }

        #endregion

        private void _bindingSource_CurrentChanged
            (
                object sender,
                EventArgs e
            )
        {
            if (ReferenceEquals(_foundRecords, null))
            {
                return;
            }

            int index = _bindingSource.Position;
            if (index < 0 && index >= _foundRecords.Length)
            {
                return;
            }

            FoundRecord found = _foundRecords[index];
            MstRecord64 mstRecord = _mstFile.ReadRecord(found.Position);
            _leaderBox.Text = mstRecord.Leader.ToString();
            _grid.AutoGenerateColumns = false;
            _grid.DataSource = mstRecord.Dictionary;
        }

        protected override void OnHandleDestroyed
            (
            EventArgs e
            )
        {
            if (!ReferenceEquals(_mstFile, null))
            {
                _mstFile.Dispose();
                _mstFile = null;
            }

            base.OnHandleDestroyed(e);
        }
    }
}
