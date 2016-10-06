/* StringGridColumn.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Описание одной колонки.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class StringGridColumn
    {
        /// <summary>
        /// Грид-владелец колонки.
        /// </summary>
        private readonly StringGrid _grid;

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringGridColumn
            (
                StringGrid grid
            )
        {
            _grid = grid;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringGridColumn
            (
                StringGrid grid,
                StringGridColumn column
            )
        {
            _grid = grid;
            _header = column._header;
            _width = column._width;
            _alignment = column._alignment;
            _readOnly = column._readOnly;
        }

        private string _header;

        /// <summary>
        /// Заголовок колонки.
        /// </summary>
        public string Header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
                if (_grid != null)
                {
                    _grid.Invalidate();
                }
            }
        }

        private const int DefaultWidth = 100;
        private int _width = DefaultWidth;

        /// <summary>
        /// Ширина колонки в пикселах.
        /// </summary>
        [DefaultValue(DefaultWidth)]
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                if (_grid != null)
                {
                    _grid.Invalidate();
                }
            }
        }

        private const StringAlignment DefaultAlignment
            = StringAlignment.Near;
        private StringAlignment _alignment = DefaultAlignment;

        /// <summary>
        /// Property Alignment (StringAlignment)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultAlignment)]
        public StringAlignment Alignment
        {
            get
            {
                return _alignment;
            }
            set
            {
                _alignment = value;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return _header;
        }

        private const bool DefaultReadOnly = false;
        private bool _readOnly = DefaultReadOnly;

        /// <summary>
        /// Property ReadOnly (bool)
        /// </summary>
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
            }
        }
    }
}
