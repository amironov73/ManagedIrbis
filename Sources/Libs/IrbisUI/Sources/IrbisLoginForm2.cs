/* IrbisLoginForm2.cs -- логин и пароль для входа в ИРБИС
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Окно с вводом адреса сервера, логина 
    /// и пароля для входа в ИРБИС.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class IrbisLoginForm2 
        : Form
    {
        #region Properties

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        [CanBeNull]
        public string Host
        {
            get { return _serverBox.Text; }
            set { _serverBox.Text = value; }
        }

        /// <summary>
        /// Порт для подключения.
        /// </summary>
        [CanBeNull]
        public string Port
        {
            get { return _portBox.Text; }
            set { _portBox.Text = value; }
        }

        /// <summary>
        /// Логин.
        /// </summary>
        [CanBeNull]
        public string Username
        {
            get { return _nameBox.Text; }
            set { _nameBox.Text = value; }
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        [CanBeNull]
        public string Password
        {
            get { return _passwordBox.Text; }
            set { _passwordBox.Text = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisLoginForm2()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void _okButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (string.IsNullOrEmpty(_serverBox.Text))
            {
                _serverBox.Focus();
                DialogResult = DialogResult.None;
            }
            else if (string.IsNullOrEmpty(_portBox.Text))
            {
                _portBox.Focus();
                DialogResult = DialogResult.None;
            }
            else if (string.IsNullOrEmpty(_nameBox.Text))
            {
                _nameBox.Focus();
                DialogResult = DialogResult.None;
            }
            else if (string.IsNullOrEmpty(_passwordBox.Text))
            {
                _passwordBox.Focus();
                DialogResult = DialogResult.None;
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="ConnectionSettings"/>.
        /// </summary>
        [NotNull]
        public IrbisLoginForm2 ApplySettings
            (
                [NotNull] ConnectionSettings settings
            )
        {
            Code.NotNull(settings, "settings");

            Host = settings.Host;
            Port = settings.Port.ToInvariantString();
            Username = settings.Username;
            Password = settings.Password;

            return this;
        }

        /// <summary>
        /// Convert to <see cref="ConnectionSettings"/>.
        /// </summary>
        [NotNull]
        public ConnectionSettings GatherSettings()
        {
            ConnectionSettings result = new ConnectionSettings
            {
                Host = Host,
                Port = int.Parse(Port.ThrowIfNull("Port")),
                Username = Username,
                Password = Password
            };

            return result;
        }

        #endregion

    }
}
