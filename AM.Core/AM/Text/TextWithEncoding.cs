/* TextWithEncoding.cs -- текст с заданной кодировкой
 */

#region Using directives

using System;
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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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

        private bool Equals
            (
                [NotNull] TextWithEncoding other
            )
        {
            Code.NotNull(other, "other");

            return string.Equals(Text, other.Text);
        }

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

        public override int GetHashCode()
        {
            return (Text != null ? Text.GetHashCode() : 0);
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
