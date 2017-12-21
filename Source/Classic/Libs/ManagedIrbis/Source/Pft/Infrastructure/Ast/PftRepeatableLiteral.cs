// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftRepeatableLiteral.cs -- повторяющийся литерал
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM;
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
    /// Определяет текст, который будет выведен только
    /// в случае присутствия в записи соответствующего
    /// ему поля или подполя. Однако, если поле
    /// повторяющееся, литерал будет выводиться
    /// для каждого экземпляра поля/подполя.
    /// Повторяющиеся литералы заключаются
    /// в вертикальные черты (|), например, |Автор: |.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftRepeatableLiteral
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Prefix or postfix?
        /// </summary>
        public bool IsPrefix { get; set; }

        /// <summary>
        /// Plus?
        /// </summary>
        public bool Plus { get; set; }

        /// <inheritdoc cref="PftNode.Text" />
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = PftUtility.PrepareText(value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRepeatableLiteral()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRepeatableLiteral
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            Text = text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRepeatableLiteral
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.RepeatableLiteral);

            try
            {
                Text = token.Text.ThrowIfNull("token.Text");
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftRepeatableLiteral::Constructor",
                        exception
                    );

                throw new PftSyntaxException(token, exception);
            }
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode" />
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            PftRepeatableLiteral otherLiteral
                = (PftRepeatableLiteral) otherNode;
            if (IsPrefix != otherLiteral.IsPrefix
                || Plus != otherLiteral.Plus)
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
            compiler.StartMethod(this);

            if (!string.IsNullOrEmpty(Text))
            {
                PftField field = Parent as PftField;
                if (ReferenceEquals(field, null))
                {
                    throw new PftCompilerException();
                }

                FieldInfo info = compiler.Fields.Get(field);
                if (ReferenceEquals(info, null))
                {
                    throw new PftCompilerException();
                }

                compiler
                    .WriteIndent()
                    .WriteLine
                        (
                            "DoRepeatableLiteral(\"{0}\", {1}, {2}, {3});",
                            CompilerUtility.Escape(Text),
                            info.Reference,
                            CompilerUtility.BooleanToText(IsPrefix),
                            CompilerUtility.BooleanToText(Plus)
                        );
            }

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

            IsPrefix = reader.ReadBoolean();
            Plus = reader.ReadBoolean();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            PftField field = context.CurrentField;

            if (!ReferenceEquals(field, null))
            {
                bool flag = field.HaveRepeat(context);

                if (flag)
                {
                    string value = field.GetValue(context);

                    flag = field.CanOutput(value);
                }

                if (flag && Plus)
                {
                    flag = IsPrefix
                        ? !field.IsFirstRepeat(context)
                        : !field.IsLastRepeat(context);
                }

                if (flag)
                {
                    string text = Text;
                    if (context.UpperMode
                        && !ReferenceEquals(text, null))
                    {
                        text = IrbisText.ToUpper(text);
                    }
                    context.Write(this, text);
                    context.OutputFlag = true;
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            if (string.IsNullOrEmpty(Text))
            {
                // Take the node away from the AST

                return null;
            }

            return this;
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.Write(IsPrefix);
            writer.Write(Plus);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            if (!IsPrefix && Plus)
            {
                printer.Write('+');
            }
            printer.Write('|');
            printer.Write(Text);
            printer.Write('|');
            if (IsPrefix && Plus)
            {
                printer.Write('+');
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            if (!IsPrefix && Plus)
            {
                result.Append('+');
            }
            result.Append('|');
            result.Append(Text);
            result.Append('|');
            if (IsPrefix && Plus)
            {
                result.Append('+');
            }

            return result.ToString();
        }

        #endregion
    }
}
