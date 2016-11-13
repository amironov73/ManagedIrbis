/* SiberianRow.cs -- 
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
    public class SiberianRow
    {
        #region Constants

        /// <summary>
        /// Default height.
        /// </summary>
        public const int DefaultHeight = 20;

        #endregion

        #region Properties

        /// <summary>
        /// Index.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Data.
        /// </summary>
        [CanBeNull]
        public object Data { get; set; }

        /// <summary>
        /// Grid.
        /// </summary>
        [NotNull]
        public SiberianGrid Grid { get; internal set; }

        /// <summary>
        /// Cells.
        /// </summary>
        [NotNull]
        public NonNullCollection<SiberianCell> Cells { get; private set; }

        /// <summary>
        /// Height.
        /// </summary>
        [DefaultValue(DefaultHeight)]
        public int Height { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        internal SiberianRow()
        {
            Height = DefaultHeight;
            Cells = new NonNullCollection<SiberianCell>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

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
