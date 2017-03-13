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
using System.Windows.Forms;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.ImportExport;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace ViewDeleted
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _foundRecords = new List<int>();
        }

        private IrbisConnection _client;
        private int[] _deletedRecords;
        private readonly List<int> _foundRecords;
        private string _targetText;
        private string[] _targetWords;

        private void MainForm_Load
            (
                object sender, 
                EventArgs e
            )
        {
            _client = new IrbisConnection();
            string connectionString = CM.AppSettings["connection-string"];
            _client.ParseConnectionString(connectionString);
            try
            {
                _client.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }

            DatabaseInfo[] bases = _client.ListDatabases();
            _databaseBox.DisplayMember = "Name";
            _databaseBox.Items.AddRange(bases);
            _databaseBox.SelectedIndex = 0;
        }

        private void MainForm_FormClosed
            (
                object sender, 
                FormClosedEventArgs e
            )
        {
            _client.Dispose();
        }

        private void _databaseBox_SelectedIndexChanged
            (
                object sender, 
                EventArgs e
            )
        {
            _web.Navigate("about:blank");
            _allBox.Items.Clear();
            _foundBox.Items.Clear();
            _modeBox.Checked = false;

            string database = ((DatabaseInfo)_databaseBox.SelectedItem).Name;

            DatabaseInfo info = _client.GetDatabaseInfo(database);
            _deletedRecords = info.LogicallyDeletedRecords;
            _allBox.BeginUpdate();
            foreach (int mfn in _deletedRecords)
            {
                _allBox.Items.Add(mfn);
            }
            _allBox.EndUpdate();
            if (_formatBox.Items.Count != 0)
            {
                _formatBox.SelectedIndex = 0;
            }
            _SetStatus("Всего удалённых записей: {0}", _deletedRecords.Length);
            if (_allBox.Items.Count != 0)
            {
                _allBox.SelectedIndex = 0;
            }
        }

        private int CurrentMfn
        {
            get
            {
                ListBox box = _modeBox.Checked
                    ? _foundBox
                    : _allBox;

                int index = box.SelectedIndex;
                if (index < 0)
                {
                    return 0;
                }
                return (int) box.SelectedItem;
            }
        }

        private void _ShowCurrentRecord()
        {
            _web.Navigate("about:blank");
            int mfn = CurrentMfn;
            if (mfn <= 0)
            {
                return;
            }
            string formatted = _client.FormatRecord
                (
                    _formatBox.Text, 
                    mfn
                );
            _web.DocumentText = formatted;
        }

        private void _allBox_SelectedIndexChanged
            (
                object sender, 
                EventArgs e
            )
        {
            if (!_modeBox.Checked)
            {
                _ShowCurrentRecord();
            }
        }

        private void _SetStatus
            (
                string format,
                params object[] args
            )
        {
            _statusLabel.Text = string.Format(format, args);
        }

        private void _formatBox_SelectedIndexChanged
            (
                object sender, 
                EventArgs e
            )
        {
            _ShowCurrentRecord();
        }

        private void _modeBox_CheckedChanged
            (
                object sender, 
                EventArgs e
            )
        {
            _ShowCurrentRecord();
        }

        private void _searchButton_Click
            (
                object sender, 
                EventArgs e
            )
        {
            _web.Navigate("about:blank");
            _foundBox.Items.Clear();
            _foundRecords.Clear();
            _targetText = _valueBox.Text.Trim().ToUpperInvariant();
            _targetWords = _targetText.Split
                (
                    new []{' '}, 
                    StringSplitOptions.RemoveEmptyEntries
                );
            _progressBar.Value = 0;
            if (string.IsNullOrEmpty(_targetText))
            {
                return;
            }

            if (_deletedRecords.Length == 0)
            {
                return;
            }

            _progressBar.Maximum = _deletedRecords.Length;
            _backgroundWorker.RunWorkerAsync ();
        }

        private void _backgroundWorker_DoWork
            (
                object sender, 
                DoWorkEventArgs e
            )
        {
            try
            {
                BatchRecordReader reader = new BatchRecordReader
                    (
                        _client,
                        _client.Database,
                        500,
                        _deletedRecords
                    );
                int index = 0;
                foreach (MarcRecord record in reader)
                {
                    string text = record.ToPlainText().ToUpperInvariant();
                    bool flag = true;
                    foreach (string word in _targetWords)
                    {
                        if (!text.Contains(word))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        _foundRecords.Add(record.Mfn);
                    }

                    index++;
                    _backgroundWorker.ReportProgress(index);
                }
            }
            catch (Exception ex)
            {
                _SetStatus("Возникло исключение: {0}", ex);
            }
        }

        private void _backgroundWorker_ProgressChanged
            (
                object sender, 
                ProgressChangedEventArgs e
            )
        {
            _progressBar.Value = e.ProgressPercentage;
        }

        private void _backgroundWorker_RunWorkerCompleted
            (
                object sender, 
                RunWorkerCompletedEventArgs e
            )
        {
            _progressBar.Value = 0;

            _SetStatus("Найдено записей: {0}", _foundRecords.Count);

            _foundBox.BeginUpdate();
            foreach (int mfn in _foundRecords)
            {
                _foundBox.Items.Add(mfn);
            }            
            _foundBox.EndUpdate();

            if (_foundRecords.Count == 0)
            {
                _modeBox.Checked = false;
            }
            else
            {
                _modeBox.Checked = true;
                _foundBox.SelectedIndex = 0;
            }            
        }

        private void _foundBox_SelectedIndexChanged
            (
                object sender, 
                EventArgs e
            )
        {
            if (_modeBox.Checked)
            {
                _ShowCurrentRecord();
            }
        }

        private void _undeleteButton_Click
            (
                object sender, 
                EventArgs e
            )
        {
            int mfn = CurrentMfn;
            if (mfn != 0)
            {
                MarcRecord record = _client.ReadRecord(mfn);
                if (record.Deleted)
                {
                    _client.UndeleteRecord(record.Mfn);
                }
            }
        }
    }
}
