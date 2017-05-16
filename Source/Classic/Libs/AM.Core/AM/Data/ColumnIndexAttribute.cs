// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ColumnIndexAttribute.cs --
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

namespace AM.Data
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnIndexAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Column index.
        /// </summary>
        public int Index { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ColumnIndexAttribute
            (
                int index
            )
        {
            Index = index;
        }

        #endregion
    }
}
