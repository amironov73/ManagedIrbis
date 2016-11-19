/* FieldSpecification -- field/subfield specifcation.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Field/subfield specification.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FieldSpecification2
    {
        #region Properties

        /// <summary>
        /// Command code (must be lowercase).
        /// </summary>
        public char Command { get; set; }

        /// <summary>
        /// Embedded field tag.
        /// </summary>
        [CanBeNull]
        public string Embedded { get; set; }

        /// <summary>
        /// Красная строка.
        /// </summary>
        public int FirstLine { get; set; }

        /// <summary>
        /// Общий абзацный отступ.
        /// </summary>
        public int ParagraphIndent { get; set; }

        /// <summary>
        /// Смещение.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Длина.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Field repeat.
        /// </summary>
        public IndexSpecification FieldRepeat { get; set; }

        /// <summary>
        /// Subfield.
        /// </summary>
        public char SubField { get; set; }

        /// <summary>
        /// Subfield repeat.
        /// </summary>
        public IndexSpecification SubFieldRepeat { get; set; }

        /// <summary>
        /// Tag.
        /// </summary>
        [CanBeNull]
        public string Tag { get; set; }

        /// <summary>
        /// Unparsed field specification.
        /// </summary>
        [CanBeNull]
        public string RawText { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specification from navigator.
        /// </summary>
        public bool Parse
            (
                [NotNull] TextNavigator navigator
            )
        {
            Code.NotNull(navigator, "navigator");

            int start = navigator.Position;
            TextPosition saved = navigator.SavePosition();
            char c = navigator.ReadChar();
            StringBuilder builder = new StringBuilder();

            switch (c)
            {
                case 'd':
                case 'D':
                    Command = 'd';
                    break;

                case 'g':
                case 'G':
                    Command = 'g';
                    break;

                case 'n':
                case 'N':
                    Command = 'n';
                    break;

                case 'v':
                case 'V':
                    Command = 'v';
                    break;

                // TODO: support $x

                default:
                    navigator.RestorePosition(saved);
                    return false;
            } // switch

            c = navigator.ReadChar();
            if (!c.IsArabicDigit())
            {
                return false;
            }
            builder.Append(c);

            while (true)
            {
                c = navigator.PeekChar();
                if (!c.IsArabicDigit())
                {
                    break;
                }
                navigator.ReadChar();
                builder.Append(c);
            }
            Tag = builder.ToString();

            // now c is peeked char

            if (c == '@')
            {
                builder.Length = 0;
                navigator.ReadChar();

                while (true)
                {
                    c = navigator.PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadChar();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Embedded = builder.ToString();
            } // c == '@'

            // TODO here parse the field repeat

            // c still is peeked char

            if (c == '^')
            {
                navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    throw new PftSyntaxException(navigator);
                }
                c = navigator.ReadChar();
                if (!SubFieldCode.IsValidCode(c))
                {
                    throw new PftSyntaxException(navigator);
                }
                SubField = SubFieldCode.Normalize(c);

                c = navigator.PeekChar();

                // TODO here parse subfield repeat

            } // c == '^'

            if (Command != 'v')
            {
                goto DONE;
            }
            // c still is peeked char

            if (c == '*')
            {
                navigator.ReadChar();
                builder.Length = 0;

                while (true)
                {
                    c = navigator.PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadChar();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }

                Offset = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );
            } // c == '*'

            if (c == '.')
            {
                navigator.ReadChar();
                builder.Length = 0;

                while (true)
                {
                    c = navigator.PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadChar();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }

                Length = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );

                if (navigator.PeekChar() == '*')
                {
                    throw new PftSyntaxException(navigator);
                }
            } // c == '.'

            if (c == '(')
            {
                navigator.ReadChar();
                builder.Length = 0;

                while (true)
                {
                    c = navigator.PeekChar();
                    if (c == ')')
                    {
                        navigator.ReadChar();
                        c = navigator.PeekChar();
                        break;
                    }
                    if (!c.IsArabicDigit())
                    {
                        throw new PftSyntaxException(navigator);
                    }
                    navigator.ReadChar();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                ParagraphIndent = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );
            } // c == '('

DONE:
            int length = navigator.Position - start;
            RawText = navigator.Substring(start, length);

            return true;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(Command);
            result.Append(Tag);
            if (!string.IsNullOrEmpty(Embedded))
            {
                result.Append('@');
                result.Append(Embedded);
            }
            if (SubField != '\0')
            {
                result.Append('^');
                result.Append(SubField);
            }
            if (Offset != 0)
            {
                result.Append('*');
                result.Append(Offset);
            }
            if (Length != 0)
            {
                result.Append('.');
                result.Append(Length);
            }
            if (ParagraphIndent != 0)
            {
                result.Append('(');
                result.Append(ParagraphIndent);
                result.Append(')');
            }

            return result.ToString();
        }


        #endregion
    }
}
