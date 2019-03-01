// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedIrbis.Magazines
{
    /// <summary>
    ///
    /// </summary>
    public static class MagazineComparer
    {
        /// <summary>
        ///
        /// </summary>
        public class ByTitle : IComparer<MagazineInfo>
        {
            /// <inheritdoc cref="IComparer{T}.Compare"/>
            public int Compare(MagazineInfo x, MagazineInfo y)
            {
                return string.Compare(x.Title, y.Title, StringComparison.CurrentCulture);
            }
        }
    }
}
