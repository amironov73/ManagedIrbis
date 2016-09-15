/* ConsoleControl.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Pseudo-console.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolboxBitmap(typeof(BusyStripe), "Images.ConsoleControl.bmp")]
    public sealed class ConsoleControl
        : Control
    {
        #region Constants

        /// <summary>
        /// Default window height.
        /// </summary>
        public const int DefaultWindowHeight = 25;

        /// <summary>
        /// Default window width.
        /// </summary>
        public const int DefaultWindowWidth = 80;

        /// <summary>
        /// Default font name.
        /// </summary>
        public const string DefaultFontName = "Courier New";

        /// <summary>
        /// Default font size.
        /// </summary>
        public const float DefaultFontSize = 8f;

        #endregion

        #region Nested classes

        struct Cell
        {
            public char Character;

            public Color ForeColor;

            public Color BackColor;

            public override string ToString()
            {
                return new string(Character, 1);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Column position of the cursor.
        /// </summary>
        public int CursorLeft { get; set; }

        /// <summary>
        /// Row position of the cursor.
        /// </summary>
        public int CursorTop { get; set; }

        /// <summary>
        /// Window height.
        /// </summary>
        [DefaultValue(DefaultWindowHeight)]
        public int WindowHeight { get; set; }

        /// <summary>
        /// Window width.
        /// </summary>
        [DefaultValue(DefaultWindowWidth)]
        public int WindowWidth { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleControl()
        {
            DoubleBuffered = true;

            WindowHeight = DefaultWindowHeight;
            WindowWidth = DefaultWindowWidth;

            Font = new Font
                (
                    DefaultFontName,
                    DefaultFontSize,
                    FontStyle.Regular
                );
            BackColor = Color.Black;
            ForeColor = Color.LightGray;

            _cellWidth = 8;
            _cellHeight = 12;

            _cells = new Cell[WindowHeight * WindowWidth];
        }

        #endregion

        #region Private members

        private Cell[] _cells;

        private int _cellWidth;
        private int _cellHeight;

        private void _AdvanceCursor()
        {
            CursorLeft++;
            if (CursorLeft >= WindowWidth)
            {
                CursorLeft = 0;
                CursorTop++;
            }
            if (CursorTop >= WindowHeight)
            {
                ScrollUp();
                CursorTop--;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the console.
        /// </summary>
        public void Clear()
        {
            CursorTop = 0;
            CursorLeft = 0;

            int total = WindowWidth * WindowHeight;

            Cell empty = new Cell
            {
                Character = ' ',
                ForeColor = ForeColor,
                BackColor = BackColor
            };
            for (int i = 0; i < total; i++)
            {
                _cells[i] = empty;
            }

            Invalidate();
        }

        /// <summary>
        /// Scroll up by one line.
        /// </summary>
        public void ScrollUp()
        {
            int total = WindowWidth * WindowHeight;
            int scrollingEnd = total - WindowWidth;

            for (int i = 0; i < scrollingEnd; i++)
            {
                _cells[i] = _cells[i + WindowWidth];
            }

            Cell empty = new Cell
            {
                Character = ' ',
                ForeColor = ForeColor,
                BackColor = BackColor
            };
            for (int i = scrollingEnd; i < total; i++)
            {
                _cells[i] = empty;
            }
            
            Invalidate();
        }

        /// <summary>
        /// WriteCharacter.
        /// </summary>
        public void Write
            (
                char c,
                Color foreColor,
                Color backColor
            )
        {
            Cell cell = new Cell
            {
                Character = c,
                BackColor = backColor,
                ForeColor = foreColor
            };

            _cells[CursorTop*WindowWidth + CursorLeft] = cell;
            _AdvanceCursor();
            Invalidate();
        }

        /// <summary>
        /// Write character.
        /// </summary>
        public void Write
            (
                int row,
                int column,
                char c,
                Color foreColor,
                Color backColor
            )
        {
            if (
                row < 0
                || row >= WindowHeight
                || column < 0
                || column >= WindowWidth
                )
            {
                return;
            }

            Cell cell = new Cell
            {
                Character = c,
                BackColor = backColor,
                ForeColor = foreColor
            };
            _cells[row*WindowWidth + column] = cell;
            Invalidate();
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public void Write
            (
                [CanBeNull] string text,
                Color foreColor,
                Color backColor
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            foreach (char c in text)
            {
                Write(c, foreColor, backColor);
            }
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public void Write
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            foreach (char c in text)
            {
                Write(c, ForeColor, BackColor);
            }
        }

        /// <summary>
        /// Move cursor to beginning of next line.
        /// </summary>
        public void WriteLine()
        {
            CursorLeft = 0;
            CursorTop++;
            if (CursorTop >= WindowHeight)
            {
                ScrollUp();
                CursorTop--;
            }
        }

        /// <summary>
        /// Write text and move cursor to beginning of next line.
        /// </summary>
        public void WriteLine
            (
                [CanBeNull] string text
            )
        {
            Write(text);
            WriteLine();
        }

        #endregion

        #region Control members

        /// <inheritdoc />
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams result = base.CreateParams;

                return result;
            }
        }

        /// <inheritdoc />
        protected override Size DefaultSize
        {
            get { return new Size(640, 300); }
        }

        /// <inheritdoc />
        protected override void OnPaint
            (
                PaintEventArgs paintEvent
            )
        {
            Graphics graphics = paintEvent.Graphics;

            int cellOffset = 0;
            int rowOffset = 0;

            for (int row = 0; row < WindowHeight; row++)
            {
                int columnOffset = 0;

                for (int column = 0; column < WindowWidth; column++)
                {
                    Cell cell = _cells[cellOffset];

                    Rectangle rectangle = new Rectangle
                        (
                            columnOffset,
                            rowOffset,
                            _cellWidth,
                            _cellHeight
                        );

                    using (Brush foreBrush = new SolidBrush(cell.ForeColor))
                    using (Brush backBrush = new SolidBrush(cell.BackColor))
                    {
                        graphics.FillRectangle(backBrush, rectangle);
                        string s = new string(cell.Character, 1);
                        graphics.DrawString(s, Font, foreBrush, rectangle);
                    }

                    cellOffset++;
                    columnOffset += _cellWidth;
                }

                rowOffset += _cellHeight;
            }

            base.OnPaint(paintEvent);
        }

        /// <inheritdoc />
        protected override void OnPaintBackground
            (
                PaintEventArgs paintEvent
            )
        {
            // Do nothing
        }

        #endregion
    }
}
