/* NumberRangeCollection.cs -- набор диапазонов чисел.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public sealed class NumberRangeCollection
        : IEnumerable<NumberText>
    {
        #region Constants

        /// <summary>
        /// Разделитель по умолчанию.
        /// </summary>
        public const string DefaultDelimiter = ",";

        #endregion

        #region Properties

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
        /// <param name="range"></param>
        /// <returns></returns>
        public NumberRangeCollection Add
            (
                NumberRange range
            )
        {
            if (ReferenceEquals(range, null))
            {
                throw new ArgumentNullException("range");
            }
            _items.Add(range);
            return this;
        }

        /// <summary>
        /// Добавление диапазона в набор.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public NumberRangeCollection Add
            (
                string start,
                string stop
            )
        {
            if (string.IsNullOrEmpty(start))
            {
                throw new ArgumentNullException("start");
            }
            if (string.IsNullOrEmpty(stop))
            {
                throw new ArgumentNullException("stop");
            }

            return Add
                (
                    new NumberRange(start, stop)
                );
        }

        /// <summary>
        /// Добавление диапазона в набор.
        /// </summary>
        /// <param name="startAndStop"></param>
        /// <returns></returns>
        public NumberRangeCollection Add
            (
                string startAndStop
            )
        {
            if (string.IsNullOrEmpty(startAndStop))
            {
                throw new ArgumentNullException("startAndStop");
            }
            return Add
                (
                    new NumberRange(startAndStop)
                );
        }

        /// <summary>
        /// Проверка, содержит ли набор указанное число.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool Contains
            (
                NumberText number
            )
        {
            if (ReferenceEquals(number, null))
            {
                throw new ArgumentNullException("number");
            }
            return _items.Any(item => item.Contains(number));
        }

        /// <summary>
        /// Разбор текстового представления.
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
                if (navigator.PeekChar() == '-')
                {
                    navigator.ReadChar();
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
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static NumberRangeCollection Cumulate
            (
                List<NumberText> numbers
            )
        {
            NumberRangeCollection result
                = new NumberRangeCollection();

            if (numbers.Count != 0)
            {
                numbers.Sort();

                NumberText previous = numbers[0];
                NumberText last = previous.Copy();
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
                        previous = current.Copy();
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
        public static NumberRangeCollection Cumulate
            (
                IEnumerable<string> texts
            )
        {
            if (ReferenceEquals(texts, null))
            {
                throw new ArgumentNullException("texts");
            }

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

        #region Object members

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
