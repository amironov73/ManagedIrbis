/* PftField.cs -- ссылка на поле
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM;
using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Ссылка на поле.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftField
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

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftField
            (
                [NotNull] string text
            )
            : this()
        {
            Code.NotNullNorEmpty(text, "text");

            FieldSpecification specification = new FieldSpecification();
            if (!specification.Parse(text))
            {
                throw new PftSyntaxException();
            }
            Apply(specification);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftField
            (
                [NotNull] PftToken token
            )
            : this()
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.V);

            FieldSpecification specification = ((FieldSpecification)token.UserData)
                .ThrowIfNull("token.UserData");
            Apply(specification);
        }

        #endregion

        #region Private members

        private void _Execute
            (
                PftContext context
            )
        {
            if (!context.BreakFlag)
            {
                try
                {
                    context.CurrentField = this;

                    foreach (PftNode node in LeftHand)
                    {
                        node.Execute(context);
                    }

                    string value = GetValue(context);
                    if (!string.IsNullOrEmpty(value))
                    {
                        context.Write(this, value);
                    }
                    if (HaveRepeat(context))
                    {
                        context.OutputFlag = true;
                    }

                    foreach (PftNode node in RightHand)
                    {
                        node.Execute(context);
                    }
                }
                finally
                {
                    context.CurrentField = null;
                }
            }
        }

        [CanBeNull]
        private static string _SafeSubString
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
        /// Число повторений _поля_ в записи.
        /// </summary>
        public int GetCount
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            MarcRecord record = context.Record;
            if (record == null
                || string.IsNullOrEmpty(Tag))
            {
                return 0;
            }

            return record.Fields.GetField(Tag).Length;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        [CanBeNull]
        public string GetValue
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

            string result = _SafeSubString(text, offset, length);

            if (Indent != 0)
            {
                result = new string(' ', Indent) + result;
            }

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
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (context.CurrentField != null)
            {
                throw new PftSemanticException("nested field");
            }

            if (context.CurrentGroup != null)
            {
                _Execute(context);
            }
            else
            {
                context.Index = 0;

                while (true)
                {
                    context.OutputFlag = false;

                    _Execute(context);

                    if (!context.OutputFlag
                        || context.BreakFlag)
                    {
                        break;
                    }

                    context.Index++;
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc />
        public override void Write
            (
                StreamWriter writer
            )
        {
            foreach (PftNode node in LeftHand)
            {
                node.Write(writer);
            }

            writer.Write(ToString());

            foreach (PftNode node in RightHand)
            {
                node.Write(writer);
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToSpecification().ToString();
        }

        #endregion
    }
}
