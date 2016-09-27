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
                        kind = PftTokenKind.UnkonditionalLiteral;
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
                        kind = PftTokenKind.Bang;
                        break;

                    case 'a':
                    case 'A':
                        // TODO Differentiate from literal
                        kind = PftTokenKind.A;
                        break;

                    case 'c':
                    case 'C':
                        kind = PftTokenKind.C;
                        break;

                    case 'l':
                    case 'L':
                        kind = PftTokenKind.L;
                        break;

                    case 'p':
                    case 'P':
                        kind = PftTokenKind.P;
                        break;

                    case 'v':
                    case 'V':
                        kind = PftTokenKind.V;
                        break;

                    case 'x':
                    case 'X':
                        kind = PftTokenKind.X;
                        break;

                    case '&':
                        value = navigator.ReadWhile(Identifier).ThrowIfNull();
                        kind = PftTokenKind.Unifor;
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
                        value = navigator.ReadWhile(Identifier);
                        kind = PftTokenKind.Identifier;
                        if (string.IsNullOrEmpty(value))
                        {
                            throw new IrbisException();
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
