// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftNested.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Временное переключение контекста
    /// на альтернативный (проще говоря,
    /// на другую запись).
    /// </summary>
    /// <example>
    /// <code>
    /// v200^a, " : "v200^e, " / "v200^f
    /// #
    /// {
    ///    /* Выводятся значения полей от другой записи
    ///    v200^a, " : "v200^e, " / "v200^f
    /// }
    /// </code>
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftNested
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNested()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNested
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.LeftCurly);
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

            using (PftContextGuard guard = new PftContextGuard(context))
            {
                PftContext copy = guard.ChildContext;
                copy.Output = context.Output;
                MarcRecord temp = copy.Record;
                copy.Record = copy.AlternativeRecord;
                copy.AlternativeRecord = temp;
                copy.Execute(Children);
            }

            OnAfterExecution(context);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append('{');
            bool first = true;
            foreach (PftNode child in Children)
            {
                if (!first)
                {
                    result.Append(' ');
                }
                result.Append(child);
                first = false;
            }
            result.Append('}');

            return result.ToString();
        }

        #endregion
    }
}
