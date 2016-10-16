/* Dispatch.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.Collections;
using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Pft.Infrastructure.Ast;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    static class Dispatch
    {
        #region Properties

        /// <summary>
        /// Field reference context.
        /// </summary>
        [NotNull]
        public static Dictionary<PftTokenKind, TokenDispatcher> FieldMap { get; private set; }

        /// <summary>
        /// Main script context.
        /// </summary>
        [NotNull]
        public static Dictionary<PftTokenKind, TokenDispatcher> MainMap { get; private set; }

        public static PftTokenKind[] MainTokens =
        {
            PftTokenKind.Break, PftTokenKind.C, PftTokenKind.Comma, 
            PftTokenKind.Comment, 
        };

        /// <summary>
        /// Numeric expression context.
        /// </summary>
        [NotNull]
        public static Dictionary<PftTokenKind, TokenDispatcher> NumericMap { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Dispatch()
        {
            MainMap = new CloneableDictionary<PftTokenKind, TokenDispatcher>();
            FieldMap = new CloneableDictionary<PftTokenKind, TokenDispatcher>();
            NumericMap = new CloneableDictionary<PftTokenKind, TokenDispatcher>();
        }

        #endregion

        #region Private members

        private static PftNode ParseBreak(PftTokenList tokenList)
        {
            PftNode result = new PftBreak(tokenList.Current);
            tokenList.MoveNext();
            return result;
        }


        private static PftNode ParseC(PftTokenList tokenList)
        {
            PftNode result = new PftC(tokenList.Current);
            tokenList.MoveNext();
            return result;
        }
        private static PftNode ParseComma(PftTokenList tokenList)
        {
            PftNode result = new PftComma(tokenList.Current);
            tokenList.MoveNext();
            return result;
        }

        private static PftNode ParseComment(PftTokenList tokenList)
        {
            PftNode result = new PftComment(tokenList.Current);
            tokenList.MoveNext();
            return result;
        }

        private static PftNode ParseConditionalLiteral(PftTokenList tokenList)
        {
            PftNode result = new PftConditionalLiteral(tokenList.Current);
            tokenList.MoveNext();
            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create next AST node from token list.
        /// </summary>
        [CanBeNull]
        public static PftNode Get
            (
                [NotNull] PftTokenList tokenList,
                [NotNull] Dictionary<PftTokenKind, TokenDispatcher> map,
                [NotNull] PftTokenKind[] expectedTokens
            )
        {
            PftNode result = null;
            PftToken token = tokenList.Current;

            if (Array.IndexOf(expectedTokens, token.Kind) >= 0)
            {
                TokenDispatcher function;
                if (!map.TryGetValue(token.Kind, out function))
                {
                    throw new PftException
                        (
                            "don't know how to handle token "
                            + token.Kind
                        );
                }
                result = function(tokenList);
            }

            return result;

        }

        #endregion
    }
}
