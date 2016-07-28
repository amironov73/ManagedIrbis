/* NumberRange.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !NETCORE

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Ranges
{
    /// <summary>
    /// Диапазон чисел, содержащих нечисловые фрагменты.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NumberRange
        : IEnumerable<NumberText>
    {
        #region Properties

        /// <summary>
        /// Стартовое значение.
        /// </summary>
        public NumberText Start { get; set; }

        /// <summary>
        /// Стоповое значение.
        /// </summary>
        public NumberText Stop { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public NumberRange()
        {
        }

        /// <summary>
        /// Конструктор для диапазона, состоящего
        /// из одного числа.
        /// </summary>
        public NumberRange
            (
                NumberText startAndStop
            )
        {
            Start = startAndStop;
            Stop = startAndStop;
        }

        /// <summary>
        /// Конструктор для произвольного диапазона.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        public NumberRange
            (
                NumberText start, 
                NumberText stop
            )
        {
            Start = start;
            Stop = stop;
        }

        internal NumberRange
            (
                NumberRangesParser.ItemContext itemContext
            )
        {
            NumberRangesParser.OneContext oneContext = itemContext.one();
            if (!ReferenceEquals(oneContext, null))
            {
                string number = oneContext.NUMBER().GetText();
                Start = number;
                Stop = number;
            }

            NumberRangesParser.RangeContext rangeContext = itemContext.range();
            if (!ReferenceEquals(rangeContext, null))
            {
                Start = rangeContext.start.Text;
                Stop = rangeContext.stop.Text;
            }
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка, содержит ли диапазон указанное значение.
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
            if (ReferenceEquals(Start, null))
            {
                throw new ArsMagnaException("Start is null");
            }
            if (ReferenceEquals(Stop, null))
            {
                throw new ArsMagnaException("Stop is null");
            }
            return ((Start.CompareTo(number) <= 0)
                    && (number.CompareTo(Stop)) <= 0);
        }

        /// <summary>
        /// Разбор текстового представления диапазона.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static NumberRange Parse
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            NumberRange result;

            if (text.Contains("-"))
            {
                string[] parts = text.Split('-');
                if (parts.Length != 2)
                {
                    throw new FormatException("text");
                }
                parts[0] = parts[0].Trim();
                parts[1] = parts[1].Trim();
                if (string.IsNullOrEmpty(parts[0])
                    || string.IsNullOrEmpty(parts[1]))
                {
                    throw new FormatException("text");
                }
                result = new NumberRange
                    (
                        parts[0],
                        parts[1]
                    );
            }
            else
            {
                result = new NumberRange(text);
            }
            return result;
        }

        /// <summary>
        /// Выполнение указанного действия на всём диапазоне.
        /// </summary>
        /// <param name="action"></param>
        public void For
            (
                Action<NumberText> action
            )
        {
            // ReSharper disable NotResolvedInText
            if (ReferenceEquals(action, null))
            {
                throw new ArgumentNullException("action");
            }
            if (ReferenceEquals(Start, null))
            {
                throw new ArgumentNullException("Start");
            }
            if (ReferenceEquals(Stop, null))
            {
                throw new ArgumentNullException("Stop");
            }
            // ReSharper restore NotResolvedInText

            for (
                    NumberText current = Start; 
                    current.CompareTo(Stop) <= 0; 
                    current = current.Increment()
                )
            {
                action
                    (
                        current
                    );
            }
        }

        /// <summary>
        /// Пересечение двух диапазонов.
        /// </summary>
        public NumberRange Intersect
            (
                NumberRange other
            )
        {
            return new NumberRange
                (
                    Stop,
                    other.Start
                );
        }

        /// <summary>
        /// Проверка, не пустой ли диапазон.
        /// </summary>
        public bool IsEmpty ()
        {
            return (Start > Stop);
        }

        /// <summary>
        /// Объединение двух диапазонов.
        /// </summary>
        public NumberRange Union
            (
                NumberRange other
            )
        {
            return new NumberRange
                (
                    NumberText.Min(Start, other.Start),
                    NumberText.Max(Stop, other.Stop)
                );
        }

        #endregion

        #region IEnumerable<NumberText> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<NumberText> GetEnumerator()
        {
            if (ReferenceEquals(Start, null))
            {
                throw new ArsMagnaException("Start is null");
            }
            if (ReferenceEquals(Stop, null))
            {
                throw new ArsMagnaException("Stop is null");
            }
            for (
                    NumberText current = Start;
                    current.CompareTo(Stop) <= 0;
                    current = current.Increment()
                )
            {
                yield return current;
            }
            
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            if (ReferenceEquals(Start, null)
                && ReferenceEquals(Stop, null))
            {
                return string.Empty;
            }
            if (ReferenceEquals(Start, null))
            {
                return Stop.ToString();
            }
            if (ReferenceEquals(Stop, null))
            {
                return Start.ToString();
            }

            if (Start.CompareTo(Stop) == 0)
            {
                return Start.ToString();
            }

            return string.Format
                (
                    "{0}-{1}",
                    Start,
                    Stop
                );
        }

        #endregion
    }
}

#endif