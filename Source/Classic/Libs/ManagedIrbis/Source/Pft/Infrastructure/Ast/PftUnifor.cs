// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
using AM;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
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
            : base(token)
        {
            Code.NotNull(token, "token");

            Name = token.Text;
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
            
            string expression = subContext.Text;

            FormatExit.Execute
                (
                    context,
                    this,
                    Name.ThrowIfNull("Name"),
                    expression
                );
        }

        #endregion
    }
}
