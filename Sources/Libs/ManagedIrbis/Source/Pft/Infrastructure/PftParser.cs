/* PftParser.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Systematization;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Parser for PFT language.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftParser
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static char[] Identifier = 
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
                'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7',
                '8', '9', '_'
            };

        private static bool IsIdentifier
            (
                char c
            )
        {
            return Array.IndexOf(Identifier, c) >= 0;
        }

        private static string ReadIdentifier
            (
                TextNavigator navigator
            )
        {
            string result = navigator.ReadWhile(Identifier)
                .ThrowIfNull();

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Tokenize the text.
        /// </summary>
        [NotNull]
        public static PftTokenList Tokenize
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            List<PftToken> result = new List<PftToken>();
            TextNavigator navigator = new TextNavigator(text);

            while (!navigator.IsEOF)
            {
                navigator.SkipWhitespace();
                if (navigator.IsEOF)
                {
                    break;
                }

                char c = navigator.ReadChar();
                char c2;
                string value = null;
                int line = navigator.Line;
                int column = navigator.Column;
                PftTokenKind kind = PftTokenKind.None;
                switch (c)
                {
                    case'\'':
                        value = navigator.ReadUntil('\'').ThrowIfNull();
                        if (navigator.ReadChar() != '\'')
                        {
                            throw new IrbisException();
                        }
                        kind = PftTokenKind.UnconditionalLiteral;
                        break;

                    case '"':
                        value = navigator.ReadUntil('"').ThrowIfNull();
                        if (navigator.ReadChar() != '"')
                        {
                            throw new IrbisException();
                        }
                        kind = PftTokenKind.ConditionalLiteral;
                        break;

                    case '|':
                        value = navigator.ReadUntil('|').ThrowIfNull();
                        if (navigator.ReadChar() != '|')
                        {
                            throw new IrbisException();
                        }
                        kind = PftTokenKind.RepeatableLiteral;
                        break;

                    case '!':
                        c2 = navigator.PeekChar();
                        if (c2 == '!')
                        {
                            kind = PftTokenKind.NotEqual2;
                            navigator.ReadChar();
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

                    case '<':
                        c2 = navigator.PeekChar();
                        if (c2 == '=')
                        {
                            kind = PftTokenKind.LessEqual;
                            navigator.ReadChar();
                        }
                        else if (c2 == '>')
                        {
                            kind = PftTokenKind.NotEqual1;
                            navigator.ReadChar();
                        }
                        else
                        {
                            kind = PftTokenKind.Less;
                        }
                        break;

                    case '>':
                        c2 = navigator.PeekChar();
                        if (c2 == '=')
                        {
                            kind = PftTokenKind.MoreEqual;
                            navigator.ReadChar();
                        }
                        else
                        {
                            kind = PftTokenKind.More;
                        }
                        break;

                    case '&':
                        value = ReadIdentifier(navigator);
                        kind = PftTokenKind.Unifor;
                        break;

                    case 'a':
                    case 'A':
                        if (IsIdentifier(navigator.PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.A;
                        break;

                    case 'c':
                    case 'C':
                        // TODO Differentiate from identifier
                        kind = PftTokenKind.C;
                        break;

                    case 'f':
                    case 'F':
                        if (IsIdentifier(navigator.PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.F;
                        break;

                    case 'l':
                    case 'L':
                        if (IsIdentifier(navigator.PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.L;
                        break;

                    case 'p':
                    case 'P':
                        if (IsIdentifier(navigator.PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.P;
                        break;

                    case 's':
                    case 'S':
                        if (IsIdentifier(navigator.PeekChar()))
                        {
                            goto default;
                        }
                        kind = PftTokenKind.S;
                        break;

                    case 'v':
                    case 'V':
                        // TODO Differentiate from identifier
                        kind = PftTokenKind.V;
                        break;

                    case 'x':
                    case 'X':
                        // TODO Differentiate from identifier
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
                        navigator.Move(-1);
                        value = navigator.ReadInteger();
                        break;

                    default:
                        navigator.Move(-1);
                        value = ReadIdentifier(navigator);
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

        #region Object members

        #endregion
    }
}
