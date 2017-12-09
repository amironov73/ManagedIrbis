// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordStateComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RecordStateComparer
    {
        #region Nested classes

        /// <summary>
        /// Compares <see cref="RecordState"/>
        /// by <see cref="RecordState.Id"/>
        /// </summary>
        public sealed class ById
            : IEqualityComparer<RecordState>
        {
            #region IEqualityComparer<T> members

            /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
            public bool Equals
                (
                    RecordState x,
                    RecordState y
                )
            {
                return x.Id == y.Id;
            }

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode
                (
                    RecordState obj
                )
            {
                return obj.Id;
            }

            #endregion
        }

        /// <summary>
        /// Compares <see cref="RecordState"/>
        /// by <see cref="RecordState.Mfn"/>
        /// </summary>
        public sealed class ByMfn
            : IEqualityComparer<RecordState>
        {
            #region IEqualityComparer<T> members

            /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
            public bool Equals
                (
                    RecordState x,
                    RecordState y
                )
            {
                return x.Mfn == y.Mfn;
            }

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode
                (
                    RecordState obj
                )
            {
                return obj.Mfn;
            }

            #endregion
        }

        /// <summary>
        /// Compares <see cref="RecordState"/>
        /// by <see cref="RecordState.Mfn"/> and
        /// <see cref="RecordState.Version"/>.
        /// </summary>
        public sealed class ByVersion
            : IEqualityComparer<RecordState>
        {
            #region IEqualityComparer<T> members

            /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
            public bool Equals
                (
                    RecordState x,
                    RecordState y
                )
            {
                return x.Mfn == y.Mfn
                    && x.Version == y.Version;
            }

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode
                (
                    RecordState obj
                )
            {
                return unchecked (obj.Mfn * 37 + obj.Version);
            }

            #endregion
        }

        #endregion
    }
}
