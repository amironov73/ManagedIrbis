/* PftUnifor.cs --
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

using ManagedIrbis.Pft.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Source.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Unifor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftUnifor
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnifor()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnifor
            (
                [NotNull] PftToken token
            )
        {
        }

        #endregion

        #region PftNode members

        /// <inheritdoc/>
        public override void Execute
            (
                PftContext context
            )
        {
            PftContext subContext = context.Push();

            foreach (PftNode node in Children)
            {
                node.Execute(subContext);
            }
            
            string argument = subContext.Text;

            context.Write(this, "unifor: '");
            context.Write(this, argument);
            context.Write(this, "'");

            //context.Write(this, "unifor");
            //foreach (PftNode node in Children)
            //{
            //    context.Write(this, " {0}", node.GetType().Name);
            //}
        }

        #endregion
    }
}
