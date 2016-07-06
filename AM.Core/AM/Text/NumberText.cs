/* NumberText.cs -- строка, содержащая числа
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Строка, содержащая числа, разделенные нечисловыми фрагментами.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class NumberText
        : IComparable<NumberText>,
        IEquatable<NumberText>,
        IEnumerable<string>
    {
        #region Inner classes

        /// <summary>
        /// Фрагмент: нечисловой префикс плюс число.
        /// </summary>
        [Serializable]
        class Chunk
        {
            #region Properties

            public bool HavePrefix
            {
                get { return !string.IsNullOrEmpty(Prefix); }
            }

            public string Prefix { get; set; }

            public bool HaveValue { get; set; }

            public long Value { get; set; }

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
                    result = (HaveValue && other.HaveValue)
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

            #region Object members

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

        #endregion

        #region Properties

        public static int DefaultIndex = 0;

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

        public bool TextOnly
        {
            get
            {
                return (Length == 1)
                       && HavePrefix(0)
                       && !HaveValue(0);
            }
        }

        public bool ValueOnly
        {
            get
            {
                return (Length == 1)
                       && !HavePrefix(0)
                       && HaveValue(0);
            }
        }

        #endregion

        #region Construction

        public NumberText()
        {
            _chunks = new LinkedList<Chunk>();
        }

        public NumberText
            (
                string text
            )
            : this()
        {
            Parse(text);
        }

        #endregion

        #region Private members

        private readonly LinkedList<Chunk> _chunks;

        private Chunk this[int index]
        {
            get
            {
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
                return (result == null)
                    ? null
                    : result.Value;
            }
        }

        #endregion

        #region Public methods

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

        public NumberText Copy()
        {
            NumberText result = new NumberText();
            foreach (Chunk chunk in _chunks)
            {
                result._chunks.AddLast(chunk.Copy());
            }
            return result;
        }

        public long GetDifference
            (
                NumberText other
            )
        {
            return GetValue(0) - other.GetValue(0);
        }

        public int GetLength
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return (chunk == null)
                ? 0
                : chunk.HaveValue
                    ? chunk.Length
                    : 0;
        }

        public string GetPrefix
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return (chunk == null)
                ? null
                : chunk.Prefix;
        }

        public long GetValue
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return (chunk == null)
                ? 0
                : (chunk.HaveValue)
                  ? chunk.Value
                  : 0;
        }

        public bool HaveChunk
            (
                int index
            )
        {
            return (this[index] != null);
        }

        public bool HavePrefix
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return (chunk != null) && chunk.HavePrefix;
        }

        public bool HaveValue
            (
                int index
            )
        {
            Chunk chunk = this[index];
            return (chunk != null) && chunk.HaveValue;
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

        public NumberText Increment
            (
                int index,
                int delta
            )
        {
            Chunk chunk = this[index];
            if ((chunk != null)
                && (chunk.HaveValue))
            {
                chunk.Value += delta;
            }
            return this;
        }

        public NumberText Increment
            (
                int index,
                long delta
            )
        {
            Chunk chunk = this[index];
            if ((chunk != null)
                && (chunk.HaveValue))
            {
                chunk.Value += delta;
            }
            return this;
        }

        public void Parse
            (
                string text
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
                        throw new Exception();
                    }

                    break;
                }

                if (
                        (c2 != '/')
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
                            (c4 != '/')
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
                    throw new Exception();
                }

                firstNumber = new NumberText(firstBuffer.ToString());
                NumberText secondNumber = new NumberText(secondBuffer.ToString());

                if (firstNumber.GetPrefix(0) != secondNumber.GetPrefix(0))
                {
                    throw new Exception();
                }

                while (firstNumber.CompareTo(secondNumber) <= 0)
                {
                    yield return firstNumber;
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

        public NumberText Verify()
        {
            for (
                    LinkedListNode<Chunk> node = _chunks.First;
                    node != null;
                    node = node.Next
                )
            {
                Chunk chunk = node.Value;
                if (!chunk.HavePrefix && !chunk.HaveValue)
                {
                    throw new ArsMagnaException();
                }
                if (node.Next != null)
                {
                    if (!chunk.HaveValue)
                    {
                        throw new ArsMagnaException();
                    }
                    if (!node.Next.Value.HavePrefix)
                    {
                        throw new ArsMagnaException();
                    }
                }
            }

            return this;
        }

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

        public static IEnumerable<NumberText> Sort
            (
                IEnumerable<NumberText> numbers
            )
        {
            List<NumberText> result = new List<NumberText>(numbers);
            result.Sort();
            return result;
        }

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

        public int CompareTo
            (
                [NotNull] NumberText other
            )
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                Chunk c1 = this[i];
                Chunk c2 = other[i];
                if ((c1 != null) && (c2 != null))
                {
                    int result = c1.CompareTo(c2);
                    if (result != 0)
                    {
                        return result;
                    }
                }
                else
                {
                    if ((c1 == null) && (c2 == null))
                    {
                        return 0;
                    }
                    return (c1 != null)
                        ? 1
                        : -1;
                }
            }
            return 0;
        }

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

        public int CompareTo
            (
                string text
            )
        {
            return CompareTo(new NumberText(text));
        }

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

        public static NumberText Max
            (
                NumberText left,
                NumberText right
            )
        {
            return (left < right)
                ? right
                : left;
        }

        public static NumberText Min
            (
                NumberText left,
                NumberText right
            )
        {
            return (left < right)
                ? left
                : right;
        }

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

            return (left.CompareTo(right) == 0);
        }

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

            return (left.CompareTo(right) == 0);
        }

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

            return (left.CompareTo(right) == 0);
        }

        public static bool operator != 
            (
                NumberText left, 
                NumberText right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return (!ReferenceEquals(right, null));
            }
            if (ReferenceEquals(right, null))
            {
                return true;
            }

            return (left.CompareTo(right) != 0);            
        }

        public static bool operator !=
            (
                NumberText left, 
                string right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return (!ReferenceEquals(right, null));
            }
            if (ReferenceEquals(right, null))
            {
                return true;
            }

            return (left.CompareTo(right) != 0);
        }

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

            return (left.CompareTo(right) != 0);
        }

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

            return (left.CompareTo(right) < 0);
        }

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

            return (left.CompareTo(right) < 0);
        }

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

            return (left.CompareTo(right) <= 0);
        }

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

            return (left.CompareTo(right) < 0);
        }

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

            return (left.CompareTo(right) > 0);
        }

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

            return (left.CompareTo(right) > 0);
        }

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

            return (left.CompareTo(right) > 0);
        }

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

            return (left.CompareTo(right) >= 0);
        }

        public bool Equals
            (
                NumberText other
            )
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return (CompareTo(other) == 0);
        }

        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is NumberText && Equals((NumberText)obj);
        }

        public override int GetHashCode()
        {
            return (_chunks != null ? _chunks.GetHashCode() : 0);
        }

        #endregion

        #region Arithmetics

        [NotNull]
        public static NumberText operator +
            (
                [NotNull] NumberText left,
                int right
            )
        {
            return left.Copy().Increment(right);
        }

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

        public IEnumerator<string> GetEnumerator()
        {
            foreach (Chunk chunk in _chunks)
            {
                yield return chunk.ToString();
            }
        }

        #endregion

        #region Object members

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
