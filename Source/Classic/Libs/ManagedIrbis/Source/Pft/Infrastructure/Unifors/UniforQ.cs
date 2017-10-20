// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforQ.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть заданную строку в нижнем регистре – &uf('Q
    // Вид функции: Q.
    // Назначение: Вернуть заданную строку в нижнем регистре.
    // Формат (передаваемая строка):
    // Q<строка>
    //
    // Пример:
    //
    // &unifor("Q"v200)
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class UniforQ
    {
        #region Public methods

        /// <summary>
        /// Convert the string to lower case.
        /// </summary>
        public static void ToLower
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {

                string output = StringUtility.ToLowerInvariant(expression);

                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
