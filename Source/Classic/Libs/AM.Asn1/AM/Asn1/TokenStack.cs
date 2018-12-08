// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TokenStack.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

#endregion

namespace AM.Asn1
{
    sealed class TokenStack
        : Stack<AsnTokenKind>
    {
        #region Properties

        [NotNull]
        public AsnTokenList Tokens { get; private set; }

        [NotNull]
        public TokenPair[] Pairs { get; private set; }

        #endregion

        #region Construction

        public TokenStack
        (
            [NotNull] AsnTokenList tokens,
            [NotNull] TokenPair[] pairs
        )
        {
            Tokens = tokens;
            Pairs = pairs;
        }

        #endregion

        #region Public methods

        public void Pop(AsnTokenKind current)
        {
            AsnTokenKind open = Pop();
            AsnTokenKind expected = Pairs.First(p => p.Open == open).Close;

            if (expected != current)
            {
                throw new AsnSyntaxException(Tokens);
            }
        }

        public void Verify()
        {
            if (Count != 0)
            {
                throw new AsnSyntaxException(Tokens);
            }
        }

        #endregion
    }
}
