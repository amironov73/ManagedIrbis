// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SingleOrArray.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using UnsafeCode;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

#endregion

namespace UnsafeAM.Json
{
    /// <summary>
    /// Хранит одно значение либо целый массив.
    /// </summary>
    [PublicAPI]
    public sealed class SingleOrArray<T>
        : IEnumerable<T>
    {
        #region Properties

        /// <summary>
        /// Whether the instance is empty?
        /// </summary>
        public bool IsEmpty
        {
            get { return _isEmpty; }
        }

        /// <summary>
        /// Whether the instance holds single value?
        /// </summary>
        public bool IsSingle
        {
            get { return Values.Length <= 1; }
        }

        /// <summary>
        /// Single value.
        /// </summary>
        [CanBeNull]
        public T Value { get; private set; }

        /// <summary>
        /// Array of values.
        /// </summary>
        [NotNull]
        public T[] Values { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SingleOrArray()
        {
            _isEmpty = true;
            Value = default(T);
            Values = EmptyArray<T>.Value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SingleOrArray
            (
                [CanBeNull] T value
            )
        {
            _isEmpty = false;
            Value = value;
            Values = new[] { value };
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SingleOrArray
            (
                [NotNull] T[] values
            )
        {
            Code.NotNull(values, "values");

            Values = values;
            if (values.Length == 0)
            {
                _isEmpty = true;
                Value = default(T);
            }
            else
            {
                Value = values.First();
            }
        }

        #endregion

        #region Private members

        private readonly bool _isEmpty;

        #endregion

        #region Public methods

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        [NotNull]
        public static implicit operator SingleOrArray<T>
            (
                [CanBeNull] T value
            )
        {
            return new SingleOrArray<T>(value);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        [NotNull]
        public static implicit operator SingleOrArray<T>
            (
                [NotNull] T[] values
            )
        {
            return new SingleOrArray<T>(values);
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        [CanBeNull]
        public static implicit operator T
            (
                [NotNull] SingleOrArray<T> soa
            )
        {
            Code.NotNull(soa, "soa");

            return soa.Value;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        [CanBeNull]
        public static implicit operator T[]
            (
                [NotNull] SingleOrArray<T> soa
            )
        {
            Code.NotNull(soa, "soa");

            return soa.Values;
        }

        /// <summary>
        /// Make <see cref="SingleOrArray{T}"/> from the token.
        /// </summary>
        [NotNull]
        public static SingleOrArray<T> FromJson
            (
                [CanBeNull] JProperty property
            )
        {
            if (ReferenceEquals(property, null))
            {
                return new SingleOrArray<T>();
            }

            JToken value = property.Value;
            JArray array = value as JArray;
            if (!ReferenceEquals(array, null))
            {
                return FromSequence(array.Values<T>());
            }

            return new SingleOrArray<T>(value.Value<T>());
        }

        /// <summary>
        /// Make <see cref="SingleOrArray{T}"/> from the sequence.
        /// </summary>
        [NotNull]
        public static SingleOrArray<T> FromSequence
            (
                [NotNull] IEnumerable<T> sequence
            )
        {
            Code.NotNull(sequence, "sequence");

            T[] array = sequence.ToArray();

            return new SingleOrArray<T>(array);
        }

        #endregion

        #region IEnumerable members

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            if (Values.Length == 0)
            {
                yield return Value;
            }
            else
            {
                foreach (T value in Values)
                {
                    yield return value;
                }
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Value.ToVisibleString();
        }

        #endregion
    }
}
