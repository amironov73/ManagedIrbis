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
    using Ast;

    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Parser for PFT language.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftParser
    {
        #region Properties

        private static PftTokenKind[] ComparisonTokens =
        {
            PftTokenKind.Mfn, PftTokenKind.Nl,
            PftTokenKind.UnconditionalLiteral, PftTokenKind.V,
            PftTokenKind.Unifor, PftTokenKind.S,

            PftTokenKind.Identifier, PftTokenKind.Variable,
        };

        /// <summary>
        /// Field reference context.
        /// </summary>
        [NotNull]
        private Dictionary<PftTokenKind, Func<PftNode>> FieldMap { get; set; }

        private static PftTokenKind[] LeftHandItems1 =
        {
            PftTokenKind.ConditionalLiteral, PftTokenKind.C,
            PftTokenKind.Comma, PftTokenKind.Comment, PftTokenKind.Hash,
            PftTokenKind.Mpl, PftTokenKind.Nl, PftTokenKind.Percent,
            PftTokenKind.Slash, PftTokenKind.X
        };

        private static PftTokenKind[] LeftHandItems2 =
        {
            PftTokenKind.C, PftTokenKind.Comma, PftTokenKind.Comment,
            PftTokenKind.Hash, PftTokenKind.Nl, PftTokenKind.Percent,
            PftTokenKind.Slash, PftTokenKind.X
        };

        /// <summary>
        /// Main script context.
        /// </summary>
        [NotNull]
        private Dictionary<PftTokenKind, Func<PftNode>> MainMap { get; set; }

        /// <summary>
        /// Numeric expression context.
        /// </summary>
        [NotNull]
        private static Dictionary<PftTokenKind, Func<PftNode>> NumericMap { get; set; }

        private static PftTokenKind[] NumericTokens =
        {
            PftTokenKind.Number, PftTokenKind.Val, PftTokenKind.Rsum,
            PftTokenKind.Ravr, PftTokenKind.Rmax, PftTokenKind.Rmin,
            PftTokenKind.Mfn, PftTokenKind.Variable,

            PftTokenKind.L,
        };

        private static PftTokenKind[] NumericGoodies =
        {
            PftTokenKind.Number, PftTokenKind.Val, PftTokenKind.Rsum,
            PftTokenKind.Ravr, PftTokenKind.Rmax, PftTokenKind.Rmin,
            PftTokenKind.Mfn, PftTokenKind.Plus, PftTokenKind.Minus,
            PftTokenKind.Star, PftTokenKind.Div,

            PftTokenKind.L,
        };

        private static PftTokenKind[] NumericLimiter =
        {
            PftTokenKind.Semicolon
        };

        private static PftTokenKind[] RightHandItems =
        {
            PftTokenKind.C, PftTokenKind.Comment,
            PftTokenKind.Hash, PftTokenKind.Nl, PftTokenKind.Percent,
            PftTokenKind.Slash, PftTokenKind.X
        };

        private static PftTokenKind[] SimpleTokens =
        {
            PftTokenKind.Break, PftTokenKind.Comma, PftTokenKind.C,
            PftTokenKind.Hash, PftTokenKind.Mfn, PftTokenKind.Mpl,
            PftTokenKind.Nl, PftTokenKind.Percent, PftTokenKind.Slash,
            PftTokenKind.UnconditionalLiteral,
            PftTokenKind.X, PftTokenKind.Unifor,

            PftTokenKind.V, PftTokenKind.ConditionalLiteral,
            PftTokenKind.RepeatableLiteral,

            PftTokenKind.Identifier, PftTokenKind.Variable,

            PftTokenKind.Number, PftTokenKind.F,

            PftTokenKind.Ref,

            PftTokenKind.If
        };

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

            procedures = new PftProcedureManager();
            CreateTokenMap();
        }

        #endregion

        #region Private members

        //================================================================
        // Service variables
        //================================================================

        private bool inAssignment;

        private PftProcedureManager procedures;

        private void CreateTokenMap()
        {
            MainMap = new Dictionary<PftTokenKind, Func<PftNode>>
            {
                {PftTokenKind.A, ParseA},
                {PftTokenKind.Break, ParseBreak},
                {PftTokenKind.C, ParseC},
                {PftTokenKind.Comma, ParseComma},
                {PftTokenKind.Comment, ParseComment},
                {PftTokenKind.ConditionalLiteral, ParseField},
                {PftTokenKind.F, ParseF},
                {PftTokenKind.Hash, ParseHash},
                {PftTokenKind.Identifier, ParseFunctionCall},
                {PftTokenKind.If, ParseIf},
                {PftTokenKind.L, ParseL},
                {PftTokenKind.Mfn, ParseMfn},
                {PftTokenKind.Mpl, ParseMpl},
                {PftTokenKind.Nl, ParseNl},
                {PftTokenKind.Number, ParseNumber},
                {PftTokenKind.P,ParseP},
                {PftTokenKind.Percent, ParsePercent},
                {PftTokenKind.Ref, ParseRef},
                {PftTokenKind.RepeatableLiteral, ParseField},
                {PftTokenKind.S, ParseS},
                {PftTokenKind.Semicolon, ParseSemicolon},
                {PftTokenKind.Slash, ParseSlash},
                {PftTokenKind.UnconditionalLiteral, ParseUnconditionalLiteral},
                {PftTokenKind.Unifor, ParseUnifor},
                {PftTokenKind.V,ParseField},
                {PftTokenKind.Variable, ParseVariable},
                {PftTokenKind.X, ParseX}
            };

            FieldMap = new Dictionary<PftTokenKind, Func<PftNode>>
            {
                {PftTokenKind.C, ParseC},
                {PftTokenKind.Comma, ParseComma},
                {PftTokenKind.Comment, ParseComment},
                {PftTokenKind.ConditionalLiteral, ParseConditionalLiteral},
                {PftTokenKind.Hash, ParseHash},
                {PftTokenKind.Identifier, ParseFunctionCall},
                {PftTokenKind.Mpl, ParseMpl},
                {PftTokenKind.Nl, ParseNl},
                {PftTokenKind.Percent, ParsePercent},
                {PftTokenKind.RepeatableLiteral, ParseRepeatableLiteral},
                {PftTokenKind.Semicolon, ParseSemicolon},
                { PftTokenKind.Slash, ParseSlash},
                {PftTokenKind.UnconditionalLiteral, ParseUnconditionalLiteral},
                {PftTokenKind.X, ParseX}
            };

            NumericMap = new Dictionary<PftTokenKind, Func<PftNode>>
            {
                {PftTokenKind.L, ParseL},
                {PftTokenKind.Mfn,ParseMfn},
                {PftTokenKind.Number, ParseNumber},
                {PftTokenKind.Rsum, ParseRsum},
                {PftTokenKind.Rmax, ParseRsum},
                {PftTokenKind.Rmin, ParseRsum},
                {PftTokenKind.Ravr, ParseRsum},
                {PftTokenKind.Val, ParseVal},
                {PftTokenKind.Variable, ParseVariableReference}
            };
        }

        /// <summary>
        /// Create next AST node from token list.
        /// </summary>
        [CanBeNull]
        public PftNode Get
            (
                [NotNull] Dictionary<PftTokenKind, Func<PftNode>> map,
                [NotNull] PftTokenKind[] expectedTokens
            )
        {
            PftNode result = null;
            PftToken token = Tokens.Current;

            if (Array.IndexOf(expectedTokens, token.Kind) >= 0)
            {
                Func<PftNode> function;
                if (!map.TryGetValue(token.Kind, out function))
                {
                    throw new PftException
                        (
                            "don't know how to handle token "
                            + token.Kind
                        );
                }
                result = function();
            }

            return result;
        }

        private bool LookFor
            (
                [NotNull] PftTokenKind[] target,
                [NotNull] PftTokenKind[] limiter
            )
        {
            bool result = false;

            for (int i = 0; ; i++)
            {
                PftTokenKind kind = Tokens.Peek(i);
                if (kind == PftTokenKind.None)
                {
                    break;
                }
                if (Array.IndexOf(limiter, kind) >= 0)
                {
                    break;
                }
                if (Array.IndexOf(target, kind) >= 0)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        [NotNull]
        private T MoveNext<T>([NotNull] T node)
            where T : PftNode
        {
            Tokens.MoveNext();
            return node;
        }

        //================================================================
        // Parsing
        //================================================================

        private PftA ParseA()
        {
            PftA result = new PftA(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext(PftTokenKind.V);
            PftField field = (PftField)ParseField();
            if (ReferenceEquals(field, null))
            {
                throw new PftSyntaxException(Tokens.Current);
            }
            result.Field = field;
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            return MoveNext(result);
        }

        private PftNumeric ParseArithmetic()
        {
            PftNumeric result = (PftNumeric)Get(NumericMap, NumericTokens);

            if (!Tokens.IsEof)
            {
                PftTokenKind kind = Tokens.Current.Kind;
                if (kind == PftTokenKind.Plus
                    || kind == PftTokenKind.Minus
                    || kind == PftTokenKind.Star
                    || kind == PftTokenKind.Div
                    )
                {
                    PftNumericExpression expression = new PftNumericExpression
                    {
                        LeftOperand = result
                    };
                    expression.Operation = Tokens.Current.Text;
                    Tokens.RequireNext();
                    expression.RightOperand = ParseArithmetic();
                    result = expression;
                }
            }

            return result;
        }

        private PftNode ParseBreak()
        {
            return MoveNext(new PftBreak(Tokens.Current));
        }

        private PftNode ParseC()
        {
            return MoveNext(new PftC(Tokens.Current));
        }

        private PftNode ParseCall(PftNode result)
        {
            Tokens.RequireNext();
            return ParseCall2(result);
        }

        private PftNode ParseCall2(PftNode result)
        {
            PftToken token = Tokens.Current;
            token.MustBe(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            return ParseCall3(result);
        }

        private PftNode ParseCall3(PftNode result)
        {
            bool ok = false;
            while (!Tokens.IsEof)
            {
                PftToken token = Tokens.Current;

                if (token.Kind == PftTokenKind.RightParenthesis)
                {
                    ok = true;
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

            if (!ok)
            {
                throw new PftSyntaxException(Tokens);
            }

            return result;
        }

        private PftNode ParseComma()
        {
            return MoveNext(new PftComma(Tokens.Current));
        }

        private PftNode ParseComment()
        {
            return MoveNext(new PftComment(Tokens.Current));
        }

        private PftComparison ParseComparison()
        {
            PftComparison result = new PftComparison(Tokens.Current)
            {
                LeftOperand = ParseComparisonItem(),
                Operation = Tokens.Current.Text
            };
            Tokens.RequireNext();
            result.RightOperand = ParseComparisonItem();

            return result;
        }

        private PftNode ParseComparisonItem()
        {
            PftNode result = Get(MainMap, ComparisonTokens);

            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            throw new PftSyntaxException(Tokens.Current);
        }

        private PftNode ParseComposite()
        {
            if (Tokens.Current.Kind == PftTokenKind.Variable)
            {
                return ParseVariable();
            }
            if (Tokens.Current.Kind == PftTokenKind.LeftParenthesis)
            {
                return ParseGroup();
            }

            return ParseSimple();
        }

        private PftCondition ParseCondition
            (
                [NotNull] PftToken token
            )
        {
            PftCondition result;

            if (token.Kind == PftTokenKind.P)
            {
                result = ParseP();
            }
            else if (token.Kind == PftTokenKind.A)
            {
                result = ParseA();
            }
            else if (token.Kind == PftTokenKind.Not)
            {
                PftConditionNot not = new PftConditionNot(token);
                Tokens.RequireNext();
                not.InnerCondition = ParseCondition(Tokens.Current);
                result = not;
            }
            else if (token.Kind == PftTokenKind.LeftParenthesis)
            {
                PftConditionParenthesis parenthesis = new PftConditionParenthesis(token);
                Tokens.RequireNext();
                parenthesis.InnerCondition = ParseConditionAndOr(Tokens.Current);
                Tokens.RequireNext(PftTokenKind.RightParenthesis);
                result = parenthesis;
            }
            else
            {
                PftComparison comparison = ParseComparison();
                result = comparison;
            }

            return result;
        }

        private PftCondition ParseConditionAndOr
            (
                [NotNull] PftToken token
            )
        {
            PftCondition result = ParseCondition(token);

            while (!Tokens.IsEof)
            {
                token = Tokens.Current;

                if (token.Kind == PftTokenKind.And
                    || token.Kind == PftTokenKind.Or)
                {
                    PftConditionAndOr andOr = new PftConditionAndOr(token)
                    {
                        LeftOperand = result,
                        Operation = token.Text
                    };
                    Tokens.RequireNext();
                    PftCondition right = ParseCondition(Tokens.Current);
                    andOr.RightOperand = right;
                    result = andOr;
                }
                else if (token.Kind == PftTokenKind.Then)
                {
                    break;
                }
            }

            return result;
        }

        private PftNode ParseConditionalLiteral()
        {
            return MoveNext(new PftConditionalLiteral(Tokens.Current));
        }

        private PftNode ParseF()
        {
            PftF result = new PftF(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            result.Argument1 = ParseArithmetic();
            if (Tokens.IsEof)
            {
                throw new PftSyntaxException(Tokens);
            }
            if (Tokens.Current.Kind != PftTokenKind.RightParenthesis)
            {
                if (Tokens.Current.Kind != PftTokenKind.Comma)
                {
                    throw new PftSyntaxException(Tokens.Current);
                }
                Tokens.RequireNext();
                result.Argument2 = ParseNumber();
                if (Tokens.IsEof)
                {
                    throw new PftSyntaxException(Tokens);
                }
                if (Tokens.Current.Kind != PftTokenKind.RightParenthesis)
                {
                    if (Tokens.Current.Kind != PftTokenKind.Comma)
                    {
                        throw new PftSyntaxException(Tokens.Current);
                    }
                    Tokens.RequireNext();
                    result.Argument3 = ParseNumber();
                }
            }
            if (Tokens.IsEof)
            {
                throw new PftSyntaxException(Tokens);
            }
            if (Tokens.Current.Kind != PftTokenKind.RightParenthesis)
            {
                throw new PftSyntaxException(Tokens.Current);
            }
            Tokens.MoveNext();

            return result;
        }

        private PftNode ParseField()
        {
            List<PftNode> leftHand = new List<PftNode>();
            PftField result = new PftV();
            PftNode node;
            PftRepeatableLiteral literal;
            PftToken token;

            // Gather left hand of the field: conditional literal and friends
            while (!Tokens.IsEof)
            {
                token = Tokens.Current;
                if (token.Kind == PftTokenKind.RepeatableLiteral
                    || token.Kind == PftTokenKind.V)
                {
                    break;
                }

                node = Get(FieldMap, LeftHandItems1);
                if (!ReferenceEquals(node, null))
                {
                    leftHand.Add(node);
                }
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
                else
                {
                    node = Get(FieldMap, LeftHandItems2);
                    if (ReferenceEquals(node, null))
                    {
                        break;
                    }
                    leftHand.Add(node);
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

                FieldSpecification specification
                    = (FieldSpecification)token.UserData;
                if (ReferenceEquals(specification, null))
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

                        node = Get(FieldMap, RightHandItems);
                        if (!ReferenceEquals(node, null))
                        {
                            result.RightHand.Add(node);
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

        private PftNode ParseFunctionCall()
        {
            PftFunctionCall result = new PftFunctionCall(Tokens.Current);
            return ParseCall(result);
        }

        private PftNode ParseGroup()
        {
            PftGroup result = new PftGroup(Tokens.Current);
            ParseCall2(result);

            return result;
        }

        private PftNode ParseHash()
        {
            return MoveNext(new PftHash(Tokens.Current));
        }

        private PftNode ParseIf()
        {
            PftConditionalStatement result
                = new PftConditionalStatement(Tokens.Current);
            Tokens.RequireNext();

            PftCondition condition = ParseConditionAndOr(Tokens.Current);
            result.Condition = condition;

            Tokens.Current.MustBe(PftTokenKind.Then);
            Tokens.RequireNext();

            bool complete = false;
            bool ok = false;
            PftToken token;
            while (!Tokens.IsEof)
            {
                token = Tokens.Current;

                if (token.Kind == PftTokenKind.Fi)
                {
                    Tokens.MoveNext();
                    ok = true;
                    complete = true;
                    break;
                }

                if (token.Kind == PftTokenKind.Else)
                {
                    Tokens.MoveNext();
                    ok = true;
                    break;
                }

                //PftNode node = ParseSimple();
                PftNode node = ParseComposite();
                result.ThenBranch.Add(node);
            }

            if (!complete)
            {
                while (!Tokens.IsEof)
                {
                    token = Tokens.Current;

                    if (token.Kind == PftTokenKind.Fi)
                    {
                        Tokens.MoveNext();
                        ok = true;
                        complete = true;
                        break;
                    }

                    //PftNode node = ParseSimple();
                    PftNode node = ParseComposite();
                    result.ElseBranch.Add(node);
                }

                if (!ok)
                {
                    throw new PftSyntaxException(Tokens);
                }
            }

            if (!ok)
            {
                throw new PftSyntaxException(Tokens);
            }

            if (!complete)
            {
                throw new PftSyntaxException(Tokens);
            }

            return result;
        }

        private PftNode ParseL()
        {
            PftL result = new PftL(Tokens.Current);
            ParseCall(result);
            return result;
        }

        private PftNode ParseMfn()
        {
            return MoveNext(new PftMfn(Tokens.Current));
        }

        private PftNode ParseMpl()
        {
            return MoveNext(new PftMode(Tokens.Current));
        }

        private PftNode ParseNl()
        {
            return MoveNext(new PftNl(Tokens.Current));
        }

        private PftNumeric ParseNumber()
        {
            return MoveNext(new PftNumericLiteral(Tokens.Current));
        }

        private PftP ParseP()
        {
            PftP result = new PftP(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext(PftTokenKind.V);
            PftField field = (PftField)ParseField();
            if (ReferenceEquals(field, null))
            {
                throw new PftSyntaxException(Tokens.Current);
            }
            result.Field = field;
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            return MoveNext(result);
        }

        private PftNode ParsePercent()
        {
            return MoveNext(new PftPercent(Tokens.Current));
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

        private PftNode ParseRef()
        {
            PftRef result = new PftRef(Tokens.Current);

            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            //result.Mfn = ParseNumber();
            result.Mfn = ParseArithmetic();
            Tokens.Current.MustBe(PftTokenKind.Comma);
            Tokens.RequireNext();
            PftNode pseudo = new PftNode();
            ParseCall3(pseudo);
            result.Format.AddRange(pseudo.Children);

            return result;
        }

        private PftNode ParseRepeatableLiteral()
        {
            return MoveNext(new PftRepeatableLiteral(Tokens.Current));
        }

        private PftNode ParseRsum()
        {
            PftNode result = new PftRsum(Tokens.Current);
            return ParseCall(result);
        }

        private PftNode ParseS()
        {
            PftNode result = new PftS(Tokens.Current);
            ParseCall(result);
            return result;
        }

        [NotNull]
        private PftNode ParseSimple()
        {
            PftNode result = Get(MainMap, SimpleTokens);

            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            throw new PftSyntaxException(Tokens.Current);
        }

        private PftNode ParseSemicolon()
        {
            return MoveNext(new PftSemicolon(Tokens.Current));
        }

        private PftNode ParseSlash()
        {
            return MoveNext(new PftSlash(Tokens.Current));
        }

        private PftNode ParseUnconditionalLiteral()
        {
            return MoveNext(new PftUnconditionalLiteral(Tokens.Current));
        }

        private PftNode ParseUnifor()
        {
            PftNode result = new PftUnifor(Tokens.Current);
            return ParseCall(result);
        }

        private PftNode ParseVal()
        {
            PftNode result = new PftVal(Tokens.Current);
            return ParseCall(result);
        }

        private PftNode ParseVariable()
        {
            if (Tokens.Peek() == PftTokenKind.Equals)
            {
                if (inAssignment)
                {
                    throw new PftSyntaxException("nested assignment");
                }

                try
                {
                    inAssignment = true;

                    PftAssignment result = new PftAssignment(Tokens.Current);
                    Tokens.RequireNext(PftTokenKind.Equals);
                    Tokens.RequireNext();

                    if (LookFor(NumericGoodies, NumericLimiter))
                    {
                        while (!Tokens.IsEof)
                        {
                            if (Tokens.Current.Kind == PftTokenKind.Semicolon)
                            {
                                Tokens.MoveNext();
                                break;
                            }
                            PftNode node = ParseArithmetic();
                            result.Children.Add(node);
                        }
                    }
                    else
                    {
                        while (!Tokens.IsEof)
                        {
                            if (Tokens.Current.Kind == PftTokenKind.Semicolon)
                            {
                                Tokens.MoveNext();
                                break;
                            }
                            PftNode node = ParseComposite();
                            result.Children.Add(node);
                        }
                    }

                    return result;
                }
                finally
                {
                    inAssignment = false;
                }
            }

            PftNode reference = new PftVariableReference(Tokens.Current);

            return MoveNext(reference);
        }

        private PftNode ParseVariableReference()
        {
            return MoveNext(new PftVariableReference(Tokens.Current));
        }

        private PftNode ParseX()
        {
            return MoveNext(new PftX(Tokens.Current));
        }

        //================================================================
        // Other routines
        //================================================================

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the tokens.
        /// </summary>
        [NotNull]
        public PftProgram Parse()
        {
            PftProgram result = ParseProgram();
            result.Procedures = procedures;

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
