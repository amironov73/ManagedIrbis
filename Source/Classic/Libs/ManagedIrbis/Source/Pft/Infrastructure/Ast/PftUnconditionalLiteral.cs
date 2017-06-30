// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftUnconditionalLiteral.cs -- unconditional literal
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Unconditional literal.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftUnconditionalLiteral
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.Text" />
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = PftUtility.PrepareText(value); }
        }

        /// <summary>
        /// Throw an exception when empty literal detected.
        /// </summary>
        public static bool ThrowOnEmpty { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PftUnconditionalLiteral()
        {
            ThrowOnEmpty = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnconditionalLiteral()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnconditionalLiteral
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
        public PftUnconditionalLiteral
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.UnconditionalLiteral);

            try
            {
                Text = token.Text.ThrowIfNull("token.Text");
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftUnconditionalLiteral::Constructor",
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

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string text = Text;

            if (string.IsNullOrEmpty(text))
            {
                Log.Error
                    (
                        "PftUnconditionalLiteral::Execute: "
                        + "empty literal: "
                        + this
                    );

                if (ThrowOnEmpty)
                {
                    throw new PftSemanticException
                        (
                            "Empty literal detected: "
                            + this
                        );
                }
            }

            if (context.UpperMode
                && !ReferenceEquals(text, null))
            {
                text = text.ToUpper();
            }

            context.Write
                (
                    this,
                    text
                );

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Write" />
        public override void Write
            (
                StreamWriter writer
            )
        {
            writer.Write('\'');
            writer.Write(Text);
            writer.Write('\'');
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return '\'' + Text + '\'';
        }

        #endregion
    }
}
