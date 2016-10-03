/* PftField.cs -- ссылка на поле
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.Collections;
using AM.Text;
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

            Parse(text);
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

            try
            {
                Parse
                    (
                        token.Text
                        .ThrowIfNull("token.Text")
                    );
            }
            catch (Exception exception)
            {
                throw new PftSyntaxException(token, exception);
            }
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

            RecordField field = record.Fields.GetField(Tag, context.Index);
            if (field == null)
            {
                return null;
            }

            string result;

            if (SubField == NoSubField)
            {
                result = field.ToText();
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
        /// Parse the text.
        /// </summary>
        public void Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            TextNavigator navigator = new TextNavigator(text);
            char c = navigator.ReadChar();
            StringBuilder builder = new StringBuilder();

            switch (c)
            {
                case 'd':
                case 'D':
                    Command = 'd';
                    break;

                case 'n':
                case 'N':
                    Command = 'n';
                    break;

                case 'v':
                case 'V':
                    Command = 'v';
                    break;

                default:
                    throw new PftSyntaxException(navigator);
            }

            c = navigator.ReadChar();
            if (!c.IsArabicDigit())
            {
                throw new PftSyntaxException(navigator);
            }
            builder.Append(c);

            while (true)
            {
                c = navigator.PeekChar();
                if (!c.IsArabicDigit())
                {
                    break;
                }
                navigator.ReadChar();
                builder.Append(c);
            }
            Tag = builder.ToString();

            if (c == '@')
            {
                builder.Length = 0;
                navigator.ReadChar();
                while (true)
                {
                    c = navigator.PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadChar();
                    builder.Append(c);
                }
                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Embedded = builder.ToString();
            }

            if (c == '^')
            {
                navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    throw new PftSyntaxException(navigator);
                }
                c = navigator.ReadChar();
                if (!SubFieldCode.IsValidCode(c))
                {
                    throw new PftSyntaxException(navigator);
                }
                SubField = c;
                c = navigator.ReadChar();
            }

            if (c == '[')
            {
                navigator.ReadChar();
                navigator.SkipWhitespace();

                string index;

                if (navigator.PeekChar() == '-')
                {
                    navigator.ReadChar();
                    navigator.SkipWhitespace();
                    index = navigator.ReadInteger();
                    if (string.IsNullOrEmpty(index))
                    {
                        throw new PftSyntaxException(navigator);
                    }
                    int indexTo = int.Parse
                        (
                            index,
                            CultureInfo.InvariantCulture
                        );
                    IndexTo = indexTo;
                }
                else
                {
                    index = navigator.ReadInteger();
                    if (string.IsNullOrEmpty(index))
                    {
                        throw new PftSyntaxException(navigator);
                    }
                    int indexFrom = int.Parse
                        (
                            index,
                            CultureInfo.InvariantCulture
                        );
                    IndexFrom = indexFrom;
                    IndexTo = indexFrom;
                }

                navigator.SkipWhitespace();
                if (navigator.SkipChar('-'))
                {
                    navigator.SkipWhitespace();
                    index = navigator.ReadInteger();
                    if (string.IsNullOrEmpty(index))
                    {
                        IndexTo = 0;
                    }
                    else
                    {
                        int indexTo = int.Parse
                            (
                                index,
                                CultureInfo.InvariantCulture
                            );
                        IndexTo = indexTo;
                    }
                }

                navigator.SkipWhitespace();
                if (navigator.ReadChar() != ']')
                {
                    throw new PftSyntaxException(navigator);
                }
            }

            if (c == '*')
            {
                builder.Length = 0;

                while (true)
                {
                    c = navigator.ReadChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Offset = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );
            }

            if (c == '.')
            {
                builder.Length = 0;

                while (true)
                {
                    c = navigator.ReadChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Length = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );
            }

            if (c == '(')
            {
                builder.Length = 0;

                while (true)
                {
                    c = navigator.ReadChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    builder.Append(c);
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Indent = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );
            }

            if (c == '#')
            {
                navigator.ReadChar();

                string index = navigator.ReadInteger();
                if (string.IsNullOrEmpty(index))
                {
                    throw new PftSyntaxException(navigator);
                }
                int indexFrom = int.Parse(index);
                IndexFrom = indexFrom;
                IndexTo = indexFrom;
            }
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
            StringBuilder result = new StringBuilder();

            result.Append(Command);
            result.Append(Tag);
            if (!string.IsNullOrEmpty(Embedded))
            {
                result.Append('@');
                result.Append(Embedded);
            }
            if (SubField != NoSubField)
            {
                result.Append('^');
                result.Append(SubField);
            }
            if (IndexFrom != 0 || IndexTo != 0)
            {
                result.Append('[');
                if (IndexFrom == IndexTo)
                {
                    result.Append(IndexFrom);
                }
                else
                {
                    if (IndexFrom != 0)
                    {
                        result.Append(IndexFrom);
                    }
                    result.Append('-');
                    if (IndexTo != 0)
                    {
                        result.Append(IndexTo);
                    }
                }
                result.Append(']');
            }
            if (Offset != 0)
            {
                result.Append('*');
                result.Append(Offset);
            }
            if (Length != 0)
            {
                result.Append('.');
                result.Append(Length);
            }
            if (Indent != 0)
            {
                result.Append('(');
                result.Append(Indent);
                result.Append(')');
            }

            return result.ToString();
        }

        #endregion
    }
}
