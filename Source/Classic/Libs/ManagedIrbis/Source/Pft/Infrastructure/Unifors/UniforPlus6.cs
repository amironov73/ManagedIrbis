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

    //
    // ИРБИС64+
    //
    // &uf('+60') - неизвестно (то же, что и '+6'?)
    // &uf('+61') - неизвестно (0?)
    // &uf('+62') - неизвестно (1?)
    // &uf('+63') - статус актуализации полных текстов
    //              0 - актуализировано
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
            string result;

            if (expression == "+6"
                || expression == "+60") // TODO check this
            {
                result = "1";
                if (!ReferenceEquals(context.Record, null)
                    && context.Record.Deleted)
                {
                    result = "0";
                }
            }
            else if (expression == "61")
            {
                result = "1"; // TODO implement properly
            }
            else if (expression == "62")
            {
                result = "0"; // TODO implement properly
            }
            else if (expression == "63")
            {
                // Full-text actualization status
                result = "0"; // TODO implement properly
            }
            else
            {
                result = string.Empty;
            }

            context.WriteAndSetFlag(node, result);
        }

        #endregion
    }
}
