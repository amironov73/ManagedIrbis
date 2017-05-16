// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SortIndexAttribute.cs --
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
    public sealed class SortIndexAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Sort index.
        /// </summary>
        public int Index { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="index"></param>
        public SortIndexAttribute
            (
                int index
            )
        {
            Index = index;
        }

        #endregion
    }
}
