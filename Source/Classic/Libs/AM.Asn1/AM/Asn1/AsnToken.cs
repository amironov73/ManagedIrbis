// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsnToken.cs --
 *  Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Logging;

using JetBrains.Annotations;

#endregion

namespace AM.Asn1
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public sealed class AsnToken
    {
        #region Properties

        /// <summary>
        /// Column.
        /// </summary>
        public int Column;

        /// <summary>
        /// Token kind.
        /// </summary>
        public AsnTokenKind Kind;

        /// <summary>
        /// Line number.
        /// </summary>
        public int Line;

        /// <summary>
        /// Token text.
        /// </summary>
        public string Text;

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        public object UserData;

        #endregion

        #region Public methods

        /// <summary>
        /// Requires specified kind of token.
        /// </summary>
        [NotNull]
        public AsnToken MustBe
            (
                AsnTokenKind kind
            )
        {
            if (Kind != kind)
            {
                Log.Error
                (
                    "PftToken::MustBe: "
                    + "expecting="
                    + kind
                    + ", got="
                    + Kind
                );

                throw new AsnSyntaxException();
            }

            return this;
        }

        #endregion
    }
}
