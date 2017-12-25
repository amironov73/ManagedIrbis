// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftConditionalLiteral.cs -- условный литерал
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Определяет текст, который будет выведен только
    /// в случае присутствия в записи соответствующего
    /// ему поля/подполя. Если поле является повторяющимся,
    /// то текст будет выведен только один раз,
    /// независимо от количества экземпляров поля/подполя.
    /// Условные  литералы заключаются в двойные кавычки ("),
    /// например, "Заглавие: ".
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftConditionalLiteral
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Whether the literal is on right or on left hand?
        /// </summary>
        public bool IsSuffix { get; set; }

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
        public PftConditionalLiteral()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionalLiteral
            (
                [NotNull] string text,
                bool isSuffix
            )
        {
            Code.NotNull(text, "text");

            Text = text;
            IsSuffix = isSuffix;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionalLiteral
            (
                [NotNull] PftToken token,
                bool isSuffix
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.ConditionalLiteral);

            IsSuffix = isSuffix;

            try
            {
                Text = token.Text.ThrowIfNull("token.Text");
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftConditionLiteral::Constructor",
                        exception
                    );

                throw new PftSyntaxException(token, exception);
            }
        }

        #endregion

        #region Private members

        private void _Execute
            (
                [NotNull] PftContext context,
                [NotNull] PftField field
            )
        {
            string value = field.GetValue(context);

            if (field.CanOutput(value))
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

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode" />
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            if (IsSuffix != ((PftConditionalLiteral) otherNode).IsSuffix)
            {
                throw new IrbisException();
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
                        "DoConditionalLiteral(\"{0}\", {1}, {2});",
                        CompilerUtility.Escape(Text),
                        info.Reference,
                        CompilerUtility.BooleanToText(IsSuffix)
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

            IsSuffix = reader.ReadBoolean();
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
                if (IsSuffix)
                {
                    if (field.IsLastRepeat(context))
                    {
                        _Execute(context, field);
                    }
                }
                else
                {
                    if (field.IsFirstRepeat(context))
                    {
                        _Execute(context, field);
                    }
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

            writer.Write(IsSuffix);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .WriteIndentIfNeeded()
                .Write('"')
                .Write(Text)
                .Write('"')
                .WriteLineIfNeeded();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return '"' + Text + '"';
        }

        #endregion
    }
}
