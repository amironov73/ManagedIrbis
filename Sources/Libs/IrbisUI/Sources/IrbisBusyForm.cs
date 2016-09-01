/* IrbisBusyForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class IrbisBusyForm
        : Form
    {
        #region Events

        /// <summary>
        /// Break button pressed.
        /// </summary>
        public event EventHandler BreakPressed;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisBusyForm()
        {
            Debug.WriteLine("ENTER IrbisBusyForm..ctor");
            Debug.WriteLine("THREAD=" + Thread.CurrentThread.Name);

            InitializeComponent();

            Debug.WriteLine("LEAVE IrbisBusyForm..ctor");
        }

        #endregion

        #region Private members

        private void _breakButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            EventHandler handler = BreakPressed;
            if (!ReferenceEquals(handler, null))
            {
                handler(sender, e);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set form title.
        /// </summary>
        public void SetTitle
            (
                [NotNull] string title
            )
        {
            Code.NotNull(title, "title");

            Text = title;
        }

        /// <summary>
        /// Set form message.
        /// </summary>
        public void SetMessage
            (
                [NotNull]string message
            )
        {
            Code.NotNull(message, "message");

            _messageLabel.Text = message;
        }

        #endregion
    }
}
