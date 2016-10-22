/* IrbisCredentialsForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class IrbisCredentialsForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Server.
        /// </summary>
        public string Server
        {
            get { return _serverBox.Text; }
            set { _serverBox.Text = value; }
        }

        /// <summary>
        /// Port.
        /// </summary>
        public string Port
        {
            get { return _portBox.Text; }
            set { _portBox.Text = value; }
        }

        /// <summary>
        /// User.
        /// </summary>
        public string User
        {
            get { return _loginBox.Text; }
            set { _loginBox.Text = value; }
        }

        /// <summary>
        /// Password.
        /// </summary>
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
        public IrbisCredentialsForm()
        {
            InitializeComponent();
        }

        #endregion
    }
}
