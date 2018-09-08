// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LoginBox.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

#endregion

namespace IrbisUI
{
    /// <summary>
    ///
    /// </summary>
    public sealed partial class LoginBox
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

        public LoginBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members


        private void LoginBox_OnPrimaryButtonClick
            (
                ContentDialog sender,
                ContentDialogButtonClickEventArgs args
            )
        {
            if (string.IsNullOrEmpty(Password))
            {
                passwordBox.Focus(FocusState.Programmatic);
            }
            else
            {
                //DialogResult = true;
            }
        }

        private void LoginBox_OnSecondaryButtonClick
            (
                ContentDialog sender,
                ContentDialogButtonClickEventArgs args
            )
        {
            // DialogResult = false
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
                loginBox.Focus(FocusState.Programmatic);
            }
            else
            {
                passwordBox.Focus(FocusState.Programmatic);
            }
        }

        #endregion
    }
}
