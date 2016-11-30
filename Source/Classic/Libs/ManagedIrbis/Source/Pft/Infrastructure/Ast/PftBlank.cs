// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftBlank.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftBlank
        : PftCondition
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBlank()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBlank
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Blank);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Is the string blank?
        /// </summary>
        public static bool IsBlank
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            foreach (char c in text)
            {
                if (!char.IsWhiteSpace(c))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string text = context.Evaluate(Children);

            Value = IsBlank(text);

            OnAfterExecution(context);
        }

        #endregion
    }
}
