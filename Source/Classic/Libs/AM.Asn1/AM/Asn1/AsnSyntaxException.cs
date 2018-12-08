// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsnSyntaxException.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Asn1
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public class AsnSyntaxException
        : AsnException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                [NotNull] AsnToken token
            )
            : this("Unexpected token: " + token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                [NotNull] AsnTokenList tokenList
            )
            : this
                (
                  "Unexpected end of file:"
                  + tokenList.ShowLastTokens(3)
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                [NotNull] AsnTokenList tokenList,
                Exception innerException
            )
            : this
                (
                    "Unexpected end of file: "
                        + tokenList.ShowLastTokens(3),
                    innerException
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                string message,
                Exception innerException
            )
            : base
                (
                    message,
                    innerException
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                [NotNull] AsnToken token,
                Exception innerException
            )
            : this
                (
                    "Unexpected token: " + token,
                    innerException
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                [NotNull] TextNavigator navigator
            )
            : this("Syntax error at: " + navigator)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                [NotNull] AsnNode node
            )
            : this("Syntax error at: " + node)
        {
        }

        #endregion
    }
}
