/* Umarci.cs --
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
    /// Umarci.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Umarci
        : IFormatExit
    {
        #region Properties

        #endregion

        #region Construction

        static Umarci()
        {
            
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IFormatExit members

        /// <inheritdoc/>
        public string Name { get { return "umarci"; } }

        /// <inheritdoc/>
        public void Execute
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(node, "node");

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            context.Write
                (
                    node,
                    expression
                );
        }

        #endregion
    }
}
