/* TextWithEncoding.cs -- text with given encoding
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
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
    /// Text with given encoding.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Text={Text} Encoding={Encoding}")]
#endif
    public sealed class TextWithEncoding
        : IComparable<TextWithEncoding>
    {
        #region Properties

        /// <summary>
        /// Text itself.
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        /// <summary>
        /// Encoding.
        /// </summary>
        /// <remarks><c>null</c> treated as default encoding.</remarks>
        [CanBeNull]
        public Encoding Encoding { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TextWithEncoding()
        {
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
                ? Encoding.GetEncoding(0)
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

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert text to byte representation.
        /// </summary>
        [NotNull]
        public byte[] ToBytes()
        {
            if (Text == null)
            {
                return new byte[0];
            }

            Encoding encoding = Encoding 
                ?? Encoding.GetEncoding(0);
            
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
