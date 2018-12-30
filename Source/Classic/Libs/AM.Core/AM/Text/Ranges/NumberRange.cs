// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NumberRange.cs --range of numbers containing non-numeric fragments
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 * TODO make Delimiters and DelimitersOrMinus read-only
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Ranges
{
    /// <summary>
    /// Range of numbers containing non-numeric fragments.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Start} - {Stop}")]
    public sealed class NumberRange
        : IEnumerable<NumberText>,
        IHandmadeSerializable,
        IEquatable<NumberRange>,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Delimiters.
        /// </summary>
        [NotNull]
        public static char[] Delimiters
        {
            get { return _delimiters; }
        }

        /// <summary>
        /// Delimiters or minus sign.
        /// </summary>
        [NotNull]
        public static char[] DelimitersOrMinus
        {
            get { return _delimitersOrMinus; }
        }

        /// <summary>
        /// Start value.
        /// </summary>
        [CanBeNull]
        public NumberText Start { get; set; }

        /// <summary>
        /// Stop value. Can coincide
        /// with <see cref="Start"/> value.
        /// </summary>
        [CanBeNull]
        public NumberText Stop { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
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

        #endregion

        #region Private members

        private static readonly char[] _delimiters
            = { ' ', '\t', '\r', '\n', ',', ';' };

        private static readonly char[] _delimitersOrMinus
            = { ' ', '\t', '\r', '\n', ',', ';', '-' };

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка, содержит ли диапазон указанное значение.
        /// </summary>
        public bool Contains
            (
                [NotNull] NumberText number
            )
        {
            Code.NotNull(number, "number");

            if (ReferenceEquals(Start, null))
            {
                Log.Error
                    (
                        "NumberRange::Contains: "
                        + "start is null"
                    );

                throw new ArsMagnaException("Start is null");
            }

            if (ReferenceEquals(Stop, null))
            {
                Log.Error
                    (
                        "NumberRange::Contains: "
                        + "stop is null"
                    );

                throw new ArsMagnaException("Stop is null");
            }

            return Start.CompareTo(number) <= 0
                   && number.CompareTo(Stop) <= 0;
        }

        /// <summary>
        /// Parse text representation.
        /// </summary>
        [NotNull]
        public static NumberRange Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            TextNavigator navigator = new TextNavigator(text);

            navigator.SkipWhile(Delimiters);
            if (navigator.IsEOF)
            {
                Log.Error
                    (
                        "NumberRange::Parse: "
                        + "unexpected end of text"
                    );

                throw new FormatException();
            }

            NumberRange result;
            string start = navigator.ReadUntil(DelimitersOrMinus);
            if (string.IsNullOrEmpty(start))
            {
                Log.Error
                    (
                        "NumberRange::Parse: "
                        + "start sequence not found"
                    );

                throw new FormatException();
            }

            navigator.SkipWhitespace();
            if (navigator.PeekChar() == '-')
            {
                navigator.ReadChar();
                navigator.SkipWhitespace();
                string stop = navigator.ReadUntil(DelimitersOrMinus);
                if (string.IsNullOrEmpty(stop))
                {
                    Log.Error
                        (
                            "NumberRange::Parse: "
                            + "stop sequence not found"
                        );

                    throw new FormatException();
                }

                result = new NumberRange(start, stop);
            }
            else
            {
                result = new NumberRange(start);
            }
            navigator.SkipWhile(Delimiters);

            if (!navigator.IsEOF)
            {
                Log.Error
                    (
                        "NumberRange::Parse: "
                        + "garbage behind range"
                    );

                throw new FormatException();
            }

            return result;
        }

        /// <summary>
        /// Выполнение указанного действия на всём диапазоне.
        /// </summary>
        public void For
            (
                [NotNull] Action<NumberText> action
            )
        {
            Code.NotNull(action, "action");

            // ReSharper disable NotResolvedInText
            if (ReferenceEquals(Start, null))
            {
                Log.Error
                    (
                        "NumberRange::Parse: "
                        + "start is null"
                    );

                throw new ArgumentNullException("Start");
            }

            if (ReferenceEquals(Stop, null))
            {
                Log.Error
                    (
                        "NumberRange::Parse: "
                        + "stop is null"
                    );

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
            // coverity[SWAPPED_ARGUMENTS]
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
            return Start > Stop;
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

        /// <summary>
        /// Returns an enumerator that iterates through
        /// the collection.
        /// </summary>
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
                    current = current.Clone().Increment()
                )
            {
                yield return current;
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
            Code.NotNull(reader, "reader");

            Start = reader.RestoreNullable<NumberText>();
            Stop = reader.RestoreNullable<NumberText>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Start)
                .WriteNullable(Stop);
        }

        #endregion

        #region IEquatable members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals
            (
                NumberRange other
            )
        {
            Code.NotNull(other, "other");

            if (ReferenceEquals(Start, null)
                || ReferenceEquals(Stop, null))
            {
                return false;
            }

            other = other.ThrowIfNull("other");

            bool result = Start.Equals(other.Start)
                   && Stop.Equals(other.Stop);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<NumberRange> verifier
                = new Verifier<NumberRange>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNull(Start, "Start")
                .NotNull(Stop, "Stop");

            if (verifier.Result)
            {
                verifier
                    .VerifySubObject(Start, "Start")
                    .VerifySubObject(Stop, "Stop")
                    .Assert
                    (
                        Start.CompareTo(Stop) <= 0,
                        "Start <= Stop"
                    );
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            int result = 0;

            if (!ReferenceEquals(Start, null))
            {
                result = Start.GetHashCode();
            }

            if (!ReferenceEquals(Stop, null))
            {
                result = result * 137 + Stop.GetHashCode();
            }

            return result;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            if (ReferenceEquals(Start, null)
                && ReferenceEquals(Stop, null))
            {
                return string.Empty;
            }

            if (ReferenceEquals(Start, null))
            {
                return Stop.ToString(); //-V3125 //-V3095
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
