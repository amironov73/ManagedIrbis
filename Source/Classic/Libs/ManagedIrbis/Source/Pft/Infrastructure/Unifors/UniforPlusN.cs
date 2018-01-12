// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusN.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать количество повторений поля – &uf('+N')
    // Вид функции: +N.
    // Назначение: Выдать количество повторений поля,
    // метка которого указана после идентификатора функции.
    // Формат (передаваемая строка): +N
    //
    // Пример:
    //
    // &unifor('+N910')
    //

    static class UniforPlusN
    {
        #region Public methods

        /// <summary>
        /// Get field count.
        /// </summary>
        public static void GetFieldCount
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int tag = expression.SafeToInt32();
            int count = 0;
            MarcRecord record = context.Record;

            if (!ReferenceEquals(record, null)
                && tag > 0)
            {
                count = record.Fields.GetFieldCount(tag);
            }

            string output = count.ToInvariantString();
            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
