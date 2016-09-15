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
        /// Default cursor height (lines).
        /// </summary>
        public const int DefaultCursorHeight = 2;

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

        /// <summary>
        /// Maximal window height.
        /// </summary>
        public const int MaximalWindowHeight = 100;

        /// <summary>
        /// Maximal window width.
        /// </summary>
        public const int MaximalWindowWidth = 100;

        /// <summary>
        /// Minimal window height.
        /// </summary>
        public const int MinimalWindowHeight = 2;

        /// <summary>
        /// Minimal window width.
        /// </summary>
        public const int MinimalWindowWidth = 2;

        #endregion

        #region Nested classes

        struct Cell
        {
            public bool Emphasized;

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
        /// Bold version of the <see cref="P:Font"/>.
        /// </summary>
        [NotNull]
        public Font ItalicFont { get { return _italicFont; } }

        /// <summary>
        /// Cursor height.
        /// </summary>
        [DefaultValue(DefaultCursorHeight)]
        public int CursorHeight
        {
            get { return _cursorHeight; }
            set
            {
                if (value < 1
                    || value > _cellHeight)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _cursorHeight = value;
            }
        }

        /// <summary>
        /// Column position of the cursor.
        /// </summary>
        public int CursorLeft { get; set; }

        /// <summary>
        /// Row position of the cursor.
        /// </summary>
        public int CursorTop { get; set; }

        /// <summary>
        /// Whether the cursor is visible.
        /// </summary>
        [DefaultValue(true)]
        public bool CursorVisible
        {
            get { return _cursorVisible; }
            set
            {
                _cursorVisible = value;
                _CursorHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Window height.
        /// </summary>
        [DefaultValue(DefaultWindowHeight)]
        public int WindowHeight
        {
            get {return _windowHeight;}
            set
            {
                if (value < MinimalWindowHeight
                    || value > MaximalWindowHeight)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _windowHeight = value;
                _SetupWindow();
            }
        }

        /// <summary>
        /// Window width.
        /// </summary>
        [DefaultValue(DefaultWindowWidth)]
        public int WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                if (value < MinimalWindowWidth
                    || value > MaximalWindowWidth)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _windowWidth = value;
                _SetupWindow();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleControl()
        {
            DoubleBuffered = true;

            BackColor = Color.Black;
            ForeColor = Color.LightGray;
            Font = new Font
                (
                    DefaultFontName,
                    DefaultFontSize,
                    FontStyle.Regular
                );

            WindowHeight = DefaultWindowHeight;
            WindowWidth = DefaultWindowWidth;
            _SetupWindow();

            _cursorTimer = new Timer
            {
                Enabled = true,
                Interval = 200
            };
            _cursorTimer.Tick += _CursorHandler;
            CursorHeight = DefaultCursorHeight;
            CursorVisible = true;
        }

        #endregion

        #region Private members

        private Cell[] _cells;

        private int _cellWidth;
        private int _cellHeight;
        private int _windowWidth;
        private int _windowHeight;

        private int _cursorHeight;
        private bool _cursorVisible;
        private bool _cursorVisibleNow;

        private readonly Timer _cursorTimer;

        private Font _font;
        private Font _italicFont;

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

        private void _CursorHandler
            (
                object sender,
                EventArgs eventArgs
            )
        {
            if (!_cursorVisible)
            {
                return;
            }

            _cursorVisibleNow = !_cursorVisibleNow;

            using (Graphics graphics = Graphics.FromHwnd(Handle))
            {
                Rectangle rectangle = new Rectangle
                    (
                        _cellWidth * CursorLeft,
                        _cellHeight * CursorTop,
                        _cellWidth,
                        _cellHeight
                    );

                Cell cell = _cells[CursorTop*WindowWidth + CursorLeft];
                using (Brush foreBrush = new SolidBrush(cell.ForeColor))
                using (Brush backBrush = new SolidBrush(cell.BackColor))
                using (Brush cursorBrush = new SolidBrush(ForeColor))
                {
                    string s = new string(cell.Character, 1);
                    graphics.FillRectangle(backBrush, rectangle);
                    graphics.DrawString(s, Font, foreBrush, rectangle);

                    if (_cursorVisibleNow)
                    {
                        rectangle = new Rectangle
                            (
                                rectangle.Left,
                                rectangle.Top + _cellHeight - CursorHeight,
                                rectangle.Width,
                                CursorHeight
                            );
                        graphics.FillRectangle(cursorBrush, rectangle);
                    }
                }
            }
        }

        private void _SetupCells()
        {
            if (WindowHeight == 0
                || WindowWidth == 0)
            {
                return;
            }

            int cellCount = WindowHeight * WindowWidth;
            _cells = new Cell[cellCount];
            Cell empty = new Cell
            {
                Character = ' ',
                ForeColor = ForeColor,
                BackColor = BackColor
            };
            for (int i = 0; i < cellCount; i++)
            {
                _cells[i] = empty;
            }

            Size clientSize = ClientSize;
            _cellHeight = clientSize.Height/WindowHeight;
            _cellWidth = clientSize.Width/WindowWidth;
        }

        private void _SetupFont()
        {
            _italicFont = new Font(_font, FontStyle.Italic);

            _SetupCells();
        }

        private void _SetupWindow()
        {
            _SetupCells();
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
                Color backColor,
                bool emphasize
            )
        {
            Cell cell = new Cell
            {
                Character = c,
                BackColor = backColor,
                ForeColor = foreColor,
                Emphasized = emphasize
            };

            _cells[CursorTop * WindowWidth + CursorLeft] = cell;
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
                Color backColor,
                bool emphasize
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
                ForeColor = foreColor,
                Emphasized = emphasize
            };
            _cells[row * WindowWidth + column] = cell;
            Invalidate();
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public void Write
            (
                Color foreColor,
                Color backColor,
                bool emphasize,
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            foreach (char c in text)
            {
                Write(c, foreColor, backColor, emphasize);
            }
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public void Write
            (
                bool emphasize,
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            foreach (char c in text)
            {
                Write(c, ForeColor, BackColor, emphasize);
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
                Write(c, ForeColor, BackColor, false);
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

        /// <summary>
        /// Write text and move cursor to beginning of next line.
        /// </summary>
        public void WriteLine
            (
                bool emphasize,
                [CanBeNull] string text
            )
        {
            Write(emphasize, text);
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
        protected override void Dispose
            (
                bool disposing
            )
        {
            if (_cursorTimer != null)
            {
                _cursorTimer.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <inheritdoc />
        public override Font Font
        {
            get { return _font; }
            set
            {
                Code.NotNull(value, "value");

                _font = value;
                _SetupFont();
            }
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
                        graphics.DrawString
                            (
                                s,
                                cell.Emphasized ? ItalicFont : Font,
                                foreBrush,
                                rectangle
                            );
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

        /// <inheritdoc />
        protected override void OnClientSizeChanged
            (
                EventArgs e
            )
        {
            base.OnClientSizeChanged(e);

            _SetupCells();
        }

        #endregion
    }
}
