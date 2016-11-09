/* SiberianCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
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

    public class SiberianCell
    {
        #region Properties

        /// <summary>
        /// Column.
        /// </summary>
        [NotNull]
        public SiberianColumn Column { get; internal set; }

        /// <summary>
        /// Grid.
        /// </summary>
        [NotNull]
        public SiberianGrid Grid { get { return Row.Grid; } }

        /// <summary>
        /// Row.
        /// </summary>
        [NotNull]
        public SiberianRow Row { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SiberianCell()
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Draw the cell.
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
