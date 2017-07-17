// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftComma.cs -- оператор "запятая"
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Оператор "запятая".
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftComma
        : PftNode
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftComma()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftComma
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Comma);
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

            // Nothing to do actually

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            bool newLine = false;
            if (printer.Column == 0)
            {
                if (printer.EatNewLine())
                {
                    newLine = true;
                }
            }
            printer.EatWhitespace();
            printer
                .WriteIndendIfNeeded()
                .Write(", ");
            if (newLine)
            {
                printer.WriteLine();
            }
        }

        #endregion
    }
}
