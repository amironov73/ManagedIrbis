// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StringSlice.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct StringSlice
    {
        #region Properties

        /// <summary>
        /// Text.
        /// </summary>
        [NotNull]
        public string Text { get; private set; }

        /// <summary>
        /// Offset.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// Length.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Whether the slice is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return Length == 0; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StringSlice
            (
                [NotNull] string text,
                int offset,
                int length
            )
            : this()
        {
            Code.NotNull(text, "text");
            Code.Nonnegative(offset, "offset");
            Code.Nonnegative(length, "length");

            Text = text;
            Offset = offset;
            Length = length;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Equality.
        /// </summary>
        public static bool operator ==
            (
                StringSlice left,
                [CanBeNull] string right
            )
        {
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            int length = left.Length;
            if (length != right.Length)
            {
                return false;
            }

            string text = left.Text;
            int offset = left.Offset;
            for (int i = 0; i < length; i++)
            {
                if (text[offset + i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Inequality.
        /// </summary>
        public static bool operator !=
            (
                StringSlice left,
                [CanBeNull] string right
            )
        {
            if (ReferenceEquals(right, null))
            {
                return true;
            }

            int length = left.Length;
            if (length != right.Length)
            {
                return true;
            }

            string text = left.Text;
            int offset = left.Offset;
            for (int i = 0; i < length; i++)
            {
                if (text[offset + i] != right[i])
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Equality.
        /// </summary>
        public bool Equals
            (
                StringSlice other
            )
        {
            int length = Length;
            if (length != other.Length)
            {
                return false;
            }

            int myOffset = Offset, otherOffset = other.Offset;
            string myText = Text, otherText = other.Text;
            for (int i = 0; i < length; i++)
            {
                if (myText[myOffset + i] != otherText[otherOffset + i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is StringSlice && Equals((StringSlice) obj);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 0, offset = Offset, length = Length;
                string text = Text;
                for (int i = 0; i < length; i++)
                {
                    result = (result * 397) ^ text[offset + i];
                }

                return result;
            }
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Text.Substring(Offset, Length);
        }

        #endregion
    }
}
