/* FuncUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Runtime;

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
    public static class FuncUtility
    {
        #region Public methods

        /// <summary>
        /// Memoizes the specified function.
        /// </summary>
        public static Func<TArg, TRes> Memoize<TArg, TRes>
            (
                [NotNull] this Func<TArg, TRes> func
            )
        {
            var dictionary = new ConcurrentDictionary<TArg, TRes>();

            return arg => dictionary.GetOrAdd(arg, func);
        }

        /// <summary>
        /// Borrowed from Stephen Toub book.
        /// </summary>
        public static T RetryOnFault<T>
            (
                Func<T> function,
                int maxTries
            )
        {
            Code.NotNull(function, "function");
            Code.Positive(maxTries, "maxTries");

            for (int i = 0; i < maxTries; i++)
            {
                try
                {
                    return function();
                }
                catch
                {
                    if (i == (maxTries - 1))
                    {
                        throw;
                    }
                }
            }

            return default(T);
        }

        #endregion
    }
}

#endif

