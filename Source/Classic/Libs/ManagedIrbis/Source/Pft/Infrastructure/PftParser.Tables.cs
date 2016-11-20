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
        // ReSharper disable InconsistentNaming

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
        private Dictionary<PftTokenKind, Func<PftNode>> MainModeMap { get; set; }

        /// <summary>
        /// Numeric expression context.
        /// </summary>
        [NotNull]
        // ReSharper disable once NotNullMemberIsNotInitialized
        private static Dictionary<PftTokenKind, Func<PftNode>> NumericMap { get; set; }

        private static PftTokenKind[] NumericModeItems =
        {
            PftTokenKind.Number, PftTokenKind.Val, PftTokenKind.Rsum,
            PftTokenKind.Ravr, PftTokenKind.Rmax, PftTokenKind.Rmin,
            PftTokenKind.Mfn, PftTokenKind.Variable,

            PftTokenKind.Abs, PftTokenKind.Ceil, PftTokenKind.Frac,
            PftTokenKind.Floor, PftTokenKind.Pow, PftTokenKind.Round,
            PftTokenKind.Sign, PftTokenKind.Trunc,

            PftTokenKind.L,
        };

        private static PftTokenKind[] RightHandItems =
        {
            PftTokenKind.C, PftTokenKind.Comment,
            PftTokenKind.Hash, PftTokenKind.Nl, PftTokenKind.Percent,
            PftTokenKind.Slash, PftTokenKind.X
        };

        private static PftTokenKind[] MainModeItems =
        {
            PftTokenKind.Break, PftTokenKind.Comma, PftTokenKind.C,
            PftTokenKind.Hash, PftTokenKind.Mfn, PftTokenKind.Mpl,
            PftTokenKind.Nl, PftTokenKind.Percent, PftTokenKind.Slash,
            PftTokenKind.TripleLess, PftTokenKind.UnconditionalLiteral,
            PftTokenKind.X, PftTokenKind.Unifor,

            PftTokenKind.V, PftTokenKind.ConditionalLiteral,
            PftTokenKind.RepeatableLiteral,

            PftTokenKind.Identifier, PftTokenKind.Variable,

            PftTokenKind.Number, PftTokenKind.F, PftTokenKind.F2,

            PftTokenKind.S,

            PftTokenKind.Comment,

            PftTokenKind.Ref,

            PftTokenKind.If, PftTokenKind.For, PftTokenKind.While,
            PftTokenKind.ForEach, PftTokenKind.From,

            PftTokenKind.LeftParenthesis,

            PftTokenKind.TripleCurly,

            PftTokenKind.At,

            PftTokenKind.Proc,

            PftTokenKind.EatOpen,

            PftTokenKind.Bang
        };

        // ================================================================

        private void CreateTokenMap()
        {
            MainModeMap = new Dictionary<PftTokenKind, Func<PftNode>>
            {
                {PftTokenKind.A, ParseA},
                {PftTokenKind.At, ParseAt},
                {PftTokenKind.Bang, ParseBang},
                {PftTokenKind.Break, ParseBreak},
                {PftTokenKind.C, ParseC},
                {PftTokenKind.Comma, ParseComma},
                {PftTokenKind.Comment, ParseComment},
                {PftTokenKind.ConditionalLiteral, ParseField},
                {PftTokenKind.EatOpen, ParseEat},
                {PftTokenKind.F, ParseF},
                {PftTokenKind.F2, ParseF2 },
                {PftTokenKind.For, ParseFor},
                {PftTokenKind.ForEach, ParseForEach},
                {PftTokenKind.From, ParseFrom},
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
                {PftTokenKind.Proc, ParseProc},
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
                {PftTokenKind.While, ParseWhile},
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
                {PftTokenKind.Floor, ParseFloor},
                {PftTokenKind.L, ParseL},
                {PftTokenKind.Mfn,ParseMfn},
                {PftTokenKind.Number, ParseNumber},
                {PftTokenKind.Pow, ParsePow},
                {PftTokenKind.Ravr, ParseRsum},
                {PftTokenKind.Rmax, ParseRsum},
                {PftTokenKind.Rmin, ParseRsum},
                {PftTokenKind.Round, ParseRound},
                {PftTokenKind.Rsum, ParseRsum},
                {PftTokenKind.Sign, ParseSign},
                {PftTokenKind.Trunc, ParseTrunc},
                {PftTokenKind.Val, ParseVal},
                {PftTokenKind.Variable, ParseVariableReference}
            };
        }

        //================================================================
        // Open and close tokens
        //================================================================

        private static PftTokenKind[] _andStop =
        {
            PftTokenKind.And, PftTokenKind.Or
        };

        private static PftTokenKind[] _comparisonStop =
        {
            PftTokenKind.Less, PftTokenKind.LessEqual,
            PftTokenKind.More, PftTokenKind.MoreEqual,
            PftTokenKind.Equals, PftTokenKind.NotEqual1,
            PftTokenKind.NotEqual2,

            PftTokenKind.Colon, PftTokenKind.Tilda
        };

        private static PftTokenKind[] _doStop =
        {
            PftTokenKind.Do
        };

        private static PftTokenKind[] _elseStop =
        {
            PftTokenKind.Else, PftTokenKind.Fi
        };

        private static PftTokenKind[] _emptyClose = { };

        private static PftTokenKind[] _emptyOpen = { };

        private static PftTokenKind[] _fiStop =
        {
            PftTokenKind.Fi
        };

        private static PftTokenKind[] _ifClose =
        {
            PftTokenKind.Fi
        };

        private static PftTokenKind[] _ifOpen =
        {
            PftTokenKind.If
        };

        private static PftTokenKind[] _loopClose =
        {
            PftTokenKind.End
        };

        private static PftTokenKind[] _loopOpen =
        {
            PftTokenKind.For, PftTokenKind.ForEach, PftTokenKind.While
        };

        private static PftTokenKind[] _loopStop =
        {
            PftTokenKind.End
        };

        private static PftTokenKind[] _orderStop =
        {
            PftTokenKind.Order, PftTokenKind.End
        };

        private static PftTokenKind[] _parenthesisClose =
        {
            PftTokenKind.RightParenthesis
        };

        private static PftTokenKind[] _parenthesisOpen =
        {
            PftTokenKind.LeftParenthesis
        };

        private static PftTokenKind[] _parenthesisStop =
        {
            PftTokenKind.RightParenthesis
        };

        private static PftTokenKind[] _procedureStop =
        {
            PftTokenKind.End
        };

        private static PftTokenKind[] _selectStop =
        {
            PftTokenKind.Select
        };

        private static PftTokenKind[] _semicolonStop =
        {
            PftTokenKind.Semicolon
        };

        private static PftTokenKind[] _thenStop =
        {
            PftTokenKind.Then
        };

        private static PftTokenKind[] _whereStop =
        {
            PftTokenKind.Where, PftTokenKind.Select
        };

    }
}
