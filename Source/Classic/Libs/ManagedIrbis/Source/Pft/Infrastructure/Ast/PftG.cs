// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftG.cs -- global variable reference
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Global variable reference
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftG
        : PftField
    {
        #region Properties

        /// <summary>
        /// Number of the variable.
        /// </summary>
        public int Number { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            FieldSpecification specification = new FieldSpecification();
            if (!specification.Parse(text))
            {
                throw new PftSyntaxException();
            }

            Apply(specification);

            Number = int.Parse
                (
                    Tag.ThrowIfNull("Tag")
                );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                int number
            )
        {
            Code.Positive(number, "number");

            Command = 'g';
            Number = number;
            Tag = number.ToInvariantString();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                int number,
                char subField
            )
        {
            Code.Positive(number, "number");

            Command = 'g';
            Number = number;
            Tag = number.ToInvariantString();
            SubField = subField;
        }

        #endregion

        #region Private members

        private int _count;

        private void _Execute
            (
                PftContext context
            )
        {
            try
            {
                context.CurrentField = this;

                context.Execute(LeftHand);

                string value = GetValue(context);
                if (!string.IsNullOrEmpty(value))
                {
                    if (Indent != 0
                        && IsFirstRepeat(context)
                       )
                    {
                        value = new string(' ', Indent) + value;
                    }

                    context.Write(this, value);
                }
                if (HaveRepeat(context))
                {
                    context.OutputFlag = true;
                    context.VMonitor = true;
                }

                context.Execute(RightHand);
            }
            finally
            {
                context.CurrentField = null;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Число повторений _поля_ в записи.
        /// </summary>
        public int GetCount
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            int result = context.Globals.Get(Number).Length;

            return result;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        public override string GetValue
            (
                PftContext context
            )
        {
            Code.NotNull(context, "context");

            if (_count == 0)
            {
                return null;
            }

            int index = context.Index;

            RecordField field = context.Globals.Get(Number)
                .GetOccurrence(index);
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            string result;

            if (SubField == NoSubField)
            {
                result = field.FormatField
                    (
                        context.FieldOutputMode,
                        context.UpperMode
                    );
            }
            else if (SubField == '*')
            {
                result = field.GetValueOrFirstSubField();
            }
            else
            {
                result = field.GetFirstSubFieldValue(SubField);
            }

            result = LimitText(result);

            return result;
        }

        /// <summary>
        /// Have value?
        /// </summary>
        public override bool HaveRepeat
            (
                PftContext context
            )
        {
            Code.NotNull(context, "context");

            return context.Index < GetCount(context);
        }

        #endregion

        #region PftField members

        /// <inheritdoc cref="PftField.IsLastRepeat" />
        public override bool IsLastRepeat
            (
                PftContext context
            )
        {
            return context.Index >= _count - 1;
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

            if (Number != ((PftG) otherNode).Number)
            {
                throw new PftSerializationException();
            }
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (Number == 0)
            {
                Number = NumericUtility.ParseInt32
                    (
                        Tag.ThrowIfNull("Tag")
                    );
            }

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
                        "DoGlobal({0}, {1}, leftHand, rightHand);",
                        Number.ToInvariantString(),
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

            Number = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!ReferenceEquals(context.CurrentField, null))
            {
                Log.Error
                    (
                        "PftG::Execute: "
                        + "nested field detected"
                    );

                throw new PftSemanticException("nested field");
            }

            if (Number == 0)
            {
                Number = NumericUtility.ParseInt32
                    (
                        Tag.ThrowIfNull("Tag")
                    );
            }

            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                if (IsFirstRepeat(context))
                {
                    _count = GetCount(context);
                }

                _Execute(context);
            }
            else
            {
                _count = GetCount(context);

                context.DoRepeatableAction(_Execute);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftField.GetAffectedFields" />
        public override int[] GetAffectedFields()
        {
            return new int[0];
        }

        /// <inheritdoc cref="PftField.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            foreach (PftNode node in LeftHand)
            {
                node.PrettyPrint(printer);
            }

            printer.Write(ToString());

            foreach (PftNode node in RightHand)
            {
                node.PrettyPrint(printer);
            }
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
            BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WritePackedInt32(Number);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            PftUtility.NodesToText(result, LeftHand);
            result.Append(ToSpecification());
            PftUtility.NodesToText(result, RightHand);

            return result.ToString();
        }

        #endregion
    }
}
