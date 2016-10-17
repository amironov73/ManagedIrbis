/* PftAssignment.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftAssignment
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Variable name.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftAssignment()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftAssignment
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftAssignment
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Name = token.Text;
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

            string name = Name.ThrowIfNull("name");
            string stringValue = context.Evaluate(Children);
            if (Children.Count != 0
                && Children[0] is PftNumeric)
            {
                PftNumeric numeric = Children[0] as PftNumeric;
                double numericValue = numeric.Value;
                context.Variables.SetVariable
                    (
                        name,
                        numericValue
                    );
            }
            else
            {
                context.Variables.SetVariable
                    (
                        name,
                        stringValue
                    );
            }

            OnAfterExecution(context);
        }

        #endregion
    }
}
