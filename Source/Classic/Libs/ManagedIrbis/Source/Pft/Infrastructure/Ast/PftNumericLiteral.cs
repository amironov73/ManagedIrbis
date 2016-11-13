/* PftNumericExpression.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftNumericLiteral
        : PftNumeric
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNumericLiteral()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNumericLiteral
            (
                double value
            )
            : base(value)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNumericLiteral
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Value = double.Parse
                (
                    token.Text.ThrowIfNull("token.Text"),
                    CultureInfo.InvariantCulture
                );
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

            OnAfterExecution(context);
        }

        #endregion
    }
}
