// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftParser.Tables.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
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

            PftTokenKind.First, PftTokenKind.Last,

            PftTokenKind.L,
        };

        private static PftTokenKind[] RightHandItems =
        {
            PftTokenKind.C, PftTokenKind.Comment,
            PftTokenKind.Nl, PftTokenKind.X
        };

        private static PftTokenKind[] MainModeItems =
        {
            PftTokenKind.Break, PftTokenKind.Comma, PftTokenKind.C,
            PftTokenKind.Hash, PftTokenKind.Mfn, PftTokenKind.Mpl,
            PftTokenKind.Nl, PftTokenKind.Percent, PftTokenKind.Slash,
            PftTokenKind.TripleLess, PftTokenKind.UnconditionalLiteral,
            PftTokenKind.X, PftTokenKind.Unifor,

            PftTokenKind.GraveAccent,

            PftTokenKind.Eval, PftTokenKind.CsEval,

            PftTokenKind.V, PftTokenKind.ConditionalLiteral,
            PftTokenKind.RepeatableLiteral,

            PftTokenKind.Identifier, PftTokenKind.Variable,

            PftTokenKind.Number, PftTokenKind.F, PftTokenKind.Fmt,

            PftTokenKind.S,

            PftTokenKind.Comment,

            PftTokenKind.Ref,

            PftTokenKind.If, PftTokenKind.For,
            PftTokenKind.While, PftTokenKind.ForEach,
            PftTokenKind.From, PftTokenKind.Local,
            PftTokenKind.With,

            PftTokenKind.LeftParenthesis,

            PftTokenKind.LeftCurly,

            PftTokenKind.TripleCurly,

            PftTokenKind.At,

            PftTokenKind.Proc,

            PftTokenKind.EatOpen,

            PftTokenKind.Bang,

            PftTokenKind.Parallel,
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
                {PftTokenKind.CsEval, ParseCsEval},
                {PftTokenKind.EatOpen, ParseEat},
                {PftTokenKind.Eval, ParseEval},
                {PftTokenKind.F, ParseF},
                {PftTokenKind.Fmt, ParseF2 },
                {PftTokenKind.For, ParseFor},
                {PftTokenKind.ForEach, ParseForEach},
                {PftTokenKind.From, ParseFrom},
                {PftTokenKind.GraveAccent, ParseGraveAccent},
                {PftTokenKind.LeftCurly, ParseNested},
                {PftTokenKind.LeftParenthesis, ParseGroup},
                {PftTokenKind.Hash, ParseHash},
                {PftTokenKind.Identifier, ParseFunctionCall},
                {PftTokenKind.If, ParseIf},
                {PftTokenKind.L, ParseL},
                {PftTokenKind.Local, ParseLocal},
                {PftTokenKind.Mfn, ParseMfn},
                {PftTokenKind.Mpl, ParseMpl},
                {PftTokenKind.Nl, ParseNl},
                {PftTokenKind.Number, ParseNumber},
                {PftTokenKind.P,ParseP},
                {PftTokenKind.Parallel, ParseParallel},
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
                {PftTokenKind.V,ParseFieldAssignment},
                {PftTokenKind.Variable, ParseVariable},
                {PftTokenKind.While, ParseWhile},
                {PftTokenKind.With, ParseWith},
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
                {PftTokenKind.First, ParseFirst},
                {PftTokenKind.Frac, ParseFrac},
                {PftTokenKind.Floor, ParseFloor},
                {PftTokenKind.L, ParseL},
                {PftTokenKind.Last, ParseLast},
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

        private static TokenPair[] _curlyPair =
        {
            new TokenPair
            {
                Open = PftTokenKind.LeftCurly,
                Close = PftTokenKind.RightCurly
            }
        };

        private static PftTokenKind[] _curlyClose =
        {
            PftTokenKind.RightCurly
        };

        private static PftTokenKind[] _curlyOpen =
        {
            PftTokenKind.LeftCurly
        };

        private static PftTokenKind[] _curlyStop =
        {
            PftTokenKind.RightCurly
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

        private static TokenPair[] _loopPairs =
        {
            new TokenPair
            {
                Open = PftTokenKind.For,
                Close = PftTokenKind.End
            },
            new TokenPair
            {
                Open = PftTokenKind.ForEach,
                Close = PftTokenKind.End
            },
            new TokenPair
            {
                Open = PftTokenKind.Local,
                Close = PftTokenKind.End
            },
            new TokenPair
            {
                Open = PftTokenKind.With,
                Close = PftTokenKind.End
            },
            new TokenPair
            {
                Open = PftTokenKind.While,
                Close = PftTokenKind.End
            },
            new TokenPair
            {
                Open = PftTokenKind.LeftCurly,
                Close = PftTokenKind.RightCurly
            }
        };

        private static PftTokenKind[] _loopClose =
        {
            PftTokenKind.End, PftTokenKind.RightCurly
        };

        private static PftTokenKind[] _loopOpen =
        {
            PftTokenKind.For, PftTokenKind.ForEach,
            PftTokenKind.Local, PftTokenKind.While,
            PftTokenKind.With, PftTokenKind.LeftCurly
        };

        private static PftTokenKind[] _loopStop =
        {
            PftTokenKind.End
        };

        private static PftTokenKind[] _orderStop =
        {
            PftTokenKind.Order, PftTokenKind.End
        };

        private static TokenPair[] _parenthesisPairs =
        {
            new TokenPair
            {
                Open = PftTokenKind.LeftParenthesis,
                Close = PftTokenKind.RightParenthesis
            },
            new TokenPair
            {
                Open = PftTokenKind.LeftCurly,
                Close = PftTokenKind.RightCurly
            },
        };

        private static PftTokenKind[] _parenthesisClose =
        {
            PftTokenKind.RightParenthesis, PftTokenKind.RightCurly
        };

        private static PftTokenKind[] _parenthesisOpen =
        {
            PftTokenKind.LeftParenthesis, PftTokenKind.LeftCurly
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

        private static PftTokenKind[] _squareClose =
        {
            PftTokenKind.RightSquare
        };

        private static PftTokenKind[] _squareOpen =
        {
            PftTokenKind.LeftSquare
        };

        private static PftTokenKind[] _squareStop =
        {
            PftTokenKind.RightSquare
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
