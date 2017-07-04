// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

using AM;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

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
#if PORTABLE

            bool result = false;

            foreach (char c in s)
            {
                if (!IsInteger(c))
                {
                    return false;
                }
                result = true;
            }

            return result;

#else

            return s.All(c => IsInteger (c));

#endif
        }

        private int Line { get { return _navigator.Line; } }

        private char PeekChar()
        {
            //return _navigator.PeekChar();
            return _navigator.PeekCharNoCrLf();
        }

        private string PeekString(int length)
        {
            //return _navigator.PeekString(length);
            return _navigator.PeekStringNoCrLf(length);
        }

        private char ReadChar()
        {
            //return _navigator.ReadChar();
            return _navigator.ReadCharNoCrLf();
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
            string result = _navigator.ReadWhileNoCrLf(Identifier);

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
                    "PftLexer::ThrowSyntax: "
                    + message
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
                char c2, c3;
                string value = null;
                FieldSpecification field = null;
                PftTokenKind kind;
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
                            value = "!=";
                            ReadChar();
                            if (PeekChar() == '=')
                            {
                                ReadChar();
                                value = "!==";
                            }
                        }
                        else if (c2 == '~')
                        {
                            kind = PftTokenKind.NotEqual2;
                            value = "!~";
                            ReadChar();
                            if (PeekChar() == '~')
                            {
                                ReadChar();
                                value = "!~~";
                            }
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
                        if (PeekChar() == ':')
                        {
                            ReadChar();
                            value = "::";
                        }
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
                        if (PeekChar() == '=')
                        {
                            ReadChar();
                            value = "==";
                        }
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
                        c2 = PeekChar();
                        if (c2 == '{')
                        {
                            c3 = _navigator.LookAhead(1);
                            if (c3 == '{')
                            {
                                ReadChar();
                                ReadChar();
                                kind = PftTokenKind.TripleCurly;
                                value = _navigator.ReadTo("}}}");
                                if (ReferenceEquals(value, null))
                                {
                                    ThrowSyntax();
                                }
                            }
                            else
                            {
                                kind = PftTokenKind.LeftCurly;
                                value = c.ToString();
                            }
                        }
                        else
                        {
                            kind = PftTokenKind.LeftCurly;
                            value = c.ToString();
                        }
                        break;

                    case '[':
                        c2 = PeekChar();
                        c3 = _navigator.LookAhead(1);
                        if (c2 == '[' && c3 == '[')
                        {
                            ReadChar();
                            ReadChar();
                            kind = PftTokenKind.EatOpen;
                            value = "[[[";
                        }
                        else
                        {
                            kind = PftTokenKind.LeftSquare;
                            value = c.ToString();
                        }
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
                        c2 = PeekChar();
                        c3 = _navigator.LookAhead(1);
                        if (c2 == ']' && c3 == ']')
                        {
                            ReadChar();
                            ReadChar();
                            kind = PftTokenKind.EatClose;
                            value = "]]]";
                        }
                        else
                        {
                            kind = PftTokenKind.RightSquare;
                            value = c.ToString();
                        }
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
                        //if (IsInteger(PeekChar()))
                        //{
                        //    kind = PftTokenKind.Number;
                        //    value = c + ReadFloat();
                        //}
                        //else
                        //{
                            kind = PftTokenKind.Minus;
                            value = c.ToString();
                        //}
                        break;

                    case '*':
                        kind = PftTokenKind.Star;
                        value = c.ToString();
                        break;

                    case '~':
                        kind = PftTokenKind.Tilda;
                        value = c.ToString();
                        if (PeekChar() == '~')
                        {
                            ReadChar();
                            value = "~~";
                        }
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
                            c3 = _navigator.LookAhead(1);
                            if (c3 == '<')
                            {
                                ReadChar();
                                ReadChar();
                                kind = PftTokenKind.TripleLess;
                                value = _navigator.ReadTo(">>>");
                                if (ReferenceEquals(value, null))
                                {
                                    ThrowSyntax();
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
                            ThrowSyntax();
                        }
                        kind = PftTokenKind.Unifor;
                        break;

                    case '$':
                        value = ReadIdentifier();
                        if (string.IsNullOrEmpty(value))
                        {
                            ThrowSyntax();
                        }
                        kind = PftTokenKind.Variable;
                        break;

                    case '@':
                        value = ReadIdentifier();
                        if (string.IsNullOrEmpty(value))
                        {
                            ThrowSyntax();
                        }
                        kind = PftTokenKind.At;
                        break;

                    case '^':
                        kind = PftTokenKind.Hat;
                        value = "^";
                        break;

                    case '\x1C':
                    case '\u221F':
                        value = _navigator.ReadUntil('\x1D', '\u2194');
                        if (string.IsNullOrEmpty(value)
                            || !ReadChar().OneOf('\x1D', '\u2194'))
                        {
                            ThrowSyntax();
                        }
                        kind = PftTokenKind.At;
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
                        value = "a";
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
                        value = field.RawText;
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
                        value = field.RawText;
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
                        string value2 = value.ToLower();
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
                                        c3 = PeekChar();
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
                                        ThrowSyntax();
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
                        value = field.RawText;
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
                        value = field.RawText;
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
                            ThrowSyntax();
                        }

                        switch (value.ThrowIfNull().ToLower())
                        {
                            case "abs":
                                kind = PftTokenKind.Abs;
                                value = "abs";
                                break;

                            case "all":
                                kind = PftTokenKind.All;
                                value = "all";
                                break;

                            case "and":
                            case "и":
                                kind = PftTokenKind.And;
                                value = "and";
                                break;

                            case "any":
                                kind = PftTokenKind.Any;
                                value = "any";
                                break;

                            case "blank":
                                kind = PftTokenKind.Blank;
                                value = "blank";
                                break;

                            case "break":
                                kind = PftTokenKind.Break;
                                value = "break";
                                break;

                            case "ceil":
                                kind = PftTokenKind.Ceil;
                                value = "ceil";
                                break;

                            case "div":
                                kind = PftTokenKind.Div;
                                value = "div";
                                break;

                            case "do":
                                kind = PftTokenKind.Do;
                                value = "do";
                                break;

                            case "else":
                            case "иначе":
                                kind = PftTokenKind.Else;
                                value = "else";
                                break;

                            case "empty":
                                kind = PftTokenKind.Empty;
                                value = "empty";
                                break;

                            case "end":
                            case "конец":
                                kind = PftTokenKind.End;
                                value = "end";
                                break;

                            case "f2":
                                kind = PftTokenKind.F2;
                                value = "f2";
                                break;

                            case "false":
                            case "ложь":
                                kind = PftTokenKind.False;
                                value = "false";
                                break;

                            case "fi":
                            case "все":
                            case "всё":
                            case "илсе":
                            case "фи":
                                kind = PftTokenKind.Fi;
                                value = "fi";
                                break;

                            case "first":
                                kind = PftTokenKind.First;
                                value = "first";
                                break;

                            case "floor":
                                kind = PftTokenKind.Floor;
                                value = "floor";
                                break;

                            case "for":
                            case "для":
                                kind = PftTokenKind.For;
                                value = "for";
                                break;

                            case "foreach":
                                kind = PftTokenKind.ForEach;
                                value = "foreach";
                                break;

                            case "frac":
                                kind = PftTokenKind.Frac;
                                value = "frac";
                                break;

                            case "from":
                                kind = PftTokenKind.From;
                                value = "from";
                                break;

                            case "global":
                                kind = PftTokenKind.Global;
                                value = "global";
                                break;

                            case "have":
                                kind = PftTokenKind.Have;
                                value = "have";
                                break;

                            case "if":
                            case "если":
                                kind = PftTokenKind.If;
                                value = "if";
                                break;

                            case "in":
                                kind = PftTokenKind.In;
                                value = "in";
                                break;

                            case "last":
                                kind = PftTokenKind.Last;
                                value = "last";
                                break;

                            case "local":
                                kind = PftTokenKind.Local;
                                value = "local";
                                break;

                            case "nl":
                                kind = PftTokenKind.Nl;
                                value = "nl";
                                break;

                            case "not":
                            case "не":
                                kind = PftTokenKind.Not;
                                value = "not";
                                break;

                            case "or":
                            case "или":
                                kind = PftTokenKind.Or;
                                value = "or";
                                break;

                            case "order":
                                kind = PftTokenKind.Order;
                                value = "order";
                                break;

                            case "parallel":
                            case "параллельно":
                                kind = PftTokenKind.Parallel;
                                value = "parallel";
                                break;

                            case "pow":
                                kind = PftTokenKind.Pow;
                                value = "pow";
                                break;

                            case "proc":
                                kind = PftTokenKind.Proc;
                                value = "proc";
                                break;

                            case "ravr":
                                kind = PftTokenKind.Ravr;
                                value = "ravr";
                                break;

                            case "ref":
                                kind = PftTokenKind.Ref;
                                value = "ref";
                                break;

                            case "rmax":
                                kind = PftTokenKind.Rmax;
                                value = "rmax";
                                break;

                            case "rmin":
                                kind = PftTokenKind.Rmin;
                                value = "rmin";
                                break;

                            case "round":
                                kind = PftTokenKind.Round;
                                value = "round";
                                break;

                            case "rsum":
                                kind = PftTokenKind.Rsum;
                                value = "rsum";
                                break;

                            case "select":
                            case "выбор":
                                kind = PftTokenKind.Select;
                                value = "select";
                                break;

                            case "sign":
                                kind = PftTokenKind.Sign;
                                value = "sign";
                                break;

                            case "then":
                            case "то":
                            case "тогда":
                                kind = PftTokenKind.Then;
                                value = "then";
                                break;

                            case "true":
                            case "истина":
                                kind = PftTokenKind.True;
                                value = "true";
                                break;

                            case "trunc":
                                kind = PftTokenKind.Trunc;
                                value = "trunc";
                                break;

                            case "val":
                                kind = PftTokenKind.Val;
                                value = "val";
                                break;

                            case "where":
                                kind = PftTokenKind.Where;
                                value = "where";
                                break;

                            case "while":
                            case "пока":
                                kind = PftTokenKind.While;
                                value = "while";
                                break;

                            case "with":
                                kind = PftTokenKind.With;
                                value = "with";
                                break;

                            default:
                                kind = PftTokenKind.Identifier;
                                break;
                        }

                        break;
                }

                if (kind == PftTokenKind.None) //-V3022
                {
                    ThrowSyntax();
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
