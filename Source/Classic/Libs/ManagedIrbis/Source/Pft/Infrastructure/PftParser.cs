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
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Source.Pft.Infrastructure.Ast;
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

        /// <summary>
        /// Token list.
        /// </summary>
        [NotNull]
        public PftTokenList Tokens { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParser
            (
                [NotNull] PftTokenList tokens
            )
        {
            Code.NotNull(tokens, "tokens");

            Tokens = tokens;
            CreateTokenMap();
        }

        #endregion

        #region Private members

        #region Useful routines

        //================================================================
        // Automate token handling.
        //================================================================

        private Dictionary<PftTokenKind, Func<PftToken, PftNode>> TokenMap;

        private void CreateTokenMap()
        {
            TokenMap = new Dictionary<PftTokenKind, Func<PftToken, PftNode>>
            {
                {PftTokenKind.C, ParseC},
                {PftTokenKind.Comma, ParseComma},
                {PftTokenKind.Comment, ParseComment},
                {PftTokenKind.ConditionalLiteral, ParseConditionalLiteral},
                {PftTokenKind.Hash, ParseHash},
                {PftTokenKind.Mfn, ParseMfn},
                {PftTokenKind.Mpl, ParseMpl},
                {PftTokenKind.Nl, ParseNl},
                {PftTokenKind.Percent, ParsePercent},
                {PftTokenKind.RepeatableLiteral, ParseRepeatableLiteral},
                {PftTokenKind.Slash, ParseSlash},
                {PftTokenKind.UnconditionalLiteral, ParseUnconditionalLiteral},
                {PftTokenKind.Unifor, ParseUnifor},
                {PftTokenKind.X, ParseX}
            };
        }

        //================================================================
        // Dumb but useful routines.
        //================================================================

        private PftNode Move(PftNode node)
        {
            Tokens.MoveNext();
            return node;
        }

        private PftNode ParseC(PftToken token)
        {
            return Move(new PftC(token));
        }

        private PftNode ParseComma(PftToken token)
        {
            return Move(new PftComma(token));
        }

        private PftNode ParseComment(PftToken token)
        {
            return Move(new PftComment(token));
        }

        private PftNode ParseConditionalLiteral(PftToken token)
        {
            return Move(new PftConditionalLiteral(token));
        }

        private PftNode ParseHash(PftToken token)
        {
            return Move(new PftHash(token));
        }

        private PftNode ParseMfn(PftToken token)
        {
            return Move(new PftMfn(token));
        }

        private PftNode ParseMpl(PftToken token)
        {
            return Move(new PftMode(token));
        }

        private PftNode ParseNl(PftToken token)
        {
            return Move(new PftNl(token));
        }

        private PftNode ParsePercent(PftToken token)
        {
            return Move(new PftPercent(token));
        }

        private PftNode ParseRepeatableLiteral(PftToken token)
        {
            return Move(new PftRepeatableLiteral(token));
        }

        private PftNode ParseSlash(PftToken token)
        {
            return Move(new PftSlash(token));
        }

        private PftNode ParseUnconditionalLiteral(PftToken token)
        {
            return Move(new PftUnconditionalLiteral(token));
        }

        private PftNode ParseX(PftToken token)
        {
            return Move(new PftX(token));
        }

        //================================================================
        // Gather tokens etc.
        //================================================================

        [CanBeNull]
        private PftNode GetNode
            (
                params PftTokenKind[] kinds
            )
        {
            PftNode result = null;
            PftToken token = Tokens.Current;

            if (Array.IndexOf(kinds, token.Kind) >= 0)
            {
                Func<PftToken, PftNode> func;
                if (!TokenMap.TryGetValue(token.Kind, out func))
                {
                    throw new PftException("don't know how to handle token " + token.Kind);
                }
                result = func(token);
            }

            return result;
        }

        private PftNode[] GetNodes
            (
                params PftTokenKind[] kinds
            )
        {
            List<PftNode> result = new List<PftNode>();

            while (true)
            {
                PftNode node = GetNode(kinds);
                if (ReferenceEquals(node, null))
                {
                    break;
                }
                result.Add(node);
            }

            return result.ToArray();
        }

        #endregion

        //================================================================
        // Other routines
        //================================================================

        private PftNode ParseGroup()
        {
            PftGroup result = new PftGroup();

            PftToken token = Tokens.Current;

            token.MustBe(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            while (true)
            {
                token = Tokens.Current;

                if (token.Kind == PftTokenKind.RightParenthesis)
                {
                    Tokens.MoveNext();
                    break;
                }

                if (token.Kind == PftTokenKind.LeftParenthesis)
                {
                    throw new PftSyntaxException(token);
                }

                PftNode node = ParseSimple();
                result.Children.Add(node);
            }

            return result;
        }

        [NotNull]
        private PftNode ParseSimple()
        {
            PftNode result = null;
            bool moveNext = true;
            PftToken token = Tokens.Current;

            switch (token.Kind)
            {
                case PftTokenKind.Comma:
                    result = new PftComma();
                    break;

                case PftTokenKind.C:
                    result = new PftC(token);
                    break;

                case PftTokenKind.ConditionalLiteral:
                case PftTokenKind.V:
                case PftTokenKind.RepeatableLiteral:
                    result = ParseField();
                    moveNext = false;
                    break;

                case PftTokenKind.Hash:
                    result = new PftHash();
                    break;

                case PftTokenKind.Mfn:
                    result = new PftMfn(token);
                    break;

                case PftTokenKind.Mpl:
                    result = new PftMode(token);
                    break;

                case PftTokenKind.Nl:
                    result = new PftNl(token);
                    break;

                case PftTokenKind.Percent:
                    result = new PftPercent();
                    break;

                case PftTokenKind.Slash:
                    result = new PftSlash();
                    break;

                case PftTokenKind.UnconditionalLiteral:
                    result = new PftUnconditionalLiteral(token);
                    break;

                case PftTokenKind.X:
                    result = new PftX(token);
                    break;

                case PftTokenKind.Unifor:
                    result = ParseUnifor(token);
                    moveNext = false;
                    break;

                default:
                    throw new PftSyntaxException(token);
            }

            if (ReferenceEquals(result, null))
            {
                throw new PftSyntaxException(token);
            }
            if (moveNext)
            {
                Tokens.MoveNext();
            }

            return result;
        }

        private PftNode ParseField()
        {
            List<PftNode> leftHand = new List<PftNode>();
            PftField result = new PftV();
            PftToken token;
            PftNode node;
            PftRepeatableLiteral literal;

            // Gather left hand of the field: conditional literal and friends
            while (!Tokens.IsEof)
            {
                node = null;
                token = Tokens.Current;
                switch (token.Kind)
                {
                    case PftTokenKind.ConditionalLiteral:
                        node = new PftConditionalLiteral(token);
                        break;

                    case PftTokenKind.C:
                        node = new PftC(token);
                        break;

                    case PftTokenKind.Comma:
                        node = new PftComma();
                        break;

                    case PftTokenKind.Comment:
                        node = new PftComment(token);
                        break;

                    case PftTokenKind.Hash:
                        node = new PftHash();
                        break;

                    case PftTokenKind.Mpl:
                        node = new PftMode(token);
                        break;

                    case PftTokenKind.Nl:
                        node = new PftNl(token);
                        break;

                    case PftTokenKind.Percent:
                        node = new PftPercent();
                        break;

                    case PftTokenKind.RepeatableLiteral:
                        // goto next step
                        break;

                    case PftTokenKind.Slash:
                        node = new PftSlash();
                        break;

                    case PftTokenKind.V:
                        // goto next step
                        break;

                    case PftTokenKind.X:
                        node = new PftX(token);
                        break;
                }

                if (node == null)
                {
                    break;
                }

                leftHand.Add(node);
                Tokens.MoveNext();
            } // Tokens.IsEof

            // Gather left hand of the field: repeatable literal
            while (!Tokens.IsEof)
            {
                token = Tokens.Current;
                if (token.Kind == PftTokenKind.RepeatableLiteral)
                {
                    literal = new PftRepeatableLiteral(token)
                    {
                        IsPrefix = true
                    };
                    leftHand.Add(literal);

                    if (Tokens.Peek() == PftTokenKind.Plus)
                    {
                        literal.Plus = true;
                        Tokens.MoveNext();
                    }

                    Tokens.MoveNext();
                }
                else if (token.Kind == PftTokenKind.Nl)
                {
                    leftHand.Add(new PftNl(token));
                    Tokens.MoveNext();
                }
                else if (token.Kind == PftTokenKind.Comma)
                {
                    leftHand.Add(new PftComma(token));
                    Tokens.MoveNext();
                }
                else if (token.Kind == PftTokenKind.Comment)
                {
                    leftHand.Add(new PftComment(token));
                    Tokens.MoveNext();
                }
                else
                {
                    break;
                }
            } // Tokens.IsEof

            // Orphaned left hand?
            if (Tokens.IsEof)
            {
                result = new PftOrphan();
                result.LeftHand.AddRange(leftHand);
                goto DONE;
            }

            // Parse field itself
            if (!Tokens.IsEof)
            {
                token = Tokens.Current;

                // Orphaned?
                if (token.Kind != PftTokenKind.V)
                {
                    result = new PftOrphan();
                    result.LeftHand.AddRange(leftHand);
                    goto DONE;
                }
                if (string.IsNullOrEmpty(token.Text))
                {
                    throw new PftSyntaxException(token);
                }

                FieldSpecification specification = (FieldSpecification)token.UserData;
                if (specification == null)
                {
                    throw new PftSyntaxException(token);
                }

                // Check for command code
                switch (specification.Command)
                {
                    case 'v':
                    case 'V':
                        // Already V
                        break;

                    case 'd':
                    case 'D':
                        result = new PftD();
                        break;

                    case 'n':
                    case 'N':
                        result = new PftN();
                        break;

                    case 'g':
                    case 'G':
                        result = new PftG();
                        break;

                    default:
                        throw new PftSyntaxException(token);
                }

                result.LeftHand.AddRange(leftHand);
                result.Apply(specification);
                Tokens.MoveNext();
            } // Tokens.IsEof

            // Gather right hand (for V command only)
            if (result is PftV)
            {
                if (!Tokens.IsEof)
                {
                    bool plus = false;
                    token = Tokens.Current;
                    if (token.Kind == PftTokenKind.Plus)
                    {
                        plus = true;
                        Tokens.RequireNext();
                        token = Tokens.Current;
                    }
                    if (token.Kind == PftTokenKind.RepeatableLiteral)
                    {
                        literal = new PftRepeatableLiteral(token)
                        {
                            Plus = plus
                        };
                        result.RightHand.Add(literal);
                        Tokens.MoveNext();
                    }
                    else
                    {
                        if (plus)
                        {
                            throw new PftSyntaxException(token);
                        }
                    }
                } // Tokens.IsEof

                if (!Tokens.IsEof)
                {
                    token = Tokens.Current;
                    if (token.Kind == PftTokenKind.ConditionalLiteral)
                    {
                        node = new PftConditionalLiteral(token);
                        result.RightHand.Add(node);
                        Tokens.MoveNext();
                    }
                }
            } // result is PftV

            DONE: return result;
        }

        private PftUnifor ParseUnifor
            (
                PftToken token
            )
        {
            PftUnifor result = new PftUnifor(token);

            Tokens.RequireNext();
            token = Tokens.Current;
            token.MustBe(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();

            while (true)
            {
                token = Tokens.Current;

                if (token.Kind == PftTokenKind.RightParenthesis)
                {
                    Tokens.MoveNext();
                    break;
                }

                if (token.Kind == PftTokenKind.LeftParenthesis)
                {
                    throw new PftSyntaxException(token);
                }

                PftNode node = ParseSimple();
                result.Children.Add(node);
            }

            return result;
        }

        private PftNode ParseComposite()
        {
            if (Tokens.Current.Kind == PftTokenKind.LeftParenthesis)
            {
                return ParseGroup();
            }

            return ParseSimple();
        }

        private PftProgram ParseProgram()
        {
            PftProgram result = new PftProgram();

            while (!Tokens.IsEof)
            {
                PftNode node = ParseComposite();
                result.Children.Add(node);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the tokens.
        /// </summary>
        [NotNull]
        public PftProgram Parse()
        {
            PftProgram result = ParseProgram();

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
