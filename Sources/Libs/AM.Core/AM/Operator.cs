/* Operator.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

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
        public static Func<T> New { get { return _new; } }

        #endregion

        #region Private members

        static readonly Func<T> _new = Expression
            .Lambda<Func<T>>
            (
                Expression.New(typeof(T))
            )
            .Compile();

        #endregion
    }
}
