// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FuncUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Logging;

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
#if WINMOBILE || POCKETPC || SILVERLIGHT

            throw new NotImplementedException();

#else

            var dictionary = new System.Collections.Concurrent.ConcurrentDictionary<TArg, TRes>();

            return arg => dictionary.GetOrAdd(arg, func);

#endif
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
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "FuncUtility::RetryOnFault",
                            exception
                        );

                    if (i == maxTries - 1)
                    {
                        Log.Error
                            (
                                "FuncUtility::RetryOnFault: "
                                + "giving up"
                            );

                        throw;
                    }
                }
            }

            return default(T);
        }

        #endregion
    }
}

