/* NumberRangeCollection.cs -- набор диапазонов чисел.
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
using System.Text;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;


#endregion

namespace AM.Text.Ranges
{
    /// <summary>
    /// Набор диапазонов чисел.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Count={Count}")]
#endif
    public sealed class NumberRangeCollection
        : IEnumerable<NumberText>,
        IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Разделитель по умолчанию.
        /// </summary>
        public const string DefaultDelimiter = ",";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collection item count.
        /// </summary>
        public int Count { get { return _items.Count; } }

        /// <summary>
        /// Разделитель диапазонов.
        /// </summary>
        public string Delimiter { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public NumberRangeCollection()
        {
            Delimiter = DefaultDelimiter;
            _items = new List<NumberRange>();
        }

        #endregion

        #region Private members

        private readonly List<NumberRange> _items;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление диапазона в набор.
        /// </summary>
        [NotNull]
        public NumberRangeCollection Add
            (
                [NotNull] NumberRange range
            )
        {
            Code.NotNull(range, "range");

            _items.Add(range);

            return this;
        }

        /// <summary>
        /// Добавление диапазона в набор.
        /// </summary>
        [NotNull]
        public NumberRangeCollection Add
            (
                [NotNull] string start,
                [NotNull] string stop
            )
        {
            Code.NotNullNorEmpty(start, "start");
            Code.NotNullNorEmpty(stop, "stop");

            NumberRangeCollection result = Add
                (
                    new NumberRange(start, stop)
                );

            return result;
        }

        /// <summary>
        /// Добавление диапазона в набор.
        /// </summary>
        [NotNull]
        public NumberRangeCollection Add
            (
                [NotNull] string startAndStop
            )
        {
            Code.NotNullNorEmpty(startAndStop, "startAndStop");

            NumberRangeCollection result = Add
                (
                    new NumberRange(startAndStop)
                );

            return result;
        }

        /// <summary>
        /// Проверка, содержит ли набор указанное число.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool Contains
            (
                [NotNull] NumberText number
            )
        {
            Code.NotNull(number, "number");

            bool result = _items.Any
                (
                    item => item.Contains(number)
                );

            return result;
        }

        /// <summary>
        /// Parse the text representation
        /// </summary>
        [NotNull]
        public static NumberRangeCollection Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            TextNavigator navigator = new TextNavigator(text);
            navigator.SkipWhile(NumberRange.Delimiters);
            if (navigator.IsEOF)
            {
                throw new FormatException();
            }

            NumberRangeCollection result = new NumberRangeCollection();

            while (true)
            {
                navigator.SkipWhile(NumberRange.Delimiters);
                if (navigator.IsEOF)
                {
                    break;
                }

                string start = navigator
                    .ReadUntil(NumberRange.DelimitersOrMinus);
                NumberRange range;
                if (string.IsNullOrEmpty(start))
                {
                    throw new FormatException();
                }
                navigator.SkipWhitespace();
                if (navigator.PeekChar() == '-')
                {
                    navigator.ReadChar();
                    navigator.SkipWhitespace();
                    string stop = navigator
                        .ReadUntil(NumberRange.Delimiters);
                    if (string.IsNullOrEmpty(stop))
                    {
                        throw new FormatException();
                    }

                    range = new NumberRange(start, stop);
                }
                else
                {
                    range = new NumberRange(start);
                }
                result.Add(range);
            }

            return result;
        }

        /// <summary>
        /// Кумуляция (сжатие).
        /// </summary>
        [NotNull]
        public static NumberRangeCollection Cumulate
            (
                [NotNull] List<NumberText> numbers
            )
        {
            Code.NotNull(numbers, "numbers");

            NumberRangeCollection result
                = new NumberRangeCollection();

            if (numbers.Count != 0)
            {
                numbers.Sort();

                NumberText previous = numbers[0];
                NumberText last = previous.Clone();
                for (int i = 1; i < numbers.Count; i++)
                {
                    NumberText current = numbers[i];
                    NumberText next = last + 1;
                    if (current != next)
                    {
                        result.Add
                            (
                                new NumberRange
                                    (
                                        previous,
                                        last
                                    )
                            );
                        previous = current.Clone();
                    }
                    last = current;
                }
                result.Add
                    (
                        new NumberRange
                            (
                                previous,
                                last
                            )
                    );
            }

            return result;
        }

        /// <summary>
        /// Кумуляция (сжатие).
        /// </summary>
        [NotNull]
        public static NumberRangeCollection Cumulate
            (
                [NotNull] IEnumerable<string> texts
            )
        {
            Code.NotNull(texts, "texts");

            List<NumberText> numbers = texts
                .Select(text => new NumberText(text))
                .ToList();

            return Cumulate(numbers);
        }

        /// <summary>
        /// Выполнение указанного действия
        /// на всех диапазонах набора.
        /// </summary>
        /// <param name="action"></param>
        public void For
            (
                Action<NumberText> action
            )
        {
            foreach (NumberRange range in _items)
            {
                foreach (NumberText number in range)
                {
                    action
                        (
                            number
                        );
                }
            }
        }

        #endregion

        #region IEnumerable<NumberText> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through
        /// the collection.
        /// </summary>
        public IEnumerator<NumberText> GetEnumerator()
        {
            foreach (NumberRange range in _items)
            {
                foreach (NumberText number in range)
                {
                    yield return number;
                }
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            _items.Clear();
            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                NumberRange item = new NumberRange();
                item.RestoreFromStream(reader);
                _items.Add(item);
            }
        }

        /// <summary>
        /// Save object state to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WritePackedInt32(_items.Count);
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].SaveToStream(writer);
            }
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
            StringBuilder result = new StringBuilder();
            bool first = true;
            
            foreach (NumberRange item in _items)
            {
                string text = item.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    if (!first)
                    {
                        result.Append(Delimiter);
                    }
                    result.Append(text);
                    first = false;
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
