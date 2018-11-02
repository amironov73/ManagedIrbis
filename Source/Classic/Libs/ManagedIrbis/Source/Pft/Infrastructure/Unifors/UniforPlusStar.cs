// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusStar.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Получение GUID записи
    //

    //
    // Встречается в IBIS.FST
    //
    // 1001 0 MHL,if &uf('+*') <> '' then 'GUID=',&uf('+*') fi
    //
    // С помощью v2147483647 получить GUID нельзя
    //

    static class UniforPlusStar
    {
        #region Public methods

        // ================================================================

        public static void GetGuid
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            string guid = IrbisGuid.Get(context.Record);
            context.WriteAndSetFlag(node, guid);
        }

        // ================================================================

        #endregion
    }
}
