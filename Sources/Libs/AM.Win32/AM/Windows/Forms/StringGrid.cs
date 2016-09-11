/* StringGrid.cs --  
   Ars Magna project, http://library.istu.edu/am */

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

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Простейшая таблица со строками.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    //[ToolboxBitmap(typeof(StringGrid), "Images.StringGrid.bmp")]
    [Serializable]
    public class StringGrid
        : Control
    {
        #region Delegates

        //===========================================================
        // DELEGATES
        //===========================================================

        /// <summary>
        /// 
        /// </summary>
        public class CellChangeEventArgs
            : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            public int Column;

            /// <summary>
            /// 
            /// </summary>
            public int Row;

            /// <summary>
            /// 
            /// </summary>
            public string OldValue;

            /// <summary>
            /// 
            /// </summary>
            public string NewValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public class CellClickEventArgs
            : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            public int Column;

            /// <summary>
            /// 
            /// </summary>
            public int Row;
        }

        /// <summary>
        /// 
        /// </summary>
        public class HeaderClickEventArgs
            : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            public int Column;
        }

        /// <summary>
        /// 
        /// </summary>
        public class CellDrawEventArgs
            : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            public int Column;

            /// <summary>
            /// 
            /// </summary>
            public int Row;

            /// <summary>
            /// 
            /// </summary>
            public string Value;

            /// <summary>
            /// 
            /// </summary>
            public RectangleF Rectangle;

            /// <summary>
            /// 
            /// </summary>
            public StringFormat Format;

            /// <summary>
            /// 
            /// </summary>
            public Graphics Graphics;

            /// <summary>
            /// 
            /// </summary>
            public bool Drawn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CellChangingHandler
            (
                StringGrid sender,
                CellChangeEventArgs e
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CellChangedHandler
            (
                StringGrid sender,
                CellChangeEventArgs e
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CellClickHandler
            (
                StringGrid sender,
                CellClickEventArgs e
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void HeaderClickHandler
            (
                StringGrid sender,
                HeaderClickEventArgs e
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CellDrawHandler
            (
                StringGrid sender, CellDrawEventArgs e
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        public delegate void ModifiedChangedHandler
            (
                StringGrid sender
            );

        #endregion

        #region Public events

        //===========================================================
        // PUBLIC EVENTS
        //===========================================================

        /// <summary>
        /// 
        /// </summary>
        public event CellChangingHandler CellChanging;

        /// <summary>
        /// 
        /// </summary>
        public event CellChangedHandler CellChanged;

        /// <summary>
        /// 
        /// </summary>
        public event CellClickHandler CellClick;

        /// <summary>
        /// 
        /// </summary>
        public event HeaderClickHandler HeaderClick;

        /// <summary>
        /// 
        /// </summary>
        public event CellDrawHandler CellDraw;

        /// <summary>
        /// 
        /// </summary>
        public event ModifiedChangedHandler ModifiedChanged;

        #endregion

        #region Helper classes

        //===========================================================
        // HELPER CLASSES
        //===========================================================

        /// <summary>
        /// Описание одной колонки.
        /// </summary>
        [Serializable]
        public class Column
        {
            /// <summary>
            /// Грид-владелец колонки.
            /// </summary>
            private StringGrid grid;

            /// <summary>
            /// Initializes a new instance of the <see cref="Column"/> class.
            /// </summary>
            /// <param name="grid">The grid.</param>
            public Column(StringGrid grid)
            {
                this.grid = grid;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Column"/> class.
            /// </summary>
            /// <param name="grid">The grid.</param>
            /// <param name="col">The col.</param>
            public Column(StringGrid grid, Column col)
            {
                this.grid = grid;
                header = col.header;
                width = col.width;
                alignment = col.alignment;
                readOnly = col.readOnly;
            }

            private string header = null;

            /// <summary>
            /// Заголовок колонки.
            /// </summary>
            [Browsable(true)]
            [DefaultValue(null)]
            public string Header
            {
                get
                {
                    return header;
                }
                set
                {
                    header = value;
                    if (grid != null)
                    {
                        grid.Invalidate();
                    }
                }
            }

            private const int DEFAULT_WIDTH = 100;
            private int width = DEFAULT_WIDTH;

            /// <summary>
            /// Ширина колонки в пикселах.
            /// </summary>
            [Browsable(true)]
            [DefaultValue(DEFAULT_WIDTH)]
            public int Width
            {
                get
                {
                    return width;
                }
                set
                {
                    width = value;
                    if (grid != null)
                    {
                        grid.Invalidate();
                    }
                }
            }

            private const StringAlignment DEFAULT_ALIGNMENT = StringAlignment.Near;
            private StringAlignment alignment = DEFAULT_ALIGNMENT;

            /// <summary>
            /// Property Alignment (StringAlignment)
            /// </summary>
            [Browsable(true)]
            [DefaultValue(DEFAULT_ALIGNMENT)]
            public StringAlignment Alignment
            {
                get
                {
                    return alignment;
                }
                set
                {
                    alignment = value;
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return header;
            }

            private const bool DEFAULT_READ_ONLY = false;
            private bool readOnly = DEFAULT_READ_ONLY;

            /// <summary>
            /// Property ReadOnly (bool)
            /// </summary>
            [Browsable(true)]
            [DefaultValue(DEFAULT_READ_ONLY)]
            public bool ReadOnly
            {
                get
                {
                    return readOnly;
                }
                set
                {
                    readOnly = value;
                }
            }
        }

        /// <summary>
        /// Collection of columns.
        /// </summary>
        /// <remarks>
        /// Doesn't work?
        /// </remarks>
        [Serializable]
        public class ColumnsCollection : CollectionBase
        {
            /// <summary>
            /// Ссылка на грид-владелец.
            /// </summary>
            private StringGrid grid;

            /// <summary>
            /// Initializes a new instance of the <see cref="ColumnsCollection"/> class.
            /// </summary>
            /// <param name="grid">The grid.</param>
            public ColumnsCollection(StringGrid grid)
            {
                this.grid = grid;
            }

            /// <summary>
            /// News the column.
            /// </summary>
            /// <returns></returns>
            public Column NewColumn()
            {
                return new Column(grid);
            }

            /// <summary>
            /// Gets or sets the <see cref="StringGrid.Column"/> at the specified index.
            /// </summary>
            /// <value></value>
            public Column this[int index]
            {
                get
                {
                    return (Column)List[index];
                }
                set
                {
                    List[index] = value;
                }
            }

            /// <summary>
            /// Adds the specified value.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public int Add(Column value)
            {
                return (List.Add(value));
            }

            /// <summary>
            /// Indexes the of.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public int IndexOf(Column value)
            {
                return (List.IndexOf(value));
            }

            /// <summary>
            /// Inserts the specified index.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <param name="value">The value.</param>
            public void Insert(int index, Column value)
            {
                List.Insert(index, value);
            }

            /// <summary>
            /// Removes the specified value.
            /// </summary>
            /// <param name="value">The value.</param>
            public void Remove(Column value)
            {
                List.Remove(value);
            }

            /// <summary>
            /// Determines whether [contains] [the specified value].
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>
            /// 	<c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
            /// </returns>
            public bool Contains(Column value)
            {
                return (List.Contains(value));
            }

#if NOTDEF

            protected override void OnInsert ( int index, object value )  
            {
                if ( value.GetType () != Type.GetType ( "Column" ) )
                    throw new ArgumentException ( "value must be of type Column.", "value" );
            }

            protected override void OnRemove ( int index, object value )  
            {
                if ( value.GetType () != Type.GetType ( "Column" ) )
                    throw new ArgumentException ( "value must be of type Column", "value" );
            }

            protected override void OnSet ( int index, object oldValue, object newValue )  
            {
                if ( newValue.GetType () != Type.GetType ( "Column" ) )
                    throw new ArgumentException ( "newValue must be of type Column", "newValue" );
            }

            protected override void OnValidate ( object value )  
            {
                if ( value.GetType () != Type.GetType ( "Column" ) )
                    throw new ArgumentException ( "value must be of type Column", "value" );
            }
#endif
        }

        #endregion

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container _components = null;

        #region Public properties

        //===========================================================
        // PUBLIC PROPERTIES
        //===========================================================

        private static readonly Color DEFAULT_GRID_COLOR = Color.DarkGray;
        private Color gridColor = DEFAULT_GRID_COLOR;

        /// <summary>
        /// Цвет линий.
        /// </summary>
        [Browsable(true)]
        public Color GridColor
        {
            get
            {
                return gridColor;
            }
            set
            {
                gridColor = value;
                Invalidate();
            }
        }

        private static readonly Color DEFAULT_CURRENT_CELL_COLOR = Color.White;
        private Color currentCellColor = DEFAULT_CURRENT_CELL_COLOR;

        /// <summary>
        /// Property CurrentCellColor (Color)
        /// </summary>
        [Browsable(true)]
        public Color CurrentCellColor
        {
            get
            {
                return currentCellColor;
            }
            set
            {
                currentCellColor = value;
                Invalidate();
            }
        }

        private static readonly Color DEFAULT_HEADER_BACK_COLOR = Color.Blue;
        private Color headerBackColor = DEFAULT_HEADER_BACK_COLOR;

        /// <summary>
        /// Property HeaderBackColor (Color)
        /// </summary>
        [Browsable(true)]
        public Color HeaderBackColor
        {
            get
            {
                return headerBackColor;
            }
            set
            {
                headerBackColor = value;
                Invalidate();
            }
        }

        private static readonly Color DEFAULT_HEADER_FORE_COLOR = Color.White;
        private Color headerForeColor = DEFAULT_HEADER_FORE_COLOR;

        /// <summary>
        /// Property HeaderForeColor (Color)
        /// </summary>
        [Browsable(true)]
        public Color HeaderForeColor
        {
            get
            {
                return headerForeColor;
            }
            set
            {
                headerForeColor = value;
                Invalidate();
            }
        }

        private const bool DEFAULT_SHOW_GRID_LINES = true;
        private bool showGridLines = DEFAULT_SHOW_GRID_LINES;

        /// <summary>
        /// Property ShowGridLines (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_SHOW_GRID_LINES)]
        public bool ShowGrid
        {
            get
            {
                return showGridLines;
            }
            set
            {
                showGridLines = value;
                Invalidate();
            }
        }

        private const int DEFAULT_COLUMN_COUNT = 2;
        private int columnCount = DEFAULT_COLUMN_COUNT;

        /// <summary>
        /// Property ColumnCount (int)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_COLUMN_COUNT)]
        public int ColumnCount
        {
            get
            {
                return columnCount;
            }
            set
            {
                Debug.Assert(value > 0, "ColumnCount must be > 0");
                CreateCells(value, rowCount, true);
            }
        }

        private const int DEFAULT_ROW_COUNT = 3;
        private int rowCount = DEFAULT_ROW_COUNT;

        /// <summary>
        /// Property RowCount (int)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_ROW_COUNT)]
        public int RowCount
        {
            get
            {
                return rowCount;
            }
            set
            {
                Debug.Assert(value > 0, "ColumnCount must be > 0");
                CreateCells(columnCount, value, true);
            }
        }

        private ColumnsCollection columns;

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        [Browsable(true)]
        public ColumnsCollection Columns
        {
            get
            {
                return columns;
            }
            set
            {
                Debug.Assert(value.Count == columnCount);
                columns = value;
                Invalidate();
            }
        }

        private const bool DEFAULT_SHOW_COLUMN_HEADER = true;
        private bool showColumnHeader = DEFAULT_SHOW_COLUMN_HEADER;

        /// <summary>
        /// Property ShowColumnHeader (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_SHOW_COLUMN_HEADER)]
        public bool ShowColumnHeader
        {
            get
            {
                return showColumnHeader;
            }
            set
            {
                showColumnHeader = value;
                Invalidate();
            }
        }

        private const bool DEFAULT_ALLOW_COLUMN_RESIZE = true;
        private bool allowColumnResize = DEFAULT_ALLOW_COLUMN_RESIZE;

        /// <summary>
        /// Property AllowColumnResize (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_ALLOW_COLUMN_RESIZE)]
        public bool AllowColumnResize
        {
            get
            {
                return allowColumnResize;
            }
            set
            {
                allowColumnResize = value;
            }
        }

        private string[,] cells = null;

        /// <summary>
        /// Property Cells (string[])
        /// </summary>
        public string this[int col, int row]
        {
            get
            {
                return cells[col, row];
            }
            set
            {
                CellChangeEventArgs e = new CellChangeEventArgs();
                e.Column = col;
                e.Row = row;
                e.OldValue = cells[col, row];
                e.NewValue = value;

                if (CellChanging != null)
                {
                    CellChanging(this, e);
                }

                cells[col, row] = value;

                if (autoColumnSize)
                {
                    AutoSizeColumn(col);
                }

                if (CellChanged != null)
                {
                    CellChanged(this, e);
                }

                Invalidate();
            }
        }

        private const int DEFAULT_CURRENT_COLUMN = 0;
        private int currentColumn = DEFAULT_CURRENT_COLUMN;

        /// <summary>
        /// Property CurrentColumn (int)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DEFAULT_CURRENT_COLUMN)]
        public int CurrentColumn
        {
            get
            {
                return currentColumn;
            }
            set
            {
                MoveTo(value, currentRow);
            }
        }

        private const int DEFAULT_CURRENT_ROW = 0;
        private int currentRow = DEFAULT_CURRENT_ROW;

        /// <summary>
        /// Property CurrentRow (int)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DEFAULT_CURRENT_ROW)]
        public int CurrentRow
        {
            get
            {
                return currentRow;
            }
            set
            {
                MoveTo(currentColumn, value);
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
                return this[currentColumn, currentRow];
            }
            set
            {
                this[currentColumn, currentRow] = value;
            }
        }

        private const int DEFAULT_TOP_ROW = 0;
        private int topRow = DEFAULT_TOP_ROW;

        /// <summary>
        /// Property TopRow (int)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DEFAULT_TOP_ROW)]
        public int TopRow
        {
            get
            {
                return topRow;
            }
        }

        private const int DEFAULT_LEFT_COLUMN = 0;
        private int leftColumn = DEFAULT_LEFT_COLUMN;

        /// <summary>
        /// Property LeftColumn (int)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DEFAULT_LEFT_COLUMN)]
        public int LeftColumn
        {
            get
            {
                return leftColumn;
            }
        }

        private const bool DEFAULT_READ_ONLY = false;
        private bool readOnly = DEFAULT_READ_ONLY;

        /// <summary>
        /// Property ReadOnly (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_READ_ONLY)]
        public bool ReadOnly
        {
            get
            {
                return readOnly;
            }
            set
            {
                readOnly = value;
                AcceptText();
            }
        }

        private const bool DEFAULT_ALLOW_APPEND = true;
        private bool allowAppend = DEFAULT_ALLOW_APPEND;

        /// <summary>
        /// Property AllowAppend (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_ALLOW_APPEND)]
        public bool AllowAppend
        {
            get
            {
                return allowAppend;
            }
            set
            {
                allowAppend = value;
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
                    vr2 = rowCount - topRow;
                if (showColumnHeader)
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
                for (col = leftColumn; col < columnCount; col++)
                {
                    width += columns[col].Width;
                    if (width >= Width)
                    {
                        break;
                    }
                }
                return (col - leftColumn);
            }
        }

        private const string DEFAULT_DELIMITER = ";";
        private string delimiter = DEFAULT_DELIMITER;

        /// <summary>
        /// Property Delimiter (string)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_DELIMITER)]
        public string Delimiter
        {
            get
            {
                return delimiter;
            }
            set
            {
                delimiter = value;
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
                string[] tmp = new string[columnCount];
                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < columnCount; col++)
                    {
                        tmp[col] = cells[col, row];
                    }
                    sb.Append(string.Join(delimiter, tmp));
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
            set
            {
                StringReader sr = new StringReader(value);
                string line;
                char[] sep = delimiter.ToCharArray();
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
                    if (nc > columnCount)
                    {
                        nc = columnCount;
                    }
                    for (int col = 0; col < nc; col++)
                    {
                        cells[col, row] = tmp[col];
                    }
                }
                AutoSizeColumns();
                Invalidate();
            }
        }

        private const bool DEFAULT_IS_MODIFIED = false;
        private bool isModified = DEFAULT_IS_MODIFIED;

        /// <summary>
        /// Property IsModified (bool)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DEFAULT_IS_MODIFIED)]
        public bool IsModified
        {
            get
            {
                return isModified;
            }
            set
            {
                if ((value != isModified)
                     && (ModifiedChanged != null))
                {
                    isModified = value;
                    ModifiedChanged(this);
                }
                else
                {
                    isModified = value;
                }
            }
        }

        private const ScrollBars DEFAULT_SCROLLBARS = ScrollBars.Both;
        private ScrollBars scrollBars = DEFAULT_SCROLLBARS;

        /// <summary>
        /// Property ScrollBars (ScrollBars)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_SCROLLBARS)]
        public ScrollBars ScrollBars
        {
            get
            {
                return scrollBars;
            }
            set
            {
                scrollBars = value;
                if (Created)
                {
                    RecreateHandle();
                }
            }
        }

        private const bool DEFAULT_FULL_WIDTH = true;
        private bool fullWidth = DEFAULT_FULL_WIDTH;

        /// <summary>
        /// Property FullWidth (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_FULL_WIDTH)]
        public bool FullWidth
        {
            get
            {
                return fullWidth;
            }
            set
            {
                fullWidth = value;
                Invalidate();
            }
        }

        private const bool DEFAULT_AUTO_COLUMN_SIZE = false;
        private bool autoColumnSize = DEFAULT_AUTO_COLUMN_SIZE;

        /// <summary>
        /// Property AutoColumnSize (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_AUTO_COLUMN_SIZE)]
        public bool AutoColumnSize
        {
            get
            {
                return autoColumnSize;
            }
            set
            {
                autoColumnSize = value;
                AutoSizeColumns();
            }
        }

        private const int DEFAULT_MINIMAL_COLUMN_WIDTH = 50;
        private int minimalColumnWidth = DEFAULT_MINIMAL_COLUMN_WIDTH;

        /// <summary>
        /// Property MinimalColumnWidth (int)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_MINIMAL_COLUMN_WIDTH)]
        public int MinimalColumnWidth
        {
            get
            {
                return minimalColumnWidth;
            }
            set
            {
                minimalColumnWidth = value;
                AutoSizeColumns();
            }
        }

        #endregion

        #region Constructor

        //===========================================================
        // CONSTRUCTOR
        //===========================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="StringGrid"/> class.
        /// </summary>
        public StringGrid()
            : this(DEFAULT_COLUMN_COUNT, DEFAULT_ROW_COUNT)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringGrid"/> class.
        /// </summary>
        /// <param name="ncols">The ncols.</param>
        /// <param name="nrows">The nrows.</param>
        public StringGrid(int ncols, int nrows)
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);
            SetStyle(ControlStyles.UserMouse, true);

            CreateCells(ncols, nrows, false);
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
            if (!readOnly
                 && !columns[currentColumn].ReadOnly)
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
            if (!readOnly
                 && !columns[currentColumn].ReadOnly)
            {
                IDataObject obj = Clipboard.GetDataObject();
                if (obj.GetDataPresent(DataFormats.Text))
                {
                    CurrentCell = (string)(obj.GetData(DataFormats.Text));
                }
            }
        }

        /// <summary>
        /// Autoes the size column.
        /// </summary>
        /// <param name="col">The col.</param>
        public void AutoSizeColumn(int col)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF r = g.MeasureString(columns[col].Header, Font);
                int width = (int)r.Width;
                for (int row = 0; row < rowCount; row++)
                {
                    r = g.MeasureString(cells[col, row], Font);
                    int cur = (int)r.Width;
                    if (cur > width)
                    {
                        width = cur;
                    }
                }
                if (width < minimalColumnWidth)
                {
                    width = minimalColumnWidth;
                }
                columns[col].Width = width;
            }
        }

        #endregion

        #region Worker routines

        //===========================================================
        // WORKER ROUTINES
        //===========================================================

        /// <summary>
        /// Autoes the size columns.
        /// </summary>
        public void AutoSizeColumns()
        {
            if (autoColumnSize)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    AutoSizeColumn(col);
                }
            }
        }

        protected void CreateCells(int ncols, int nrows, bool copyData)
        {
            string[,] oldCells = cells;
            ColumnsCollection oldColumns = columns;

            int maxRow = nrows;
            if (maxRow > rowCount)
            {
                maxRow = rowCount;
            }
            int maxCol = ncols;
            if (maxCol > columnCount)
            {
                maxCol = columnCount;
            }
            cells = new string[ncols, nrows];
            if (copyData && (oldCells != null))
            {
                for (int col = 0; col < maxCol; col++)
                {
                    for (int row = 0; row < maxRow; row++)
                    {
                        cells[col, row] = oldCells[col, row];
                    }
                }
            }

            columns = new ColumnsCollection(this);
            if (oldColumns != null)
            {
                for (int col = 0;
                      (col < maxCol) && (col < oldColumns.Count);
                      col++)
                {
                    columns.Add(oldColumns[col]);
                }
            }
            while (columns.Count < ncols)
            {
                columns.Add(columns.NewColumn());
            }

            columnCount = ncols;
            HScrollRange = ncols;
            rowCount = nrows;
            VScrollRange = nrows;
        }

        protected void MoveTo(int col, int row)
        {
            AcceptText();

            if (col < 0)
            {
                col = 0;
            }
            if (row < 0)
            {
                row = 0;
            }
            if (col >= columnCount)
            {
                col = columnCount - 1;
            }
            if (row >= rowCount)
            {
                row = rowCount - 1;
            }

            int vr = VisibleRows;
            if (row < topRow)
            {
                topRow = row;
            }
            if (row >= (topRow + vr))
            {
                topRow = row - vr + 1;
                if (topRow < 0)
                {
                    topRow = 0;
                }
            }

            if (col < leftColumn)
            {
                leftColumn = col;
            }
            int vc = VisibleColumns;
            if (col >= (leftColumn + vc))
            {
                leftColumn = col - vc + 1;
                if (leftColumn < 0)
                {
                    leftColumn = 0;
                }
            }

            currentColumn = col;
            HScrollPos = col;
            currentRow = row;
            VScrollPos = row;
            Invalidate();
        }

        protected void MoveOneLineUp()
        {
            MoveTo(currentColumn, currentRow - 1);
        }

        protected void MoveOneLineDown()
        {
            if (currentRow == (rowCount - 1))
            {
                AppendRow();
            }
            MoveTo(currentColumn, currentRow + 1);
        }

        protected void MoveOnePageDown()
        {
            if (currentRow == (rowCount - 1))
            {
                AppendRow();
            }
            MoveTo(currentColumn, currentRow + VisibleRows);
        }

        protected void MoveOnePageUp()
        {
            MoveTo(currentColumn, currentRow - VisibleRows);
        }

        protected void MoveOneColumnLeft()
        {
            MoveTo(currentColumn - 1, currentRow);
        }

        protected void MoveOneColumnRight()
        {
            MoveTo(currentColumn + 1, currentRow);
        }

        protected void MoveToFirstColumn()
        {
            MoveTo(0, currentRow);
        }

        protected void MoveToLastColumn()
        {
            MoveTo(columnCount - 1, currentRow);
        }

        private TextBox textBox = null;

        /// <summary>
        /// Accepts the text.
        /// </summary>
        public void AcceptText()
        {
            if (textBox != null)
            {
                if (CurrentCell != textBox.Text)
                {
                    IsModified = true;
                }
                CurrentCell = textBox.Text;
                textBox.Dispose();
                textBox = null;
            }
        }

        protected void DiscardText()
        {
            if (textBox != null)
            {
                textBox.Dispose();
                textBox = null;
            }
        }

        protected Rectangle GetCellRectangle(int col, int row)
        {
            Rectangle r = new Rectangle();

            r.Y = (row - topRow) * FontHeight;
            if (showColumnHeader)
            {
                r.Y += FontHeight;
            }
            for (int i = leftColumn; i < col; i++)
            {
                r.X += columns[i].Width;
            }
            r.Height = FontHeight;
            r.Width = columns[col].Width;
            return r;
        }

        protected void CreateTextBox(string txt)
        {
            AcceptText();
            if (readOnly)
            {
                return;
            }

            textBox = new TextBox();
            Rectangle r = GetCellRectangle(currentColumn, currentRow);
            r.Inflate(-2, 0);
            textBox.Location = r.Location;
            textBox.Size = r.Size;
            textBox.Font = Font;
            textBox.Visible = true;
            textBox.BorderStyle = BorderStyle.None;
            textBox.TextAlign = A2A(columns[currentColumn].Alignment);
            Controls.Add(textBox);
            textBox.Focus();
            textBox.KeyDown += new KeyEventHandler(tb_KeyDown);
            if (txt != null)
            {
                textBox.AppendText(txt);
            }
        }

        protected void CreateTextBox()
        {
            CreateTextBox(null);
        }

        protected void AppendRow()
        {
            if (!allowAppend)
            {
                return;
            }
            CreateCells(columnCount, rowCount + 1, true);
            IsModified = true;
            Invalidate();
        }

        protected void AppendColumn()
        {
            if (!allowAppend)
            {
                return;
            }
            CreateCells(columnCount + 1, rowCount, true);
            IsModified = true;
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

        protected int OverColumnDelimiter(MouseEventArgs e)
        {
            if (allowColumnResize)
            {
                for (int col = leftColumn,
                          x = 0;
                      col < columnCount;
                      col++)
                {
                    x += columns[col].Width;
                    if (x == e.X)
                    {
                        return col;
                    }
                }
            }
            return -1;
        }

        protected Point OverCell(MouseEventArgs e)
        {
            Point p = new Point(-1, -1);

            p.Y = e.Y / FontHeight + topRow;
            if (showColumnHeader)
            {
                p.Y--;
            }
            int col,
                x;
            for (col = leftColumn, x = 0; col < columnCount; col++)
            {
                x += columns[col].Width;
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

        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor); // ???

            int height = FontHeight,
                width = ClientSize.Width;
            int delta = 0;
            CellDrawEventArgs ea = new CellDrawEventArgs();
            Brush foreBrush = new SolidBrush(ForeColor);
            Brush currCellBrush = new SolidBrush(currentCellColor);
            Pen gridPen = new Pen(gridColor);
            int col,
                row,
                x,
                y;
            StringFormat sf = new StringFormat();
            RectangleF r = new RectangleF();
            int endRow = topRow + VisibleRows;
            int endCol = leftColumn + VisibleColumns;
            if (endCol < columnCount)
            {
                endCol++;
            }

            // Вычисляем ширину перерисовываемой области
            if (!fullWidth)
            {
                for (col = leftColumn, width = 0; col < endCol; col++)
                {
                    width += columns[col].Width;
                }
            }

            // Рисуем заголовки колонок
            if (showColumnHeader)
            {
                Brush hdrBack = new SolidBrush(headerBackColor);
                Brush hdrFore = new SolidBrush(headerForeColor);
                Font hdrFont = new Font(Font, FontStyle.Bold);

                g.FillRectangle(hdrBack, 0, 0, width, height);
                r.X = 0;
                r.Y = 0;
                r.Height = height;
                for (col = leftColumn; col < endCol; col++)
                {
                    r.Width = columns[col].Width;
                    sf.Alignment = columns[col].Alignment;
                    g.DrawString(columns[col].Header, hdrFont, hdrFore, r, sf);
                    r.X += r.Width;
                }
                delta += height;
            }

            // Рисуем грид
            for (row = topRow, y = height + delta; row < endRow; row++)
            {
                if (showGridLines)
                {
                    g.DrawLine(gridPen, 0, y, width, y);
                }
                y += height;
            }
            for (col = leftColumn, x = 0, y -= height; col < endCol; col++)
            {
                x += columns[col].Width;
                if (showGridLines)
                {
                    g.DrawLine(gridPen, x, 0, x, y);
                }
            }

            // Рисуем собственно данные
            for (row = topRow, r.Y = delta, r.Height = height; row < endRow; row++)
            {
                r.X = 0;
                for (col = leftColumn; col < endCol; col++)
                {
                    r.Width = columns[col].Width;
                    sf.Alignment = columns[col].Alignment;
                    // Обработка текущей ячейки
                    if ((row == currentRow)
                         && (col == currentColumn))
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
                        ea.Value = cells[col, row];
                        ea.Graphics = g;
                        CellDraw(this, ea);
                    }
                    if (ea.Drawn == false)
                    {
                        g.DrawString(cells[col, row], Font, foreBrush, r, sf);
                    }
                    r.X += r.Width;
                }
                r.Y += height;
            }

            base.OnPaint(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
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
                        if (!columns[currentColumn].ReadOnly)
                        {
                            CreateTextBox(CurrentCell);
                        }
                        break;
                    case Keys.Delete:
                        if (!readOnly
                             && !columns[currentColumn].ReadOnly)
                        {
                            CurrentCell = null;
                            IsModified = true;
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

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (readOnly || columns[currentColumn].ReadOnly)
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

        protected override void OnMouseDown(MouseEventArgs e)
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
                    for (int i = leftColumn; i < col; i++)
                    {
                        columnPosition += columns[i].Width;
                    }
                    inColumnResize = true;
                    return;
                }

                Point where = OverCell(e);
                if (where.Y < 0)
                {
                    if (HeaderClick != null)
                    {
                        HeaderClickEventArgs ea = new HeaderClickEventArgs();
                        ea.Column = where.X;
                        HeaderClick(this, ea);
                    }
                }
                else
                {
                    if (CellClick != null)
                    {
                        CellClickEventArgs ea = new CellClickEventArgs();
                        ea.Column = where.X;
                        ea.Row = where.Y;
                        CellClick(this, ea);
                    }
                    MoveTo(where.X, where.Y);
                    if (!columns[currentColumn].ReadOnly
                         && (e.Clicks > 1))
                    {
                        CreateTextBox(CurrentCell);
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (inColumnResize)
            {
                if (e.X
                     > (columnPosition + minimalColumnWidth))
                {
                    columns[resizingColumn].Width = e.X - columnPosition;
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

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (inColumnResize)
            {
                inColumnResize = false;
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            int delta = e.Delta / 120;
            MoveTo(currentColumn, currentRow - delta);
        }

        private const int WM_HSCROLL = 0x0114;
        private const int WM_VSCROLL = 0x0115;

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
                            MoveTo(currentColumn - VisibleColumns, currentRow);
                            break;
                        case SB_PAGERIGHT:
                            MoveTo(currentColumn + VisibleColumns, currentRow);
                            break;
                        case SB_THUMBPOSITION:
                            MoveTo(((int)m.WParam) >> 16, currentRow);
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
                            MoveTo(currentColumn, rowCount - 1);
                            break;
                        case SB_TOP:
                            MoveTo(currentColumn, 0);
                            break;
                        case SB_THUMBPOSITION:
                            MoveTo(currentColumn, ((int)m.WParam) >> 16);
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
        //private const int SB_ENDSCROLL = 8;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams par = base.CreateParams;

                switch (scrollBars)
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

        protected int VScrollPos
        {
            get
            {
                if ((scrollBars == ScrollBars.Vertical)
                     || (scrollBars == ScrollBars.Both))
                {
                    return GetScrollPos(Handle, SB_VERT);
                }
                return 0;
            }
            set
            {
                if ((scrollBars == ScrollBars.Vertical)
                     || (scrollBars == ScrollBars.Both))
                {
                    SetScrollPos(Handle, SB_VERT, value, true);
                }
            }
        }

        protected int HScrollPos
        {
            get
            {
                if ((scrollBars == ScrollBars.Horizontal)
                     || (scrollBars == ScrollBars.Both))
                {
                    return GetScrollPos(Handle, SB_HORZ);
                }
                return 0;
            }
            set
            {
                if ((scrollBars == ScrollBars.Horizontal)
                     || (scrollBars == ScrollBars.Both))
                {
                    SetScrollPos(Handle, SB_HORZ, value, true);
                }
            }
        }

        protected int VScrollRange
        {
            set
            {
                if ((scrollBars == ScrollBars.Vertical)
                     || (scrollBars == ScrollBars.Both))
                {
                    SetScrollRange(Handle, SB_VERT, 0, value, true);
                }
            }
        }

        protected int HScrollRange
        {
            set
            {
                if ((scrollBars == ScrollBars.Horizontal)
                     || (scrollBars == ScrollBars.Both))
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