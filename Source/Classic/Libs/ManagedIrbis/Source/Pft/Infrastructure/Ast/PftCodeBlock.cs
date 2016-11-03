/* PftCodeBlock.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
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

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

#if CLASSIC

            if (!_compiled)
            {
                _compiled = true;

                string text = Text;
                if (!string.IsNullOrEmpty(text))
                {
                    _method = SharpRunner.CompilePftSnippet
                        (
                            text,
                            this,
                            context
                        );
                }
            }

            if (!ReferenceEquals(_method, null))
            {
                _method.Invoke(null, new object[] {this, context});
            }

#endif

            OnAfterExecution(context);
        }

        /// <inheritdoc />
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

