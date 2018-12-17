// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NumberText.cs -- string containing numbers
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

using UnsafeAM.IO;
using UnsafeAM.Logging;
using UnsafeAM.Runtime;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Text
{
    /// <summary>
    /// String containing numbers separated
    /// by non-numeric fragments.
    /// </summary>
    [PublicAPI]
    public sealed class NumberText
        : IComparable<NumberText>,
        IHandmadeSerializable,
        IEquatable<NumberText>,
        IEnumerable<string>,
        IVerifiable
    {
        #region Inner classes

        /// <summary>
        /// Fragment: a prefix plus a number.
        /// </summary>
        class Chunk
            : IHandmadeSerializable,
              IVerifiable
        {
            #region Properties

            public bool HavePrefix
            {
                get { return !string.IsNullOrEmpty(Prefix); }
            }

            /// <summary>
            /// Prefix.
            /// </summary>
            [CanBeNull]
            public string Prefix { get; set; }

            /// <summary>
            /// Have value?
            /// </summary>
            public bool HaveValue { get; set; }

            /// <summary>
            /// Numeric value itself.
            /// </summary>
            public long Value { get; set; }

            /// <summary>
            /// Length (used when converting value to string).
            /// </summary>
            public int Length { get; set; }

            #endregion

            #region Construction

            #endregion

            #region Public methods

            public bool SetUp
                (
                    StringBuilder str,
                    StringBuilder number
                )
            {
                bool result = false;
                if (str.Length != 0)
                {
                    result = true;
                    Prefix = str.ToString();
                }
                if (number.Length != 0)
                {
                    result = true;
                    HaveValue = true;
                    Length = number.Length;
                    Value = long.Parse(number.ToString());
                }
                return result;
            }

            public int CompareTo
                (
                    Chunk other
                )
            {
                int result = string.Compare
                    (
                        Prefix,
                        other.Prefix,
                        StringComparison.CurrentCulture
                    );

                if (result == 0)
                {
                    result = HaveValue && other.HaveValue
                        ? Math.Sign(Value - other.Value)
                        : HaveValue.CompareTo(other.HaveValue);
                }

                return result;
            }

            public Chunk Copy()
            {
                Chunk result = new Chunk
                {
                    Prefix = Prefix,
                    HaveValue = HaveValue,
                    Value = Value,
                    Length = Length
                };

                return result;
            }

            #endregion

            #region IHandmadeSerializable

            /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                Code.NotNull(reader, nameof(reader));

                Prefix = reader.ReadNullableString();
                Value = reader.ReadPackedInt64();
                Length = reader.ReadPackedInt32();
                HaveValue = reader.ReadBoolean();
            }

            /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                Code.NotNull(writer, nameof(writer));

                writer
                    .WriteNullable(Prefix)
                    .WritePackedInt64(Value)
                    .WritePackedInt32(Length)
                    .Write(HaveValue);
            }

            #endregion

            #region IVerifiable members

            /// <inheritdoc cref="IVerifiable.Verify" />
            public bool Verify
                (
                    bool throwOnError
                )
            {
                Verifier<Chunk> verifier = new Verifier<Chunk>(this, throwOnError);

                verifier.Assert(HavePrefix || HaveValue, "Must have prefix or value");

                return verifier.Result;
            }

            #endregion

            #region Object members

            /// <inheritdoc cref="object.ToString" />
            public override string ToString()
            {
                StringBuilder result = new StringBuilder();

                if (!ReferenceEquals(Prefix, null))
                {
                    result.Append(Prefix);
                }

                if (HaveValue)
                {
                    if (Length > 0)
                    {
                        string format = new string('0', Length);
                        result.Append(Value.ToString(format));
                    }
                    else
                    {
                        result.Append(Value);
                    }
                }

                return result.ToString();
            }

            #endregion
        }

        ///// <inheritdoc cref="Comparer{T}"/>
        //public class Comparer
        //    : Comparer<string>
        //{
        //    // ReSharper disable once MemberHidesStaticFromOuterClass

        //    /// <inheritdoc cref="Comparer{T}.Compare" />
        //    public override int Compare
        //        (
        //            string x,
        //            string y
        //        )
        //    {
        //        return NumberText.Compare(x, y);
        //    }
        //}

        #endregion

        #region Properties

        /// <summary>
        /// Default index.
        /// </summary>
        public static int DefaultIndex;

        /// <summary>
        /// Пустое ли число?
        /// </summary>
        public bool Empty
        {
            get { return _chunks.First == null; }
        }

        /// <summary>
        /// Номер последнего сегмента.
        /// </summary>
        public int LastIndex
        {
            get { return Length - 1; }
        }

        /// <summary>
        /// Количество сегментов, из которых состоит число.
        /// </summary>
        public int Length
        {
            get { return _chunks.Count; }
        }

        /// <summary>
        /// Contains only text (prefix)?
        /// </summary>
        public bool TextOnly
        {
            get
            {
                return Length == 1
                       && HavePrefix(0)
                       && !HaveValue(0);
            }
        }

        /// <summary>
        /// Contains only numeric value?
        /// </summary>
        public bool ValueOnly
        {
            get
            {
                return Length == 1
                       && !HavePrefix(0)
                       && HaveValue(0);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberText()
        {
            _chunks = new LinkedList<Chunk>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NumberText
            (
                [CanBeNull] string text
            )
            : this()
        {
            Parse(text);
        }

        #endregion

        #region Private members

        [NotNull]
        private readonly LinkedList<Chunk> _chunks;

        [CanBeNull]
        private Chunk this[int index]
        {
            get
            {
                if (index < 0)
                {
                    return null;
                }

                LinkedListNode<Chunk> result = _chunks.First;
                while (index > 0)
                {
                    if (result == null)
                    {
                        return null;
                    }
                    result = result.Next;
                    index--;
                }

                return result == null
                    ? null
                    : result.Value;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Appends the chunk.
        /// </summary>
        [NotNull]
        public NumberText AppendChunk
            (
                string prefix,
                long value,
                int length
            )
        {
            Chunk chunk = new Chunk
            {
                Prefix = prefix,
                HaveValue = true,
                Value = value,
                Length = length
            };
            _chunks.AddLast(chunk);

            return this;
        }

        /// <summary>
        /// Append chunk to the number tail.
        /// </summary>
        [NotNull]
        public NumberText AppendChunk
            (
                string prefix
            )
        {
            Chunk chunk = new Chunk
            {
                Prefix = prefix
            };
            _chunks.AddLast(chunk);

            return this;
        }

        /// <summary>
        /// Append chunk to the number tail.
        /// </summary>
        [NotNull]
        public NumberText AppendChunk
            (
                long value
            )
        {
            Chunk chunk = new Chunk
            {
                HaveValue = true,
                Value = value
            };
            _chunks.AddLast(chunk);

            return this;
        }

        /// <summary>
        /// Clone the number.
        /// </summary>
        [NotNull]
        public NumberText Clone()
        {
            NumberText result = new NumberText();
            foreach (Chunk chunk in _chunks)
            {
                result._chunks.AddLast(chunk.Copy());
            }

            return result;
        }

        /// <summary>
        /// Gets the difference.
        /// </summary>
        public long GetDifference
            (
                NumberText other
            )
        {
            return GetValue(0) - other.GetValue(0);
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public int GetLength
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return chunk == null
                ? 0
                : chunk.HaveValue
                    ? chunk.Length
                    : 0;
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        public string GetPrefix
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return chunk == null
                ? null
                : chunk.Prefix;
        }

        /// <summary>
        /// Gets the numeric value.
        /// </summary>
        public long GetValue
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return chunk == null
                ? 0
                : chunk.HaveValue
                  ? chunk.Value
                  : 0;
        }

        /// <summary>
        /// Do we have the chunk with the given index?
        /// </summary>
        public bool HaveChunk
            (
                int index
            )
        {
            return this[index] != null;
        }

        /// <summary>
        /// Do we have the prefix in the chunk
        /// with the given index?
        /// </summary>
        public bool HavePrefix
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return chunk != null && chunk.HavePrefix;
        }

        /// <summary>
        /// Do we have numeric value in the chunk
        /// with the given index?
        /// </summary>
        public bool HaveValue
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return chunk != null && chunk.HaveValue;
        }

        /// <summary>
        /// Увеличение на единицу последнего сегмента.
        /// </summary>
        public NumberText Increment()
        {
            return Increment
                (
                    LastIndex,
                    1
                );
        }

        /// <summary>
        /// Увеличение последнего сегмента на указанное число.
        /// </summary>
        public NumberText Increment
            (
                int delta
            )
        {
            return Increment
                (
                    LastIndex,
                    delta
                );
        }

        /// <summary>
        /// Increments numeric value in the chunk with
        /// the specified index.
        /// </summary>
        public NumberText Increment
            (
                int index,
                int delta
            )
        {
            Chunk chunk = this[index];
            if (chunk != null
                && chunk.HaveValue)
            {
                chunk.Value += delta;
            }
            return this;
        }

        /// <summary>
        /// Increments numeric value in the chunk with
        /// the specified index.
        /// </summary>
        public NumberText Increment
            (
                int index,
                long delta
            )
        {
            Chunk chunk = this[index];
            if (chunk != null
                && chunk.HaveValue)
            {
                chunk.Value += delta;
            }
            return this;
        }

        /// <summary>
        /// Parse the specified text.
        /// </summary>
        public void Parse
            (
                [CanBeNull] string text
            )
        {
            _chunks.Clear();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            StringReader reader = new StringReader(text);
            Chunk chunk = new Chunk();
            _chunks.AddLast(chunk);
            bool textPart = true;
            StringBuilder str = new StringBuilder();
            StringBuilder number = new StringBuilder();
            int code;
            while ((code = reader.Read()) != -1)
            {
                char c = (char)code;
                if (textPart)
                {
                    if (char.IsDigit(c))
                    {
                        number.Append(c);
                        textPart = false;
                    }
                    else
                    {
                        str.Append(c);
                    }
                }
                else
                {
                    if (char.IsDigit(c))
                    {
                        number.Append(c);
                    }
                    else
                    {
                        chunk.SetUp(str, number);
                        chunk = new Chunk();
                        _chunks.AddLast(chunk);
                        str.Length = 0;
                        str.Append(c);
                        number.Length = 0;
                        textPart = true;
                    }
                }
            }
            if (!chunk.SetUp(str, number))
            {
                _chunks.RemoveLast();
            }
        }

        /// <summary>
        /// Parses the specified tex for ranges.
        /// </summary>
        public static IEnumerable<NumberText> ParseRanges
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                yield break;
            }

            StringReader reader = new StringReader(text);

            StringBuilder firstBuffer = new StringBuilder();
            StringBuilder secondBuffer = new StringBuilder();
            NumberText firstNumber;

        BEGIN:

            int c1;
            while ((c1 = reader.Read()) != -1)
            {
                char c2 = (char)c1;
                if (c2 == '-')
                {
                    if (firstBuffer.Length == 0)
                    {
                        Log.Error
                            (
                                "NumberText::ParseRanges: "
                                + "syntax error"
                            );

                        throw new ArsMagnaException();
                    }

                    break;
                }

                if (
                        c2 != '/'
                        &&
                        (   char.IsSeparator(c2)
                        ||  char.IsPunctuation(c2)
                        )
                    )
                {
                    if (firstBuffer.Length == 0)
                    {
                        continue;
                    }

                    firstNumber = new NumberText(firstBuffer.ToString());
                    yield return firstNumber;
                    firstBuffer.Length = 0;
                    goto BEGIN;
                }

                firstBuffer.Append(c2);
            }

            if (reader.Peek() == -1)
            {
                firstNumber = new NumberText(firstBuffer.ToString());
                yield return firstNumber;
                firstBuffer.Length = 0;
            }
            else
            {
                int c3;
                while ((c3 = reader.Read()) != -1)
                {
                    char c4 = (char)c3;

                    if (char.IsWhiteSpace(c4))
                    {
                        if (secondBuffer.Length == 0)
                        {
                            continue;
                        }
                    }

                    if (
                            c4 != '/'
                            &&
                            (   char.IsSeparator(c4)
                            ||  char.IsPunctuation(c4)
                            )
                        )
                    {
                        break;
                    }

                    secondBuffer.Append(c4);
                }

                if (secondBuffer.Length == 0)
                {
                    Log.Error
                        (
                            "NumberText::ParseRanges: "
                            + "syntax error"
                        );

                    throw new Exception();
                }

                firstNumber = new NumberText(firstBuffer.ToString());
                NumberText secondNumber = new NumberText(secondBuffer.ToString());

                if (firstNumber.GetPrefix(0) != secondNumber.GetPrefix(0))
                {
                    Log.Error
                        (
                            "NumberText::ParseRanges: "
                            + "prefix mismatch: '"
                            + firstNumber.GetPrefix(0)
                            + "' and '"
                            + secondNumber.GetPrefix(0)
                            + "'"
                        );

                    throw new Exception();
                }

                while (firstNumber.CompareTo(secondNumber) <= 0)
                {
                    yield return firstNumber.Clone();
                    firstNumber = firstNumber.Increment();
                }

                firstBuffer.Length = 0;
                secondBuffer.Length = 0;
            }

            if (reader.Peek() != -1)
            {
                goto BEGIN;
            }
        }

        /// <summary>
        /// Remove the chunk with the specified index.
        /// </summary>
        public NumberText RemoveChunk
            (
                int index
            )
        {
            Chunk chunk = this[index];
            if (!ReferenceEquals(chunk, null))
            {
                _chunks.Remove(chunk);
            }
            return this;
        }

        /// <summary>
        /// Set length for text conversion of
        /// numeric value in the specified chunk.
        /// </summary>
        public NumberText SetLength
            (
                int index,
                int length
            )
        {
            Chunk chunk = this[index];
            if (chunk != null)
            {
                chunk.Length = length;
            }
            return this;
        }

        /// <summary>
        /// Set the prefix for the specified chunk.
        /// </summary>
        public NumberText SetPrefix
            (
                int index,
                string prefix
            )
        {
            Chunk chunk = this[index];
            if (chunk != null)
            {
                chunk.Prefix = prefix;
            }
            return this;
        }

        /// <summary>
        /// Set the numeric value for the specified chunk.
        /// </summary>
        public NumberText SetValue
            (
                int index,
                long value
            )
        {
            Chunk chunk = this[index];
            if (chunk != null)
            {
                chunk.HaveValue = true;
                chunk.Value = value;
            }
            return this;
        }

        /// <summary>
        /// Soft given text lines.
        /// </summary>
        public static IEnumerable<string> Sort
            (
                IEnumerable<string> lines
            )
        {
            List<NumberText> result = lines
                .Select(item => new NumberText(item))
                .ToList();

            result.Sort();

            return result.Select(item => item.ToString());
        }

        /// <summary>
        /// Soft given numbers.
        /// </summary>
        public static IEnumerable<NumberText> Sort
            (
                IEnumerable<NumberText> numbers
            )
        {
            List<NumberText> result = new List<NumberText>(numbers);
            result.Sort();
            return result;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator NumberText
            (
                string text
            )
        {
            return new NumberText
                (
                    text
                );
        }

        #endregion

        #region Comparison

        /// <inheritdoc cref="IComparable{T}.CompareTo" />
        public int CompareTo
            (
                [NotNull] NumberText other
            )
        {
            for (int i = 0; i < int.MaxValue; i++) //-V3022
            {
                Chunk c1 = this[i];
                Chunk c2 = other[i];
                if (c1 != null && c2 != null)
                {
                    int result = c1.CompareTo(c2);
                    if (result != 0)
                    {
                        return result;
                    }
                }
                else
                {
                    if (c1 == null && c2 == null)
                    {
                        return 0;
                    }
                    return c1 != null
                        ? 1
                        : -1;
                }
            }

            return 0;
        }

        /// <summary>
        /// Compares to the <see cref="System.Int64"/>.
        /// </summary>
        public int CompareTo
            (
                long value
            )
        {
            Chunk chunk = this[0];
            if (chunk == null)
            {
                return -1;
            }
            if (chunk.HavePrefix)
            {
                return 1;
            }
            if (!chunk.HaveValue)
            {
                return -1;
            }
            return Math.Sign(chunk.Value - value);
        }

        /// <summary>
        /// Compares to the <see cref="System.String"/>.
        /// </summary>
        public int CompareTo
            (
                string text
            )
        {
            return CompareTo(new NumberText(text));
        }

        /// <summary>
        /// Compares two specified strings.
        /// </summary>
        public static int Compare
            (
                string left,
                string right
            )
        {
            NumberText one = new NumberText(left);
            NumberText two = new NumberText(right);

            return one.CompareTo ( two );
        }

        /// <summary>
        /// Compute maximal value.
        /// </summary>
        public static NumberText Max
            (
                NumberText left,
                NumberText right
            )
        {
            return left < right
                ? right
                : left;
        }

        /// <summary>
        /// Compute minimal value.
        /// </summary>
        public static NumberText Min
            (
                NumberText left,
                NumberText right
            )
        {
            return left < right
                ? left
                : right;
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        public static bool operator ==
            (
                NumberText left,
                NumberText right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return left.CompareTo(right) == 0;
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        public static bool operator ==
            (
                NumberText left,
                string right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return left.CompareTo(right) == 0;
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        public static bool operator ==
            (
                NumberText left,
                int right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return false;
            }

            return left.CompareTo(right) == 0;
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        public static bool operator !=
            (
                NumberText left,
                NumberText right
            )
        {
            if (ReferenceEquals(left, null)
                ||ReferenceEquals(right, null))
            {
                return true;
            }

            if (ReferenceEquals(left, right))
            {
                return false;
            }

            return left.CompareTo(right) != 0;
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        public static bool operator !=
            (
                NumberText left,
                string right
            )
        {
            if (ReferenceEquals(left, null)
                ||ReferenceEquals(right, null))
            {
                return true;
            }

            return left.CompareTo(right) != 0;
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        public static bool operator !=
            (
                NumberText left,
                int right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return true;
            }

            return left.CompareTo(right) != 0;
        }

        /// <summary>
        /// Implements the &lt; operator.
        /// </summary>
        public static bool operator <
            (
                NumberText left,
                NumberText right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return !ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Implements the &lt; operator.
        /// </summary>
        public static bool operator <
            (
                NumberText left,
                string right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return !ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Implements the &lt;= operator.
        /// </summary>
        public static bool operator <=
            (
                NumberText left,
                NumberText right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return !ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Implements the &lt; operator.
        /// </summary>
        public static bool operator <
            (
                NumberText left,
                int right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return true;
            }

            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Implements the &gt; operator.
        /// </summary>
        public static bool operator >
            (
                NumberText left,
                NumberText right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return true;
            }

            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Implements the &gt; operator.
        /// </summary>
        public static bool operator >
            (
                NumberText left,
                string right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return true;
            }

            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Implements the &gt; operator.
        /// </summary>
        public static bool operator >
            (
                NumberText left,
                int right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return false;
            }

            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Implements the &gt;= operator.
        /// </summary>
        public static bool operator >=
            (
                NumberText left,
                NumberText right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return true;
            }

            return left.CompareTo(right) >= 0;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals
            (
                NumberText other
            )
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return CompareTo(other) == 0;
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is NumberText && Equals((NumberText)obj);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return _chunks.GetHashCode();
        }

        #endregion

        #region Arithmetics

        /// <summary>
        /// Implements the + operator.
        /// </summary>
        [NotNull]
        public static NumberText operator +
            (
                [NotNull] NumberText left,
                int right
            )
        {
            return left.Clone().Increment(right);
        }

        /// <summary>
        /// Implements the - operator.
        /// </summary>
        public static long operator -
            (
                [NotNull] NumberText left,
                [NotNull] NumberText right
            )
        {
            return left.GetDifference(right);
        }

        #endregion

        #region  IEnumerable<T> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<string> GetEnumerator()
        {
            foreach (Chunk chunk in _chunks)
            {
                yield return chunk.ToString();
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, nameof(reader));

            _chunks.Clear();

            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                Chunk chunk = new Chunk();
                chunk.RestoreFromStream(reader);
                _chunks.AddLast(chunk);
            }
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, nameof(writer));

            int count = _chunks.Count;
            writer.WritePackedInt32(count);
            foreach (Chunk chunk in _chunks)
            {
                chunk.SaveToStream(writer);
            }
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<NumberText> verifier = new Verifier<NumberText>(this, throwOnError);

            verifier.Positive(_chunks.Count, "_chunks.Count");
            foreach (Chunk chunk in _chunks)
            {
                verifier.VerifySubObject(chunk, "chunk");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (Chunk chunk in _chunks)
            {
                result.Append(chunk);
            }

            return result.ToString();
        }

        #endregion
    }
}
