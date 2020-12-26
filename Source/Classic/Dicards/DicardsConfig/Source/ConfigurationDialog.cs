// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* ConfigurationDialog.cs -- диалог конфигурирования приложения.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Windows.Forms;

using AM;
using AM.IO;

using DevExpress.XtraEditors;
using DevExpress.XtraLayout;

using ManagedIrbis;

using RestfulIrbis.OsmiCards;

#endregion

namespace DicardsConfig
{
    /// <summary>
    /// Диалог конфигурирования приложения.
    /// </summary>
    public partial class ConfigurationDialog
        : XtraForm
    {
        #region Construction

        public ConfigurationDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void ClearLog()
        {
            _logBox.Clear();
        }

        private void WriteLog
            (
                string format,
                params object[] args
            )
        {
            _logBox.Output.WriteLine(format, args);
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
                LayoutControlItem item
            )
        {
            var textEdit = (TextEdit) item.Control;
            var label = item.Text;

            if (string.IsNullOrEmpty(textEdit.Text.Trim()))
            {
                XtraMessageBox.Show
                    (
                        $"Не заполнено поле '{label}'",
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
            return CheckField(_hostItem)
                   && CheckField(_portItem)
                   && CheckField(_loginItem)
                   && CheckField(_passwordItem)
                   && CheckField(_databaseItem)
                   && CheckField(_urlItem)
                   && CheckField(_idItem)
                   && CheckField(_keyItem)
                   && CheckField(_templateItem)
                   && CheckField(_readerIdItem)
                   && CheckField(_ticketItem);
        }

        private void FromConfig
            (
                DicardsConfiguration config
            )
        {
            var connection = new IrbisConnection();
            connection.ParseConnectionString(config.ConnectionString);
            if (string.IsNullOrEmpty(config.ConnectionString))
            {
                connection.Database = "RDR";
            }

            _hostBox.Text = connection.Host;
            _portBox.Text = connection.Port.ToInvariantString();
            _loginBox.Text = connection.Username;
            _passwordBox.Text = connection.Password;
            _databaseBox.Text = connection.Database;

            _categoryBox.Text = config.Category;
            _prefixBox.Text = config.Prefix;
            _formatBox.Text = config.Format;

            _idBox.Text = config.ApiId;
            _keyBox.Text = config.ApiKey;
            _urlBox.Text = config.BaseUri;
            _templateBox.Text = config.Template;
            _fieldBox.Text = config.Field;
            _readerIdBox.Text = config.ReaderId;
            _ticketBox.Text = config.Ticket;
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
            if (result.Database == "IBIS")
            {
                result.Database = "RDR";
            }
            if (result.Port == 0)
            {
                result.Port = 6666;
            }

            return result;
        }

        private DicardsConfiguration ToConfig()
        {
            var result = new DicardsConfiguration();
            var connection = ToConnection();
            result.ConnectionString
                = ConnectionSettings.FromConnection(connection).Encode();
            result.Category = _categoryBox.Text.Trim();
            result.Prefix = _prefixBox.Text.Trim();
            result.Format = _formatBox.Text.Trim();
            result.ApiId = _idBox.Text.Trim();
            result.ApiKey = _keyBox.Text.Trim();
            result.BaseUri = _urlBox.Text.Trim();
            result.Template = _templateBox.Text.Trim();
            result.Field = _fieldBox.Text.Trim();
            result.ReaderId = _readerIdBox.Text.Trim();
            result.Ticket = _ticketBox.Text.Trim();

            return result;
        }

        private string ThrowIfEmpty
            (
                string name,
                string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ApplicationException($"Поле '{name}' не заполнено");
            }

            return value;
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
                    var status = NetworkChecker.PingHost(connection.Host);
                    if (status != IPStatus.Success)
                    {
                        WriteLog("Ошибка связи с ИРБИС64: {0}", status);
                        return false;
                    }

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

        private bool CheckOsmi()
        {
            try
            {
                var hostname = new Uri(_urlBox.Text.Trim()).Host;
                var status = NetworkChecker.PingHost(hostname);
                if (status != IPStatus.Success)
                {
                    WriteLog("Ошибка связи с DiCards: {0}", status);
                    return false;
                }

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

        private bool CheckRdrSettings(DicardsConfiguration config)
        {
            var styles = NumberStyles.Any;
            var culture = CultureInfo.InvariantCulture;
            if (!int.TryParse(config.ReaderId, styles, culture, out int readerId))
            {
                WriteLog("Неверно задано поле для идентификатора читателя");
                return false;
            }

            if (!int.TryParse(config.ReaderId, styles, culture, out int ticket))
            {
                WriteLog("Неверно задано поле для номера пропуска");
            }

            return true;
        }

        private string DicardsJson()
        {
            var result = PathUtility.MapPath("dicards.json");

            return result;
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
                var fileName = DicardsJson();
                var config
                    = DicardsConfiguration.LoadConfiguration (fileName);

                FromConfig(config);
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

        private bool _DoChecks()
        {
            var config = ToConfig();
            config.Verify(true);
            bool result = CheckIrbis()
                          && CheckRdrSettings(config)
                          && CheckOsmi();

            WriteLog
                (
                    "\r\n\r\n{0}",
                    result
                        ? "Конфигурация работоспособна"
                        : "Ошибка в конфигурации!"
                );

            return result;
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
                _DoChecks();
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
                if (!_DoChecks())
                {
                    WriteLog("\r\n\r\nКонфигурация не проходит проверку, её нельзя сохранять");
                    return;
                }

                var config = ToConfig();
                config.SaveConfiguration(DicardsJson());
                WriteLog("\r\n\r\nКонфигурация успешно сохранена");
                Close();
            }
            catch (Exception exception)
            {
                WriteError(exception);
            }
        }

        private void _exportButton_Click(object sender, EventArgs e)
        {
            if (_saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                var fileName = _saveDialog.FileName;
                var config = ToConfig();
                config.SaveConfiguration(fileName);
                WriteLog("\r\n\r\nКонфигурация успешно сохранена в файл {0}", fileName);
            }
        }

        private void _importButton_Click(object sender, EventArgs e)
        {
            if (_openDialog.ShowDialog(this) == DialogResult.OK)
            {
                var fileName = _openDialog.FileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    var config = DicardsConfiguration.LoadConfiguration(fileName);
                    FromConfig(config);
                    WriteLog("\r\nКонфигурация была загружена из файла {0}", fileName);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Показывает диалог конфигурирования.
        /// </summary>
        public static bool Configure
            (
                IWin32Window owner
            )
        {
            using (var dialog = new ConfigurationDialog())
            {
                if (ReferenceEquals(owner, null))
                {
                    dialog.TopMost = true;
                    dialog.ShowInTaskbar = true;
                }

                if (dialog.ShowDialog(owner) == DialogResult.OK)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}