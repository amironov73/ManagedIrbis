// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftN.cs -- fake field reference
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Fake field reference.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftOrphan
        : PftField
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ConstantExpression" />
        public override bool ConstantExpression
        {
            get { return true; }
        }

        /// <inheritdoc cref="PftNode.RequiresConnection" />
        public override bool RequiresConnection
        {
            get { return false; }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private bool _traced;

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftField.Compile" />
        public override void Compile(PftCompiler compiler)
        {
            compiler.StartMethod(this);

            // Nothing to do here

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftField.GetAffectedFields" />
        public override int[] GetAffectedFields()
        {
            return new int[0];
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            // Nothing to do here

            if (!_traced)
            {
                Log.Trace("PftOrphan::Execute");
                _traced = true;
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            // Take the node away from the AST

            return null;
        }

        #endregion
    }
}
