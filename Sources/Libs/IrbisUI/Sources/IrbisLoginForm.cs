/* IrbisLoginForm.cs -- логин и пароль для входа в ИРБИС
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Окно с вводом логина и пароля для входа в ИРБИС.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class IrbisLoginForm 
        : Form
    {
        #region Properties

        /// <summary>
        /// Логин.
        /// </summary>
        public string UserName
        {
            get { return _nameBox.Text; }
            set { _nameBox.Text = value; }
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string UserPassword
        {
            get { return _passwordBox.Text; }
            set { _passwordBox.Text = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public IrbisLoginForm()
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
            if (string.IsNullOrEmpty(_nameBox.Text))
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
    }
}
