// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LoginBox.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Windows;

#endregion

namespace AM.Windows.Irbis
{
    /// <summary>
    /// Interaction logic for LoginBox.xaml
    /// </summary>
    public partial class LoginBox
    {
        #region Properties

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username
        {
            get { return loginBox.Text; }
            set { loginBox.Text = value; }
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password
        {
            get { return passwordBox.Password; }
            set { passwordBox.Password = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LoginBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Private methods

        private void LoginButton_OnClick
            (
                object sender,
                RoutedEventArgs e
            )
        {
            if (string.IsNullOrEmpty(Password))
            {
                passwordBox.Focus();
            }
            else
            {
                DialogResult = true;
            }
        }

        private void CancelButton_OnClick
            (
                object sender,
                RoutedEventArgs e
            )
        {
            DialogResult = false;

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Установка фокуса ввода.
        /// </summary>
        public void SetupFocus()
        {
            if (string.IsNullOrEmpty(Username))
            {
                loginBox.Focus();
            }
            else
            {
                passwordBox.Focus();
            }
        }

        #endregion
    }
}
