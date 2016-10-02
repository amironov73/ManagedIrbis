/* PftC.cs -- табуляция в указанную позицию строки
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
    /// Перемещает виртуальный курсор в n-ю позицию строки
    /// (табуляция в указанную позицию строки).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftC
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Новая позиция курсора.
        /// </summary>
        public int NewPosition { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftC()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftC
            (
                int position
            )
        {
            NewPosition = position;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftC
            (
                [NotNull] PftToken token
            )
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.X);

            try
            {
                NewPosition = int.Parse
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

            base.Execute(context);

            int desired = NewPosition * 8;
            int current = context.Output.GetCaretPosition();
            int delta = desired - current;
            if (delta > 0)
            {
                context.Write
                    (
                        this,
                        new string(' ', delta)
                    );
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
                    "c{0}", // Всегда в нижнем регистре
                    NewPosition.ToInvariantString()
                );
        }

        #endregion
    }
}
