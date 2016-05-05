/* TextWithEncoding.cs -- текст с заданной кодировкой
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Текст с заданной кодировкой.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Text={Text} Encoding={Encoding}")]
    public sealed class TextWithEncoding
        : IComparable<TextWithEncoding>
    {
        #region Properties

        /// <summary>
        /// Собственно текст.
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        /// <summary>
        /// Кодировка.
        /// </summary>
        [CanBeNull]
        public Encoding Encoding { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// Не заданы ни текст, ни кодировка.
        /// </summary>
        public TextWithEncoding()
        {
        }

        /// <summary>
        /// Текст с кодировкой UTF8.
        /// </summary>
        public TextWithEncoding
            (
                [CanBeNull] string text
            )
        {
            Text = text;
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Текст с кодировкой ANSI либо UTF8.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ansi"></param>
        public TextWithEncoding
            (
                [CanBeNull] string text,
                bool ansi
            )
        {
            Text = text;
            Encoding = ansi
                ? Encoding.Default
                : Encoding.UTF8;
        }

        /// <summary>
        /// Текст с явно заданной кодировкой.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        public TextWithEncoding
            (
                [CanBeNull] string text,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

            Text = text;
            Encoding = encoding;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Конверсия в байтовое представление.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public byte[] ToBytes()
        {
            if (Text == null)
            {
                return new byte[0];
            }

            Encoding encoding = Encoding 
                ?? Encoding.Default;
            
            return encoding.GetBytes(Text);
        }

        /// <summary>
        /// Неявное преобразование текста
        /// в текст с кодировкой.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [NotNull]
        public static implicit operator TextWithEncoding
            (
                [CanBeNull] string text
            )
        {
            return new TextWithEncoding
                (
                    text
                );
        }

        #endregion

        #region Comparison

        /// <summary>
        /// Compares the current object with another
        /// object of the same type.
        /// </summary>
        /// <param name="other">An object to compare
        /// with this object.</param>
        /// <returns>A value that indicates
        /// the relative order of the objects being compared.
        /// The return value has the following meanings:
        /// Value Meaning Less than zero This object is less
        /// than the <paramref name="other" /> parameter.
        /// Zero This object is equal to <paramref name="other" />.
        /// Greater than zero This object is greater than
        /// <paramref name="other" />.</returns>
        public int CompareTo
            (
                [CanBeNull] TextWithEncoding other
            )
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            return string.Compare
                (
                    Text,
                    other.Text,
                    StringComparison.CurrentCulture
                );
        }

        /// <summary>
        /// Оператор сравнения двух текстов.
        /// </summary>
        public static bool operator ==
            (
                [CanBeNull] TextWithEncoding left,
                [CanBeNull] TextWithEncoding right
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

            return left.Text == right.Text;
        }

        /// <summary>
        /// Оператор сравнения двух текстов.
        /// </summary>
        public static bool operator !=
            (
                [CanBeNull] TextWithEncoding left,
                [CanBeNull] TextWithEncoding right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return !ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return true;
            }
            return left.Text != right.Text;
        }

        /// <summary>
        /// Determines whether the specified
        /// <see cref="TextWithEncoding" /> is equal to this instance.
        /// </summary>
        private bool Equals
            (
                [NotNull] TextWithEncoding other
            )
        {
            Code.NotNull(other, "other");

            return string.Equals(Text, other.Text);
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
        public override bool Equals
            (
                [CanBeNull] object obj
            )
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is TextWithEncoding 
                && Equals((TextWithEncoding) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance,
        /// suitable for use in hashing algorithms
        /// and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (Text != null ? Text.GetHashCode() : 0);
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
            // ReSharper disable once AssignNullToNotNullAttribute
            return Text;
        }

        #endregion
    }
}
