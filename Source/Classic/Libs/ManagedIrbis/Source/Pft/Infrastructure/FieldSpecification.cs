// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldSpecification -- field/subfield specification.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Serialization;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Field/subfield specification.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FieldSpecification
        : ICloneable
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
        public int Tag { get; set; }

        /// <summary>
        /// Tag specification.
        /// </summary>
        [CanBeNull]
        public string TagSpecification { get; set; }

        /// <summary>
        /// Subfield specification.
        /// </summary>
        [CanBeNull]
        public string SubFieldSpecification { get; set; }

        /// <summary>
        /// Unparsed field specification.
        /// </summary>
        [CanBeNull]
        public string RawText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool ParseSubFieldSpecification { get; set; }

        #endregion

        #region Construction

        static FieldSpecification()
        {
            ParseSubFieldSpecification = true;
        }

        #endregion

        #region Private members

        private static char[] _openChars = { '[' };
        private static char[] _closeChars = { ']' };
        private static char[] _stopChars = { ']' };

        private IndexSpecification _ParseIndex
            (
                [NotNull] TextNavigator navigator,
                [NotNull] string text
            )
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                Log.Error
                    (
                        "FieldSpecification::_ParseIndex: "
                        + "text="
                        + text.ToVisibleString()
                    );

                throw new PftSyntaxException(navigator);
            }

            IndexSpecification result = new IndexSpecification
            {
                Expression = text
            };

            if (text == "*")
            {
                result.Kind = IndexKind.LastRepeat;
            }
            else if (text == "+")
            {
                result.Kind = IndexKind.NewRepeat;
            }
            else if (text == "-")
            {
                result.Kind = IndexKind.AllRepeats;
            }
            else if (text == ".")
            {
                result.Kind = IndexKind.CurrentRepeat;
            }
            else
            {
                int index;
                if (NumericUtility.TryParseInt32(text, out index))
                {
                    result.Kind = IndexKind.Literal;
                    result.Literal = index;
                }
                else
                {
                    result.Kind = IndexKind.Expression;
                }
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare two specifications.
        /// </summary>
        public static bool Compare
            (
                [NotNull] FieldSpecification left,
                [NotNull] FieldSpecification right
            )
        {
            bool result = left.Command == right.Command
                && PftSerializationUtility.CompareStrings
                    (
                        left.Embedded, right.Embedded
                    )
                && left.FirstLine == right.FirstLine
                && left.ParagraphIndent == right.ParagraphIndent
                && left.Offset == right.Offset
                && left.Length == right.Length
                && IndexSpecification.Compare
                    (
                        left.FieldRepeat,
                        right.FieldRepeat
                    )
                && left.SubField == right.SubField
                && IndexSpecification.Compare
                    (
                        left.SubFieldRepeat,
                        right.SubFieldRepeat
                    )
                && left.Tag == right.Tag
                && PftSerializationUtility.CompareStrings
                    (
                        left.TagSpecification,
                        right.TagSpecification
                    )
                && PftSerializationUtility.CompareStrings
                    (
                        left.SubFieldSpecification,
                        right.SubFieldSpecification
                    );

            return result;
        }

        /// <summary>
        /// Deserialize the specification.
        /// </summary>
        public void Deserialize
            (
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Command = reader.ReadChar();
            Embedded = reader.ReadNullableString();
            FirstLine = reader.ReadPackedInt32();
            ParagraphIndent = reader.ReadPackedInt32();
            Offset = reader.ReadPackedInt32();
            Length = reader.ReadPackedInt32();
            FieldRepeat.Deserialize(reader);
            SubField = reader.ReadChar();
            SubFieldRepeat.Deserialize(reader);
            Tag = reader.ReadPackedInt32();
            TagSpecification = reader.ReadNullableString();
            SubFieldSpecification = reader.ReadNullableString();
            RawText = reader.ReadNullableString();
            ParseSubFieldSpecification = reader.ReadBoolean();
        }

        /// <summary>
        /// Parse the specification from text.
        /// </summary>
        public bool Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            TextNavigator navigator = new TextNavigator(text);

            return Parse(navigator);
        }

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

                default:
                    navigator.RestorePosition(saved);
                    return false;
            } // switch

            c = navigator.ReadCharNoCrLf();

            if (c == '[')
            {
                string text = navigator.ReadUntil
                    (
                        _openChars,
                        _closeChars,
                        _stopChars
                    );
                if (ReferenceEquals(text, null))
                {
                    Log.Error
                        (
                            "FieldSpecification::Parse: "
                            + "unclosed ["
                        );

                    throw new PftSyntaxException(navigator);
                }

                text = text.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    Log.Error
                        (
                            "FieldSpecification::Parse: "
                            + "empty []"
                        );

                    throw new PftSyntaxException(navigator);
                }

                TagSpecification = text;

                navigator.ReadCharNoCrLf();
            }
            else
            {
                if (!c.IsArabicDigit())
                {
                    return false;
                }
                builder.Append(c);

                while (true)
                {
                    c = navigator.PeekCharNoCrLf();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadCharNoCrLf();
                    builder.Append(c);
                }
                Tag = NumericUtility.ParseInt32(builder.ToString());
            }

            navigator.SkipWhitespace();
            c = navigator.PeekCharNoCrLf();

            // now c is peeked char

            if (c == '@')
            {
                builder.Length = 0;
                navigator.ReadCharNoCrLf();

                navigator.SkipWhitespace();

                while (true)
                {
                    c = navigator.PeekCharNoCrLf();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadCharNoCrLf();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Embedded = builder.ToString();
            } // c == '@'

            navigator.SkipWhitespace();
            c = navigator.PeekCharNoCrLf();

            if (c == '[')
            {
                // parse the field repeat

                navigator.ReadCharNoCrLf();
                navigator.SkipWhitespace();

                string text = navigator.ReadUntil
                    (
                        _openChars,
                        _closeChars,
                        _stopChars
                    );
                if (ReferenceEquals(text, null))
                {
                    Log.Error
                        (
                            "FieldSpecification::Parse: "
                            + "unclosed ["
                        );

                    throw new PftSyntaxException(navigator);
                }

                FieldRepeat = _ParseIndex
                    (
                        navigator,
                        text
                    );

                navigator.ReadCharNoCrLf();
            }

            navigator.SkipWhitespace();
            c = navigator.PeekCharNoCrLf();

            if (c == '^')
            {
                navigator.ReadCharNoCrLf();
                if (navigator.IsEOF)
                {
                    throw new PftSyntaxException(navigator);
                }
                
                c = navigator.ReadCharNoCrLf();

                if (c == '['
                    & ParseSubFieldSpecification)
                {
                    string text = navigator.ReadUntil
                        (
                            _openChars,
                            _closeChars,
                            _stopChars
                        );
                    if (ReferenceEquals(text, null))
                    {
                        SubField = c;
                    }
                    else
                    {
                        SubFieldSpecification = text;

                        navigator.ReadCharNoCrLf();
                    }
                }
                else
                {

                    if (!SubFieldCode.IsValidCode(c))
                    {
                        Log.Error
                            (
                                "FieldSpecification::Parse: "
                                + "unexpected code="
                                + c.ToVisibleString()
                            );

                        throw new PftSyntaxException(navigator);
                    }
                    SubField = SubFieldCode.Normalize(c);
                }

                navigator.SkipWhitespace();
                c = navigator.PeekCharNoCrLf();

                // parse subfield repeat

                if (c == '[')
                {
                    navigator.ReadCharNoCrLf();
                    navigator.SkipWhitespace();

                    string text = navigator.ReadUntil
                        (
                            _openChars,
                            _closeChars,
                            _stopChars
                        );
                    if (ReferenceEquals(text, null))
                    {
                        Log.Error
                            (
                                "FieldSpecification::Parse: "
                                + "unclosed ["
                            );

                        throw new PftSyntaxException(navigator);
                    }

                    SubFieldRepeat = _ParseIndex
                        (
                            navigator,
                            text
                        );

                    navigator.ReadCharNoCrLf();
                }
            } // c == '^'

            if (Command != 'v'
                && Command != 'g')
            {
                goto DONE;
            }

            navigator.SkipWhitespace();
            c = navigator.PeekCharNoCrLf();

            if (c == '*')
            {
                navigator.ReadCharNoCrLf();
                navigator.SkipWhitespace();
                builder.Length = 0;

                while (true)
                {
                    c = navigator.PeekCharNoCrLf();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadCharNoCrLf();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    Log.Error
                        (
                            "FieldSpecification: "
                            + "empty offset"
                        );

                    throw new PftSyntaxException(navigator);
                }

                Offset = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );
            } // c == '*'

            navigator.SkipWhitespace();
            c = navigator.PeekCharNoCrLf();

            if (c == '.')
            {
                navigator.ReadCharNoCrLf();
                navigator.SkipWhitespace();
                builder.Length = 0;

                while (true)
                {
                    c = navigator.PeekCharNoCrLf();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadCharNoCrLf();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    Log.Error
                        (
                            "FieldSpecification::Parse: "
                            + "empty length"
                        );

                    throw new PftSyntaxException(navigator);
                }

                Length = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );

                if (navigator.PeekChar() == '*')
                {
                    Log.Error
                        (
                            "FieldSpecification::Parse: "
                            + "offset after length"
                        );

                    throw new PftSyntaxException(navigator);
                }
            } // c == '.'

            navigator.SkipWhitespace();
            c = navigator.PeekCharNoCrLf();

            if (c == '(')
            {
                navigator.ReadCharNoCrLf();
                navigator.SkipWhitespace();
                builder.Length = 0;

                while (true)
                {
                    c = navigator.PeekCharNoCrLf();
                    if (c == ')')
                    {
                        navigator.ReadCharNoCrLf();
                        break;
                    }
                    if (!c.IsArabicDigit())
                    {
                        Log.Error
                            (
                                "FieldSpecification::Parse: "
                                + "unexpected character="
                                + c.ToVisibleString()
                            );

                        throw new PftSyntaxException(navigator);
                    }
                    navigator.ReadCharNoCrLf();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    Log.Error
                        (
                            "FieldSpecification::Parse: "
                            + "empty paragraph indent"
                        );

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

        /// <summary>
        /// Parse short specification from text.
        /// </summary>
        public bool ParseShort
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            TextNavigator navigator = new TextNavigator(text);
            return ParseShort(navigator);
        }

        /// <summary>
        /// Parse short specification from navigator.
        /// </summary>
        public bool ParseShort
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
                case 'g':
                case 'G':
                    Command = 'g';
                    break;

                case 'v':
                case 'V':
                    Command = 'v';
                    break;

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
            Tag = NumericUtility.ParseInt32(builder.ToString());

            navigator.SkipWhitespace();
            c = navigator.PeekChar();

            if (c == '^')
            {
                navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    Log.Error
                        (
                            "FieldSpecification::ParseShort: "
                            + "unexpected end of stream"
                        );

                    throw new PftSyntaxException(navigator);
                }

                c = navigator.ReadChar();
                if (!SubFieldCode.IsValidCode(c))
                {
                    Log.Error
                        (
                            "FieldSpecification::ParseShort: "
                            + "unexpected code="
                            + c.ToVisibleString()
                        );

                    throw new PftSyntaxException(navigator);
                }

                SubField = SubFieldCode.Normalize(c);

                /* c = navigator.PeekChar(); */
            } // c == '^'

            int length = navigator.Position - start;
            RawText = navigator.Substring(start, length);

            return true;
        }

        /// <summary>
        /// Parse specification for Unifor.
        /// </summary>
        public bool ParseUnifor
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            TextNavigator navigator = new TextNavigator(text);

            return ParseUnifor(navigator);
        }

        /// <summary>
        /// Parse short specification for Unifor from navigator.
        /// </summary>
        public bool ParseUnifor
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
                case 'v':
                case 'V':
                    Command = 'v';
                    break;

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
            Tag = NumericUtility.ParseInt32(builder.ToString());

            // now c is peeked char

            if (c == '^')
            {
                navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    Log.Error
                        (
                            "FieldSpecification::ParseUnifor: "
                            + "unexpected end of stream"
                        );

                    throw new PftSyntaxException(navigator);
                }

                c = navigator.ReadChar();
                if (!SubFieldCode.IsValidCode(c))
                {
                    Log.Error
                        (
                            "FieldSpecification::ParseUnifor: "
                            + "unexpected code="
                            + c.ToVisibleString()
                        );

                    throw new PftSyntaxException(navigator);
                }

                SubField = SubFieldCode.Normalize(c);

                c = navigator.PeekChar();
            } // c == '^'

            if (c == '*')
            {
                navigator.ReadCharNoCrLf();
                navigator.SkipWhitespace();
                builder.Length = 0;

                while (true)
                {
                    c = navigator.PeekCharNoCrLf();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadCharNoCrLf();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    Log.Error
                        (
                            "FieldSpecification: "
                            + "empty offset"
                        );

                    throw new PftSyntaxException(navigator);
                }

                Offset = int.Parse
                (
                    builder.ToString(),
                    CultureInfo.InvariantCulture
                );

                c = navigator.PeekChar();
            } // c == '*'

            if (c == '.')
            {
                navigator.ReadCharNoCrLf();
                navigator.SkipWhitespace();
                builder.Length = 0;

                while (true)
                {
                    c = navigator.PeekCharNoCrLf();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadCharNoCrLf();
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    Log.Error
                        (
                            "FieldSpecification::Parse: "
                            + "empty length"
                        );

                    throw new PftSyntaxException(navigator);
                }

                Length = int.Parse
                (
                    builder.ToString(),
                    CultureInfo.InvariantCulture
                );

                if (navigator.PeekChar() == '*')
                {
                    Log.Error
                        (
                            "FieldSpecification::Parse: "
                            + "offset after length"
                        );

                    throw new PftSyntaxException(navigator);
                }
            } // c == '.'

            if (c == '#')
            {
                navigator.ReadChar();

                bool minus = navigator.PeekChar() == '-';
                if (minus)
                {
                    navigator.ReadChar();
                }
                string indexText = navigator.ReadInteger();
                if (string.IsNullOrEmpty(indexText))
                {
                    Log.Error
                        (
                            "FieldSpecification::ParseUnifor: "
                            + "empty index"
                        );

                    throw new PftSyntaxException(navigator);
                }

                int indexValue = int.Parse(indexText);
                if (minus)
                {
                    indexValue = -indexValue;
                }
                FieldRepeat = new IndexSpecification
                {
                    Kind = IndexKind.Literal,
                    Expression = indexText,
                    Literal = indexValue
                };
            }

            int length = navigator.Position - start;
            RawText = navigator.Substring(start, length);

            return true;
        }

        /// <summary>
        /// Serialize the specification.
        /// </summary>
        public void Serialize
            (
                [NotNull] BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.Write(Command);
            writer.WriteNullable(Embedded);
            writer.WritePackedInt32(FirstLine);
            writer.WritePackedInt32(ParagraphIndent);
            writer.WritePackedInt32(Offset);
            writer.WritePackedInt32(Length);
            FieldRepeat.Serialize(writer);
            writer.Write(SubField);
            SubFieldRepeat.Serialize(writer);
            writer.WritePackedInt32(Tag);
            writer.WriteNullable(TagSpecification);
            writer.WriteNullable(SubFieldSpecification);
            writer.WriteNullable(RawText);
            writer.Write(ParseSubFieldSpecification);
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            FieldSpecification result
                = (FieldSpecification) MemberwiseClone();

            result.FieldRepeat = (IndexSpecification) FieldRepeat.Clone();
            result.SubFieldRepeat
                = (IndexSpecification) SubFieldRepeat.Clone();

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
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
