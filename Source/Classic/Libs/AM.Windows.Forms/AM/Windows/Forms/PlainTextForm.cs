// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlainTextForm.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable VirtualMemberCallInConstructor

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public partial class PlainTextForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Text editor.
        /// </summary>
        [NotNull]
        public PlainTextEditor Editor
        {
            get { return _textControl; }
        }

        /// <inheritdoc cref="Control.Text" />
        public override string Text
        {
            get
            {
                return ReferenceEquals(_textControl, null)
                    ? string.Empty
                    :_textControl.Text;
            }
            set
            {
                if (!ReferenceEquals(_textControl, null))
                {
                    _textControl.Text = value;
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextForm
            (
                [CanBeNull] string text
            )
            : this()
        {
            Text = text;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add button to the toolbox.
        /// </summary>
        public void AddButton
            (
                [NotNull] ToolStripButton button
            )
        {
            Code.NotNull(button, "button");

            Editor.AddButton(button);
        }

        /// <summary>
        /// Show the windows with the text.
        /// </summary>
        public static DialogResult ShowDialog
            (
                [CanBeNull] IWin32Window owner,
                [CanBeNull] string text
            )
        {
            using (PlainTextForm form = new PlainTextForm(text))
            {
                return form.ShowDialog(owner);
            }
        }

        #endregion
    }
}
