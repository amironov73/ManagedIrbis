/* SiberianGrid.cs -- 
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
    public class SiberianGrid
        : Control
    {
        #region Properties

        /// <summary>
        /// Columns.
        /// </summary>
        [NotNull]
        public NonNullCollection<SiberianColumn> Columns { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianGrid()
        {
            Columns = new NonNullCollection<SiberianColumn>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Create column of specified type.
        /// </summary>
        [NotNull]
        public SiberianColumn CreateColumn<T>()
            where T: SiberianColumn, new()
        {
            T result = new T
            {
                Grid = this
            };

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
