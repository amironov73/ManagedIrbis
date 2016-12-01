// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CharSet.cs -- character set
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Character set.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{ToString()}")]
#endif
    public sealed class CharSet
        : IHandmadeSerializable,
        IEnumerable<char>,
        IEquatable<CharSet>
    {
        #region Constants

        /// <summary>
        /// Default capacity of <see cref="CharSet"/>.
        /// </summary>
        public const int DefaultCapacity = 0x10000;

        #endregion

        #region Nested classes

        private class CharSetEnumerator
            : IEnumerator<char>
        {
            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public CharSetEnumerator
                (
                    CharSet charSet
                )
            {
                _data = charSet.ToArray();
                _index = -1;
            }

            #endregion

            #region Private members

            private readonly char[] _data;

            private int _index;

            #endregion

            #region IEnumerator members

            /// <summary>
            /// Performs application-defined tasks associated
            /// with freeing, releasing, or resetting
            /// unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // Nothing to do here
            }

            /// <summary>
            /// Gets the element in the collection
            /// at the current position of the enumerator.
            /// </summary>
            char IEnumerator<char>.Current
            {
                get { return _data[_index]; }
            }

            /// <summary>
            /// Gets the element in the collection
            /// at the current position of the enumerator.
            /// </summary>
            public object Current
            {
                get
                {
                    return _data[_index];
                }
            }

            /// <summary>
            /// Advances the enumerator to the next element
            /// of the collection.
            /// </summary>
            /// <returns>true if the enumerator was successfully
            /// advanced to the next element; false
            /// if the enumerator has passed the end
            /// of the collection.</returns>
            public bool MoveNext()
            {
                _index++;
                if (_index >= _data.Length)
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position,
            /// which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                _index = -1;
            }

            #endregion
        }

#if !WINMOBILE && !PocketPC

        /// <summary>
        /// Converter for JSON serialization.
        /// </summary>
        public class CharSetConverter
            : JsonConverter
        {
            #region Constructor

            /// <summary>
            /// Constructor.
            /// </summary>
            public CharSetConverter(params Type[] types)
            {
                _types = types;
            }

            #endregion

            #region Private members

            private readonly Type[] _types;

            #endregion

            #region JsonConverter members

            /// <summary>
            /// Writes the JSON representation of the object.
            /// </summary>
            public override void WriteJson
                (
                    JsonWriter writer,
                    object value,
                    JsonSerializer serializer
                )
            {
                CharSet charSet = (CharSet) value;

                JObject o = new JObject();
                o.AddFirst(new JProperty
                    (
                        "charset",
                        charSet.ToString()
                    ));

                o.WriteTo(writer);
            }

            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            public override object ReadJson
                (
                    JsonReader reader,
                    Type objectType,
                    object existingValue,
                    JsonSerializer serializer
                )
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Determines whether this instance can convert
            /// the specified object type.
            /// </summary>
            public override bool CanConvert
                (
                    Type objectType
                )
            {
                return _types.Contains(objectType);
            }

            #endregion
        }

#endif

        #endregion

        #region Properties

        ///<summary>
        /// Indexer.
        ///</summary>
        public bool this[char index]
        {
            [DebuggerStepThrough]
            get
            {
                return _data[index];
            }
            [DebuggerStepThrough]
            set
            {
                _data[index] = value;
            }
        }

        /// <summary>
        /// Count of elements.
        /// </summary>
        public int Count
        {
            get
            {
                int result = 0;
                for (int i = 0; i < _data.Length; i++)
                {
                    if (_data[i])
                    {
                        result++;
                    }
                }
                return result;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet()
            : this (DefaultCapacity)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                int capacity
            )
        {
            Code.Positive(capacity, "capacity");

            _data = new BitArray(capacity);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                [NotNull] BitArray data
            )
        {
            _data = new BitArray(data);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public CharSet
            (
                [NotNull] CharSet other
            )
        {
            Code.NotNull(other, "other");

            _data = new BitArray(other._data);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                [NotNull] IEnumerable<char> characters
            )
            : this()
        {
            Code.NotNull(characters, "characters");

            foreach (char c in characters)
            {
                Add(c);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                params char[] characters
            )
            : this()
        {
            Add(characters);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharSet
            (
                [NotNull] string characters
            )
            : this()
        {
            Code.NotNull(characters, "characters");

            Add(characters);
        }

        #endregion

        #region Private members

        private readonly BitArray _data;

        private void _Add(char[] ch, bool val)
        {
            for (int i = 0; i < ch.Length; i++)
            {
                _data[ch[i]] = val;
            }
        }

        private void _Add(string s, bool val)
        {
            int len1 = s.Length - 1;
            int len2 = s.Length - 2;
            int i = 0;
            for (; i < len1; i++)
            {
                if (s[i] == '\\')
                {
                    _data[s[i + 1]] = val;
                    i++;
                }
                else if (s[i + 1] == '-')
                {
                    if (i >= len2)
                    {
                        throw new ArgumentException();
                    }
                    for (int c = s[i]; c <= s[i + 2]; c++)
                    {
                        _data[c] = val;
                    }
                    i += 2;
                }
                else
                {
                    _data[s[i]] = val;
                }
            }
            for (; i < s.Length; i++)
            {
                if (s[i] == '\\')
                {
                    throw new ArgumentException();
                }
                _data[s[i]] = val;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add a character.
        /// </summary>
        [NotNull]
        public CharSet Add
            (
                char character
            )
        {
            _data[character] = true;

            return this;
        }

        /// <summary>
        /// Add some characters.
        /// </summary>
        [NotNull]
        public CharSet Add
            (
                params char[] range
            )
        {
            _Add(range, true);

            return this;
        }

        /// <summary>
        /// Add some characters.
        /// </summary>
        [NotNull]
        public CharSet Add
            (
                [NotNull] string range
            )
        {
            Code.NotNull(range, "range");

            _Add(range, true);

            return this;
        }

        /// <summary>
        /// Add range of characters.
        /// </summary>
        [NotNull]
        public CharSet AddRange
            (
                char from,
                char to
            )
        {
            for (char c = from; c <= to; c++)
            {
                Add(c);
            }

            return this;
        }

        /// <summary>
        /// Logical multiplication.
        /// </summary>
        [NotNull]
        public CharSet And
            (
                [NotNull] CharSet other
            )
        {
            Code.NotNull(other, "other");

            CharSet result = Clone();
            result._data.And(other._data);

            return result;
        }

        /// <summary>
        /// Check the string for characters not included in the charset.
        /// </summary>
        public bool CheckText
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (!this[text[i]])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Clear the <see cref="CharSet"/>.
        /// </summary>
        [NotNull]
        public CharSet Clear()
        {
            return SetAll(false);
        }

        /// <summary>
        /// Create clone of the <see cref="CharSet"/>.
        /// </summary>
        [NotNull]
        public CharSet Clone()
        {
            return new CharSet(this);
        }

        /// <summary>
        /// Determines whether the <see cref="CharSet"/>
        /// contains given character.
        /// </summary>
        public bool Contains
            (
                char c
            )
        {
            return _data[c];
        }

        /// <summary>
        /// Invert.
        /// </summary>
        [NotNull]
        public CharSet Not()
        {
            CharSet result = Clone();
            result._data.Not();

            return result;
        }

        /// <summary>
        /// Logical addition.
        /// </summary>
        [NotNull]
        public CharSet Or
            (
                [NotNull] CharSet other
            )
        {
            Code.NotNull(other, "other");

            CharSet result = Clone();
            result._data.Or(other._data);

            return result;
        }

        /// <summary>
        /// Remove a character.
        /// </summary>
        [NotNull]
        public CharSet Remove
            (
                char character
            )
        {
            _data[character] = false;

            return this;
        }

        /// <summary>
        /// Remove some characters.
        /// </summary>
        [NotNull]
        public CharSet Remove
            (
                params char[] range
            )
        {
            _Add(range, false);

            return this;
        }

        /// <summary>
        /// Remove some characters.
        /// </summary>
        [NotNull]
        public CharSet Remove
            (
                [NotNull] string range
            )
        {
            Code.NotNull(range, "range");

            _Add(range, false);

            return this;
        }

        /// <summary>
        /// Remove range of characters.
        /// </summary>
        public CharSet RemoveRange
            (
                char from,
                char to
            )
        {
            for (char c = from; c <= to; c++)
            {
                Remove(c);
            }

            return this;
        }

        /// <summary>
        /// Set all characters to the <paramref name="state"/>.
        /// </summary>
        [NotNull]
        public CharSet SetAll
            (
                bool state
            )
        {
            _data.SetAll(state);

            return this;
        }

        /// <summary>
        /// Create array of characters.
        /// </summary>
        [NotNull]
        public char[] ToArray()
        {
            int length = _data.Length/8 + 1;
            List<char> result = new List<char>(length);

            for (int i = 0; i < _data.Length; i++)
            {
                if (_data[i])
                {
                    result.Add((char)i);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Logical XOR.
        /// </summary>
        [NotNull]
        public CharSet Xor
            (
                [NotNull] CharSet other
            )
        {
            Code.NotNull(other, "other");

            CharSet result = Clone();
            result._data.Xor(other._data);

            return this;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Logical addition.
        /// </summary>
        [NotNull]
        public static CharSet operator +
            (
                [NotNull] CharSet left,
                [NotNull] CharSet right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            return left.Or(right);
        }

        /// <summary>
        /// Logical addition.
        /// </summary>
        [NotNull]
        public static CharSet operator +
            (
                [NotNull] CharSet left,
                [NotNull] string right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            CharSet result = new CharSet(left);

            return result.Add(right);
        }

        /// <summary>
        /// Logical addition.
        /// </summary>
        [NotNull]
        public static CharSet operator +
            (
                [NotNull] CharSet left,
                char right
            )
        {
            Code.NotNull(left, "left");

            CharSet result = new CharSet(left);

            return result.Add(right);
        }

        /// <summary>
        /// Logical subtraction.
        /// </summary>
        [NotNull]
        public static CharSet operator -
            (
                [NotNull] CharSet left,
                [NotNull] CharSet right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            return left.And(right.Not());
        }

        /// <summary>
        /// Logical subtraction.
        /// </summary>
        [NotNull]
        public static CharSet operator -
            (
                [NotNull] CharSet left,
                [NotNull] string right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            CharSet result = new CharSet(left);

            return result.Remove(right);
        }

        /// <summary>
        /// Logical subtraction.
        /// </summary>
        [NotNull]
        public static CharSet operator -
            (
                [NotNull] CharSet left,
                char right
            )
        {
            Code.NotNull(left, "left");

            CharSet result = new CharSet(left);

            return result.Remove(right);
        }

        /// <summary>
        /// Logical multiplication.
        /// </summary>
        [NotNull]
        public static CharSet operator *
            (
                [NotNull] CharSet left,
                [NotNull] CharSet right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            return left.And(right);
        }

        /// <summary>
        /// Logical multiplication.
        /// </summary>
        [NotNull]
        public static CharSet operator *
            (
                [NotNull] CharSet left,
                [NotNull] string right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            return left.And(new CharSet(right));
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Clear();
            string text = reader.ReadString();
            Add(text);
        }

        /// <summary>
        /// Save object state to the stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            string text = ToString();
            writer.Write(text);
        }

        #endregion

        #region IEnumerable<char> members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" />
        /// object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" />
        /// that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<char> GetEnumerator()
        {
            return new CharSetEnumerator(this);
        }

        #endregion

        #region IEquatable<CharSet> members

        /// <summary>
        /// Indicates whether the current object is equal
        /// to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.
        /// </param>
        /// <returns>true if the current object is equal
        /// to the <paramref name="other" /> parameter;
        /// otherwise, false.</returns>
        public bool Equals
            (
                [NotNull] CharSet other
            )
        {
            Code.NotNull(other, "other");

            return BitArrayUtility.AreEqual
                (
                    _data,
                    other._data
                );
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return new string(ToArray());
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance,
        /// suitable for use in hashing algorithms
        /// and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified
        /// <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with
        /// the current object.</param>
        /// <returns><c>true</c> if the specified
        /// <see cref="System.Object" /> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            CharSet charset = obj as CharSet;
            if (charset == null)
            {
                return false;
            }

            return BitArrayUtility.AreEqual
                (
                    _data,
                    charset._data
                );
        }

        #endregion
    }
}
