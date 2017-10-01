// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* .cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM.Configuration;

using LitresTicket.Properties;

using ManagedIrbis;

#endregion

namespace LitresTicket
{
    public partial class MainForm
        : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private int _litresTag;

        public IrbisConnection GetConnection()
        {
            IrbisConnection result
                = IrbisConnectionUtility.GetClientFromConfig();

            return result;
        }

        private void _startupTimer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            Timer timer = (Timer) sender;
            timer.Enabled = false;

            _litresTag = ConfigurationUtility.GetInt32("litresTag", 101);

            using (IrbisConnection connection = GetConnection())
            {
                connection.NoOp();
            }
        }

        private void _searchButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _resultBox.Clear();
            _loginBox.Clear();
            _passwordBox.Clear();

            string ticket = _ticketBox.Text.Trim();
            if (string.IsNullOrEmpty(ticket))
            {
                _resultBox.Text = Resources.MainForm_NoIogunbTicket;

                return;
            }

            using (IrbisConnection connection = GetConnection())
            {
                string expression = string.Format
                    (
                        "\"RI={0}\"",
                        ticket
                    );
                int[] found = connection.Search(expression);
                if (found.Length == 0)
                {
                    _resultBox.Text = Resources.MainForm_NoReaderFound;
                    _ticketBox.Focus();

                    return;
                }
                if (found.Length != 1)
                {
                    _resultBox.Text = Resources.MainForm_ManyReadersFound;
                    _ticketBox.Focus();

                    return;
                }

                MarcRecord record = connection.ReadRecord(found[0]);
                string description = connection.FormatRecord("@brief", found[0]);
                _resultBox.Text = description;

                RecordField litresField = record.Fields
                    .GetFirstField(_litresTag);
                if (!ReferenceEquals(litresField, null))
                {
                    _loginBox.Text = litresField.GetFirstSubFieldValue('a');
                    _passwordBox.Text = litresField.GetFirstSubFieldValue('b');
                }

                _loginBox.Focus();
            }
        }

        private void _ticketBox_KeyDown
            (
                object sender,
                KeyEventArgs e
            )
        {
            if (e.KeyData == Keys.Enter)
            {
                _searchButton.PerformClick();
                e.Handled = true;
            }
        }

        private void _bindButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _resultBox.Clear();

            string ticket = _ticketBox.Text.Trim();
            if (string.IsNullOrEmpty(ticket))
            {
                _resultBox.Text = Resources.MainForm_NoIogunbTicket;
                _ticketBox.Focus();

                return;
            }
            string login = _loginBox.Text.Trim();
            string pasword = _passwordBox.Text.Trim();
            if (string.IsNullOrEmpty(login)
                != string.IsNullOrEmpty(pasword))
            {
                _resultBox.Text = Resources.MainForm_BothLoginAndPasswordRequired;
                _loginBox.Focus();

                return;
            }

            using (IrbisConnection connection = GetConnection())
            {
                string expression = string.Format
                (
                    "\"RI={0}\"",
                    ticket
                );
                int[] found = connection.Search(expression);
                if (found.Length == 0)
                {
                    _resultBox.Text = Resources.MainForm_NoReaderFound;
                    _ticketBox.Focus();

                    return;
                }
                if (found.Length != 1)
                {
                    _resultBox.Text = Resources.MainForm_ManyReadersFound;
                    _ticketBox.Focus();

                    return;
                }

                MarcRecord record = connection.ReadRecord(found[0]);
                record.Modified = false;
                RecordField litresField = record.Fields
                    .GetFirstField(_litresTag);
                if (ReferenceEquals(litresField, null))
                {
                    if (!string.IsNullOrEmpty(login))
                    {
                        litresField = new RecordField(_litresTag)
                            .AddSubField('a', login)
                            .AddSubField('b', pasword);
                        record.Fields.Add(litresField);
                    }
                    else
                    {
                        MessageBox.Show
                            (
                                Resources.MainForm_NothingToDo,
                                Resources.MainForm_QuestionTitle,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                    }
                }
                else
                {
                    if (_AskForRewrite())
                    {
                        if (string.IsNullOrEmpty(login))
                        {
                            litresField.RemoveSubField('a');
                        }
                        else
                        {
                            litresField.SetSubField('a', login);
                        }
                        if (string.IsNullOrEmpty(pasword))
                        {
                            litresField.RemoveSubField('b');
                        }
                        else
                        {
                            litresField.SetSubField('b', pasword);
                        }
                    }
                }

                if (record.Modified)
                {
                    connection.WriteRecord(record);
                    _resultBox.Text = Resources.MainForm_DataWasCommited;
                }

                _ticketBox.Clear();
                _loginBox.Clear();
                _passwordBox.Clear();
                _ticketBox.Focus();
            }
        }

        private bool _AskForRewrite()
        {
            DialogResult result = MessageBox.Show
                (
                    Resources.MainForm_AlreadyBinded,
                    Resources.MainForm_QuestionTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1
                );

            return result == DialogResult.Yes;
        }
    }
}
