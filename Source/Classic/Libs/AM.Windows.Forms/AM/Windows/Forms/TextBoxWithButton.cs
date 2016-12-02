// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextBoxWithButton.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="TextBox"/> with <see cref="Button"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [ToolboxBitmap(typeof(Clocks), "Images.TextBoxWithButton.bmp")]
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class TextBoxWithButton
        : UserControl
    {
        #region Events

        /// <summary>
        /// Raised on button click.
        /// </summary>
        public event EventHandler ButtonClick;

        /// <summary>
        /// Raised when text changed.
        /// </summary>
        public new event EventHandler TextChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Button.
        /// </summary>
        [NotNull]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Button Button { get { return _button; } }

        /// <summary>
        /// Text box.
        /// </summary>
        [NotNull]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextBox TextBox { get { return _textBox; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextBoxWithButton()
        {
            InitializeComponent();

            Button.Click += _Button_Click;
            Button.Width = 20;
            TextBox.TextChanged += _TextBox_TextChanged;
        }

        #endregion

        #region Private members

        private void _Button_Click
            (
                object sender,
                EventArgs e
            )
        {
            ButtonClick.Raise(this, e);
        }

        private void _TextBox_TextChanged
            (
                object sender,
                EventArgs e
            )
        {
            TextChanged.Raise(this, e);
        }

        #endregion

        #region Public methods

        #endregion

        #region Control members

        /// <inheritdoc/>
        public override string Text 
        {
            get { return TextBox.Text; }
            set { TextBox.Text = value; }
        }

        #endregion
    }
}
