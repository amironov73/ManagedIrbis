/* SiberianColumn.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    public abstract class SiberianColumn
    {
        #region Constants

        /// <summary>
        /// Default fill width.
        /// </summary>
        public const int DefaultFillWidth = 0;

        /// <summary>
        /// Default width.
        /// </summary>
        public const int DefaultWidth = 200;

        /// <summary>
        /// Default minimal width.
        /// </summary>
        public const int DefaultMinWidth = 100;

        #endregion

        #region Properties

        /// <summary>
        /// Background color.
        /// </summary>
        public Color BackColor { get; set; }

        /// <summary>
        /// Foreground color.
        /// </summary>
        public Color ForeColor { get; set; }

        /// <summary>
        /// Column index.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Data member (property name).
        /// </summary>
        [CanBeNull]
        public string Member { get; set; }

        /// <summary>
        /// Read only column?
        /// </summary>
        [DefaultValue(false)]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Grid.
        /// </summary>
        [CanBeNull]
        public SiberianGrid Grid { get; internal set; }

        /// <summary>
        /// Title.
        /// </summary>
        [CanBeNull]
        public string Title { get; set; }

        /// <summary>
        /// Fill width.
        /// </summary>
        [DefaultValue(DefaultFillWidth)]
        public int FillWidth
        {
            get { return _fillWidth; }
            set
            {
                _fillWidth = value;
                if (!ReferenceEquals(Grid, null))
                {
                    Grid.AutoSizeColumns();
                }
            }
        }

        /// <summary>
        /// Width, pixels.
        /// </summary>
        [DefaultValue(DefaultWidth)]
        public int Width
        {
            get { return _width; }
            set
            {
                _width = Math.Max(value, MinWidth);
                if (!ReferenceEquals(Grid, null))
                {
                    Grid.AutoSizeColumns();
                }
            }
        }

        /// <summary>
        /// Minimal width of the column, pixels.
        /// </summary>
        [DefaultValue(DefaultMinWidth)]
        public int MinWidth { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        protected SiberianColumn()
        {
            BackColor = Color.Transparent;
            FillWidth = DefaultFillWidth;
            Width = DefaultWidth;
            MinWidth = DefaultMinWidth;
        }

        #endregion

        #region Private members

        private int _fillWidth;

        private int _width;

        #endregion

        #region Public methods

        /// <summary>
        /// Create cell.
        /// </summary>
        [NotNull]
        public abstract SiberianCell CreateCell();

        /// <summary>
        /// Create editor for the cell.
        /// </summary>
        [CanBeNull]
        public virtual Control CreateEditor
            (
                [NotNull] SiberianCell cell,
                bool edit,
                object state
            )
        {
            Code.NotNull(cell, "cell");

            return null;
        }

        /// <summary>
        /// Get data from the object and put it to the cell.
        /// </summary>
        public virtual void GetData
            (
                [CanBeNull] object theObject,
                [NotNull] SiberianCell cell
            )
        {
            Code.NotNull(cell, "cell");

            // Nothing to do here
        }

        /// <summary>
        /// Draw the column.
        /// </summary>
        public virtual void Paint
            (
                PaintEventArgs args
            )
        {
            Graphics graphics = args.Graphics;
            Rectangle rectangle = args.ClipRectangle;

            if (BackColor != Color.Transparent
                && BackColor != Color.Empty)
            {
                using (Brush brush = new SolidBrush(BackColor))
                {
                    graphics.FillRectangle
                    (
                        brush,
                        rectangle
                    );
                }
            }
        }

        /// <summary>
        /// Draw the column header.
        /// </summary>
        public virtual void PaintHeader
            (
                PaintEventArgs args
            )
        {
            Graphics graphics = args.Graphics;
            Rectangle rectangle = args.ClipRectangle;

            using (Brush brush = new SolidBrush(Color.LightSalmon))
            {
                graphics.FillRectangle(brush, rectangle);

                TextFormatFlags flags 
                    = TextFormatFlags.Left
                    | TextFormatFlags.NoPrefix
                    | TextFormatFlags.VerticalCenter
                    | TextFormatFlags.EndEllipsis;

                TextRenderer.DrawText
                    (
                        graphics,
                        Title,
                        Grid.Font,
                        rectangle,
                        ForeColor,
                        flags
                    );
            }
        }

        /// <summary>
        /// Get data from the cell and put it to the object.
        /// </summary>
        public virtual void PutData
            (
                [CanBeNull] object theObject,
                [NotNull] SiberianCell cell
            )
        {
            Code.NotNull(cell, "cell");
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}: {1}",
                    GetType().Name,
                    Title
                );
        }

        #endregion
    }
}
