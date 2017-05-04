// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TermLinkComparer.cs --
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

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TermLinkComparer
    {
        #region Nested classes

        /// <summary>
        /// Compares two <see cref="TermLink"/>s by MFN.
        /// </summary>
        public sealed class ByMfn
            : IEqualityComparer<TermLink>
        {
            #region IEqualityComparer members

            /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
            public bool Equals
                (
                    TermLink x,
                    TermLink y
                )
            {
                Code.NotNull(x, "x");
                Code.NotNull(y, "y");

                return x.Mfn == y.Mfn;
            }

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode
                (
                    TermLink obj
                )
            {
                Code.NotNull(obj, "obj");

                return obj.Mfn;
            }

            #endregion
        }

        /// <summary>
        /// Compares two <see cref="TermLink"/>s by MFN
        /// and field tag.
        /// </summary>
        public sealed class ByTag
            : IEqualityComparer<TermLink>
        {
            #region IEqualityComparer members

            /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
            public bool Equals
                (
                    TermLink x,
                    TermLink y
                )
            {
                Code.NotNull(x, "x");
                Code.NotNull(y, "y");

                return x.Mfn == y.Mfn && x.Tag == y.Tag;
            }

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode
                (
                    TermLink obj
                )
            {
                Code.NotNull(obj, "obj");

                return unchecked (obj.Mfn * 37 + obj.Tag);
            }

            #endregion
        }

        /// <summary>
        /// Compares two <see cref="TermLink"/>s by MFN,
        /// field tag and occurrence.
        /// </summary>
        public sealed class ByOccurrence
            : IEqualityComparer<TermLink>
        {
            #region IEqualityComparer members

            /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
            public bool Equals
                (
                    TermLink x,
                    TermLink y
                )
            {
                Code.NotNull(x, "x");
                Code.NotNull(y, "y");

                return x.Mfn == y.Mfn
                    && x.Tag == y.Tag
                    && x.Occurrence == y.Occurrence;
            }

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode
                (
                    TermLink obj
                )
            {
                Code.NotNull(obj, "obj");

                return unchecked ((obj.Mfn * 37 + obj.Tag) * 37
                    + obj.Occurrence);
            }

            #endregion
        }

        /// <summary>
        /// Compares two <see cref="TermLink"/>s by MFN,
        /// field tag, occurrence and index.
        /// </summary>
        public sealed class ByIndex
            : IEqualityComparer<TermLink>
        {
            #region IEqualityComparer members

            /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
            public bool Equals
                (
                    TermLink x,
                    TermLink y
                )
            {
                Code.NotNull(x, "x");
                Code.NotNull(y, "y");

                return x.Mfn == y.Mfn
                    && x.Tag == y.Tag
                    && x.Occurrence == y.Occurrence
                    && Math.Abs(x.Index - y.Index) == 1;
            }

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode
                (
                    TermLink obj
                )
            {
                Code.NotNull(obj, "obj");

                // obj.Index not forgotten!

                return unchecked ((obj.Mfn * 37 + obj.Tag) * 37
                    + obj.Occurrence);
            }

            #endregion
        }

        #endregion
    }
}
