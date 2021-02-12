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
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;

using AM;
using AM.Win32;

using DevExpress.XtraEditors;

using JetBrains.Annotations;

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

        private DicardsConfiguration GetConfig()
            => ((DicardsConfiguration) _propertyGrid.SelectedObject)
                .ThrowIfNull("DicardsConfiguration");

        private void ClearLog()
        {
            _logBox.Clear();
        }

        [StringFormatMethod("format")]
        private void WriteLog
            (
                [NotNull] string format,
                params object[] args
            )
        {
            _logBox.Output.WriteLine(format, args);
        }

        private void WriteError
            (
                [NotNull] Exception exception
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
                [NotNull] Expression<Func<DicardsConfiguration, string>> lambda
            )
        {
            var property = GetProperty(lambda);
            return CheckField(property);
        }

        [NotNull]
        private string GetName
            (
                [NotNull] PropertyInfo property
            )
        {
            var attribute = property.GetCustomAttribute<DisplayNameAttribute>()
                .ThrowIfNull("No attribute for " + property.Name);
            var label = attribute.DisplayName;

            return label;
        }

        [NotNull]
        private string GetName
            (
                [NotNull] Expression<Func<DicardsConfiguration, string>> lambda
            )
        {
            var property = GetProperty(lambda);
            return GetName(property);
        }

        private bool CheckField
            (
                [NotNull] PropertyInfo property
            )
        {
            var config = GetConfig();
            var value = ((string) property.GetValue(config))
                ?.Trim();
            var label = GetName(property);

            if (string.IsNullOrEmpty(value))
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

        [NotNull]
        private PropertyInfo GetProperty<TSource, TProperty>
            (
                [NotNull] Expression<Func<TSource, TProperty>> lambda
            )
        {
            var member = (MemberExpression)lambda.Body;
            var info = (PropertyInfo)member.Member;

            return info;
        }

        /// <summary>
        /// Проверяем заполнение полей.
        /// </summary>
        /// <remarks>Умеет проверять только текстовые поля.</remarks>
        private bool CheckFields()
        {
            return CheckField(c => c.Host)
                   && CheckField(c => c.Login)
                   && CheckField(c => c.Password)
                   && CheckField(c => c.Database)
                   && CheckField(c => c.BaseUri)
                   && CheckField(c => c.ApiId)
                   && CheckField(c => c.ApiKey)
                   && CheckField(c => c.Ticket)
                   && CheckField(c => c.Template)
                   && CheckField(c => c.FioField)
                   && CheckField(c => c.ReaderId)
                   && CheckField(c => c.ReminderField)
                   && CheckField(c => c.ReminderMessage)
                   && CheckField(c => c.TotalCountField)
                   && CheckField(c => c.TotalCountFormat)
                   && CheckField(c => c.ExpirecCountField)
                   && CheckField(c => c.ExpiredCountFormat)
                   && CheckField(c => c.TotalListField)
                   && CheckField(c => c.TotalListFormat)
                   && CheckField(c => c.ExpiredListField)
                   && CheckField(c => c.ExpiredListFormat);
        }

        /// <summary>
        /// Разбор конфигурации (в основном, возня вокруг
        /// строки подключения к ИРБИС64).
        /// </summary>
        private void FromConfig
            (
                [NotNull] DicardsConfiguration config
            )
        {
            var connection = new IrbisConnection();
            var connectionString = config.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                connection.Database = "RDR";
            }
            else
            {
                connection.ParseConnectionString(connectionString);
            }

            config.Host = connection.Host;
            config.Port = connection.Port;
            config.Login = connection.Username;
            config.Password = connection.Password;
            config.Database = connection.Database;

            _propertyGrid.SelectedObject = config;
        }

        /// <summary>
        /// Получаем (неактивное) подключение к серверу ИРБИС64.
        /// </summary>
        [NotNull]
        private IrbisConnection ToConnection()
        {
            var config = GetConfig();
            var result = new IrbisConnection
            {
                Host = config.Host.ThrowIfNull("config.Host"),
                Port = config.Port,
                Username = config.Login.ThrowIfNull("config.Login"),
                Password = config.Password.ThrowIfNull("config.Password"),
                Database = config.Database.ThrowIfNull("config.Database")
            };

            if (result.Database.SameString("IBIS"))
            {
                result.Database = "RDR";
            }

            if (result.Port <= 0)
            {
                result.Port = 6666;
            }

            return result;
        }

        [NotNull]
        private DicardsConfiguration ToConfig()
        {
            var result = GetConfig();
            var connection = ToConnection();
            result.ConnectionString
                = ConnectionSettings.FromConnection(connection).Encode();

            return result;
        }

        /// <summary>
        /// Если строка пустая, бросаемся исключением.
        /// </summary>
        /// <param name="name">Имя, которое будет отображено на экране.</param>
        /// <param name="value">Значение строки.</param>
        /// <returns>Переданное в функцию значение строки.</returns>
        [NotNull]
        private string ThrowIfEmpty
            (
                [NotNull] string name,
                [CanBeNull] string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ApplicationException($"Поле '{name}' не заполнено");
            }

            return value;
        }

        /// <summary>
        /// Создаем клиента DICARDS API.
        /// </summary>
        [NotNull]
        private OsmiCardsClient ToClient()
        {
            var config = GetConfig();
            var baseUri = ThrowIfEmpty("URL", config.BaseUri?.Trim());
            var apiId = ThrowIfEmpty("ID", config.ApiId?.Trim());
            var apiKey = ThrowIfEmpty("Key", config.ApiKey?.Trim());

            var result = new OsmiCardsClient
                (
                    baseUri,
                    apiId,
                    apiKey
                );

            return result;
        }

        /// <summary>
        /// Проверяем связь с сервером ИРБИС64.
        /// </summary>
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
                    WriteLog("ИРБИС64: {0}\r\n", serverVersion);
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

        /// <summary>
        /// Проверяем связь с DICARDS API.
        /// </summary>
        private bool CheckOsmi()
        {
            try
            {
                var config = GetConfig();
                var baseUri = config.BaseUri?.Trim();
                baseUri = ThrowIfEmpty
                    (
                        GetName(c => c.BaseUri),
                        baseUri
                    );
                var hostname = new Uri(baseUri).Host;
                var status = NetworkChecker.PingHost(hostname);
                if (status != IPStatus.Success)
                {
                    WriteLog("Ошибка связи с DiCards: {0}", status);
                    return false;
                }

                var client = ToClient();
                var templateName = config.Template?.Trim();
                templateName = ThrowIfEmpty
                    (
                        GetName(c=> c.Template),
                        templateName
                    );
                WriteLog($"Подключаемся к API. Считываем шаблон {templateName}\r\n");
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

        /// <summary>
        /// Проверяем настойки базы данных RDR.
        /// </summary>
        private bool CheckRdrSettings
            (
                [NotNull] DicardsConfiguration config
            )
        {
            var styles = NumberStyles.Any;
            var culture = CultureInfo.InvariantCulture;
            if (!int.TryParse(config.ReaderId, styles, culture, out int _))
            {
                WriteLog("Неверно задано поле для идентификатора читателя");
                return false;
            }

            if (!int.TryParse(config.ReaderId, styles, culture, out int _))
            {
                WriteLog("Неверно задано поле для номера пропуска");
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
                var fileName = OsmiUtility.DicardsJson();
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
                config.SaveConfiguration(OsmiUtility.DicardsJson());
                WriteLog("\r\n\r\nКонфигурация успешно сохранена");
                Close();
            }
            catch (Exception exception)
            {
                WriteError(exception);
            }
        }

        private void _exportButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                var fileName = _saveDialog.FileName;
                var config = ToConfig();
                config.SaveConfiguration(fileName);
                WriteLog("\r\n\r\nКонфигурация успешно сохранена в файл {0}", fileName);
            }
        }

        private void _importButton_Click
            (
                object sender,
                EventArgs e
            )
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
                [CanBeNull] IWin32Window owner
            )
        {
            using (var dialog = new ConfigurationDialog())
            {
                if (ReferenceEquals(owner, null))
                {
                    // если нет окна-владельца, значит, мы запущены из-под конфигуратора
                    // в этом случае окно прячется под инсталлятором
                    // чтобы показать его, делаем TopMost
                    // решение идиотское, но срабатывает в отличие от BringWindowToTop,
                    // ведь окно ещё не отображено на экране
                    if (dialog.IsHandleCreated)
                    {
                        User32.BringWindowToTop(dialog.Handle);
                    }
                    else
                    {
                        dialog.TopMost = true;
                    }
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
