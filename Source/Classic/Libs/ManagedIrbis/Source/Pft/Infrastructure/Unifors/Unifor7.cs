// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor7.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Client;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Расформатирование группы связанных документов из другой БД
    // Вид функции: 7.
    // Назначение: Расформатирование группы связанных документов
    // из другой БД(отношение «от одного к многим»).
    // Функция обеспечивает возможность связать запись
    // с рядом других записей по какому бы то ни было общему признаку.
    // К примеру, можно отобрать все записи с определенным заглавие,
    // индексом УДК/ББК, ключевым словом.
    // Присутствует в версиях ИРБИС с 2004.1.
    // Формат (передаваемая строка):
    // 7<имя_БД>,</termin/>,<@имя_формата|формат|*>
    // где:
    // имя_БД – имя базы данных, из которой будут браться
    // связанные документы; по умолчанию используется текущая БД.
    // /termin/ – ключевой термин, на основе которого отбираются
    // связанные документы; термин заключается в уникальные
    // ограничители (например. /), в качестве которых используется
    // символ, не входящий(гарантированно) в термин.
    // @имя_формата|формат|* – имя формата или формат в явном виде,
    // в соответствии с которым будут расформатироваться связанные
    // документы. Если задается имя формата, то он берется
    // из директории БД, заданной параметром <имя_БД>.
    // Если задается *, данные выводятся по прямой ссылке
    // (метка поля, номер повторения).
    // Примеры:
    // &unifor('7TEST,',"/T="v200^a"/",',v903"\par "')
    //
    // &uf(|7EK,!FAK= 23.01!,&uf('av907^A#1'),&uf('6brief')/|d90),
    //

    static class Unifor7
    {
        #region Private members

        #endregion

        #region Public methods

        public static void FormatDocuments
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
            if (navigator.ReadChar() == TextNavigator.EOF)
            {
                return;
            }
            IrbisProvider provider = context.Provider;
            if (string.IsNullOrEmpty(database))
            {
                database = provider.Database;
            }
            char delimiter = navigator.ReadChar();
            if (delimiter == TextNavigator.EOF)
            {
                return;
            }
            string term = navigator.ReadUntil(delimiter);
            if (string.IsNullOrEmpty(term))
            {
                return;
            }
            if (navigator.ReadChar() != delimiter
                || navigator.ReadChar() != ',')
            {
                return;
            }
            string format = navigator.GetRemainingText();
            if (string.IsNullOrEmpty(format))
            {
                return;
            }

            string previousDatabase = provider.Database;
            try
            {
                provider.Database = database;
                int[] found = provider.Search(term);
                if (found.Length != 0)
                {
                    PftLexer lexer = new PftLexer();
                    PftTokenList tokens = lexer.Tokenize(format);
                    PftParser parser = new PftParser(tokens);
                    PftProgram program = parser.Parse();

                    using (PftContextGuard guard
                        = new PftContextGuard(context))
                    {
                        PftContext copy = guard.ChildContext;
                        copy.Output = context.Output;
                        foreach (int mfn in found)
                        {
                            MarcRecord record
                                = copy.Provider.ReadRecord(mfn);
                            if (!ReferenceEquals(record, null))
                            {
                                copy.Record = record;
                                program.Execute(copy);
                            }
                        }
                    }
                }
            }
            finally
            {
                provider.Database = previousDatabase;
            }
        }

        #endregion
    }
}
