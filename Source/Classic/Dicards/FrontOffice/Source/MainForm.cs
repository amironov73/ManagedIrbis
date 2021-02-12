// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable CheckNamespace
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- главная форма приложения.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM;
using AM.Configuration;
using AM.Logging;
using AM.Net;
using AM.Windows.Forms;

using DevExpress.XtraEditors;

using DicardsConfig;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using Newtonsoft.Json.Linq;

using RestfulIrbis.OsmiCards;

#endregion

namespace FrontOffice
{
    /// <summary>
    /// Главная форма приложения.
    /// </summary>
    public partial class MainForm
        : XtraForm
    {
        #region Construction

        public MainForm()
        {
            InitializeComponent();

            Log.Debug("MainForm initialized");
        }

        #endregion

        #region Private members

        private void _Clear()
        {
            _browser.Navigate("about:blank");
        }

        private void MainForm_FormClosing
            (
                object sender,
                FormClosingEventArgs e
            )
        {
            Log.Debug("MainForm_FormClosing");
            ControlCenter.WriteLine("Shutdown");
        }

        private bool CheckConfiguration()
        {
            while (true)
            {
                try
                {
                    Log.Debug("MainForm::CheckConfiguration: about to ControlCenter.Initialize");
                    ControlCenter.Initialize(_logBox.Output);
                    Log.Debug("MainForm::CheckConfiguration: ControlCenter.Initialize done");
                    break;
                }
                catch (Exception exception)
                {
                    Log.TraceException("MainForm::CheckConfiguration", exception);
                    _logBox.Output.WriteLine("ERROR: {0}", exception.Message);
                }

                if (ControlCenter.CheckConfiguration())
                {
                    break;
                }

                var rc = XtraMessageBox.Show
                    (
                        "Программа неправильно сконфигурирована! Будете конфигурировать?",
                        "Регистрация карт",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error
                    );
                if (rc == DialogResult.Yes)
                {
                    Log.Debug("MainForm::CheckConfiguration: ConfigurationDialog called");
                    ConfigurationDialog.Configure(this);
                    Log.Debug("MainFOrm::CheckConfiguration: ConfigurationDialog done");
                }
                else
                {
                    Log.Debug("MainForm::CheckConfiguration = False");
                    return false;
                }
            }

            Log.Debug("MainForm::CheckConfiguration = True");

            return true;
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            _browser.Navigate("about:blank");
            while (_browser.IsBusy)
            {
                Application.DoEvents();
            }
            Application.DoEvents();

            this.ShowVersionInfoInTitle();
            _logBox.Output.PrintSystemInformation();

            if (ControlCenter.Output is null)
            {
                ControlCenter.Output = _logBox.Output;
            }

            if (!CheckConfiguration())
            {
                ControlCenter.WriteLine("\r\nПРОВЕРЬТЕ КОНФИГУРАЦИЮ!");
                // Environment.Exit(1);
            }

            try
            {
                ControlCenter.Ping();
            }
            catch (Exception exception)
            {
                Log.TraceException("MainForm::Load", exception);

                ControlCenter.WriteLine
                    (
                        "\r\nОШИБКА: {0}: {1}",
                        exception.GetType().Name,
                        exception.Message
                    );
                ControlCenter.WriteLine(string.Empty);
                ControlCenter.WriteLine("\r\nПРОВЕРЬТЕ КОНФИГУРАЦИЮ!");
                ControlCenter.WriteLine(string.Empty);
                return;
            }

            ControlCenter.WriteLine("Ready");
            ControlCenter.WriteLine(string.Empty);
        }

        [CanBeNull]
        private ReaderInfo _PrepareReader()
        {
            _Clear();

            var ticket = _ticketBox.Text.Trim();
            if (string.IsNullOrEmpty(ticket))
            {
                XtraMessageBox.Show("Не задан читательский!");
                return null;
            }

            var reader = ControlCenter.GetReader(ticket);
            if (ReferenceEquals(reader, null))
            {
                Log.Debug("_PrepareReader not found ticket=" + ticket);
                XtraMessageBox.Show("Не найден читатель с указанным билетом!");
                return null;
            }

            var description = ControlCenter.FormatReader(reader);
            var chk = ControlCenter.CheckReader(reader);
            var exist = chk && ControlCenter.CardExists(ticket);

            _browser.DocumentText = "<html>"
                + (exist ?
                    "<p><b><font color='red'>Карта уже существует!</font></b></p>"
                    : string.Empty
                  )
                + description
                + "</html>";

            Log.Debug("_PrepareReader result=" + chk);

            return !chk
                ? null
                : reader;
        }

        private void _searchButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                _PrepareReader();
            }
            catch (Exception exception)
            {
                Log.TraceException("_searchButton_Click", exception);
                ExceptionBox.Show(this, exception);
            }
        }

        private void _createButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                var reader = _PrepareReader();
                if (ReferenceEquals(reader, null))
                {
                    return;
                }

                var ticket = _ticketBox.Text.Trim();
                if (string.IsNullOrEmpty(ticket))
                {
                    XtraMessageBox.Show("Не задан читательский!");
                    return;
                }

                if (ControlCenter.CardExists(ticket))
                {
                    Log.Debug("_createButton_Click: card already exists=" + ticket);
                    XtraMessageBox.Show("Карта уже существует");
                    return;
                }

                var card = ControlCenter.BuildCard(reader, ticket);
                ControlCenter.CreateCard
                    (
                        ticket,
                        card
                    );

                using (var connection = ControlCenter.GetIrbisConnection())
                {
                    CardUpdater.ProcessReader
                        (
                            reader,
                            ControlCenter.Config,
                            connection,
                            ControlCenter.Client
                        );
                    ControlCenter.WriteLine("Данные на карте обнобвлены");
                }
            }
            catch (Exception exception)
            {
                Log.TraceException("_createButton_Click", exception);
                ExceptionBox.Show(this, exception);
            }
        }

        private void _sendEmailButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                ReaderInfo reader = _PrepareReader();
                if (ReferenceEquals(reader, null))
                {
                    return;
                }

                string email = _emailBox.Text.Trim();
                if (!string.IsNullOrEmpty(email))
                {
                    if (!MailUtility.VerifyEmail(email))
                    {
                        Log.Debug("_sendEmailButton_Click bad email=" + email);
                        XtraMessageBox.Show("Неверный email");
                        return;
                    }

                    MarcRecord record = reader.Record
                        .ThrowIfNull("reader.Record");
                    string[] emails = record.FMA(32);
                    if (!emails.ContainsNoCase(email))
                    {
                        record.AddField(32, email);
                        ControlCenter.UpdateRecord(record);
                        ControlCenter.WriteLine
                            (
                                "Добавлен email: {0}",
                                email
                            );
                        Log.Debug("_sendEmailButton_Click: add email=" + email);
                    }
                }

                string ticket = _ticketBox.Text.Trim();
                if (!ControlCenter.CardExists(ticket))
                {
                    Log.Debug("_sendEmailButton_Click: card doesn't exist =" + ticket);
                    XtraMessageBox.Show("Карты не существует");
                    return;
                }

                ControlCenter.SendEmail(reader);
            }
            catch (Exception exception)
            {
                Log.TraceException("_sendEmailButton_Click", exception);
                ExceptionBox.Show(this, exception);
            }
        }

        /*

        private void _sendSmsButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                ReaderInfo reader = _PrepareReader();
                if (ReferenceEquals(reader, null))
                {
                    return;
                }

                string ticket = (reader.PassCard ?? reader.Ticket)
                    .ThrowIfNull("ticket");
                if (!ControlCenter.CardExists(ticket))
                {
                    MessageBox.Show("Карты не существует");
                    return;
                }

                string phoneNumber = reader.HomePhone;
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    MessageBox.Show("Не задан телефон!");
                    return;
                }

                ControlCenter.SendSms(reader);
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(this, exception);
            }
        }

        private void _deleteButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                ReaderInfo reader = _PrepareReader();
                if (ReferenceEquals(reader, null))
                {
                    return;
                }

                string ticket = (reader.PassCard ?? reader.Ticket)
                    .ThrowIfNull("ticket");
                if (!ControlCenter.CardExists(ticket))
                {
                    MessageBox.Show("Карты не существует");
                    return;
                }

                ControlCenter.DeleteCard(reader);
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(this, exception);
            }
        }

        private void _clearButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _ticketBox.EditValue = string.Empty;
            _Clear();
        }

        */

        private void _configButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            var password1 = ConfigurationUtility.GetString
                (
                    "password",
                    string.Empty
                );

            password1 = DicardsConfiguration.Unprotect(password1);

            if (!string.IsNullOrEmpty(password1))
            {
                var password2 = string.Empty;
                InputBox.PasswordChar = '*';
                var rc = InputBox.Query
                    (
                        "Конфигурация системы",
                        "Введите пароль",
                        "Для изменения конфигурации необходимо ввести пароль",
                        ref password2
                    );
                if (rc != DialogResult.OK)
                {
                    return;
                }

                if (password1 != password2)
                {
                    XtraMessageBox.Show
                        (
                            "Введен неверный пароль!",
                            "Конфигурация системы",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    return;
                }

                Log.Debug("ConfigurationDialog called");
                ConfigurationDialog.Configure(this);
                Log.Debug("ConfigurationDialog done");
            }
        }

        private void _updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                var reader = _PrepareReader();
                if (ReferenceEquals(reader, null))
                {
                    return;
                }

                var ticket = _ticketBox.Text.Trim();
                if (string.IsNullOrEmpty(ticket))
                {
                    XtraMessageBox.Show("Не задан читательский!");
                    return;
                }

                if (!ControlCenter.CardExists(ticket))
                {
                    Log.Debug("_updateButton_Click: card not exists=" + ticket);
                    XtraMessageBox.Show("Карты не существует");
                    return;
                }

                using (var connection = ControlCenter.GetIrbisConnection())
                {
                    CardUpdater.ProcessReader
                        (
                            reader,
                            ControlCenter.Config,
                            connection,
                            ControlCenter.Client
                        );
                    ControlCenter.WriteLine("Данные на карте обнобвлены");
                }
            }
            catch (Exception exception)
            {
                Log.TraceException("_updateButton_Click", exception);
                ExceptionBox.Show(this, exception);
            }
        }

        #endregion
    }
}
