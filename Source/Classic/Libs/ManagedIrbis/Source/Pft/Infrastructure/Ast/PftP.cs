// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftP.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
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
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftP
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        [CanBeNull]
        public PftField Field { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Field, null))
                    {
                        nodes.Add(Field);
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
                        "PftP::Children: "
                        + "set value="
                        + value.NullableToVisibleString()
                    );
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftP()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftP
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        /// <summary>
        /// Have the specified field repeat?
        /// </summary>
        public static bool HaveRepeat
            (
                [NotNull] MarcRecord record,
                [NotNull] string tag,
                char code,
                int index
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");

            RecordField field = record.Fields.GetField
                (
                    tag,
                    index
                );
            if (ReferenceEquals(field, null))
            {
                return false;
            }

            if (code == SubField.NoCode)
            {
                return true;
            }

            bool result = field.HaveSubField(code);

            return result;
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftP result = (PftP)base.Clone();

            if (!ReferenceEquals(Field, null))
            {
                result.Field = (PftField)Field.Clone();
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

            PftSerializationUtility.CompareNodes
                (
                    Field,
                    ((PftA)otherNode).Field
                );
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(Field, null))
            {
                throw new PftCompilerException();
            }

            FieldInfo info = compiler.CompileField(Field);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("MarcRecord record = Context.Record;")
                .WriteIndent()
                .WriteLine("string tag = {0}.Tag;", info.Reference)
                .WriteIndent()
                .WriteLine("int index = Context.Index;")
                .WriteIndent()
                .WriteLine
                    (
                        "bool flag = PftP.HaveRepeat(record, "
                        + "tag, {0}.SubField, index);",
                        info.Reference
                    )
                .WriteIndent()
                .WriteLine("if (PftP.HaveRepeat(record, "
                    + "tag, SubField.NoCode, index))")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("HaveOutput();")
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}")
                .WriteIndent()
                .WriteLine("return flag;");

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Field = (PftField)PftSerializer.DeserializeNullable(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (ReferenceEquals(Field, null))
            {
                Log.Error
                    (
                        "PftP::Execute: "
                        + "Field not specified"
                    );

                throw new PftSyntaxException(this);
            }

            string tag = Field.Tag.ThrowIfNull("Field.Tag");
            MarcRecord record = context.Record;
            int index = context.Index;

            if (!ReferenceEquals(record, null))
            {
                // ИРБИС64 вне группы всегда проверяет
                // на наличие лишь первое повторение поля!

                Value = HaveRepeat
                    (
                        record,
                        tag,
                        Field.SubField,
                        index
                    );

                // Само по себе обращение к P крутит группу
                // при наличии повторения поля

                if (HaveRepeat(record, tag, SubField.NoCode, index))
                {
                    context.OutputFlag = true;
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

            if (!ReferenceEquals(Field, null))
            {
                PftNodeInfo fieldInfo = new PftNodeInfo
                {
                    Node = Field,
                    Name = "Field"
                };
                fieldInfo.Children.Add(Field.GetNodeInfo());
                result.Children.Add(fieldInfo);
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, Field);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("p(");
            if (!ReferenceEquals(Field, null))
            {
                Field.PrettyPrint(printer);
            }
            printer.Write(')');
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return "p(" + Field + ")";
        }

        #endregion
    }
}

