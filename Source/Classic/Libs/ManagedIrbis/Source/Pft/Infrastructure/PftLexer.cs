/* PftLexer.cs -- lexer for PFT.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        // ReSharper disable once InconsistentNaming
        private static char[] Integer =
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
            };

        // ReSharper disable once InconsistentNaming
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

        private static bool IsInteger
            (
                char c
            )
        {
            return Array.IndexOf(Integer, c) >= 0;
        }

        private static bool IsInteger
            (
                string s
            )
        {
            return s.All(IsIdentifier);
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
        private FieldSpecification ReadField()
        {
            FieldSpecification result = new FieldSpecification();

            TextPosition position = _navigator.SavePosition();
            _navigator.Move(-1);

            if (!result.Parse(_navigator))
            {
                _navigator.RestorePosition(position);
                return null;
            }

            return result;
        }

        [CanBeNull]
        private string ReadIdentifier()
        {
            string result = _navigator.ReadWhile(Identifier);

            return result;
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
            if (c != '+'
                && c != '-'
                && c != '.'
                && !c.IsArabicDigit())
            {
                return null;
            }
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
                throw new PftSyntaxException(_navigator);
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
                    c = PeekChar();
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
                    throw new PftSyntaxException(_navigator);
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
                string value2;
                FieldSpecification field = null;
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
                            value = c.ToString();
                        }
                        break;

                    case ':':
                        kind = PftTokenKind.Colon;
                        value = c.ToString();
                        break;

                    case ';':
                        kind = PftTokenKind.Semicolon;
                        value = c.ToString();
                        break;

                    case ',':
                        kind = PftTokenKind.Comma;
                        value = c.ToString();
                        break;

                    case '\\':
                        kind = PftTokenKind.Backslash;
                        value = c.ToString();
                        break;

                    case '=':
                        kind = PftTokenKind.Equals;
                        value = c.ToString();
                        break;

                    case '#':
                        kind = PftTokenKind.Hash;
                        value = c.ToString();
                        break;

                    case '%':
                        kind = PftTokenKind.Percent;
                        value = c.ToString();
                        break;

                    case '{':
                        kind = PftTokenKind.LeftCurly;
                        value = c.ToString();
                        break;

                    case '[':
                        kind = PftTokenKind.LeftSquare;
                        value = c.ToString();
                        break;

                    case '(':
                        kind = PftTokenKind.LeftParenthesis;
                        value = c.ToString();
                        break;

                    case '}':
                        kind = PftTokenKind.RightCurly;
                        value = c.ToString();
                        break;

                    case ']':
                        kind = PftTokenKind.RightSquare;
                        value = c.ToString();
                        break;

                    case ')':
                        kind = PftTokenKind.RightParenthesis;
                        value = c.ToString();
                        break;

                    case '+':
                        kind = PftTokenKind.Plus;
                        value = c.ToString();
                        break;

                    case '-':
                        if (IsInteger(PeekChar()))
                        {
                            kind = PftTokenKind.Number;
                            value = c + ReadFloat();
                        }
                        else
                        {
                            kind = PftTokenKind.Minus;
                            value = c.ToString();
                        }
                        break;

                    case '*':
                        kind = PftTokenKind.Star;
                        value = c.ToString();
                        break;

                    case '~':
                        kind = PftTokenKind.Tilda;
                        value = c.ToString();
                        break;

                    case '?':
                        kind = PftTokenKind.Question;
                        value = c.ToString();
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
                            value = c.ToString();
                        }
                        break;

                    case '<':
                        c2 = PeekChar();
                        if (c2 == '=')
                        {
                            kind = PftTokenKind.LessEqual;
                            value = "<=";
                            ReadChar();
                        }
                        else if (c2 == '<')
                        {
                            char c3 = _navigator.PeekChar(1);
                            if (c3 == '<')
                            {
                                ReadChar();
                                ReadChar();
                                kind = PftTokenKind.TripleLess;
                                value = _navigator.ReadTo(">>>");
                                if (ReferenceEquals(value, null))
                                {
                                    throw new PftSyntaxException(_navigator);
                                }
                            }
                            else
                            {
                                kind = PftTokenKind.Less;
                                value = c.ToString();
                            }
                        }
                        else if (c2 == '>')
                        {
                            kind = PftTokenKind.NotEqual1;
                            value = "<>";
                            ReadChar();
                        }
                        else
                        {
                            kind = PftTokenKind.Less;
                            value = c.ToString();
                        }
                        break;

                    case '>':
                        c2 = PeekChar();
                        if (c2 == '=')
                        {
                            kind = PftTokenKind.MoreEqual;
                            value = ">=";
                            ReadChar();
                        }
                        else
                        {
                            kind = PftTokenKind.More;
                            value = c.ToString();
                        }
                        break;

                    case '&':
                        value = ReadIdentifier();
                        if (string.IsNullOrEmpty(value))
                        {
                            throw new PftSyntaxException(_navigator);
                        }
                        kind = PftTokenKind.Unifor;
                        break;

                    case '$':
                        value = ReadIdentifier();
                        if (string.IsNullOrEmpty(value))
                        {
                            throw new PftSyntaxException(_navigator);
                        }
                        kind = PftTokenKind.Variable;
                        break;

                    case 'a':
                    case 'A':
                        value = ReadIdentifier();
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = c + value;
                            goto default;
                        }
                        kind = PftTokenKind.A;
                        break;

                    case 'c':
                    case 'C':
                        value = ReadInteger();
                        if (!string.IsNullOrEmpty(value))
                        {
                            kind = PftTokenKind.C;
                            break;
                        }
                        value = ReadIdentifier();
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = c + value;
                            goto default;
                        }
                        kind = PftTokenKind.Identifier;
                        break;

                    case 'd':
                    case 'D':
                        field = ReadField();
                        if (field == null)
                        {
                            goto default;
                        }
                        value = field.Text;
                        kind = PftTokenKind.V;
                        break;

                    case 'f':
                    case 'F':
                        value = ReadIdentifier();
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = c + value;
                            goto default;
                        }
                        kind = PftTokenKind.F;
                        break;

                    case 'g':
                    case 'G':
                        field = ReadField();
                        if (field == null)
                        {
                            goto default;
                        }
                        value = field.Text;
                        kind = PftTokenKind.V;
                        break;

                    case 'l':
                    case 'L':
                        value = ReadIdentifier();
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = c + value;
                            goto default;
                        }
                        kind = PftTokenKind.L;
                        break;

                    case 'm':
                    case 'M':
                        value = ReadIdentifier();
                        if (string.IsNullOrEmpty(value))
                        {
                            goto default;
                        }
                        value2 = value.ToLower();
                        if (value2.Length == 2)
                        {
                            if (value2 == "fn")
                            {
                                StringBuilder builder = new StringBuilder();

                                if (PeekChar() == '(')
                                {
                                    builder.Append('(');
                                    ReadChar();

                                    bool ok = false;
                                    while (!IsEOF)
                                    {
                                        char c3 = PeekChar();
                                        builder.Append(c3);
                                        ReadChar();
                                        if (c3 == ')')
                                        {
                                            ok = true;
                                            break;
                                        }
                                    }
                                    if (!ok)
                                    {
                                        throw new PftSyntaxException(_navigator);
                                    }
                                }

                                kind = PftTokenKind.Mfn;
                                value = "mfn" + builder;
                                break;
                            }
                            if ((value2[0] == 'h'
                                || value2[0] == 'd'
                                || value2[0] == 'p')
                                && (value2[1] == 'l'
                                    || value2[1] == 'u'))
                            {
                                kind = PftTokenKind.Mpl;
                                value = c + value2;
                                break;
                            }
                        }
                        value = c + value;
                        goto default;

                    case 'n':
                    case 'N':
                        field = ReadField();
                        if (field == null)
                        {
                            goto default;
                        }
                        value = field.Text;
                        kind = PftTokenKind.V;
                        break;

                    case 'p':
                    case 'P':
                        value = ReadIdentifier();
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = c + value;
                            goto default;
                        }
                        kind = PftTokenKind.P;
                        break;

                    case 's':
                    case 'S':
                        value = ReadIdentifier();
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = c + value;
                            goto default;
                        }
                        kind = PftTokenKind.S;
                        break;

                    case 'v':
                    case 'V':
                        field = ReadField();
                        if (field == null)
                        {
                            goto default;
                        }
                        value = field.Text;
                        kind = PftTokenKind.V;
                        break;

                    case 'x':
                    case 'X':
                        value = ReadInteger();
                        if (!string.IsNullOrEmpty(value))
                        {
                            kind = PftTokenKind.X;
                            break;
                        }
                        value = ReadIdentifier();
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = c + value;
                            goto default;
                        }
                        kind = PftTokenKind.Identifier;
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
                        value = ReadFloat();
                        kind = PftTokenKind.Number;
                        break;

                    default:
                        if (string.IsNullOrEmpty(value))
                        {
                            _navigator.Move(-1);
                            value = ReadIdentifier();
                        }
                        if (string.IsNullOrEmpty(value))
                        {
                            throw new IrbisException();
                        }
                        switch (value.ToLower())
                        {
                            case "and":
                                kind = PftTokenKind.And;
                                value = "and";
                                break;

                            case "break":
                                kind = PftTokenKind.Break;
                                break;

                            case "div":
                                kind = PftTokenKind.Div;
                                value = "div";
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

                            case "nl":
                                kind = PftTokenKind.Nl;
                                break;

                            case "not":
                                kind = PftTokenKind.Not;
                                break;

                            case "or":
                                kind = PftTokenKind.Or;
                                value = "or";
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
                    throw new PftSyntaxException(_navigator);
                }

                PftToken token = new PftToken(kind, line, column, value);
                if (kind == PftTokenKind.V)
                {
                    token.UserData = field;
                }

                result.Add(token);

            }

            return new PftTokenList(result);
        }

        #endregion
    }
}
