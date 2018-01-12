// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforJ.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть кол-во ссылок для заданного термина – &uf('J')
    // Вид функции: J.
    // Назначение: Вернуть кол-во ссылок для заданного термина.
    // Формат (передаваемая строка):
    // J<dbn>,<термин>
    // <dbn> – имя БД; по умолчанию используется текущая.
    //
    // Пример:
    //
    // &unifor('JBOOK,',"A="v200^a)
    //

    static class UniforJ
    {
        #region Public methods

        public static void GetTermRecordCountDB
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);

            string database = navigator.ReadUntil(',');
            if (string.IsNullOrEmpty(database))
            {
                database = context.Provider.Database;
            }

            if (navigator.ReadChar() != ',')
            {
                return;
            }

            string term = navigator.GetRemainingText();
            if (string.IsNullOrEmpty(term))
            {
                return;
            }

            string saveDatabase = context.Provider.Database;
            try
            {
                context.Provider.Database = database;

                int[] found = context.Provider.Search(term);
                string output = found.Length.ToInvariantString();
                context.WriteAndSetFlag(node, output);
            }
            finally
            {
                context.Provider.Database = saveDatabase;
            }
        }

        #endregion
    }
}
