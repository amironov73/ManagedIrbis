/* SiberianCheckCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianCheckCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// State.
        /// </summary>
        public bool State
        {
            get { return _state; }
            set { _SetState(value); }
        }

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        public string Text
        {
            get { return _text; }
            set { _SetText(value); }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private bool _state;

        private string _text;

        private void _SetState
            (
                bool state
            )
        {
            _state = state;
            Column.PutData(Row.Data, this);
            Grid.Invalidate();
        }

        private void _SetText
            (
                string text
            )
        {
            _text = text;
            Grid.Invalidate();
        }

        #endregion

        #region Public methods

        #endregion

        #region SiberianCell members

        /// <inheritdoc />
        public override void CloseEditor
            (
                bool accept
            )
        {
            if (!ReferenceEquals(Grid.Editor, null))
            {
                if (accept)
                {
                    // State = ....
                }
            }

            base.CloseEditor(accept);
        }

        /// <inheritdoc/>
        public override void Paint
            (
                PaintEventArgs args
            )
        {
            Graphics graphics = args.Graphics;
            Rectangle rectangle = args.ClipRectangle;
            Rectangle textRectangle = new Rectangle
                (
                    rectangle.Left + 20,
                    rectangle.Y,
                    rectangle.Width - 20,
                    rectangle.Height
                );

            //Color foreColor = Color.Black;
            //if (ReferenceEquals(Row, Grid.CurrentRow))
            //{
            //    foreColor = Color.White;
            //}

            if (ReferenceEquals(this, Grid.CurrentCell))
            {
                Color backColor = Color.Blue;
                using (Brush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, rectangle);
                }
            }

            TextFormatFlags flags
                = TextFormatFlags.TextBoxControl
                | TextFormatFlags.EndEllipsis
                | TextFormatFlags.NoPrefix
                | TextFormatFlags.VerticalCenter;

            CheckBoxState state = State
                ? CheckBoxState.CheckedNormal
                : CheckBoxState.UncheckedNormal;

            Point point = new Point
                (
                    rectangle.X + 2,
                    rectangle.Y + 2
                );

            CheckBoxRenderer.DrawCheckBox
                (
                    graphics,
                    point,
                    textRectangle,
                    Text,
                    Grid.Font,
                    flags,
                    false,
                    state
                );
        }

        #endregion

        #region Object members

        #endregion

    }
}
