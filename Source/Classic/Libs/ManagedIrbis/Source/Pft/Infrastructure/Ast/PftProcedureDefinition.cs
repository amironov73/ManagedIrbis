/* PftProcedureDefinition.cs --
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
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftProcedureDefinition
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Procedure itself.
        /// </summary>
        [CanBeNull]
        public PftProcedure Procedure { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedureDefinition()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedureDefinition
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

            OnAfterExecution(context);
        }

        /// <inheritdoc />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = "Procedure"
            };

            if (!ReferenceEquals(Procedure, null))
            {
                result.Value = Procedure.Name;

                PftNodeInfo body = new PftNodeInfo
                {
                    Name = "Body"
                };
                result.Children.Add(body);

                foreach (PftNode node in Procedure.Body)
                {
                    body.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        }

        #endregion
    }
}
