/* PftNumeric.cs --
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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class PftNumeric
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Value.
        /// </summary>
        public double Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftNumeric()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftNumeric
            (
                double value
            )
        {
            Value = value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftNumeric
            (
                [NotNull] PftToken token
            )
            : base(token)
        {

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

            base.Execute(context);

            OnAfterExecution(context);
        }

        #endregion
    }
}
