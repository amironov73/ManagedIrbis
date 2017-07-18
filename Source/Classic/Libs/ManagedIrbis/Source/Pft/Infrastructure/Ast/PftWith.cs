// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftWith.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// <code>
    /// with $x in v692, v910
    /// do
    ///     $x = '^zNewSubField', $x;
    /// end
    /// </code>
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftWith
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Variable reference.
        /// </summary>
        [CanBeNull]
        public PftVariableReference Variable { get; set; }

        /// <summary>
        /// Fields.
        /// </summary>
        [NotNull]
        public NonNullCollection<FieldSpecification> Fields { get; private set; }

        /// <summary>
        /// Body.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Body { get; private set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Variable, null))
                    {
                        nodes.Add(Variable);
                    }
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here

                Log.Error
                    (
                        "PftWith::Children: "
                        + "set value="
                        + value.NullableToVisibleString()
                    );
            }
        }

        /// <inheritdoc cref="PftNode.ComplexExpression" />
        public override bool ComplexExpression
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftWith()
        {
            Fields = new NonNullCollection<FieldSpecification>();
            Body = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftWith
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.With);

            Fields = new NonNullCollection<FieldSpecification>();
            Body = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftWith result = (PftWith)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(Variable, null))
            {
                result.Variable = (PftVariableReference)Variable.Clone();
            }

            result.Fields = new NonNullCollection<FieldSpecification>();
            foreach (FieldSpecification field in Fields)
            {
                result.Fields.Add
                    (
                        (FieldSpecification)field.Clone()
                    );
            }
            foreach (PftNode node in Body)
            {
                result.Body.Add((PftNode)node.Clone());
            }

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode" />
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            PftWith otherWith = (PftWith) otherNode;
            PftSerializationUtility.CompareNodes
                (
                    Variable,
                    otherWith.Variable
                );
            PftSerializationUtility.CompareLists
                (
                    Fields,
                    otherWith.Fields
                );
            PftSerializationUtility.CompareLists
                (
                    Body,
                    otherWith.Body
                );
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Variable = (PftVariableReference) PftSerializer
                .DeserializeNullable(reader);
            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                FieldSpecification field = new FieldSpecification();
                field.Deserialize(reader);
                Fields.Add(field);
            }
            PftSerializer.Deserialize(reader, Body);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (ReferenceEquals(Variable, null))
            {
                Log.Error
                    (
                        "PftWith::Execute: "
                        + "variable not specified"
                    );

                throw new PftException("Variable");
            }

            string name = Variable.Name
                .ThrowIfNull("Variable.Name");

            using (PftContextGuard guard = new PftContextGuard(context))
            {
                PftContext localContext = guard.ChildContext;
                localContext.Output = context.Output;

                PftVariableManager localManager
                    = new PftVariableManager(context.Variables);

                localContext.SetVariables(localManager);

                PftVariable variable = new PftVariable
                    (
                        name,
                        false
                    );

                localManager.Registry.Add
                    (
                        name,
                        variable
                    );

                foreach (FieldSpecification field in Fields)
                {
                    string tag = field.Tag.ThrowIfNull("field.Tag");
                    string[] lines = field.SubField == SubField.NoCode
                        ? PftUtility.GetFieldValue
                            (
                                context,
                                tag,
                                field.FieldRepeat
                            )
                        : PftUtility.GetSubFieldValue
                            (
                                context,
                                tag,
                                field.FieldRepeat,
                                field.SubField,
                                field.SubFieldRepeat
                            );

                    List<string> lines2 = new List<string>();
                    foreach (string line in lines)
                    {
                        variable.StringValue = line;

                        localContext.Execute(Body);

                        string value = variable.StringValue;
                        if (!string.IsNullOrEmpty(value))
                        {
                            lines2.Add(value);
                        }
                    }

                    bool flag = lines2.Count != lines.Length;
                    if (!flag)
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i] != lines2[i])
                            {
                                flag = true;
                                break;
                            }
                        }
                    }

                    if (flag)
                    {
                        string value = string.Join
                            (
                                Environment.NewLine,
                                lines2.ToArray()
                            );

                        if (field.SubField == SubField.NoCode)
                        {
                            PftUtility.AssignField
                                (
                                    context,
                                    tag,
                                    field.FieldRepeat,
                                    value
                                );
                        }
                        else
                        {
                            PftUtility.AssignSubField
                                (
                                    context,
                                    tag,
                                    field.FieldRepeat,
                                    field.SubField,
                                    field.SubFieldRepeat,
                                    value
                                );
                        }
                    }
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
                Name = "With"
            };

            PftNodeInfo fields = new PftNodeInfo
            {
                Name = "Fields",
                Value = StringUtility.Join
                    (
                        ", ",
                        Fields
                    )
            };
            result.Children.Add(fields);

            PftNodeInfo body = new PftNodeInfo
            {
                Name = "Body"
            };
            result.Children.Add(body);
            foreach (PftNode node in Body)
            {
                body.Children.Add(node.GetNodeInfo());
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer.EatNewLine();
            printer.WriteLine();
            printer
                .WriteIndent()
                .Write("with ");
            if (!ReferenceEquals(Variable, null))
            {
                Variable.PrettyPrint(printer);
            }
            printer.SingleSpace();
            printer.Write("in ");
            bool first = true;
            foreach (FieldSpecification field in Fields)
            {
                if (!first)
                {
                    printer.Write(", ");
                }
                printer.Write(field.ToString());
                first = false;
            }
            printer.WriteLine()
                .WriteIndent()
                .WriteLine("do")
                .IncreaseLevel()
                .WriteIndent()
                .WriteNodes(Body);
            printer.EatWhitespace();
            printer.EatNewLine();
            printer
                .DecreaseLevel()
                .WriteLine()
                .WriteIndent()
                .Write("end");
            if (!ReferenceEquals(Variable, null))
            {
                printer.Write(" /* with ");
                Variable.PrettyPrint(printer);
            }
            printer.WriteLine();
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, Variable);
            writer.WritePackedInt32(Fields.Count);
            foreach (FieldSpecification field in Fields)
            {
                field.Serialize(writer);
            }
            PftSerializer.Serialize(writer, Body);
        }

        #endregion
    }
}
