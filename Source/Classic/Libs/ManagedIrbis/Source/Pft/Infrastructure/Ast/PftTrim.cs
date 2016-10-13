/* PftTrim.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Trim the string
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftTrim
        : PftNode
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTrim()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PftTrim
            (
                [NotNull] PftToken token
            )
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Trim);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string expression = context.Evaluate(Children);
            if (!string.IsNullOrEmpty(expression))
            {
                string result = expression.Trim();
                if (!string.IsNullOrEmpty(result))
                {
                    context.Write(this, result);
                }
            }

            OnAfterExecution(context);
        }

        #endregion
    }
}
