/* CellDrawEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class CellDrawEventArgs
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
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RectangleF Rectangle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StringFormat Format { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Drawn { get; set; }
    }
}
