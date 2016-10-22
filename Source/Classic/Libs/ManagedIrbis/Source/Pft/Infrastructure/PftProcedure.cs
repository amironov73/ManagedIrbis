/* PftProcedure.cs --
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

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Procedure.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftProcedure
    {
        #region Properties

        /// <summary>
        /// Procedure body.
        /// </summary>
        [CanBeNull]
        public PftNode Body { get; set; }

        /// <summary>
        /// Procedure name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the procedure.
        /// </summary>
        public void Execute
            (
                [NotNull] PftContext context,
                [CanBeNull] string argument
            )
        {
        }

        #endregion
    }
}
