// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordComparer.cs -- 
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
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class RecordComparer
    {
        #region Nested classes

        sealed class ByMfnComparer
            : IComparer<MarcRecord>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    MarcRecord x,
                    MarcRecord y
                )
            {
                return x.Mfn - y.Mfn;
            }
        }

        sealed class ByIndexComparer
            : IComparer<MarcRecord>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    MarcRecord x,
                    MarcRecord y
                )
            {
                return string.CompareOrdinal
                    (
                        x.Index,
                        y.Index
                    );
            }
        }

        sealed class ByDescriptionComparer
            : IComparer<MarcRecord>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    MarcRecord x,
                    MarcRecord y
                )
            {
                return string.CompareOrdinal
                    (
                        x.Description,
                        y.Description
                    );
            }
        }

        sealed class BySortKeyComparer
            : IComparer<MarcRecord>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare(MarcRecord x, MarcRecord y)
            {
                return string.CompareOrdinal
                    (
                        x.SortKey,
                        y.SortKey
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare <see cref="MarcRecord"/> by MFN.
        /// </summary>
        [NotNull]
        public static IComparer<MarcRecord> ByMfn()
        {
            return new ByMfnComparer();
        }

        /// <summary>
        /// Compare <see cref="MarcRecord"/> by index.
        /// </summary>
        [NotNull]
        public static IComparer<MarcRecord> ByIndex()
        {
            return new ByIndexComparer();
        }

        /// <summary>
        /// Compare <see cref="MarcRecord"/> by descrption.
        /// </summary>
        [NotNull]
        public static IComparer<MarcRecord> ByDescription()
        {
            return new ByDescriptionComparer();
        }

        /// <summary>
        /// Compare <see cref="MarcRecord"/> by sort key.
        /// </summary>
        [NotNull]
        public static IComparer<MarcRecord> BySortKey()
        {
            return new BySortKeyComparer();
        }

        #endregion
    }
}
