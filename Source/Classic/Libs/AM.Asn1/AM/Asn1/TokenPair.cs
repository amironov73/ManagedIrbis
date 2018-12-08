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

namespace AM.Asn1
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
        public AsnTokenKind Open;

        /// <summary>
        /// Close token.
        /// </summary>
        public AsnTokenKind Close;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TokenPair
            (
                AsnTokenKind open,
                AsnTokenKind close
            )
        {
            Open = open;
            Close = close;
        }

        #endregion
    }
}
