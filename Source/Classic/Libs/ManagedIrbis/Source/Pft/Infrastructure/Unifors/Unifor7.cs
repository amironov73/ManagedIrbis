// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor7.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Search;

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

        [NotNull]
        private static TermLink[] ExtractLinks
            (
                [NotNull] IrbisProvider provider,
                [NotNull] string term
            )
        {
            TermLink[] result;

            if (term.EndsWith("$"))
            {
                string start = term.Substring(0, term.Length - 1);
                result = provider.ExactSearchTrimLinks(start, 100);
            }
            else
            {
                result = provider.ExactSearchLinks(term);
            }

            return result;
        }

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

            if (format == "*")
            {
                // TODO implement

                // ibatrak этот параметр игнорируется

                return;
            }

            string previousDatabase = provider.Database;
            try
            {
                // ibatrak
                // После вызова этого unifor в главном контексте
                // сбрасываются флаги пост обработки
                context.GetRootContext().PostProcessing = PftCleanup.None;

                provider.Database = database;
                TermLink[] links = ExtractLinks(provider, term);
                int[] found = TermLink.ToMfn(links);
                if (found.Length != 0)
                {
                    // TODO some caching

                    PftProgram program = PftUtility.CompileProgram(format);

                    using (PftContextGuard guard = new PftContextGuard(context))
                    {
                        PftContext nestedContext = guard.ChildContext;

                        // ibatrak
                        // формат вызывается в контексте без повторений
                        nestedContext.Reset();

                        nestedContext.Output = context.Output;
                        foreach (int mfn in found)
                        {
                            MarcRecord record = nestedContext.Provider.ReadRecord(mfn);
                            if (!ReferenceEquals(record, null))
                            {
                                nestedContext.Record = record;
                                program.Execute(nestedContext);
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
