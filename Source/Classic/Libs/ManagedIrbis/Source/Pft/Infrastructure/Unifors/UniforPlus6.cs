// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus6.cs -- 
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
    // Выдать статус записи – &uf('+6')
    // Вид функции: +6.
    // Назначение: Выдать статус записи.
    // Если запись логически удаленная, возвращается 0,
    // в противном случае – 1.
    // Присутствует в версиях ИРБИС с 2005.2.
    // Формат (передаваемая строка):
    // +6
    //
    // Пример:
    //
    // if &unifor('+6')='0' then 'запись логически удаленная' fi
    //

    static class UniforPlus6
    {
        #region Public methods

        /// <summary>
        /// Get record status: whether the record is deleted?
        /// </summary>
        public static void GetRecordStatus
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            string result = "1";

            if (!ReferenceEquals(context.Record, null)
                && context.Record.Deleted)
            {
                result = "0";
            }

            context.WriteAndSetFlag(node, result);
        }

        #endregion
    }
}
