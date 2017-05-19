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

        #endregion

        #region Construction

        #endregion

        #region Private members

        private bool _traced;

        #endregion

        #region Public methods

        /// <inheritdoc />
        public override string[] GetAffectedFields()
        {
            return new string[0];
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute"/>
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!_traced)
            {
                Log.Trace("PftOrphan::Execute");
                _traced = true;
            }

            OnAfterExecution(context);
        }

        #endregion
    }
}
