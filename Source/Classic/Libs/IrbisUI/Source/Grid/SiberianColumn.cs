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
        /// Default width.
        /// </summary>
        public const int DefaultWidth = 200;

        #endregion

        #region Properties

        /// <summary>
        /// Grid.
        /// </summary>
        [NotNull]
        public SiberianGrid Grid { get; internal set; }

        /// <summary>
        /// Title.
        /// </summary>
        [CanBeNull]
        public string Title { get; set; }

        /// <summary>
        /// Width, pixels.
        /// </summary>
        [DefaultValue(DefaultWidth)]
        public int Width { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SiberianColumn()
        {
            Width = DefaultWidth;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Create cell.
        /// </summary>
        [NotNull]
        public abstract SiberianCell CreateCell();

        /// <summary>
        /// Draw the column.
        /// </summary>
        public virtual void Paint
            (
                PaintEventArgs args
            )
        {
        }

        #endregion

        #region Object members

        #endregion
    }
}
