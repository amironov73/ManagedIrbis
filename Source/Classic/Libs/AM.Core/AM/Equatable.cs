// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Equatable.cs -- base class for implementing IEquatable<T>
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class Equatable<T>
        : IEquatable<T>
    {
        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals
            (
                T other
            )
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return GetHashCode() == other.GetHashCode();
        }

        #endregion

        #region Equality operators

        /// <summary>
        /// Determines whether the given <paramref name="left"/>
        /// is equal <paramref name="right"/>.
        /// </summary>
        public static bool operator ==
            (
                Equatable<T> left,
                Equatable<T> right
            )
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether the given <paramref name="left"/>
        /// is equal <paramref name="right"/>.
        /// </summary>
        public static bool operator !=
            (
                Equatable<T> left,
                Equatable<T> right
            )
        {
            return !Equals(left, right);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((T)obj);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public abstract override int GetHashCode();

        #endregion
    }
}
