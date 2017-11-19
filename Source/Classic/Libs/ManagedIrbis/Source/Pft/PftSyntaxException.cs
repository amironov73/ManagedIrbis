// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftSyntaxException.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Исключение, возникающее при разборе PFT-скрипта.
    /// </summary>
    public sealed class PftSyntaxException
        : PftException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                [NotNull] PftToken token
            )
            : this("Unexpected token: " + token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                [NotNull] PftTokenList tokenList
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
        public PftSyntaxException
            (
                [NotNull] PftTokenList tokenList,
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
        public PftSyntaxException
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
        public PftSyntaxException
            (
                [NotNull] PftToken token,
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
        public PftSyntaxException
            (
                [NotNull] TextNavigator navigator
            )
            : this("Syntax error at: " + navigator)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                [NotNull] PftNode node
            )
            : this("Syntax error at: " + node)
        {
        }

        #endregion
    }
}
