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
    public sealed class PftAssignment
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Whether is numeric or text assignment.
        /// </summary>
        public bool IsNumeric { get; set; }

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

        // Handle direct variable-to-variable assignment
        // Ignore IsNumeric setting
        private bool HandleDirectAssignment
            (
                PftContext context
            )
        {
            string name = Name.ThrowIfNull("name");

            if (Children.Count == 1)
            {
                PftVariableReference reference = Children.First()
                    as PftVariableReference;
                if (!ReferenceEquals(reference, null))
                {
                    PftVariable donor;
                    string donorName = reference.Name
                        .ThrowIfNull("reference.Name");
                    if (context.Variables.Registry.TryGetValue
                        (
                            donorName,
                            out donor
                        ))
                    {
                        if (donor.IsNumeric)
                        {
                            context.Variables.SetVariable
                                (
                                    name,
                                    donor.NumericValue
                                );
                        }
                        else
                        {
                            context.Variables.SetVariable
                                (
                                    name,
                                    donor.StringValue
                                );
                        }

                        return true;
                    }
                }
            }

            return false;
        }

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

            if (!HandleDirectAssignment(context))
            {
                string name = Name.ThrowIfNull("name");
                string stringValue = context.Evaluate(Children);

                if (IsNumeric)
                {
                    PftNumeric numeric =
                        (
                            Children.FirstOrDefault() as PftNumeric
                        )
                        .ThrowIfNull("numeric");
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
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = SimplifyTypeName(GetType().Name)
            };

            PftNodeInfo name = new PftNodeInfo
            {
                Name = "Name",
                Value = Name
            };
            result.Children.Add(name);

            PftNodeInfo numeric = new PftNodeInfo
            {
                Name = "IsNumeric",
                Value = IsNumeric.ToString()
            };
            result.Children.Add(numeric);

            foreach (PftNode node in Children)
            {
                result.Children.Add(node.GetNodeInfo());
            }

            return result;
        }

        #endregion
    }
}
