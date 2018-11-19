// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ValueTriplet.cs -- holds triplet of objects of given types
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
    /// Simple container that holds triplet of objects of given types.
    /// </summary>
    [PublicAPI]
    public struct ValueTriplet<T1, T2, T3>
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

        /// <summary>
        /// Third item.
        /// </summary>
        public T3 Third;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ValueTriplet(T1 first, T2 second, T3 third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        #endregion
    }
}
