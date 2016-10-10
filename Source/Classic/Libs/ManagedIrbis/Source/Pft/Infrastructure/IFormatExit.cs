/* IFormatExit.cs --
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
    /// General format exit 
    /// </summary>
    [PublicAPI]
    public interface IFormatExit
    {
        /// <summary>
        /// Name of the format exit.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Execute the expression on given context.
        /// </summary>
        void Execute
            (
                [NotNull] PftContext context,
                [NotNull] PftNode node,
                [CanBeNull] string expression
            );
    }
}
