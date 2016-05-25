/* FuncUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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
