// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftParser.Boolean.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    using Ast;

    partial class PftParser
    {
        private PftComparison ParseComparison()
        {
            PftComparison result = new PftComparison(Tokens.Current);

            PftTokenList leftTokens = Tokens.Segment(_comparisonStop)
                .ThrowIfNull("leftTokens");
            result.LeftOperand = ChangeContext
                (
                    leftTokens,
                    ParseComparisonItem
                );

            result.Operation = Tokens.Current.Text;
            Tokens.RequireNext();

            result.RightOperand = ParseComparisonItem();

            return result;
        }

        private PftNode ParseComparisonItem()
        {
            int position = Tokens.SavePosition();

            PftNode result;

            try
            {
                result = ParseArithmetic();
                if (!Tokens.IsEof)
                {
                    Log.Error
                        (
                            "PftParser::ParseComparisonItem: "
                            + "garbage detected"
                        );

                    throw new PftSyntaxException();
                }
            }
            catch
            {
                // This is intentional behavior
                // If we can't compare as arithmetic
                // then we must compare as text

                Tokens.RestorePosition(position);
                result = ParseNext();

                if (!Tokens.IsEof)
                {
                    PftNode node = new PftNode();
                    node.Children.Add(result);
                    result = node;

                    while (!Tokens.IsEof)
                    {
                        node = ParseNext();
                        result.Children.Add(node);
                    }
                }
            }

            return result;
        }

        private PftCondition _ParseCondition()
        {
            PftToken token = Tokens.Current;

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
                not.InnerCondition = ParseCondition();
                result = not;
            }
            else if (token.Kind == PftTokenKind.Have)
            {
                result = ParseHave();
            }
            else if (token.Kind == PftTokenKind.Empty)
            {
                result = ParseEmpty();
            }
            else if (token.Kind == PftTokenKind.Blank)
            {
                result = ParseBlank();
            }
            else if (token.Kind == PftTokenKind.True)
            {
                result = ParseTrue();
            }
            else if (token.Kind == PftTokenKind.False)
            {
                result = ParseFalse();
            }
            else if (token.Kind == PftTokenKind.LeftParenthesis)
            {
                PftConditionParenthesis parenthesis
                    = new PftConditionParenthesis(token);
                Tokens.RequireNext();
                PftTokenList innerTokens = Tokens.Segment
                    (
                        _parenthesisOpen,
                        _parenthesisClose,
                        _parenthesisStop
                    )
                    .ThrowIfNull("innerTokens");
                parenthesis.InnerCondition
                    = (PftCondition) ChangeContext
                    (
                        innerTokens,
                        ParseCondition
                    );
                Tokens.Current.MustBe(PftTokenKind.RightParenthesis);
                Tokens.MoveNext();
                result = parenthesis;
            }
            else if (token.Kind == PftTokenKind.All)
            {
                PftAll all = ParseAll();
                result = all;
            }
            else if (token.Kind == PftTokenKind.Any)
            {
                PftAny any = ParseAny();
                result = any;
            }
            else
            {
                PftComparison comparison = ParseComparison();
                result = comparison;
            }

            return result;
        }

        private PftCondition ParseCondition()
        {
            PftCondition result;

            PftTokenList conditionTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _andStop
                );
            if (ReferenceEquals(conditionTokens, null))
            {
                result = _ParseCondition();

                return result;
            }

            result = (PftCondition) ChangeContext
                (
                    conditionTokens,
                    _ParseCondition
                );

            while (!Tokens.IsEof)
            {
                PftToken token = Tokens.Current;

                if (token.Kind == PftTokenKind.And
                    || token.Kind == PftTokenKind.Or)
                {
                    PftConditionAndOr andOr = new PftConditionAndOr(token)
                    {
                        LeftOperand = result,
                        Operation = token.Text
                    };
                    Tokens.RequireNext();

                    PftCondition right;

                    conditionTokens = Tokens.Segment(_andStop);
                    if (ReferenceEquals(conditionTokens, null))
                    {
                        right = _ParseCondition();
                    }
                    else
                    {
                        right = (PftCondition) ChangeContext
                            (
                                conditionTokens,
                                _ParseCondition
                            );
                    }

                    andOr.RightOperand = right;
                    result = andOr;
                }
            }

            return result;
        }

        //=================================================

        private PftCondition ParseFalse()
        {
            return MoveNext(new PftFalse(Tokens.Current));
        }

        //=================================================

        private PftNode ParseIf()
        {
            PftConditionalStatement result
                = new PftConditionalStatement(Tokens.Current);
            Tokens.RequireNext();

            PftTokenList conditionTokens = Tokens.Segment
                (
                    _thenStop
                )
                .ThrowIfNull("conditionTokens");
            Tokens.Current.MustBe(PftTokenKind.Then);

            PftCondition condition
                = (PftCondition) ChangeContext
                (
                    conditionTokens,
                    ParseCondition
                );
                
            result.Condition = condition;

            Tokens.RequireNext();

            PftTokenList thenTokens = Tokens.Segment
                (
                    _ifOpen,
                    _ifClose,
                    _elseStop
                )
                .ThrowIfNull("thenTokens");
            ChangeContext
                (
                    result.ThenBranch,
                    thenTokens
                );

            if (Tokens.Current.Kind == PftTokenKind.Else)
            {
                Tokens.RequireNext();

                PftTokenList elseTokens = Tokens.Segment
                    (
                        _ifOpen,
                        _ifClose,
                        _fiStop
                    )
                    .ThrowIfNull("elseTokens");
                ChangeContext
                    (
                        result.ElseBranch,
                        elseTokens
                    );
            }

            Tokens.Current.MustBe(PftTokenKind.Fi);
            Tokens.MoveNext();

            return result;
        }

        //=================================================

        private PftCondition ParseTrue()
        {
            return MoveNext(new PftTrue(Tokens.Current));
        }
    }
}
