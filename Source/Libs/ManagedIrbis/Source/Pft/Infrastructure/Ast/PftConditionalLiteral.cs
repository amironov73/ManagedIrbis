/* PftConditionalLiteral.cs -- условный литерал
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM;

using CodeJam;

using JetBrains.Annotations;

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
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            Text = text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionalLiteral
            (
                [NotNull] PftToken token
            )
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.ConditionalLiteral);

            try
            {
                Text = token.Text.ThrowIfNull("token.Text");
            }
            catch (Exception exception)
            {
                throw new PftSyntaxException(token, exception);
            }
        }

        #endregion

        #region Private members

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

            if (!context.BreakFlag)
            {
                if (context.Index == 0)
                {
                    context.Write
                        (
                            this,
                            Text
                        );
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
            writer.Write('"');
            writer.Write(Text);
            writer.Write('"');
        }

        #endregion
    }
}
