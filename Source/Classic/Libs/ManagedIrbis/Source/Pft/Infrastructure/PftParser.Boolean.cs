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

    partial class PftParser
    {
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

        private PftNode ParseCondition()
        {
            return ParseCondition(Tokens.Current);
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

    }
}
