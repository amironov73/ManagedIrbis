// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TokenPair.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Pair of tokens.
    /// </summary>
    struct TokenPair
    {
        #region Properties

        /// <summary>
        /// Open token.
        /// </summary>
        public PftTokenKind Open;

        /// <summary>
        /// Close token.
        /// </summary>
        public PftTokenKind Close;

        #endregion
    }
}
