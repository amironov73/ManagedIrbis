/* PftParser.Arith.cs --
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
        private PftNumeric ParseArithmetic()
        {
            PftNumeric result = ParseAddition();

            return result;
        }

        private PftNumeric ParseArithmetic
            (
                params PftTokenKind[] stop
            )
        {
            PftTokenList newTokens = Tokens.Segment(stop);
            if (ReferenceEquals(newTokens, null))
            {
                throw new PftSyntaxException(Tokens);
            }

            PftTokenList saveTokens = Tokens;
            try
            {
                Tokens = newTokens;
                return ParseArithmetic();
            }
            finally
            {
                Tokens = saveTokens;
            }
        }

        private PftNumeric ParseAddition()
        {
            PftNumeric left = ParseMultiplication();
            while (!Tokens.IsEof)
            {
                PftToken token = Tokens.Current;
                if (token.Kind != PftTokenKind.Plus
                    && token.Kind == PftTokenKind.Minus)
                {
                    break;
                }
                Tokens.RequireNext();
                left = new PftNumericExpression
                {
                    LeftOperand = left,
                    Operation = token.Text,
                    RightOperand = ParseMultiplication()
                };
            }

            return left;
        }

        private PftNumeric ParseMultiplication()
        {
            PftNumeric left = ParseValue();
            while (!Tokens.IsEof)
            {
                PftToken token = Tokens.Current;
                if (token.Kind != PftTokenKind.Star
                    && token.Kind != PftTokenKind.Slash)
                {
                    break;
                }
                Tokens.RequireNext();
                left = new PftNumericExpression
                {
                    LeftOperand = left,
                    Operation = token.Text,
                    RightOperand = ParseValue()
                };
            }

            return left;
        }

        private PftNumeric ParseValue()
        {
            if (Tokens.IsEof)
            {
                throw new PftSyntaxException(Tokens);
            }
            PftToken token = Tokens.Current;

            if (token.Kind == PftTokenKind.LeftParenthesis)
            {
                Tokens.RequireNext();
                PftNumeric inner = ParseArithmetic(PftTokenKind.RightParenthesis);
                Tokens.MoveNext();
                return inner;
            }
            if (token.Kind == PftTokenKind.Minus)
            {
                PftMinus minus = new PftMinus(token);
                Tokens.RequireNext();
                PftNumeric child = ParseValue();
                minus.Children.Add(child);
                return minus;
            }

            PftNumeric result = (PftNumeric)Get
                (
                    NumericMap,
                    NumericTokens
                );

            return result;
        }

        private PftNumeric ParseFunction
            (
                PftNumeric result
            )
        {
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            PftNumeric expression = ParseArithmetic(PftTokenKind.RightParenthesis);
            result.Children.Add(expression);
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);
            Tokens.MoveNext();

            return result;
        }

        private PftNumeric ParseAbs()
        {
            PftNumeric result = new PftAbs(Tokens.Current);
            return ParseFunction(result);
        }

        private PftNumeric ParseCeil()
        {
            PftNumeric result = new PftCeil(Tokens.Current);
            return ParseFunction(result);
        }

        private PftNumeric ParseFrac()
        {
            PftNumeric result = new PftFrac(Tokens.Current);
            return ParseFunction(result);
        }

        private PftNumeric ParseFloor()
        {
            PftNumeric result = new PftFloor(Tokens.Current);
            return ParseFunction(result);
        }

        private PftNumeric ParsePow()
        {
            PftPow result = new PftPow(Tokens.Current);

            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            result.X = ParseArithmetic(PftTokenKind.Comma);
            Tokens.Current.MustBe(PftTokenKind.Comma);
            Tokens.MoveNext();
            result.Y = ParseArithmetic(PftTokenKind.RightParenthesis);
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);
            Tokens.MoveNext();

            return result;
        }

        private PftNumeric ParseRound()
        {
            PftNumeric result = new PftRound(Tokens.Current);
            return ParseFunction(result);
        }

        private PftNumeric ParseSign()
        {
            PftNumeric result = new PftSign(Tokens.Current);
            return ParseFunction(result);
        }

        private PftNumeric ParseTrunc()
        {
            PftNumeric result = new PftTrunc(Tokens.Current);
            return ParseFunction(result);
        }
    }
}
