// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ValuePair.cs -- holds pair of objects of given types
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Simple container that holds pair of objects of given types.
    /// </summary>
    [PublicAPI]
    public struct ValuePair<T1, T2>
    {
        #region Properties

        /// <summary>
        /// First item.
        /// </summary>
        public T1 First;

        /// <summary>
        /// Second item.
        /// </summary>
        public T2 Second;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ValuePair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        #endregion
    }
}
