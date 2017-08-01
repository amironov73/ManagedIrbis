// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TermComparer.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class TermComparer
    {
        #region Nested classes

        /// <summary>
        /// Numeric comparer.
        /// </summary>
        public sealed class Numeric
            : IComparer<BiblioTerm>
        {
            #region IComparer<T> members

            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    BiblioTerm x,
                    BiblioTerm y
                )
            {
                return NumberText.Compare
                    (
                        x.ThrowIfNull().Data,
                        y.ThrowIfNull().Data
                    );
            }

            #endregion
        }


        /// <summary>
        /// Trivial comparer.
        /// </summary>
        public sealed class Trivial
            : IComparer<BiblioTerm>
        {
            #region IComparer<T> members

            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    BiblioTerm x,
                    BiblioTerm y
                )
            {
                return StringComparer.CurrentCulture.Compare
                    (
                        x.ThrowIfNull().Data,
                        y.ThrowIfNull().Data
                    );
            }

            #endregion
        }

        #endregion
    }
}
