/* PftLexer.cs -- lexer for PFT.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.Text;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Lexer for PFT.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftLexer
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private TextNavigator _navigator;

        private static char[] Identifier =
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
                'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7',
                '8', '9', '_'
            };

        private int Column { get { return _navigator.Column; } }

        // ReSharper disable once InconsistentNaming
        private bool IsEOF { get { return _navigator.IsEOF; } }

        private static bool IsIdentifier
            (
                char c
            )
        {
            return Array.IndexOf(Identifier, c) >= 0;
        }

        private int Line { get { return _navigator.Line; } }

        private char PeekChar()
        {
            return _navigator.PeekChar();
        }

        private string PeekString(int length)
        {
            return _navigator.PeekString(length);
        }

        private char ReadChar()
        {
            return _navigator.ReadChar();
        }

        [CanBeNull]
        private string ReadField()
        {
            StringBuilder result = new StringBuilder();
            bool good;
            char c = PeekChar();

            if (!c.IsArabicDigit())
            {
                return null;
            }
            ReadChar();
            result.Append(c);

            while (true)
            {
                c = PeekChar();
                if (!c.IsArabicDigit())
                {
                    break;
                }
                ReadChar();
                result.Append(c);
            }

            if (c == '@')
            {
                ReadChar();
                result.Append(c);

                good = false;
                while (true)
                {
                    c = PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    ReadChar();
                    result.Append(c);
                    good = true;
                }

                if (!good)
                {
                    throw new PftSyntaxException(_navigator);
                }
            }

            if (c == '^')
            {
                ReadChar();
                result.Append(c);
                if (IsEOF)
                {
                    throw new PftSyntaxException(_navigator);
                }
                c = ReadChar();
                if (!SubFieldCode.IsValidCode(c))
                {
                    throw new PftSyntaxException(_navigator);
                }
                result.Append(c);
                c = PeekChar();
            }

            if (c == '[')
            {
                ReadChar();
                result.Append(c);

                good = false;
                while (true)
                {
                    c = ReadChar();
                    if (c == ']')
                    {
                        result.Append(c);
                        c = PeekChar();
                        break;
                    }
                    if (c.IsArabicDigit())
                    {
                        good = true;
                        result.Append(c);
                    }
                    else
                    {
                        throw new PftSyntaxException(_navigator);
                    }
                }

                if (!good)
                {
                    throw new PftSyntaxException(_navigator);
                }
            }

            if (c == '*')
            {
                ReadChar();
                result.Append(c);

                good = false;
                while (true)
                {
                    c = PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    good = true;
                    ReadChar();
                    result.Append(c);
                }

                if (!good)
                {
                    throw new PftSyntaxException(_navigator);
                }
            }

            if (c == '.')
            {
                ReadChar();
                result.Append(c);

                good = false;
                while (true)
                {
                    c = PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    good = true;
                    ReadChar();
                    result.Append(c);
                }

                if (!good)
                {
                    throw new PftSyntaxException(_navigator);
                }
            }

            if (c == '(')
            {
                ReadChar();
                result.Append(c);

                good = false;
                while (true)
                {
                    c = ReadChar();
                    if (c == ')')
                    {
                        result.Append(c);
                        break;
                    }
                    if (c.IsArabicDigit())
                    {
                        good = true;
                        result.Append(c);
                    }
                    else
                    {
                        throw new PftSyntaxException(_navigator);
                    }
                }

                if (!good)
                {
                    throw new PftSyntaxException(_navigator);
                }
            }

            return result.ToString();
        }

        [NotNull]
        private string ReadIdentifier()
        {
            string result = _navigator.ReadWhile(Identifier)
                .ThrowIfNull();

            return result;
        }

        [CanBeNull]
        private string ReadInteger()
        {
            StringBuilder result = new StringBuilder();

            while (true)
            {
                char c = PeekChar();
                if (IsIdentifier(c))
                {
                    if (!c.IsArabicDigit())
                    {
                        return null;
                    }
                    result.Append(c);
                }
                else
                {
                    break;
                }
            }

            return result.ToString();
        }

        private string ReadTo
            (
                char stop
            )
        {
            string result = _navigator.ReadUntil(stop);
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
            throw new PftSyntaxException(message);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Tokenize the text.
        /// </summary>
        [NotNull]
        public PftTokenList Tokenize
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            List<PftToken> result = new List<PftToken>();
            _navigator = new TextNavigator(text);

            while (!IsEOF)
            {
                SkipWhitespace();
                if (IsEOF)
                {
                    break;
                }

                int line = Line;
                int column = Column;
                char c = ReadChar();
                char c2;
                string value = null;
                string value2 = null;
                PftTokenKind kind = PftTokenKind.None;
                switch (c)
                {
                    case '\'':
                        value = ReadTo('\'');
                        kind = PftTokenKind.UnconditionalLiteral;
                        break;

                    case '"':
                        value = ReadTo('"');
                        kind = PftTokenKind.ConditionalLiteral;
                        break;

                    case '|':
                        value = ReadTo('|');
                        kind = PftTokenKind.RepeatableLiteral;
                        break;

                    case '!':
                        c2 = PeekChar();
                        if (c2 == '=')
                        {
                            kind = PftTokenKind.NotEqual2;
                            ReadChar();
                        }
                        else
                        {
                            kind = PftTokenKind.Bang;
                        }
                        break;

                    case ':':
                        kind = PftTokenKind.Colon;
                        break;

                    case ',':
                        kind = PftTokenKind.Comma;
                        break;

                    case '\\':
                        kind = PftTokenKind.Backslash;
                        break;

                    case '=':
                        kind = PftTokenKind.Equals;
                        break;

                    case '#':
                        kind = PftTokenKind.Hash;
                        break;

                    case '%':
                        kind = PftTokenKind.Percent;
                        break;

                    case '{':
                        kind = PftTokenKind.LeftCurly;
                        break;

                    case '[':
                        kind = PftTokenKind.LeftSquare;
                        break;

                    case '(':
                        kind = PftTokenKind.LeftParenthesis;
                        break;

                    case '}':
                        kind = PftTokenKind.RightCurly;
                        break;

                    case ']':
                        kind = PftTokenKind.RightSquare;
                        break;

                    case ')':
                        kind = PftTokenKind.RightParenthesis;
                        break;

                    case '+':
                        kind = PftTokenKind.Plus;
                        break;

                    case '-':
                        kind = PftTokenKind.Minus;
                        break;

                    case '*':
                        kind = PftTokenKind.Star;
                        break;

                    case '~':
                        kind = PftTokenKind.Tilda;
                        break;

                    case '?':
                        kind = PftTokenKind.Question;
                        break;

                    case '/':
                        c2 = PeekChar();
                        if (c2 == '*')
                        {
                            ReadChar();
                            value = _navigator.ReadUntil('\r', '\n');
                            kind = PftTokenKind.Comment;
                        }
                        else
                        {
                            kind = PftTokenKind.Slash;
                        }
                        break;

                    case '<':
                        c2 = PeekChar();
                        if (c2 == '=')
                        {
                            kind = PftTokenKind.LessEqual;
                            ReadChar();
                        }
                        else if (c2 == '>')
                        {
                            kind = PftTokenKind.NotEqual1;
                            ReadChar();
                        }
                        else
                        {
                            kind = PftTokenKind.Less;
                        }
                        break;

                    case '>':
                        c2 = PeekChar();
                        if (c2 == '=')
                        {
                            kind = PftTokenKind.MoreEqual;
                            ReadChar();
                        }
                        else
                        {
                            kind = PftTokenKind.More;
                        }
                        break;

                    case '&':
                        value = ReadIdentifier();
                        kind = PftTokenKind.Unifor;
                        break;

                    case '$':
                        value = ReadIdentifier();
                        kind = PftTokenKind.Variable;
                        break;

                    case 'a':
                    case 'A':
                        if (IsIdentifier(PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.A;
                        break;

                    case 'c':
                    case 'C':
                        value = ReadInteger();
                        if (value == null)
                        {
                            goto default;
                        }
                        kind = PftTokenKind.C;
                        break;

                    case 'd':
                    case 'D':
                        value2 = ReadField();
                        if (value2 == null)
                        {
                            goto default;
                        }
                        value = c + value2;
                        kind = PftTokenKind.V;
                        break;

                    case 'f':
                    case 'F':
                        if (IsIdentifier(PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.F;
                        break;

                    case 'l':
                    case 'L':
                        if (IsIdentifier(PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.L;
                        break;

                    case 'm':
                    case 'M':
                        value2 = PeekString(2).ToLower();
                        if (value2 == "fn")
                        {
                            kind = PftTokenKind.Mfn;
                            break;
                        }
                        if (value2.Length != 2)
                        {
                            goto default;
                        }
                        if ((value2[0] == 'h'
                             || value2[0] == 'p')
                            && (value2[1] == 'l'
                                || value2[1] == 'u'))
                        {
                            kind = PftTokenKind.Mpl;
                            value = c + value2;
                            break;
                        }
                        goto default;

                    case 'n':
                    case 'N':
                        value2 = ReadField();
                        if (value2 == null)
                        {
                            goto default;
                        }
                        value = c + value2;
                        kind = PftTokenKind.V;
                        break;

                    case 'p':
                    case 'P':
                        if (IsIdentifier(PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.P;
                        break;

                    case 's':
                    case 'S':
                        if (IsIdentifier(PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.S;
                        break;

                    case 'v':
                    case 'V':
                        value2 = ReadField();
                        if (value2 == null)
                        {
                            goto default;
                        }
                        value = c + value2;
                        kind = PftTokenKind.V;
                        break;

                    case 'x':
                    case 'X':
                        value = ReadInteger();
                        if (value == null)
                        {
                            goto default;
                        }
                        kind = PftTokenKind.X;
                        break;

                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        _navigator.Move(-1);
                        value = _navigator.ReadInteger();
                        break;

                    default:
                        _navigator.Move(-1);
                        value = ReadIdentifier();
                        if (string.IsNullOrEmpty(value))
                        {
                            throw new IrbisException();
                        }
                        switch (value.ToLower())
                        {
                            case "break":
                                kind = PftTokenKind.Break;
                                break;

                            case "chr":
                                kind = PftTokenKind.Chr;
                                break;

                            case "else":
                                kind = PftTokenKind.Else;
                                break;

                            case "f2":
                                kind = PftTokenKind.F2;
                                break;

                            case "fi":
                                kind = PftTokenKind.Fi;
                                break;

                            case "if":
                                kind = PftTokenKind.If;
                                break;

                            case "mfn":
                                kind = PftTokenKind.Mfn;
                                break;

                            case "not":
                                kind = PftTokenKind.Not;
                                break;

                            case "or":
                                kind = PftTokenKind.Or;
                                break;

                            case "ravr":
                                kind = PftTokenKind.Ravr;
                                break;

                            case "ref":
                                kind = PftTokenKind.Ref;
                                break;

                            case "rmax":
                                kind = PftTokenKind.Rmax;
                                break;

                            case "rmin":
                                kind = PftTokenKind.Rmin;
                                break;

                            case "rsum":
                                kind = PftTokenKind.Rsum;
                                break;

                            case "then":
                                kind = PftTokenKind.Then;
                                break;

                            case "val":
                                kind = PftTokenKind.Val;
                                break;

                            default:
                                kind = PftTokenKind.Identifier;
                                break;
                        }

                        break;
                }

                if (kind == PftTokenKind.None)
                {
                    throw new IrbisException();
                }

                PftToken token = new PftToken(kind, line, column, value);

                result.Add(token);

            }

            return new PftTokenList(result);
        }

        #endregion
    }
}
