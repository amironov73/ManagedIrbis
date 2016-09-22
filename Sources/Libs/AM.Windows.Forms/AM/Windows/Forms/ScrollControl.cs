/* ScrollControl.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using RC = AM.Windows.Forms.Properties.Resources;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Simplest scroll control.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ScrollControl
        : Control
    {
        #region Events

        /// <summary>
        /// Raised on scroll.
        /// </summary>
        public event ScrollEventHandler Scroll;

        #endregion

        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScrollControl()
        {
            SetStyle
                (
                    ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.UserPaint
                    | ControlStyles.StandardClick,
                    true
                );
            SetStyle
                (
                    ControlStyles.Selectable,
                    false
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Control members

        /// <inheritdoc />
        protected override Size DefaultMaximumSize
        {
            get { return new Size(16, 0); }
        }

        /// <inheritdoc />
        protected override Size DefaultMinimumSize
        {
            get { return new Size(16, 0); }
        }

        /// <inheritdoc />
        protected override Size DefaultSize
        {
            get { return new Size(16, 200); }
        }

        /// <inheritdoc />
        protected override void OnMouseDown
            (
                MouseEventArgs e
            )
        {
            base.OnMouseDown(e);

            ScrollEventType type = e.Y > Height / 2
                ? ScrollEventType.LargeIncrement
                : ScrollEventType.LargeDecrement;

            ScrollEventArgs eventArgs = new ScrollEventArgs
                (
                    type,
                    0
                );
            ScrollEventHandler handler = Scroll;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        /// <inheritdoc />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics graphics = e.Graphics;

            using (Brush brush = new SolidBrush(BackColor))
            {
                graphics.FillRectangle
                    (
                        brush,
                        e.ClipRectangle
                    );
            }

            using (Image upArrow = RC.arrowUp)
            {
                graphics.DrawImage
                (
                    upArrow,
                    0,
                    0,
                    16,
                    16
                );
            }

            using (Image downArrow = RC.arrowDown)
            {
                graphics.DrawImage
                (
                    downArrow,
                    0,
                    Height - downArrow.Height,
                    16,
                    16
                );
            }

            using (Pen pen = new Pen(ForeColor, 0.5f))
            {
                graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                graphics.DrawRectangle(pen, 0, 0, Width, 16);
                graphics.DrawRectangle(pen, 0, Height-15, Width, 16);
            }
        }

        #endregion
    }
}
