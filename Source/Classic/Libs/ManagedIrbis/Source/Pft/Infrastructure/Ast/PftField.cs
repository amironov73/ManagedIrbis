// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftField.cs -- base for field reference
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM;
using AM.IO;
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
    /// Base for field reference.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftField
        : PftNode
    {
        #region Constants

        /// <summary>
        /// No subfield specified.
        /// </summary>
        public const char NoSubField = '\0';

        #endregion

        #region Properties

        /// <summary>
        /// Left hand.
        /// </summary>
        [NotNull]
        public PftNodeCollection LeftHand { get; private set; }

        /// <summary>
        /// Right hand.
        /// </summary>
        [NotNull]
        public PftNodeCollection RightHand { get; private set; }

        /// <summary>
        /// Command.
        /// </summary>
        public char Command { get; set; }

        /// <summary>
        /// Embedded.
        /// </summary>
        [CanBeNull]
        public string Embedded { get; set; }

        /// <summary>
        /// Отступ.
        /// </summary>
        public int Indent { get; set; }

        /// <summary>
        /// Смещение.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Длина.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Subfield.
        /// </summary>
        public char SubField { get; set; }

        /// <summary>
        /// Tag.
        /// </summary>
        [CanBeNull]
        public string Tag { get; set; }

        /// <summary>
        /// Tag specification.
        /// </summary>
        [CanBeNull]
        public string TagSpecification { get; set; }

        /// <summary>
        /// Repeat count.
        /// </summary>
        public int RepeatCount { get; set; }

        /// <summary>
        /// Field repeat specification.
        /// </summary>
        public IndexSpecification FieldRepeat { get; set; }

        /// <summary>
        /// Subfield repeat specification.
        /// </summary>
        public IndexSpecification SubFieldRepeat { get; set; }

        /// <summary>
        /// Subfield specification.
        /// </summary>
        [CanBeNull]
        public string SubFieldSpecification { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    nodes.AddRange(LeftHand);
                    nodes.AddRange(RightHand);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here

                Log.Error
                    (
                        "PftField::Children: "
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
        public PftField()
        {
            LeftHand = new PftNodeCollection(this);
            RightHand = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftField
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");

            LeftHand = new PftNodeCollection(this);
            RightHand = new PftNodeCollection(this);
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        private PftNumeric _tagProgram;

        private PftNode _subFieldProgram;

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the specification.
        /// </summary>
        public void Apply
            (
                [NotNull] FieldSpecification specification
            )
        {
            Code.NotNull(specification, "specification");

            Command = specification.Command;
            Embedded = specification.Embedded;
            Indent = specification.ParagraphIndent;
            Offset = specification.Offset;
            Length = specification.Length;
            SubField = specification.SubField;
            Tag = specification.Tag.ToInvariantString();
            TagSpecification = specification.TagSpecification;
            FieldRepeat = specification.FieldRepeat;
            SubFieldRepeat = specification.SubFieldRepeat;
            SubFieldSpecification = specification.SubFieldSpecification;

            Text = specification.ToString();
        }

        /// <summary>
        /// Apply the reference.
        /// </summary>
        public void Apply
            (
                [NotNull] FieldReference reference
            )
        {
            Code.NotNull(reference, "reference");

            Command = reference.Command;
            Embedded = reference.Embedded;
            Indent = reference.Indent;
            Offset = reference.Offset;
            Length = reference.Length;
            SubField = reference.SubField;
            Tag = reference.Tag.ToInvariantString();
            TagSpecification = reference.TagSpecification;
            FieldRepeat = reference.FieldRepeat;
            SubFieldRepeat = reference.SubFieldRepeat;
            SubFieldSpecification = reference.SubFieldSpecification;

            Text = reference.ToString();
        }

        /// <summary>
        /// Can output according given value.
        /// </summary>
        public virtual bool CanOutput
            (
                [CanBeNull] string value
            )
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Evaluate tag specification (if any).
        /// </summary>
        [CanBeNull]
        public string EvaluateTagSpecification
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            string tagSpecification = TagSpecification;
            if (!string.IsNullOrEmpty(tagSpecification))
            {
                if (ReferenceEquals(_tagProgram, null))
                {
                    PftLexer lexer = new PftLexer();
                    PftTokenList tokens = lexer.Tokenize(tagSpecification);
                    PftParser parser = new PftParser(tokens);
                    _tagProgram = parser.ParseArithmetic();
                }

                string textValue = context.Evaluate(_tagProgram);
                int integerValue = (int)_tagProgram.Value;
                Tag = integerValue != 0
                    ? integerValue.ToInvariantString()
                    : textValue;
            }

            string subFieldSpecification = SubFieldSpecification;
            if (!string.IsNullOrEmpty(subFieldSpecification))
            {
                if (ReferenceEquals(_subFieldProgram, null))
                {
                    PftLexer lexer = new PftLexer();
                    PftTokenList tokens = lexer.Tokenize(subFieldSpecification);
                    PftParser parser = new PftParser(tokens);
                    _subFieldProgram = parser.Parse();
                }

                string value = context.Evaluate(_subFieldProgram);
                if (!string.IsNullOrEmpty(value))
                {
                    SubField = value[0];
                }
            }

            return Tag;
        }

        /// <inheritdoc cref="PftNode.GetAffectedFields" />
        public override int[] GetAffectedFields()
        {
            return new[] { Tag.SafeToInt32() };
        }

        /// <summary>
        /// Get value.
        /// </summary>
        [CanBeNull]
        public virtual string GetValue
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            MarcRecord record = context.Record;
            if (record == null
                || string.IsNullOrEmpty(Tag)
                && string.IsNullOrEmpty(TagSpecification)
               )
            {
                return null;
            }

            EvaluateTagSpecification(context);

            int index = context.Index;

            RecordField[] fields = PftUtility.GetArrayItem
                (
                    context,
                    record.Fields.GetField(Tag.SafeToInt32()),
                    FieldRepeat
                );

            RecordField field = fields.GetOccurrence(index);
            if (field == null)
            {
                return null;
            }

            string result = PftUtility.GetFieldValue
                (
                    context,
                    field,
                    SubField,
                    SubFieldRepeat
                );

            result = LimitText(result);

            return result;
        }

        /// <summary>
        /// Have value?
        /// </summary>
        public virtual bool HaveRepeat
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            MarcRecord record = context.Record;
            if (record == null
                || string.IsNullOrEmpty(Tag))
            {
                return false;
            }

            RecordField field = record.Fields.GetField(Tag.SafeToInt32(), context.Index);

            return !ReferenceEquals(field, null);
        }

        /// <summary>
        /// Is first repeat?
        /// </summary>
        public bool IsFirstRepeat
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            return context.Index == 0;
        }

        /// <summary>
        /// Is last repeat?
        /// </summary>
        public virtual bool IsLastRepeat
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            return true;
        }

        /// <summary>
        /// Limit text.
        /// </summary>
        [CanBeNull]
        public string LimitText
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int offset = Offset;
            int length = 100000000;
            if (Length != 0)
            {
                length = Length;
            }

            string result = PftUtility.SafeSubString
                (
                    text,
                    offset,
                    length
                );

            return result;
        }

        /// <summary>
        /// Convert to <see cref="FieldSpecification"/>.
        /// </summary>
        [NotNull]
        public FieldSpecification ToSpecification()
        {
            FieldSpecification result = new FieldSpecification
            {
                Command = Command,
                Embedded = Embedded,
                ParagraphIndent = Indent,
                FieldRepeat = FieldRepeat,
                SubFieldRepeat = SubFieldRepeat,
                Offset = Offset,
                Length = Length,
                SubField = SubField,
                Tag = Tag.SafeToInt32()
            };

            return result;
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="PftNode.Clone" />
        public override object Clone()
        {
            PftField result = (PftField)base.Clone();

            result._virtualChildren = null;

            result.LeftHand = LeftHand.CloneNodes(result).ThrowIfNull();
            result.RightHand = RightHand.CloneNodes(result).ThrowIfNull();

            if (!ReferenceEquals(_tagProgram, null))
            {
                result._tagProgram = (PftNumeric)_tagProgram.Clone();
            }

            if (!ReferenceEquals(_subFieldProgram, null))
            {
                result._subFieldProgram
                    = (PftNode)_subFieldProgram.Clone();
            }

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

            PftField otherField = (PftField)otherNode;
            if (Command != otherField.Command
                || Embedded != otherField.Embedded
                || Indent != otherField.Indent
                || Offset != otherField.Offset
                || SubField != otherField.SubField
                || Tag != otherField.Tag
                || TagSpecification != otherField.TagSpecification
                || RepeatCount != otherField.RepeatCount
                || IndexSpecification.Compare
                    (
                        FieldRepeat,
                        otherField.FieldRepeat
                    )
                || IndexSpecification.Compare
                    (
                        SubFieldRepeat,
                        otherField.SubFieldRepeat
                    )
                )
            {
                throw new IrbisException();
            }

            PftSerializationUtility.CompareLists
                (
                    LeftHand,
                    otherField.LeftHand
                );
            PftSerializationUtility.CompareLists
                (
                    RightHand,
                    otherField.RightHand
                );
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            FieldInfo info = compiler.CompileField(this);

            compiler.CompileNodes(LeftHand);
            compiler.CompileNodes(RightHand);

            string leftHand = compiler.CompileAction(LeftHand) ?? "null";
            string rightHand = compiler.CompileAction(RightHand) ?? "null";

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("Action leftHand = {0};", leftHand);

            compiler
                .WriteIndent()
                .WriteLine("Action rightHand = {0};", rightHand);

            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "DoField({0}, leftHand, rightHand);",
                        info.Reference
                    );

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
            PftSerializer.Deserialize(reader, LeftHand);
            PftSerializer.Deserialize(reader, RightHand);
            Command = reader.ReadChar();
            Embedded = reader.ReadNullableString();
            Indent = reader.ReadPackedInt32();
            Offset = reader.ReadPackedInt32();
            Length = reader.ReadPackedInt32();
            SubField = reader.ReadChar();
            Tag = reader.ReadNullableString();
            TagSpecification = reader.ReadNullableString();
            RepeatCount = reader.ReadPackedInt32();
            FieldRepeat.Deserialize(reader);
            SubFieldRepeat.Deserialize(reader);
            SubFieldSpecification = reader.ReadNullableString();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            Log.Error
                (
                    "PftField::Execute: "
                    + "must be overridden"
                );

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name),
                Value = Text
            };

            if (!string.IsNullOrEmpty(TagSpecification))
            {
                PftNodeInfo spec = new PftNodeInfo
                {
                    Name = "TagSpec",
                    Value = TagSpecification
                };
                result.Children.Add(spec);
            }

            if (FieldRepeat.Kind != IndexKind.None)
            {
                PftNodeInfo index = new PftNodeInfo
                {
                    Name = "FieldIndex",
                    Value = FieldRepeat.Expression
                };
                result.Children.Add(index);
            }

            if (SubFieldRepeat.Kind != IndexKind.None)
            {
                PftNodeInfo index = new PftNodeInfo
                {
                    Name = "SubFieldIndex",
                    Value = SubFieldRepeat.Expression
                };
                result.Children.Add(index);
            }

            if (LeftHand.Count != 0)
            {
                PftNodeInfo leftNode = new PftNodeInfo
                {
                    Name = "Left hand"
                };
                result.Children.Add(leftNode);
                foreach (PftNode node in LeftHand)
                {
                    leftNode.Children.Add(node.GetNodeInfo());
                }
            }

            if (RightHand.Count != 0)
            {
                PftNodeInfo rightNode = new PftNodeInfo
                {
                    Name = "Right hand"
                };
                result.Children.Add(rightNode);
                foreach (PftNode node in RightHand)
                {
                    rightNode.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            LeftHand.Optimize();
            RightHand.Optimize();

            return this;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer.SingleSpace();

            printer.WriteNodes(LeftHand);

            FieldSpecification specification = ToSpecification();
            printer.Write(specification.ToString());

            printer.WriteNodes(RightHand);
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);
            PftSerializer.Serialize(writer, LeftHand);
            PftSerializer.Serialize(writer, RightHand);
            writer.Write(Command);
            writer
                .WriteNullable(Embedded)
                .WritePackedInt32(Indent)
                .WritePackedInt32(Offset)
                .WritePackedInt32(Length)
                .Write(SubField);

            writer
                .WriteNullable(Tag)
                .WriteNullable(TagSpecification)
                .WritePackedInt32(RepeatCount);

            FieldRepeat.Serialize(writer);
            SubFieldRepeat.Serialize(writer);

            writer.WriteNullable(SubFieldSpecification);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return ToSpecification().ToString();
        }

        #endregion
    }
}
