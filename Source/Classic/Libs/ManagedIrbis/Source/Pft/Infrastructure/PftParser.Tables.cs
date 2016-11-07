/* PftParser.Tables.cs --
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
            PftTokenKind.TripleLess, PftTokenKind.UnconditionalLiteral,
            PftTokenKind.X, PftTokenKind.Unifor,

            PftTokenKind.V, PftTokenKind.ConditionalLiteral,
            PftTokenKind.RepeatableLiteral,

            PftTokenKind.Identifier, PftTokenKind.Variable,

            PftTokenKind.Number, PftTokenKind.F,

            PftTokenKind.Comment,

            PftTokenKind.Ref,

            PftTokenKind.If,

            PftTokenKind.LeftParenthesis,

            PftTokenKind.TripleCurly,

            PftTokenKind.At
        };

        // ================================================================

        private void CreateTokenMap()
        {
            MainMap = new Dictionary<PftTokenKind, Func<PftNode>>
            {
                {PftTokenKind.A, ParseA},
                {PftTokenKind.At, ParseAt},
                {PftTokenKind.Break, ParseBreak},
                {PftTokenKind.C, ParseC},
                {PftTokenKind.Comma, ParseComma},
                {PftTokenKind.Comment, ParseComment},
                {PftTokenKind.ConditionalLiteral, ParseField},
                {PftTokenKind.F, ParseF},
                {PftTokenKind.LeftParenthesis, ParseGroup},
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
                {PftTokenKind.TripleCurly, ParseCodeBlock},
                {PftTokenKind.TripleLess, ParseVerbatim},
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
                {PftTokenKind.Abs, ParseAbs},
                {PftTokenKind.Ceil, ParseCeil},
                {PftTokenKind.Frac, ParseFrac},
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
    }
}
