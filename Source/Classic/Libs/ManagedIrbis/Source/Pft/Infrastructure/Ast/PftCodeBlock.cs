// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCodeBlock.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#if CLASSIC

using System.Reflection;

#endif

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftCodeBlock
        : PftNode
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.TripleCurly);

            if (string.IsNullOrEmpty(token.Text))
            {
                Log.Error
                    (
                        "PftCodeBlock::Constructor: "
                        + "token text not set"
                    );

                throw new PftSyntaxException(token);
            }

            Text = token.Text;
        }

        #endregion

        #region Private members

#if CLASSIC

        private bool _compiled;

        private MethodInfo _method;

#endif

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Write" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

#if CLASSIC

            if (!_compiled)
            {
                Log.Trace("PftCodeBlock::Execute: compile method");

                _compiled = true;

                string text = Text;
                if (!string.IsNullOrEmpty(text))
                {
                    _method = SharpRunner.CompileSnippet
                        (
                            text,
                            "PftNode node, PftContext context",
                            err => context.WriteLine(this, err)
                        );
                }
            }

            if (!ReferenceEquals(_method, null))
            {
                Log.Trace ("PftCodeBlock::Execute: invoke method");

                _method.Invoke(null, new object[] {this, context});
            }

#endif

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Write" />
        public override void Write
            (
                StreamWriter writer
            )
        {
            writer.Write("{{{");
            writer.Write(Text);
            writer.Write("}}}");
        }


        #endregion
    }
}

