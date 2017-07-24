// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftAssignment.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;

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
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Index.
        /// </summary>
        public IndexSpecification Index { get; set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

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
                [NotNull] PftContext context
            )
        {
            //
            // TODO handle indexing
            //

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

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftAssignment result = (PftAssignment)base.Clone();
            result.IsNumeric = IsNumeric;
            result.Index = (IndexSpecification)Index.Clone();

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode"/>
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            if (Name != ((PftAssignment) otherNode).Name)
            {
                throw new IrbisException();
            }
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            IsNumeric = reader.ReadBoolean();
            Name = reader.ReadNullableString();
            Index.Deserialize(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
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
                            context,
                            name,
                            Index,
                            stringValue
                        );
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            PftNodeInfo name = new PftNodeInfo
            {
                Name = "Name",
                Value = Name
            };
            result.Children.Add(name);

            if (Index.Kind != IndexKind.None)
            {
                result.Children.Add(Index.GetNodeInfo());
            }

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

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write('$')
                .Write(Name)
                .Write('=');
            base.PrettyPrint(printer);
            printer.Write(';');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.Write(IsNumeric);
            writer.WriteNullable(Name);
            Index.Serialize(writer);
        }

        #endregion
    }
}
