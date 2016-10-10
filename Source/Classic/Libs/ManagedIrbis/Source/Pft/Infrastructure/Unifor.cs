/* Unifor.cs --
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
    /// Unifor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Unifor
        : IFormatExit
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static Dictionary<string, Action<PftContext, string>> Registry { get; private set; }

        #endregion

        #region Construction

        static Unifor()
        {
            Registry = new Dictionary<string, Action<PftContext, string>>
                (
                    StringComparer.InvariantCultureIgnoreCase
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IFormatExit members

        /// <inheritdoc/>
        public string Name { get { return "unifor"; } }

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
