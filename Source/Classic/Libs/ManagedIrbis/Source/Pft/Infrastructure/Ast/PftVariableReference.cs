/* PftVariableReference.cs --
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
using AM.Text;
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
    public sealed class PftVariableReference
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// Name of the variable.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Index.
        /// </summary>
        public IndexSpecification Index { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference
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
        public PftVariableReference
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Variable);

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
            PftVariable variable
                = context.Variables.GetExistingVariable(name);
            if (ReferenceEquals(variable, null))
            {
                throw new PftSemanticException
                    (
                        "unknown variable: " + name
                    );
            }
            if (variable.IsNumeric)
            {
                Value = variable.NumericValue;
                context.Write(this, variable.NumericValue.ToString());
            }
            else
            {
                string output = variable.StringValue;

                if (Index.Kind != IndexKind.None)
                {
                    string[] lines = output.SplitLines();

                    lines = PftUtility.GetArrayItem
                        (
                            context,
                            lines,
                            Index
                        );

                    output = string.Join
                        (
                            Environment.NewLine,
                            lines
                        );
                }

                context.Write(this, output);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = base.GetNodeInfo();

            if (Index.Kind != IndexKind.None)
            {
                result.Children.Add(Index.GetNodeInfo());
            }

            return result;
        }

        #endregion
    }
}
