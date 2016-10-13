/* PftX.cs -- вставляет n пробелов
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
    /// Команда горизонтального позиционирования.
    /// Вставляет n пробелов.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftX
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Количество добавляемых пробелов.
        /// </summary>
        public int Shift { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftX()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftX
            (
                int shift
            )
        {
            Shift = shift;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftX
            (
                [NotNull] PftToken token
            )
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.X);

            try
            {
                Shift = int.Parse
                (
                    token.Text.ThrowIfNull("token.Text")
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
                if (Shift > 0)
                {
                    context.Write
                        (
                            this,
                            new string(' ', Shift)
                        );
                }
            }
        }

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

            if (context.CurrentField != null)
            {
                if (context.CurrentField.IsFirstRepeat(context))
                {
                    _Execute(context);
                }
            }
            else
            {
                _Execute(context);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc />
        public override void Write
            (
                StreamWriter writer
            )
        {
            writer.Write
                (
                    "x{0}", // Всегда в нижнем регистре
                    Shift.ToInvariantString()
                );
        }

        #endregion
    }
}
