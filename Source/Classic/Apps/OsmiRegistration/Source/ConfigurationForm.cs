// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConfigurationForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Configuration;
using System.Windows.Forms;

using AM;

using ManagedIrbis;

using RestfulIrbis.OsmiCards;

using CM=System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

namespace OsmiRegistration
{
    /// <summary>
    /// Форма конфигурирования приложения.
    /// </summary>
    public partial class ConfigurationForm
        : Form
    {
        #region Properties

        private Configuration Config { get; set; }

        #endregion

        #region Construction

        public ConfigurationForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private methods

        private void ClearLog()
        {
            logBox1.Clear();
        }

        private void WriteLog
            (
                string format,
                params object[] args
            )
        {
            logBox1.Output.WriteLine(format, args);
        }

        private void WriteError
            (
                Exception exception
            )
        {
            WriteLog
                (
                    "\r\n\r\nОШИБКА: {0}: {1}",
                    exception.GetType().Name,
                    exception.Message
                );
        }

        private bool CheckField
            (
                Label label,
                TextBox textBox
            )
        {
            if (string.IsNullOrEmpty(textBox.Text.Trim()))
            {
                MessageBox.Show
                    (
                        string.Format
                            (
                                "Не заполнено поле '{0}'",
                                label.Text
                            ),
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                return false;
            }

            return true;
        }

        private bool CheckFields()
        {
            return CheckField(_hostLabel, _hostBox)
                && CheckField(_portLabel, _portBox)
                && CheckField(_loginLabel, _loginBox)
                && CheckField(_passwordLabel, _passwordBox)
                && CheckField(_databaseLabel, _databaseBox)
                && CheckField(_urlLabel, _urlBox)
                && CheckField(_idLabel, _idBox)
                && CheckField(_keyLabel, _keyBox)
                && CheckField(_templateLabel, _templateBox);
        }

        private void FromConfig()
        {
            var settings = Config.AppSettings.Settings;
            var connectionString = settings["connectionString"].Value;
            var connection = new IrbisConnection();
            connection.ParseConnectionString(connectionString);
            _hostBox.Text = connection.Host;
            _portBox.Text = connection.Port.ToInvariantString();
            _loginBox.Text = connection.Username;
            _passwordBox.Text = connection.Password;
            _databaseBox.Text = connection.Database;

            _idBox.Text = settings["apiID"].Value;
            _keyBox.Text = settings["apiKey"].Value;
            _urlBox.Text = settings["baseUri"].Value;
            _templateBox.Text = settings["template"].Value;
        }

        private IrbisConnection ToConnection()
        {
            var result = new IrbisConnection
            {
                Host = _hostBox.Text.Trim(),
                Port = _portBox.Text.Trim().SafeToInt32(),
                Username = _loginBox.Text.Trim(),
                Password = _passwordBox.Text.Trim(),
                Database = _databaseBox.Text.Trim()
            };
            if (result.Port == 0)
            {
                result.Port = 6666;
            }

            return result;
        }

        private void ToConfig()
        {
            var connection = ToConnection();
            var settings = Config.AppSettings.Settings;
            settings["connectionString"].Value
                = ConnectionSettings.FromConnection(connection).Encode();

            settings["apiID"].Value = _idBox.Text;
            settings["apiKey"].Value = _keyBox.Text;
            settings["baseUri"].Value = _urlBox.Text;
            settings["template"].Value = _templateBox.Text;
        }


        private OsmiCardsClient ToClient()
        {
            var baseUri = ThrowIfEmpty("URL", _urlBox.Text.Trim());
            var apiId = ThrowIfEmpty("ID", _idBox.Text.Trim());
            var apiKey = ThrowIfEmpty("Key", _keyBox.Text.Trim());

            var result = new OsmiCardsClient
                (
                    baseUri,
                    apiId,
                    apiKey
                );

            return result;
        }

        private bool CheckIrbis()
        {
            try
            {
                using (var connection = ToConnection())
                {
                    connection.Connect();
                    var serverVersion = connection.GetServerVersion();
                    WriteLog("ИРБИС64: {0}\r\n", serverVersion.ToString());
                    int maxMfn = connection.GetMaxMfn(connection.Database);
                    WriteLog("Max MFN={0}\r\n", maxMfn);
                }
            }
            catch (Exception exception)
            {
                WriteError(exception);
                return false;
            }

            return true;
        }

        private string ThrowIfEmpty
            (
                string name,
                string value
            )
        {
            return value;
        }

        private bool CheckOsmi()
        {
            try
            {
                var client = ToClient();
                var templateName = ThrowIfEmpty("Шаблон", _templateBox.Text.Trim());

                WriteLog("Подключаемся к API. Считываем шаблон\r\n");
                var template = client.GetTemplateInfo(templateName).ToString();
                ThrowIfEmpty("Содержимое шаблона", template);
            }
            catch (Exception exception)
            {
                WriteError(exception);
                return false;
            }

            return true;
        }

        private void ConfigurationForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            ClearLog();
            WriteLog("Необходимо заполнить все поля формы\r\n");

            try
            {
                Config = CM.OpenExeConfiguration
                    (
                        ConfigurationUserLevel.None
                    );
                FromConfig();
            }
            catch (Exception exception)
            {
                WriteError(exception);
            }
        }

        private void _closeButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Close();
        }

        private void _checkButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (!CheckFields())
            {
                return;
            }

            try
            {
                ClearLog();
                ToConfig();
                bool result = CheckIrbis()
                    && CheckOsmi();

                WriteLog
                    (
                        "\r\n\r\n{0}",
                        result
                            ? "Конфигурация работоспособна"
                            : "Ошибка в конфигурации!"
                    );
            }
            catch (Exception exception)
            {
                WriteError(exception);
            }
        }

        private void _writeButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (!CheckFields())
            {
                return;
            }

            try
            {
                ClearLog();
                ToConfig();
                Config.Save();
                CM.RefreshSection("appSettings");
                WriteLog("\r\n\r\nКонфигурация успешно сохранена");
                Close();
            }
            catch (Exception exception)
            {
                WriteError(exception);
            }
        }

        #endregion
    }
}
