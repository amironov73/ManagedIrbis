// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RetryForm.cs --
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
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class RetryForm 
        : Form
    {
        #region Properties

        /// <summary>
        /// Message text.
        /// </summary>
        public string Message
        {
            get { return _messageLabel.Text; }
            set { _messageLabel.Text = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RetryForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private static bool _Resolver
            (
                [NotNull] Exception exception
            )
        {
            using (RetryForm form = new RetryForm())
            {
                form.Message = exception.Message;
                DialogResult result = form.ShowDialog();

                return result == DialogResult.Yes;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get resolver.
        /// </summary>
        [NotNull]
        public static Func<Exception, bool> GetResolver()
        {
            return _Resolver;
        }

        #endregion
    }
}
