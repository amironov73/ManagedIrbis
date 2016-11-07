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
                // TODO implement
                return null;
            }
            if (token.Kind == PftTokenKind.Minus)
            {
                // TODO implement
                return null;
            }

            PftNumeric result = (PftNumeric)Get
                (
                    NumericMap,
                    NumericTokens
                );

            return result;
        }
    }
}
