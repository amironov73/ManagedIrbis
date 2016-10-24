/* PftField.cs -- base for field reference
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

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
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
        public NonNullCollection<PftNode> LeftHand { get; private set; }

        /// <summary>
        /// Right hand.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> RightHand { get; private set; }

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
        /// Начальный номер повторения.
        /// </summary>
        public int IndexFrom { get; set; }

        /// <summary>
        /// Конечный номер повторения.
        /// </summary>
        public int IndexTo { get; set; }

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
        /// Repeat count.
        /// </summary>
        public int RepeatCount { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftField()
        {
            LeftHand = new NonNullCollection<PftNode>();
            RightHand = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        /// <summary>
        /// Extract substring in safe manner.
        /// </summary>
        [CanBeNull]
        internal static string SafeSubString
            (
                [CanBeNull] string text,
                int offset,
                int length
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (offset < 0)
            {
                offset = 0;
            }
            if (length <= 0)
            {
                return string.Empty;
            }
            if (offset >= text.Length)
            {
                return string.Empty;
            }

            try
            {
                checked
                {
                    if ((offset + length) > text.Length)
                    {
                        length = text.Length - offset;
                        if (length <= 0)
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                throw;
            }

            string result = string.Empty;

            try
            {
                result = text.Substring
                    (
                        offset,
                        length
                    );
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                throw;
            }

            return result;
        }

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
            Indent = specification.Indent;
            IndexFrom = specification.IndexFrom;
            IndexTo = specification.IndexTo;
            Offset = specification.Offset;
            Length = specification.Length;
            SubField = specification.SubField;
            Tag = specification.Tag;

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
            IndexFrom = reference.IndexFrom;
            IndexTo = reference.IndexTo;
            Offset = reference.Offset;
            Length = reference.Length;
            SubField = reference.SubField;
            Tag = reference.Tag;

            Text = reference.ToString();
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
                || string.IsNullOrEmpty(Tag))
            {
                return null;
            }

            int index = context.Index;
            if (IndexFrom != 0)
            {
                index = index + IndexFrom - 1;
            }

            if (IndexTo != 0)
            {
                if (index >= IndexTo)
                {
                    return null;
                }
            }

            RecordField field = record.Fields.GetField(Tag, index);
            if (field == null)
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
                result = field.Value;
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
        public bool HaveRepeat
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

            RecordField field = record.Fields.GetField(Tag, context.Index);

            return field != null;
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

            string result = SafeSubString
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
                Indent = Indent,
                IndexFrom = IndexFrom,
                IndexTo = IndexTo,
                Offset = Offset,
                Length = Length,
                SubField = SubField,
                Tag = Tag
            };

            return result;
        }

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

        /// <inheritdoc/>
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = SimplifyTypeName(GetType().Name),
                Value = Text
            };

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

        /// <inheritdoc/>
        public override void PrintDebug
            (
                TextWriter writer,
                int level
            )
        {
            base.PrintDebug(writer, level);
            foreach (PftNode node in LeftHand)
            {
                node.PrintDebug(writer, level + 1);
            }
            foreach (PftNode node in RightHand)
            {
                node.PrintDebug(writer, level + 1);
            }
        }

        /// <inheritdoc />
        public override void Write
            (
                StreamWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            foreach (PftNode node in LeftHand)
            {
                node.Write(writer);
            }
        }

        #endregion
    }
}
