/* StringGridColumnCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Collection of columns.
    /// </summary>
    /// <remarks>
    /// Doesn't work?
    /// </remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public class ColumnsCollection
        : CollectionBase
    {
        /// <summary>
        /// Ссылка на грид-владелец.
        /// </summary>
        private readonly StringGrid _grid;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ColumnsCollection
            (
                StringGrid grid
            )
        {
            _grid = grid;
        }

        /// <summary>
        /// Creates new column.
        /// </summary>
        public StringGridColumn NewColumn()
        {
            return new StringGridColumn(_grid);
        }

        /// <summary>
        /// Gets or sets the <see cref="StringGridColumn"/>
        /// at the specified index.
        /// </summary>
        public StringGridColumn this[int index]
        {
            get
            {
                return (StringGridColumn)List[index];
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        public int Add(StringGridColumn value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int IndexOf(StringGridColumn value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Insert(int index, StringGridColumn value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Remove(StringGridColumn value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Determines whether the collection contains
        /// specified value.
        /// </summary>
        public bool Contains(StringGridColumn value)
        {
            return List.Contains(value);
        }
    }
}
