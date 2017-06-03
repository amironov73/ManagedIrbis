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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

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
                        x.Data,
                        y.Data
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
                        x.Data,
                        y.Data
                    );
            }

            #endregion
        }

        #endregion
    }
}
