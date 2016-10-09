/* CellChangedEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class CellChangedEventArgs
        : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NewValue { get; set; }
    }
}
