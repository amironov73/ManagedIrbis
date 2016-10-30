/* UniversalComparer.cs -- универсальный компаратор
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Универсальный компаратор.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class UniversalComparer<T>
        : IComparer<T>
    {
        #region Properties

        /// <summary>
        /// Используемый для сравнения делегат.
        /// </summary>
        [NotNull]
        public Func<T, T, int> Function { get { return _function; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UniversalComparer
            (
                [NotNull] Func<T, T, int> function
            )
        {
            Code.NotNull(function, "function");

            _function = function;
        }

        #endregion

        #region Private members

        private Func<T, T, int> _function;

        #endregion

        #region IComparer<T> members

        /// <summary>
        /// Compares the specified values.
        /// </summary>
        public int Compare
            (
                T left,
                T right
            )
        {
            return _function(left, right);
        }

        #endregion
    }
}
