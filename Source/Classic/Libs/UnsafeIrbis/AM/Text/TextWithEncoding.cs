// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextWithEncoding.cs -- text with given encoding
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Text;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Text
{
    /// <summary>
    /// Text with given encoding.
    /// </summary>
    [PublicAPI]
    [DebuggerDisplay("Text={Text} Encoding={Encoding}")]
    public sealed class TextWithEncoding
        : IComparable<TextWithEncoding>
    {
        #region Properties

        /// <summary>
        /// Text itself.
        /// </summary>
        [CanBeNull]
        public string Text { get; private set; }

        /// <summary>
        /// Encoding.
        /// </summary>
        /// <remarks><c>null</c> treated as default encoding.</remarks>
        [CanBeNull]
        public Encoding Encoding { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TextWithEncoding()
        {
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Constructor. UTF-8 encoded text.
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
        /// Constructor. UTF-8 or ANSI encoded text.
        /// </summary>
        public TextWithEncoding
            (
                [CanBeNull] string text,
                bool ansi
            )
        {
            Text = text;
            Encoding = ansi
                ? EncodingUtility.DefaultEncoding
                : Encoding.UTF8;
        }

        /// <summary>
        /// Constructor. Explicitly specified encoding.
        /// </summary>
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

        #region Public methods

        /// <summary>
        /// Convert text to byte representation.
        /// </summary>
        [NotNull]
        public byte[] ToBytes()
        {
            if (ReferenceEquals(Text, null))
            {
                return new byte[0];
            }

            Encoding encoding = Encoding
                ?? EncodingUtility.DefaultEncoding;

            return encoding.GetBytes(Text);
        }

        /// <summary>
        /// Implicit conversion.
        /// </summary>
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

        /// <inheritdoc cref="IComparable{T}.CompareTo" />
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
        /// Compare two texts.
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
        /// Compare two texts.
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

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is TextWithEncoding
                && Equals((TextWithEncoding) obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return ReferenceEquals(Text, null)
                ? 0
                : Text.GetHashCode();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Text ?? string.Empty;
        }

        #endregion
    }
}
