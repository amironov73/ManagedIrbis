// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforD.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Search;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Форматирование документа из другой БД – &uf('D')
    //
    // Назначение: Форматирование документа из другой БД 
    // (REF на другую БД – отношение «от одного к одному»).
    //
    // Формат (передаваемая строка):
    //
    // D<dbn>,<@mfn|/termin/>,<@имя_формата|формат|*>
    //
    // Передаются три параметра, разделенные запятой:
    // Первый – имя БД;
    // Второй – или непосредственно MFN с предшествующим
    // символом @ или термин, ссылающийся на документ
    // (термин – заключается в ограничительные символы);
    // Третий – или имя формата с предшествующим символом
    // @ или непосредственно формат.
    // Если задается *, данные выводятся по прямой ссылке
    // (метка поля, номер повторения).
    //
    // Пример:
    //
    // &unifor('DBOOK,/K=AAA/,v200')
    //

    static class UniforD
    {
        #region Public methods

        public static void FormatDocumentDB
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

            int[] found;
            if (navigator.PeekChar() == '@')
            {
                navigator.ReadChar();
                int mfn;
                string mfnText = navigator.ReadInteger();
                if (!NumericUtility.TryParseInt32(mfnText, out mfn))
                {
                    return;
                }
                found = new[] { mfn };
            }
            else
            {
                char delimiter = navigator.ReadChar();
                string query = navigator.ReadUntil(delimiter);
                if (string.IsNullOrEmpty(query))
                {
                    return;
                }
                navigator.ReadChar();

                string saveDatabase = context.Provider.Database;
                try
                {
                    context.Provider.Database = database;

                    TermLink[] links = context.Provider.ExactSearchLinks(query);
                    found = TermLink.ToMfn(links);
                }
                finally
                {
                    context.Provider.Database = saveDatabase;
                }
            }

            if (found.Length == 0)
            {
                return;
            }

            if (navigator.ReadChar() != ',')
            {
                return;
            }

            string format = navigator.GetRemainingText();
            if (string.IsNullOrEmpty(format))
            {
                return;
            }

            if (format == "*")
            {
                // TODO implement

                throw new NotImplementedException();
            }
            else
            {
                PftProgram program = PftUtility.CompileProgram(format);

                using (PftContextGuard guard = new PftContextGuard(context))
                {
                    PftContext copy = guard.ChildContext;
                    string saveDatabase = copy.Provider.Database;
                    try
                    {
                        copy.Provider.Database = database;
                        copy.Output = context.Output;
                        foreach (int mfn in found)
                        {
                            MarcRecord record = copy.Provider.ReadRecord(mfn);
                            if (!ReferenceEquals(record, null))
                            {
                                copy.Record = record;
                                program.Execute(copy);
                            }
                        }
                    }
                    finally
                    {
                        copy.Provider.Database = saveDatabase;
                    }
                }
            }
        }

        #endregion
    }
}
