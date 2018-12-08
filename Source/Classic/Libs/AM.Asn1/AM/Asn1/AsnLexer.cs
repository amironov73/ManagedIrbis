// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsnLexer.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text;

using AM.Collections;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Asn1
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public sealed class AsnLexer
    {
        #region Private members

        private TextNavigator _navigator;

        private static char[] _integer =
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
            };

        private static char[] _identifier =
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z',

                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
                'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
                'Y', 'Z',

                'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к',
                'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц',
                'ч', 'ш', 'щ', 'ь', 'ы', 'ъ', 'э', 'ю', 'я',

                'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К',
                'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц',
                'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ', 'Э', 'Ю', 'Я',

                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_'
            };

        private int Column { get { return _navigator.Column; } }

        private bool IsEof { get { return _navigator.IsEOF; } }

        private int Line { get { return _navigator.Line; } }

        private char PeekChar()
        {
            return _navigator.PeekChar();
        }

        private char ReadChar()
        {
            return _navigator.ReadChar();
        }

        [CanBeNull]
        private string ReadIdentifier()
        {
            if (IsEof)
            {
                return null;
            }

            StringBuilder result = new StringBuilder();
            //string[] reserved = AsnUtility.GetReservedWords();

            while (true)
            {
                char c = PeekChar();
                if (c == TextNavigator.EOF
                    || Array.IndexOf(_identifier, c) < 0)
                {
                    break;
                }

                result.Append(c);
                ReadChar();
                c = _navigator.PeekChar();
                if (c == '\r' || c == '\n')
                {
                    break;
                }
            }

            return result.ToString();
        }

        [NotNull]
        private string ReadIdentifier
            (
                char initialLetter
            )
        {
            if (IsEof)
            {
                return initialLetter.ToString();
            }

            StringBuilder result = new StringBuilder();
            result.Append(initialLetter);
            // string[] reserved = AsnUtility.GetReservedWords();

            while (true)
            {
                char c = PeekChar();
                if (c == TextNavigator.EOF
                    || Array.IndexOf(_identifier, c) < 0)
                {
                    break;
                }

                result.Append(c);
                ReadChar();
                c = _navigator.PeekChar();
                if (c == '\r' || c == '\n')
                {
                    break;
                }
            }

            return result.ToString();
        }

        [CanBeNull]
        private string ReadInteger()
        {
            StringBuilder result = new StringBuilder();

            char c = PeekChar();
            if (!c.IsArabicDigit())
            {
                return null;
            }
            result.Append(c);
            ReadChar();

            while (true)
            {
                c = PeekChar();
                if (!c.IsArabicDigit())
                {
                    break;
                }
                result.Append(c);
                ReadChar();
            }

            return result.ToString();
        }

        [CanBeNull]
        private string ReadFloat()
        {
            StringBuilder result = new StringBuilder();

            bool dotFound = false;
            bool digitFound = false;

            char c = PeekChar();
            if (c == '.')
            {
                dotFound = true;
            }

            if (c.IsArabicDigit())
            {
                digitFound = true;
            }

            result.Append(c);
            ReadChar();

            while (true)
            {
                c = PeekChar();
                if (!c.IsArabicDigit())
                {
                    break;
                }

                digitFound = true;
                result.Append(c);
                ReadChar();
            }

            if (!dotFound && c == '.')
            {
                result.Append(c);
                ReadChar();

                while (true)
                {
                    c = PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }

                    digitFound = true;
                    result.Append(c);
                    ReadChar();
                }
            }

            if (!digitFound)
            {
                ThrowSyntax();
            }

            if (c == 'E' || c == 'e')
            {
                result.Append(c);
                ReadChar();
                digitFound = false;
                c = PeekChar();

                if (c == '+' || c == '-')
                {
                    result.Append(c);
                    ReadChar();
                    PeekChar();
                }

                while (true)
                {
                    c = PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }

                    digitFound = true;
                    result.Append(c);
                    ReadChar();
                }

                if (!digitFound)
                {
                    ThrowSyntax();
                }
            }

            return result.ToString();
        }

        private string ReadTo
            (
                char stop
            )
        {
            string result = _navigator.ReadUntilNoCrLf(stop);
            if (ReferenceEquals(result, null))
            {
                ThrowSyntax();
            }

            char c = ReadChar();
            if (c != stop)
            {
                ThrowSyntax();
            }

            return result;
        }

        private void SkipWhitespace()
        {
            _navigator.SkipWhitespace();
        }

        private void ThrowSyntax()
        {
            string message = string.Format
                (
                    "Syntax error at line {0}, column{1}",
                    Line,
                    Column
                );

            Log.Error
                (
                    "AsnLexer::ThrowSyntax: "
                    + message
                );

            throw new AsnSyntaxException(message);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Tokenize the text.
        /// </summary>
        [NotNull]
        public AsnTokenList Tokenize
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            LocalList<AsnToken> result = new LocalList<AsnToken>();
            _navigator = new TextNavigator(text);

            while (!IsEof)
            {
                SkipWhitespace();
                if (IsEof)
                {
                    break;
                }

                int line = Line;
                int column = Column;
                char c = ReadChar();
                string value = null;
                AsnTokenKind kind;
                switch (c)
                {
                    default:
                        if (string.IsNullOrEmpty(value))
                        {
                            _navigator.Move(-1);
                            value = ReadIdentifier();
                        }
                        if (string.IsNullOrEmpty(value))
                        {
                            ThrowSyntax();
                        }

                        kind = AsnTokenKind.Identifier;
                        break;
                }

                if (kind == AsnTokenKind.Identifier)
                {
                    switch (value)
                    {
                        case "ABSENT":
                            kind = AsnTokenKind.Absent;
                            break;

                        case "ABSTRACT":
                            // ???
                            break;

                        case "ALL":
                            kind = AsnTokenKind.All;
                            break;

                        case "APPLICATION":
                            kind = AsnTokenKind.Application;
                            break;

                        case "AUTOMATIC":
                            kind = AsnTokenKind.Automatic;
                            break;

                        case "BEGIN":
                            kind = AsnTokenKind.Begin;
                            break;

                        case "BIT":
                            kind = AsnTokenKind.Bit;
                            break;

                        case "BOOLEAN":
                            kind = AsnTokenKind.Boolean;
                            break;

                        case "BY":
                            kind = AsnTokenKind.By;
                            break;

                        case "CHARACTER":
                            kind = AsnTokenKind.Character;
                            break;

                        case "CHOICE":
                            kind = AsnTokenKind.Choice;
                            break;

                        case "CLASS":
                            kind = AsnTokenKind.Class;
                            break;

                        case "COMPONENT":
                            kind = AsnTokenKind.Component;
                            break;

                        case "COMPONENTS":
                            kind = AsnTokenKind.Components;
                            break;

                        case "CONSTRAINED":
                            kind = AsnTokenKind.Constrained;
                            break;

                        case "CONTAINING":
                            kind = AsnTokenKind.Containing;
                            break;

                        case "DEFAULT":
                            kind = AsnTokenKind.Default;
                            break;

                        case "DEFINITIONS":
                            kind = AsnTokenKind.Definitions;
                            break;

                        case "EMBEDDED":
                            kind = AsnTokenKind.Embedded;
                            break;

                        case "ENCODED":
                            kind = AsnTokenKind.Encoded;
                            break;

                        case "END":
                            kind = AsnTokenKind.End;
                            break;

                        case "ENUMERATED":
                            kind = AsnTokenKind.Enumerated;
                            break;

                        case "EXCEPT":
                            kind = AsnTokenKind.Except;
                            break;

                        case "EXPLICIT":
                            kind = AsnTokenKind.Explicit;
                            break;

                        case "EXTENSIBILITY":
                            kind = AsnTokenKind.Extensibility;
                            break;

                        case "EXTERNAL":
                            kind = AsnTokenKind.External;
                            break;

                        case "FALSE":
                            kind = AsnTokenKind.False;
                            break;

                        case "false":
                            kind = AsnTokenKind.FalseSmall;
                            break;

                        case "FROM":
                            kind = AsnTokenKind.From;
                            break;

                        case "IDENTIFIER":
                            kind = AsnTokenKind.Identifier;
                            break;

                        case "IMPLIED":
                            kind = AsnTokenKind.Implied;
                            break;

                        case "IMPLICIT":
                            kind = AsnTokenKind.Implicit;
                            break;

                        case "IMPORTS":
                            kind = AsnTokenKind.Imports;
                            break;

                        case "INCLUDES":
                            kind = AsnTokenKind.Includes;
                            break;

                        case "INFINITY":
                            // ???
                            break;

                        case "INSTANCE":
                            kind = AsnTokenKind.Instance;
                            break;

                        case "INTERSECTION":
                            kind = AsnTokenKind.Intersection;
                            break;

                        case "MAX":
                            kind = AsnTokenKind.Max;
                            break;

                        case "MIN":
                            kind = AsnTokenKind.Min;
                            break;

                        case "MINUS":
                            // ???
                            break;

                        case "NULL":
                            kind = AsnTokenKind.Null;
                            break;

                        case "OBJECT":
                            kind = AsnTokenKind.Object;
                            break;

                        case "OCTET":
                            kind = AsnTokenKind.Octet;
                            break;

                        case "OID":
                            // ???
                            break;

                        case "OF":
                            kind = AsnTokenKind.Of;
                            break;

                        case "OPTIONAL":
                            kind = AsnTokenKind.Optional;
                            break;

                        case "PATTERN":
                            kind = AsnTokenKind.Pattern;
                            break;

                        case "PDV":
                            kind = AsnTokenKind.Pdv;
                            break;

                        case "PLUS":
                            // ???
                            break;

                        case "PRESENT":
                            kind = AsnTokenKind.Present;
                            break;

                        case "PRIVATE":
                            kind = AsnTokenKind.Private;
                            break;

                        case "REAL":
                            kind = AsnTokenKind.Real;
                            break;

                        case "RELATIVE":
                            // ???
                            break;

                        case "SET":
                            kind = AsnTokenKind.Set;
                            break;

                        case "SEQUENCE":
                            kind = AsnTokenKind.Sequence;
                            break;

                        case "SIZE":
                            kind = AsnTokenKind.Size;
                            break;

                        case "STRING":
                            kind = AsnTokenKind.String;
                            break;

                        case "TAGS":
                            kind = AsnTokenKind.Tags;
                            break;

                        case "TRUE":
                            kind = AsnTokenKind.True;
                            break;

                        case "true":
                            kind = AsnTokenKind.TrueSmall;
                            break;

                        case "UNION":
                            kind = AsnTokenKind.Union;
                            break;

                        case "UNIQUE":
                            kind = AsnTokenKind.Unique;
                            break;

                        case "WITH":
                            kind = AsnTokenKind.With;
                            break;
                    }
                }

                if (kind == AsnTokenKind.None)
                {
                    ThrowSyntax();
                }

                AsnToken token = new AsnToken(kind, line, column, value);
                result.Add(token);

            }

            return new AsnTokenList(result.ToArray());
        }

        #endregion
    }
}
