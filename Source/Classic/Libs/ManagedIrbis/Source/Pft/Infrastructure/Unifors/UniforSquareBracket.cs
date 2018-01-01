// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforSquareBracket.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанный &unifor('[').
    // Очищает текст от команд контекстного выделения
    // (постредактура)
    //

    static class UniforSquareBracket
    {
        #region Public methods

        /// <summary>
        /// Remove the context markup commands.
        /// </summary>
        public static void CleanContextMarkup
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            context.GetRootContext().PostProcessing |= PftCleanup.ContextMarkup;
        }

        #endregion
    }
}
