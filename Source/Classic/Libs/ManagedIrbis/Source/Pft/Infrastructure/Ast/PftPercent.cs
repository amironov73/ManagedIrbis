// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftPercent.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Команда % подавляет все последовательно
    /// расположенные пустые строки(если они имеются)
    /// между текущей строкой и последней непустой строкой.
    /// Таким образом, формат
    /// 
    /// %##V10%##V20%##V30 ...
    /// 
    /// приведет к созданию одной и только одной пустой
    /// строки между каждым полем, независимо от их наличия
    /// или отсутствия в документе.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftPercent
        : PftNode
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPercent()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPercent
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Percent);
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

            context.Output.RemoveEmptyLines();
            if (!context.Output.PrecededByEmptyLine())
            {
                context.WriteLine(this);
            }
            context._eatNextNewLine = true;

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            // Обрамляем пробелами
            printer.Write(" % ");
        }

        #endregion
    }
}
