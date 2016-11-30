// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforD.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.CodeDom.Compiler;

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.PlatformSpecific;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    static class UniforD
    {
        #region Private members

        #endregion

        #region Public methods

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
        // Примеры:
        // &unifor('DBOOK,/K=AAA/,v200')
        //

        public static void FormatDocumentDB
            (
                PftContext context,
                PftNode node,
                string expression
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
                database = context.Environment.Database;
            }

            if (navigator.ReadChar() != ',')
            {
                return;
            }

            int[] found = new int[0];
            if (navigator.PeekChar() == '@')
            {
                navigator.ReadChar();
                int mfn;
                string mfnText = navigator.ReadInteger();
                if (int.TryParse(mfnText, out mfn))
                {
                    return;
                }
                found = new[] {mfn};
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

                string saveDatabase = context.Environment.Database;
                try
                {
                    context.Environment.Database = database;

                    found = context.Environment.Search(query);
                }
                finally
                {
                    context.Environment.Database = saveDatabase;
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
                // Not implemented
            }
            else
            {
                PftLexer lexer = new PftLexer();
                PftTokenList tokens = lexer.Tokenize(format);
                PftParser parser = new PftParser(tokens);
                PftProgram program = parser.Parse();

                PftContext copy = context.Push();
                copy.Output = context.Output;
                try
                {
                    foreach (int mfn in found)
                    {
                        MarcRecord record = copy.Environment.ReadRecord(mfn);
                        if (!ReferenceEquals(record, null))
                        {
                            copy.Record = record;
                            program.Execute(copy);
                        }
                    }
                }
                finally
                {
                    context.Pop();
                }
            }
        }

        #endregion
    }
}
