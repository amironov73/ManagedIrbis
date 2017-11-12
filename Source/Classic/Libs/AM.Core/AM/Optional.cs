// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Optional.cs -- 
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
    /// <remarks>
    /// Borrowed from StackOverflow:
    /// http://stackoverflow.com/questions/16199227/optional-return-in-c-net
    /// </remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public struct Optional<T>
        : IEquatable<Optional<T>>
    {
        #region Properties

        /// <summary>
        /// Has value?
        /// </summary>
        public readonly bool HasValue;

        /// <summary>
        /// Value itself.
        /// </summary>
        [CanBeNull]
        public T Value
        {
            get
            {
                if (HasValue)
                {
                    return _value;
                }

                throw new InvalidOperationException();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Optional
            (
                [CanBeNull] T value
            )
        {
            _value = value;
            HasValue = true;
        }

        #endregion

        #region Private members

        private readonly T _value;

        #endregion

        #region Public methods

        /// <summary>
        /// Convert Optional to value.
        /// </summary>
        public static explicit operator T
            (
                Optional<T> optional
            )
        {
            return optional.Value;
        }

        /// <summary>
        /// Convert value to Optional.
        /// </summary>
        public static implicit operator Optional<T>
            (
                [CanBeNull] T value
            )
        {
            return new Optional<T>(value);
        }

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals
            (
                Optional<T> other
            )
        {
            if (HasValue && other.HasValue)
            {
                return Equals(_value, other._value);
            }

            return HasValue == other.HasValue;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (obj is Optional<T>)
            {
                return Equals((Optional<T>)obj);
            }

            return false;
        }

        /// <inheritdoc cref="ValueType.GetHashCode" />
        public override int GetHashCode()
        {
            if (!HasValue)
            {
                return 0;
            }

            if (ReferenceEquals(_value, null))
            {
                return -1;
            }

            return _value.GetHashCode();
        }

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            if (!HasValue)
            {
                return "(not set)";
            }

            if (ReferenceEquals(Value, null))
            {
                return "(null)";
            }

            return Value.ToString();
        }

        #endregion
    }
}
