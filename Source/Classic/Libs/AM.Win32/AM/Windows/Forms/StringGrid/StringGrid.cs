/* StringGrid.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using CodeJam;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Простейшая таблица со строками.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    //[ToolboxBitmap(typeof(StringGrid), "Images.StringGrid.bmp")]
    public class StringGrid
        : Control
    {
        #region Public events

        //===========================================================
        // PUBLIC EVENTS
        //===========================================================

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<CellChangedEventArgs> CellChanging;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<CellChangedEventArgs> CellChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<CellClickEventArgs> CellClick;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<HeaderClickEventArgs> HeaderClick;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<CellDrawEventArgs> CellDraw;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ModifiedChanged;

        #endregion

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container _components = null;

        #region Public properties

        //===========================================================
        // PUBLIC PROPERTIES
        //===========================================================

        private static readonly Color DefaultGridColor
            = Color.DarkGray;
        private Color _gridColor = DefaultGridColor;

        /// <summary>
        /// Цвет линий.
        /// </summary>
        [Browsable(true)]
        public Color GridColor
        {
            get
            {
                return _gridColor;
            }
            set
            {
                _gridColor = value;
                Invalidate();
            }
        }

        private static readonly Color DefaultCurrentCellColor
            = Color.White;
        private Color _currentCellColor = DefaultCurrentCellColor;

        /// <summary>
        /// Property CurrentCellColor (Color)
        /// </summary>
        [Browsable(true)]
        public Color CurrentCellColor
        {
            get
            {
                return _currentCellColor;
            }
            set
            {
                _currentCellColor = value;
                Invalidate();
            }
        }

        private static readonly Color DefaultHeaderBackColor
            = Color.Blue;
        private Color _headerBackColor = DefaultHeaderBackColor;

        /// <summary>
        /// Property HeaderBackColor (Color)
        /// </summary>
        [Browsable(true)]
        public Color HeaderBackColor
        {
            get
            {
                return _headerBackColor;
            }
            set
            {
                _headerBackColor = value;
                Invalidate();
            }
        }

        private static readonly Color DefaultHeaderForeColor
            = Color.White;
        private Color _headerForeColor = DefaultHeaderForeColor;

        /// <summary>
        /// Property HeaderForeColor (Color)
        /// </summary>
        [Browsable(true)]
        public Color HeaderForeColor
        {
            get
            {
                return _headerForeColor;
            }
            set
            {
                _headerForeColor = value;
                Invalidate();
            }
        }

        private const bool DefaultShowGridLines = true;
        private bool _showGridLines = DefaultShowGridLines;

        /// <summary>
        /// Property ShowGridLines (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultShowGridLines)]
        public bool ShowGrid
        {
            get
            {
                return _showGridLines;
            }
            set
            {
                _showGridLines = value;
                Invalidate();
            }
        }

        private const int DefaultColumnCount = 2;
        private int _columnCount = DefaultColumnCount;

        /// <summary>
        /// Property ColumnCount (int)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultColumnCount)]
        public int ColumnCount
        {
            get
            {
                return _columnCount;
            }
            set
            {
                Debug.Assert(value > 0, "ColumnCount must be > 0");
                CreateCells(value, _rowCount, true);
            }
        }

        private const int DefaultRowCount = 3;
        private int _rowCount = DefaultRowCount;

        /// <summary>
        /// Property RowCount (int)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultRowCount)]
        public int RowCount
        {
            get
            {
                return _rowCount;
            }
            set
            {
                Debug.Assert(value > 0, "ColumnCount must be > 0");
                CreateCells(_columnCount, value, true);
            }
        }

        private ColumnsCollection _columns;

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        [Browsable(true)]
        public ColumnsCollection Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                Debug.Assert(value.Count == _columnCount);
                _columns = value;
                Invalidate();
            }
        }

        private const bool DefaultShowColumnHeader = true;
        private bool _showColumnHeader = DefaultShowColumnHeader;

        /// <summary>
        /// Property ShowColumnHeader (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultShowColumnHeader)]
        public bool ShowColumnHeader
        {
            get
            {
                return _showColumnHeader;
            }
            set
            {
                _showColumnHeader = value;
                Invalidate();
            }
        }

        private const bool DefaultAllowColumnResize = true;
        private bool _allowColumnResize = DefaultAllowColumnResize;

        /// <summary>
        /// Property AllowColumnResize (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultAllowColumnResize)]
        public bool AllowColumnResize
        {
            get
            {
                return _allowColumnResize;
            }
            set
            {
                _allowColumnResize = value;
            }
        }

        private string[,] _cells;

        /// <summary>
        /// Property Cells (string[])
        /// </summary>
        public string this[int col, int row]
        {
            get
            {
                return _cells[col, row];
            }
            set
            {
                CellChangedEventArgs eventArgs
                    = new CellChangedEventArgs
                {
                    Column = col,
                    Row = row,
                    OldValue = _cells[col, row],
                    NewValue = value
                };

                CellChanging.Raise(this, eventArgs);

                _cells[col, row] = value;

                if (_autoColumnSize)
                {
                    AutoSizeColumn(col);
                }

                CellChanged.Raise(this, eventArgs);

                Invalidate();
            }
        }

        private const int DefaultCurrentColumn = 0;
        private int _currentColumn = DefaultCurrentColumn;

        /// <summary>
        /// Property CurrentColumn (int)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DefaultCurrentColumn)]
        public int CurrentColumn
        {
            get
            {
                return _currentColumn;
            }
            set
            {
                MoveTo(value, _currentRow);
            }
        }

        private const int DefaultCurrentRow = 0;
        private int _currentRow = DefaultCurrentRow;

        /// <summary>
        /// Property CurrentRow (int)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DefaultCurrentRow)]
        public int CurrentRow
        {
            get
            {
                return _currentRow;
            }
            set
            {
                MoveTo(_currentColumn, value);
            }
        }

        /// <summary>
        /// Current cell value
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public string CurrentCell
        {
            get
            {
                return this[_currentColumn, _currentRow];
            }
            set
            {
                this[_currentColumn, _currentRow] = value;
            }
        }

        private const int DefaultTopRow = 0;
        private int _topRow = DefaultTopRow;

        /// <summary>
        /// Property TopRow (int)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DefaultTopRow)]
        public int TopRow
        {
            get
            {
                return _topRow;
            }
        }

        private const int DefaultLeftColumn = 0;
        private int _leftColumn = DefaultLeftColumn;

        /// <summary>
        /// Property LeftColumn (int)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DefaultLeftColumn)]
        public int LeftColumn
        {
            get
            {
                return _leftColumn;
            }
        }

        private const bool DefaultReadOnly = false;
        private bool _readOnly = DefaultReadOnly;

        /// <summary>
        /// Property ReadOnly (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultReadOnly)]
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                AcceptText();
            }
        }

        private const bool DefaultAllowAppend = true;
        private bool _allowAppend = DefaultAllowAppend;

        /// <summary>
        /// Property AllowAppend (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultAllowAppend)]
        public bool AllowAppend
        {
            get
            {
                return _allowAppend;
            }
            set
            {
                _allowAppend = value;
            }
        }

        /// <summary>
        /// Number of (fully) visible rows
        /// </summary>
        [Browsable(false)]
        public int VisibleRows
        {
            get
            {
                int height = ClientSize.Height;
                int vr1 = height / FontHeight,
                    vr2 = _rowCount - _topRow;
                if (_showColumnHeader)
                {
                    vr1--;
                }
                if (vr1 > vr2)
                {
                    vr1 = vr2;
                }
                return vr1;
            }
        }

        /// <summary>
        /// Number of (fully) visible columns
        /// </summary>
        [Browsable(false)]
        public int VisibleColumns
        {
            get
            {
                int width = 0;
                int col;
                for (col = _leftColumn; col < _columnCount; col++)
                {
                    width += _columns[col].Width;
                    if (width >= Width)
                    {
                        break;
                    }
                }
                return (col - _leftColumn);
            }
        }

        private const string DefaultDelimiter = ";";
        private string _delimiter = DefaultDelimiter;

        /// <summary>
        /// Property Delimiter (string)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultDelimiter)]
        public string Delimiter
        {
            get
            {
                return _delimiter;
            }
            set
            {
                _delimiter = value;
            }
        }

        /// <summary>
        /// Для получения/присвоения данных скопом.
        /// </summary>
        [Browsable(false)]
        public string DelimitedData
        {
            get
            {
                StringBuilder sb = new StringBuilder(1000);
                string[] tmp = new string[_columnCount];
                for (int row = 0; row < _rowCount; row++)
                {
                    for (int col = 0; col < _columnCount; col++)
                    {
                        tmp[col] = _cells[col, row];
                    }
                    sb.Append(string.Join(_delimiter, tmp));
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
            set
            {
                StringReader sr = new StringReader(value);
                string line;
                char[] sep = _delimiter.ToCharArray();
                ArrayList list = new ArrayList(1000);
                int ncol = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] tmp = line.Split(sep);
                    list.Add(tmp);
                    if (tmp.Length > ncol)
                    {
                        ncol = tmp.Length;
                    }
                }
                CreateCells(ncol, list.Count, false);
                for (int row = 0; row < list.Count; row++)
                {
                    string[] tmp = (string[])list[row];
                    int nc = tmp.Length;
                    if (nc > _columnCount)
                    {
                        nc = _columnCount;
                    }
                    for (int col = 0; col < nc; col++)
                    {
                        _cells[col, row] = tmp[col];
                    }
                }
                AutoSizeColumns();
                Invalidate();
            }
        }

        private const bool DefaultIsModified = false;
        private bool _modified = DefaultIsModified;

        /// <summary>
        /// Property IsModified (bool)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DefaultIsModified)]
        public bool Modified
        {
            get
            {
                return _modified;
            }
            set
            {
                if (value != _modified)
                {
                    _modified = value;
                    ModifiedChanged.Raise(this);
                }
            }
        }

        private const ScrollBars DefaultScrollbars = ScrollBars.Both;
        private ScrollBars _scrollBars = DefaultScrollbars;

        /// <summary>
        /// Property ScrollBars (ScrollBars)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultScrollbars)]
        public ScrollBars ScrollBars
        {
            get
            {
                return _scrollBars;
            }
            set
            {
                _scrollBars = value;
                if (Created)
                {
                    RecreateHandle();
                }
            }
        }

        private const bool DefaultFullWidth = true;
        private bool _fullWidth = DefaultFullWidth;

        /// <summary>
        /// Property FullWidth (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultFullWidth)]
        public bool FullWidth
        {
            get
            {
                return _fullWidth;
            }
            set
            {
                _fullWidth = value;
                Invalidate();
            }
        }

        private const bool DefaultAutoColumnSize = false;
        private bool _autoColumnSize = DefaultAutoColumnSize;

        /// <summary>
        /// Property AutoColumnSize (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultAutoColumnSize)]
        public bool AutoColumnSize
        {
            get
            {
                return _autoColumnSize;
            }
            set
            {
                _autoColumnSize = value;
                AutoSizeColumns();
            }
        }

        private const int DefaultMinimalColumnWidth = 50;
        private int _minimalColumnWidth = DefaultMinimalColumnWidth;

        /// <summary>
        /// Property MinimalColumnWidth (int)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultMinimalColumnWidth)]
        public int MinimalColumnWidth
        {
            get
            {
                return _minimalColumnWidth;
            }
            set
            {
                _minimalColumnWidth = value;
                AutoSizeColumns();
            }
        }

        #endregion

        #region Constructor

        //===========================================================
        // CONSTRUCTOR
        //===========================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringGrid()
            : this(DefaultColumnCount, DefaultRowCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringGrid"/> class.
        /// </summary>
        public StringGrid
            (
                int columnCount,
                int rowCount
            )
        {
            Code.Positive(columnCount, "columnCount");
            Code.Positive(rowCount, "rowCount");

            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // ReSharper disable once VirtualMemberCallInConstructor
            DoubleBuffered = true;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);
            SetStyle(ControlStyles.UserMouse, true);

            CreateCells(columnCount, rowCount, false);
            AutoSizeColumns();
        }

        #endregion

        #region Public routines

        //===========================================================
        // PUBLIC ROUTINES
        //===========================================================

        /// <summary>
        /// Copies this instance.
        /// </summary>
        public void Copy()
        {
            AcceptText();
            Clipboard.SetDataObject(CurrentCell);
        }

        /// <summary>
        /// Cuts this instance.
        /// </summary>
        public void Cut()
        {
            AcceptText();
            if (!_readOnly
                 && !_columns[_currentColumn].ReadOnly)
            {
                Clipboard.SetDataObject(CurrentCell);
                CurrentCell = null;
            }
        }

        /// <summary>
        /// Pastes this instance.
        /// </summary>
        public void Paste()
        {
            DiscardText();
            if (!_readOnly
                 && !_columns[_currentColumn].ReadOnly)
            {
                IDataObject obj = Clipboard.GetDataObject();
                if (obj.GetDataPresent(DataFormats.Text))
                {
                    CurrentCell = (string)(obj.GetData(DataFormats.Text));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AutoSizeColumn(int col)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF r = g.MeasureString(_columns[col].Header, Font);
                int width = (int)r.Width;
                for (int row = 0; row < _rowCount; row++)
                {
                    r = g.MeasureString(_cells[col, row], Font);
                    int cur = (int)r.Width;
                    if (cur > width)
                    {
                        width = cur;
                    }
                }
                if (width < _minimalColumnWidth)
                {
                    width = _minimalColumnWidth;
                }
                _columns[col].Width = width;
            }
        }

        #endregion

        #region Worker routines

        //===========================================================
        // WORKER ROUTINES
        //===========================================================

        /// <summary>
        /// 
        /// </summary>
        public void AutoSizeColumns()
        {
            if (_autoColumnSize)
            {
                for (int col = 0; col < _columnCount; col++)
                {
                    AutoSizeColumn(col);
                }
            }
        }

        /// <summary>
        /// Create cells.
        /// </summary>
        protected void CreateCells
            (
                int columnCount,
                int rowCount, 
                bool copyData
            )
        {
            string[,] oldCells = _cells;
            ColumnsCollection oldColumns = _columns;

            int maxRow = rowCount;
            if (maxRow > _rowCount)
            {
                maxRow = _rowCount;
            }
            int maxCol = columnCount;
            if (maxCol > _columnCount)
            {
                maxCol = _columnCount;
            }
            _cells = new string[columnCount, rowCount];
            if (copyData && (oldCells != null))
            {
                for (int col = 0; col < maxCol; col++)
                {
                    for (int row = 0; row < maxRow; row++)
                    {
                        _cells[col, row] = oldCells[col, row];
                    }
                }
            }

            _columns = new ColumnsCollection(this);
            if (oldColumns != null)
            {
                for (int col = 0;
                      (col < maxCol) && (col < oldColumns.Count);
                      col++)
                {
                    _columns.Add(oldColumns[col]);
                }
            }
            while (_columns.Count < columnCount)
            {
                _columns.Add(_columns.NewColumn());
            }

            _columnCount = columnCount;
            HScrollRange = columnCount;
            _rowCount = rowCount;
            VScrollRange = rowCount;
        }

        /// <summary>
        /// Move cursor to specified position.
        /// </summary>
        protected void MoveTo
            (
                int column,
                int row
            )
        {
            AcceptText();

            if (column < 0)
            {
                column = 0;
            }
            if (row < 0)
            {
                row = 0;
            }
            if (column >= _columnCount)
            {
                column = _columnCount - 1;
            }
            if (row >= _rowCount)
            {
                row = _rowCount - 1;
            }

            int vr = VisibleRows;
            if (row < _topRow)
            {
                _topRow = row;
            }
            if (row >= (_topRow + vr))
            {
                _topRow = row - vr + 1;
                if (_topRow < 0)
                {
                    _topRow = 0;
                }
            }

            if (column < _leftColumn)
            {
                _leftColumn = column;
            }
            int vc = VisibleColumns;
            if (column >= (_leftColumn + vc))
            {
                _leftColumn = column - vc + 1;
                if (_leftColumn < 0)
                {
                    _leftColumn = 0;
                }
            }

            _currentColumn = column;
            HScrollPos = column;
            _currentRow = row;
            VScrollPos = row;
            Invalidate();
        }

        /// <summary>
        /// Move one line up.
        /// </summary>
        protected void MoveOneLineUp()
        {
            MoveTo(_currentColumn, _currentRow - 1);
        }

        /// <summary>
        /// Move one line down.
        /// </summary>
        protected void MoveOneLineDown()
        {
            if (_currentRow == (_rowCount - 1))
            {
                AppendRow();
            }
            MoveTo(_currentColumn, _currentRow + 1);
        }

        /// <summary>
        /// Move one page down.
        /// </summary>
        protected void MoveOnePageDown()
        {
            if (_currentRow == (_rowCount - 1))
            {
                AppendRow();
            }
            MoveTo(_currentColumn, _currentRow + VisibleRows);
        }

        /// <summary>
        /// Move one page up.
        /// </summary>
        protected void MoveOnePageUp()
        {
            MoveTo(_currentColumn, _currentRow - VisibleRows);
        }

        /// <summary>
        /// Move one column left.
        /// </summary>
        protected void MoveOneColumnLeft()
        {
            MoveTo(_currentColumn - 1, _currentRow);
        }

        /// <summary>
        /// Move one column left.
        /// </summary>
        protected void MoveOneColumnRight()
        {
            MoveTo(_currentColumn + 1, _currentRow);
        }

        /// <summary>
        /// Move to the first column.
        /// </summary>
        protected void MoveToFirstColumn()
        {
            MoveTo(0, _currentRow);
        }

        /// <summary>
        /// Move to the last column.
        /// </summary>
        protected void MoveToLastColumn()
        {
            MoveTo(_columnCount - 1, _currentRow);
        }

        private TextBox _textBox;

        /// <summary>
        /// Accepts the text.
        /// </summary>
        public void AcceptText()
        {
            if (_textBox != null)
            {
                if (CurrentCell != _textBox.Text)
                {
                    Modified = true;
                }
                CurrentCell = _textBox.Text;
                _textBox.Dispose();
                _textBox = null;
            }
        }

        /// <summary>
        /// Discard the entered text.
        /// </summary>
        protected void DiscardText()
        {
            if (_textBox != null)
            {
                _textBox.Dispose();
                _textBox = null;
            }
        }

        /// <summary>
        /// Compute rectangle for the cell.
        /// </summary>
        protected Rectangle GetCellRectangle
            (
                int column,
                int row
            )
        {
            Rectangle r = new Rectangle
            {
                Y = (row - _topRow)*FontHeight
            };

            if (_showColumnHeader)
            {
                r.Y += FontHeight;
            }
            for (int i = _leftColumn; i < column; i++)
            {
                r.X += _columns[i].Width;
            }
            r.Height = FontHeight;
            r.Width = _columns[column].Width;
            return r;
        }

        /// <summary>
        /// Create <see cref="TextBox"/> with given text.
        /// </summary>
        protected void CreateTextBox
            (
                string text
            )
        {
            AcceptText();
            if (_readOnly)
            {
                return;
            }

            _textBox = new TextBox();
            Rectangle r = GetCellRectangle(_currentColumn, _currentRow);
            r.Inflate(-2, 0);
            _textBox.Location = r.Location;
            _textBox.Size = r.Size;
            _textBox.Font = Font;
            _textBox.Visible = true;
            _textBox.BorderStyle = BorderStyle.None;
            _textBox.TextAlign = A2A(_columns[_currentColumn].Alignment);
            Controls.Add(_textBox);
            _textBox.Focus();
            _textBox.KeyDown += new KeyEventHandler(tb_KeyDown);
            if (text != null)
            {
                _textBox.AppendText(text);
            }
        }

        /// <summary>
        /// Create <see cref="TextBox"/> without text.
        /// </summary>
        protected void CreateTextBox()
        {
            CreateTextBox(null);
        }

        /// <summary>
        /// Append new row to the grid.
        /// </summary>
        protected void AppendRow()
        {
            if (!_allowAppend)
            {
                return;
            }
            CreateCells(_columnCount, _rowCount + 1, true);
            Modified = true;
            Invalidate();
        }

        /// <summary>
        /// Append column to the grid.
        /// </summary>
        protected void AppendColumn()
        {
            if (!_allowAppend)
            {
                return;
            }
            CreateCells(_columnCount + 1, _rowCount, true);
            Modified = true;
            Invalidate();
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            switch (e.KeyCode)
            {
                case Keys.Down:
                    MoveOneLineDown();
                    break;
                case Keys.Up:
                    MoveOneLineUp();
                    break;
                case Keys.PageDown:
                    MoveOnePageDown();
                    break;
                case Keys.PageUp:
                    MoveOnePageUp();
                    break;
                case Keys.Enter:
                    goto case Keys.Down;
                case Keys.Escape:
                    DiscardText();
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        /// <summary>
        /// Whether the mouse is over column delimiter?
        /// </summary>
        protected int OverColumnDelimiter
            (
                MouseEventArgs e
            )
        {
            if (_allowColumnResize)
            {
                for (int col = _leftColumn,
                          x = 0;
                      col < _columnCount;
                      col++)
                {
                    x += _columns[col].Width;
                    if (x == e.X)
                    {
                        return col;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Compute the cell where mouse.
        /// </summary>
        protected Point OverCell
            (
                MouseEventArgs e
            )
        {
            Point p = new Point(-1, -1);

            p.Y = e.Y / FontHeight + _topRow;
            if (_showColumnHeader)
            {
                p.Y--;
            }
            int col,
                x;
            for (col = _leftColumn, x = 0; col < _columnCount; col++)
            {
                x += _columns[col].Width;
                if (x >= e.X)
                {
                    break;
                }
            }
            p.X = col;

            return p;
        }

        #endregion

        #region Overriden handlers

        //===========================================================
        // OVERRIDEN HANDLERS
        //===========================================================

        /// <inheritdoc />
        protected override bool IsInputKey
            (
                Keys keyData
            )
        {
            // Enable all the keys.
            return true;
        }

        /// <inheritdoc />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor); // ???

            int height = FontHeight,
                width = ClientSize.Width;
            int delta = 0;
            CellDrawEventArgs ea = new CellDrawEventArgs();
            Brush foreBrush = new SolidBrush(ForeColor);
            Brush currCellBrush = new SolidBrush(_currentCellColor);
            Pen gridPen = new Pen(_gridColor);
            int col,
                row,
                x,
                y;
            StringFormat sf = new StringFormat();
            RectangleF r = new RectangleF();
            int endRow = _topRow + VisibleRows;
            int endCol = _leftColumn + VisibleColumns;
            if (endCol < _columnCount)
            {
                endCol++;
            }

            // Вычисляем ширину перерисовываемой области
            if (!_fullWidth)
            {
                for (col = _leftColumn, width = 0; col < endCol; col++)
                {
                    width += _columns[col].Width;
                }
            }

            // Рисуем заголовки колонок
            if (_showColumnHeader)
            {
                Brush hdrBack = new SolidBrush(_headerBackColor);
                Brush hdrFore = new SolidBrush(_headerForeColor);
                Font hdrFont = new Font(Font, FontStyle.Bold);

                g.FillRectangle(hdrBack, 0, 0, width, height);
                r.X = 0;
                r.Y = 0;
                r.Height = height;
                for (col = _leftColumn; col < endCol; col++)
                {
                    r.Width = _columns[col].Width;
                    sf.Alignment = _columns[col].Alignment;
                    g.DrawString(_columns[col].Header, hdrFont, hdrFore, r, sf);
                    r.X += r.Width;
                }
                delta += height;
            }

            // Рисуем грид
            for (row = _topRow, y = height + delta; row < endRow; row++)
            {
                if (_showGridLines)
                {
                    g.DrawLine(gridPen, 0, y, width, y);
                }
                y += height;
            }
            for (col = _leftColumn, x = 0, y -= height; col < endCol; col++)
            {
                x += _columns[col].Width;
                if (_showGridLines)
                {
                    g.DrawLine(gridPen, x, 0, x, y);
                }
            }

            // Рисуем собственно данные
            for (row = _topRow, r.Y = delta, r.Height = height; row < endRow; row++)
            {
                r.X = 0;
                for (col = _leftColumn; col < endCol; col++)
                {
                    r.Width = _columns[col].Width;
                    sf.Alignment = _columns[col].Alignment;
                    // Обработка текущей ячейки
                    if ((row == _currentRow)
                         && (col == _currentColumn))
                    {
                        RectangleF r2;
                        r2 = r;
                        r2.Inflate(-1, -1);
                        g.FillRectangle(currCellBrush, r2);
                    }
                    ea.Drawn = false;
                    if (CellDraw != null)
                    {
                        ea.Column = col;
                        ea.Row = row;
                        ea.Rectangle = r;
                        ea.Format = sf;
                        ea.Value = _cells[col, row];
                        ea.Graphics = g;
                        CellDraw(this, ea);
                    }
                    if (ea.Drawn == false)
                    {
                        g.DrawString(_cells[col, row], Font, foreBrush, r, sf);
                    }
                    r.X += r.Width;
                }
                r.Y += height;
            }

            base.OnPaint(e);
        }

        /// <inheritdoc />
        protected override void OnKeyDown
            (
                KeyEventArgs e
            )
        {
            base.OnKeyDown(e);

            if (e.Modifiers == 0)
            {
                e.Handled = true;
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        MoveOneLineUp();
                        break;
                    case Keys.Down:
                        MoveOneLineDown();
                        break;
                    case Keys.Left:
                        MoveOneColumnLeft();
                        break;
                    case Keys.Right:
                        MoveOneColumnRight();
                        break;
                    case Keys.Home:
                        MoveToFirstColumn();
                        break;
                    case Keys.End:
                        MoveToLastColumn();
                        break;
                    case Keys.PageDown:
                        MoveOnePageDown();
                        break;
                    case Keys.PageUp:
                        MoveOnePageUp();
                        break;
                    case Keys.Enter:
                        if (!_columns[_currentColumn].ReadOnly)
                        {
                            CreateTextBox(CurrentCell);
                        }
                        break;
                    case Keys.Delete:
                        if (!_readOnly
                             && !_columns[_currentColumn].ReadOnly)
                        {
                            CurrentCell = null;
                            Modified = true;
                        }
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
            if (e.Modifiers
                 == Keys.Control)
            {
                e.Handled = true;
                switch (e.KeyCode)
                {
                    case Keys.Insert:
                        Copy();
                        break;
                    case Keys.X:
                        Cut();
                        break;
                    case Keys.C:
                        Copy();
                        break;
                    case Keys.V:
                        Paste();
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
            if (e.Modifiers
                 == Keys.Shift)
            {
                e.Handled = true;
                switch (e.KeyCode)
                {
                    case Keys.Insert:
                        Paste();
                        break;
                    case Keys.Delete:
                        Cut();
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnKeyPress
            (
                KeyPressEventArgs e
            )
        {
            base.OnKeyPress(e);

            if (_readOnly || _columns[_currentColumn].ReadOnly)
            {
                return;
            }

            if (!Char.IsControl(e.KeyChar))
            {
                CreateTextBox(e.KeyChar.ToString());
                e.Handled = true;
            }
        }

        private int resizingColumn,
                    columnPosition;

        private bool inColumnResize = false;

        /// <inheritdoc />
        protected override void OnMouseDown
            (
                MouseEventArgs e
            )
        {
            AcceptText();
            base.OnMouseDown(e);

            if (e.Button
                 == MouseButtons.Left)
            {
                int col = OverColumnDelimiter(e);
                if (col >= 0)
                {
                    resizingColumn = col;
                    columnPosition = 0;
                    for (int i = _leftColumn; i < col; i++)
                    {
                        columnPosition += _columns[i].Width;
                    }
                    inColumnResize = true;
                    return;
                }

                Point point = OverCell(e);
                if (point.Y < 0)
                {
                    if (HeaderClick != null)
                    {
                        HeaderClickEventArgs ea
                            = new HeaderClickEventArgs
                            {
                                Column = point.X
                            };
                        HeaderClick(this, ea);
                    }
                }
                else
                {
                    if (CellClick != null)
                    {
                        CellClickEventArgs ea
                            = new CellClickEventArgs
                        {
                            Column = point.X,
                            Row = point.Y
                        };
                        CellClick(this, ea);
                    }
                    MoveTo(point.X, point.Y);
                    if (!_columns[_currentColumn].ReadOnly
                         && (e.Clicks > 1))
                    {
                        CreateTextBox(CurrentCell);
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (inColumnResize)
            {
                if (e.X
                     > (columnPosition + _minimalColumnWidth))
                {
                    _columns[resizingColumn].Width = e.X - columnPosition;
                    Invalidate();
                }

                return;
            }

            base.OnMouseMove(e);

            if (OverColumnDelimiter(e) >= 0)
            {
                Cursor = Cursors.SizeWE;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
        }

        /// <inheritdoc />
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (inColumnResize)
            {
                inColumnResize = false;
            }

            base.OnMouseUp(e);
        }

        /// <inheritdoc />
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            int delta = e.Delta / 120;
            MoveTo(_currentColumn, _currentRow - delta);
        }

        private const int WM_HSCROLL = 0x0114;
        private const int WM_VSCROLL = 0x0115;

        /// <inheritdoc />
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HSCROLL:
                    switch (((int)m.WParam) & 0xFFFF)
                    {
                        case SB_LEFT:
                            MoveOneColumnLeft();
                            break;
                        case SB_RIGHT:
                            MoveOneColumnRight();
                            break;
                        case SB_LINELEFT:
                            MoveOneColumnLeft();
                            break;
                        case SB_LINERIGHT:
                            MoveOneColumnRight();
                            break;
                        case SB_PAGELEFT:
                            MoveTo(_currentColumn - VisibleColumns, _currentRow);
                            break;
                        case SB_PAGERIGHT:
                            MoveTo(_currentColumn + VisibleColumns, _currentRow);
                            break;
                        case SB_THUMBPOSITION:
                            MoveTo(((int)m.WParam) >> 16, _currentRow);
                            break;
                        case SB_THUMBTRACK:
                            goto case SB_THUMBPOSITION;
                    }
                    m.Result = IntPtr.Zero;
                    break;
                case WM_VSCROLL:
                    switch (((int)m.WParam) & 0xFFFF)
                    {
                        case SB_LINEUP:
                            MoveOneLineUp();
                            break;
                        case SB_LINEDOWN:
                            MoveOneLineDown();
                            break;
                        case SB_PAGEUP:
                            MoveOnePageUp();
                            break;
                        case SB_PAGEDOWN:
                            MoveOnePageDown();
                            break;
                        case SB_BOTTOM:
                            MoveTo(_currentColumn, _rowCount - 1);
                            break;
                        case SB_TOP:
                            MoveTo(_currentColumn, 0);
                            break;
                        case SB_THUMBPOSITION:
                            MoveTo(_currentColumn, ((int)m.WParam) >> 16);
                            break;
                        case SB_THUMBTRACK:
                            goto case SB_THUMBPOSITION;
                    }
                    m.Result = IntPtr.Zero;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region Internal properties

        //===========================================================
        // INTERNAL PROPERTIES
        //===========================================================

        private const int WS_VSCROLL = 0x00200000;
        private const int WS_HSCROLL = 0x00100000;
        private const int SB_LINEUP = 0;
        private const int SB_LINELEFT = 0;
        private const int SB_LINEDOWN = 1;
        private const int SB_LINERIGHT = 1;
        private const int SB_PAGEUP = 2;
        private const int SB_PAGELEFT = 2;
        private const int SB_PAGEDOWN = 3;
        private const int SB_PAGERIGHT = 3;
        private const int SB_THUMBPOSITION = 4;
        private const int SB_THUMBTRACK = 5;
        private const int SB_TOP = 6;
        private const int SB_LEFT = 6;
        private const int SB_BOTTOM = 7;
        private const int SB_RIGHT = 7;

        /// <inheritdoc />
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams par = base.CreateParams;

                switch (_scrollBars)
                {
                    case ScrollBars.Both:
                        par.Style |= (WS_VSCROLL | WS_HSCROLL);
                        break;
                    case ScrollBars.Vertical:
                        par.Style |= WS_VSCROLL;
                        break;
                    case ScrollBars.Horizontal:
                        par.Style |= WS_HSCROLL;
                        break;
                }

                return par;
            }
        }

        /// <summary>
        /// Vertical scroll position.
        /// </summary>
        protected int VScrollPos
        {
            get
            {
                if ((_scrollBars == ScrollBars.Vertical)
                     || (_scrollBars == ScrollBars.Both))
                {
                    return GetScrollPos(Handle, SB_VERT);
                }
                return 0;
            }
            set
            {
                if ((_scrollBars == ScrollBars.Vertical)
                     || (_scrollBars == ScrollBars.Both))
                {
                    SetScrollPos(Handle, SB_VERT, value, true);
                }
            }
        }

        /// <summary>
        /// Horizontal scroll position.
        /// </summary>
        protected int HScrollPos
        {
            get
            {
                if ((_scrollBars == ScrollBars.Horizontal)
                     || (_scrollBars == ScrollBars.Both))
                {
                    return GetScrollPos(Handle, SB_HORZ);
                }
                return 0;
            }
            set
            {
                if ((_scrollBars == ScrollBars.Horizontal)
                     || (_scrollBars == ScrollBars.Both))
                {
                    SetScrollPos(Handle, SB_HORZ, value, true);
                }
            }
        }

        /// <summary>
        /// Vertical scroll range.
        /// </summary>
        protected int VScrollRange
        {
            set
            {
                if ((_scrollBars == ScrollBars.Vertical)
                     || (_scrollBars == ScrollBars.Both))
                {
                    SetScrollRange(Handle, SB_VERT, 0, value, true);
                }
            }
        }

        /// <summary>
        /// Horizontal scroll range.
        /// </summary>
        protected int HScrollRange
        {
            set
            {
                if ((_scrollBars == ScrollBars.Horizontal)
                     || (_scrollBars == ScrollBars.Both))
                {
                    SetScrollRange(Handle, SB_HORZ, 0, value, true);
                }
            }
        }

        #endregion

        #region Internal routines

        //===========================================================
        // INTERNAL ROUTINES
        //===========================================================

        private static HorizontalAlignment A2A(StringAlignment a)
        {
            switch (a)
            {
                case StringAlignment.Near:
                    return HorizontalAlignment.Left;
                case StringAlignment.Far:
                    return HorizontalAlignment.Right;
            }
            return HorizontalAlignment.Center;
        }

        #endregion

        #region Win32 functions

        //===========================================================
        // WIN32 FUNCTIONS
        //===========================================================

        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;

        [DllImport("user32.dll", EntryPoint = "GetScrollPos")]
        private static extern int GetScrollPos
            (
                IntPtr hWnd,
                int nBar
            );

        [DllImport("user32.dll", EntryPoint = "SetScrollPos")]
        private static extern int SetScrollPos
            (
                IntPtr hWnd,
                int nBar,
                int nPos,
                bool bRedraw
            );

        [DllImport("user32.dll", EntryPoint = "SetScrollRange")]
        private static extern bool SetScrollRange
            (
                IntPtr hWnd,
                int nBar,
                int nMinPos,
                int nMaxPos,
                bool bRedraw
            );

        #endregion

        #region Component Designer generated code

        //===========================================================
        // DESIGNER GENERATED CODE
        //===========================================================

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                {
                    _components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _components = new System.ComponentModel.Container();
        }

        #endregion
    }
}
