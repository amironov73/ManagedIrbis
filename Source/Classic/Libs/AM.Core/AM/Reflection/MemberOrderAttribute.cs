// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MemberOrderAttribute.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Reflection
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public sealed class MemberOrderAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Index.
        /// </summary>
        public int Index { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MemberOrderAttribute
            (
                int index
            )
        {
            Index = index;
        }

        #endregion
    }
}
