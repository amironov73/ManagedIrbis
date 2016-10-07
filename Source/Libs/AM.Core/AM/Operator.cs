/* Operator.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC

#region Using directives

using System;
using System.Linq.Expressions;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Experimental operator helpers.
    /// </summary>
    [PublicAPI]
    public class Operator<T>
    {
        #region Public methods

        /// <summary>
        /// Operator "new".
        /// </summary>
        // ReSharper disable once ConvertToAutoProperty
        public static Func<T> New { get { return _new; } }

        #endregion

        #region Private members

        // ReSharper disable once InconsistentNaming
        private static readonly Func<T> _new = Expression
            .Lambda<Func<T>>
            (
                Expression.New(typeof(T))
            )
            .Compile();

        #endregion
    }
}

#endif

