// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FunctionalUtility.cs --
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FunctionalUtility
    {
        #region Public methods

        /// <summary>
        /// Ternary operator.
        /// </summary>
        [CanBeNull]
        public static TResult Either<TInput, TResult>
            (
                [CanBeNull] this TInput input,
                [NotNull] Func<TInput, bool> condition,
                [NotNull] Func<TInput, TResult> trueAction,
                [NotNull] Func<TInput, TResult> falseAction
            )
        {
            Code.NotNull(condition, "condition");
            Code.NotNull(trueAction, "trueAction");
            Code.NotNull(falseAction, "falseAction");

            TResult result = condition(input)
                ? trueAction(input)
                : falseAction(input);

            return result;
        }

        /// <summary>
        /// Pipe the data to the function.
        /// </summary>
        [CanBeNull]
        public static TResult PipeTo<TInput, TResult>
            (
                [CanBeNull] this TInput input,
                [NotNull] Func<TInput, TResult> func
            )
        {
            Code.NotNull(func, "func");

            TResult result = func(input);

            return result;
        }

        #endregion
    }
}
