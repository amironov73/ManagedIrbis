// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftNumeric.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
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
