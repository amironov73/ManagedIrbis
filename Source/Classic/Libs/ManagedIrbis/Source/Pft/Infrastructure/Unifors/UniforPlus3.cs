// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus3.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.Logging;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Search;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Работа со строками.
    //

    static class UniforPlus3
    {
        #region Public methods

        // ================================================================

        //
        // Декодирование строки из UTF-8 – &uf('+3W')
        // Вид функции: +3W.
        // Назначение: Декодирование строки из UTF-8.
        // Формат(передаваемая строка):
        // +3W<данные>
        //

        /// <summary>
        /// Convert text from UTF8 to CP1251.
        /// </summary>
        public static void ConvertToAnsi
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = EncodingUtility.ChangeEncoding
                    (
                        expression,
                        IrbisEncoding.Utf8,
                        IrbisEncoding.Ansi
                    );
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Кодирование строки в UTF-8 – &uf('+3U')
        // Вид функции: +3U.
        // Назначение: Кодирование строки в UTF-8.
        // Формат(передаваемая строка):
        // +3U<данные>
        //

        /// <summary>
        /// Convert text from CP1251 to UTF8.
        /// </summary>
        public static void ConvertToUtf
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = EncodingUtility.ChangeEncoding
                    (
                        expression,
                        IrbisEncoding.Ansi,
                        IrbisEncoding.Utf8
                    );
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Перевод знака + в %2B – &uf('+3+')
        // Вид функции: +3+.
        // Назначение: Перевод знака + в %2B.
        // Формат (передаваемая строка):
        // +3+<данные>
        //

        /// <summary>
        /// Replace '+' sign with %2B
        /// </summary>
        public static void ReplacePlus
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string clear = expression.Replace("+", "%2B");
                context.Write(node, clear);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Декодирование данных из URL – &uf('+3D')
        // Вид функции: +3D.
        // Назначение: Декодирование данных из URL.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +3D<данные>
        //

        /// <summary>
        /// Decode text from the URL.
        /// </summary>
        public static void UrlDecode
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = StringUtility.UrlDecode
                    (
                        expression,
                        IrbisEncoding.Utf8
                    );
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Кодирование данных для представления в URL – &uf('+3E')
        // Вид функции: +3E.
        // Назначение: Кодирование данных для представления в URL.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        //  +3E<данные>
        //
        // Пример:
        //
        // &unifor('+3E', v1007)
        //

        /// <summary>
        /// Encode the text to URL format.
        /// </summary>
        public static void UrlEncode
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = StringUtility.UrlEncode
                    (
                        expression,
                        IrbisEncoding.Utf8
                    );
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // ibatrak
        //
        // Форматирование полей записи в клиентское представление без заголовка
        //
        // &uf ('+3A')

        /// <summary>
        /// Encode the record to the plain text format.
        /// </summary>
        public static void FieldsToText
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                string output = record.ToPlainText();
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        private static readonly char[] _specialChars = { '&', '"', '<', '>' };

        //
        // ibatrak
        //
        // Замена специальных символов HTML.
        //
        // Неописанная функция
        // &unifor('+3H')
        // Кривая реализация htmlspecialchars
        // заменяет 
        // & на &quot; (здесь ошибка -- надо на &amp;)
        // " на &quot;
        // < на &lt;
        // > на &gt;
        // одинарные кавычки не кодирует
        //
        public static void HtmlSpecialChars
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                if (expression.ContainsAnySymbol(_specialChars))
                {
                    StringBuilder builder = new StringBuilder(expression);
                    builder.Replace("&", "&quot;");
                    builder.Replace("\"", "&quot;");
                    builder.Replace("<", "&lt;");
                    builder.Replace(">", "&gt;");
                    expression = builder.ToString();
                }

                context.Write(node, expression);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // ibatrak
        //
        // Неописанная функция
        // &unifor('+3J')
        // Поиск термина и вывод количества ссылок
        // Поведение в целом аналогично &unifor('J'),
        // однако в отличие от &unifor('J') поиск идет
        // не полному совпадению, а по вхождению искомого термина в найденный,
        // отличается способ передачи параметра базы данных
        //

        public static void GetTermCount
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

            string[] parts = StringUtility.SplitString(expression, new[] { ',' }, 2);
            if (parts.Length != 2)
            {
                return;
            }

            IrbisProvider provider = context.Provider;
            string database = parts[0];
            string key = IrbisText.ToUpper(parts[1]).ThrowIfNull();
            if (string.IsNullOrEmpty(database))
            {
                database = provider.Database;
            }

            TermParameters parameters = new TermParameters
            {
                StartTerm = key,
                Database = database,
                NumberOfTerms = 10
            };
            TermInfo[] terms = provider.ReadTerms(parameters);
            if (terms.Length != 0)
            {
                TermInfo info = terms[0];
                if (info.Text.SafeStarts(key))
                {
                    string output = info.Count.ToInvariantString();
                    context.Write(node, output);
                }
            }
        }

        // ================================================================

        //
        // Расформатирует найденные по запросу записи  - &uf('+3S')
        // Вид функции: +3S.
        //
        // Назначение: Расформатирует найденные по запросу записи.
        // Если [количество выводимых записей]=0,
        // то возвращает только количество найденных по запросу документов.
        //
        // Формат (передаваемая строка):
        //
        // +3S[имя базы],[количество выводимых записей],[ограничитель][формат][ограничитель],[формат или @имя файла с форматом]
        //

        public static void SearchFormat
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

            IrbisProvider provider = context.Provider;
            TextNavigator navigator = new TextNavigator(expression);
            string database = navigator.ReadUntil(',');
            if (navigator.ReadChar() != ',')
            {
                return;
            }

            if (string.IsNullOrEmpty(database))
            {
                database = provider.Database;
            }

            int count = navigator.ReadUntil(',').SafeToInt32();
            if (navigator.ReadChar() != ',')
            {
                return;
            }

            char separator = navigator.ReadChar();
            if (separator == '\0')
            {
                return;
            }

            string searchExpression = navigator.ReadUntil(separator);
            if (navigator.ReadChar() != separator)
            {
                return;
            }

            if (count != 0 && navigator.ReadChar() != ',')
            {
                return;
            }

            string format = navigator.GetRemainingText() ?? string.Empty;
            format = format.Trim();
            if (string.IsNullOrEmpty(format) && count != 0)
            {
                return;
            }

            if (format.StartsWith("@"))
            {
                string fileName = format.Substring(1);
                string extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension))
                {
                    fileName += ".pft";
                }
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        database,
                        fileName
                    );
                format = provider.ReadFile(specification);
                if (string.IsNullOrEmpty(format))
                {
                    return;
                }
            }

            string saveDatabase = provider.Database;
            try
            {
                if (!string.IsNullOrEmpty(database))
                {
                    provider.Database = database;
                }

                int[] found = provider.Search(searchExpression);
                if (count == 0)
                {
                    context.Write(node, found.Length.ToInvariantString());
                    context.OutputFlag = true;

                    return;
                }

                if (found.Length == 0)
                {
                    return;
                }

                PftProgram program = new PftProgram();
                try
                {
                    // TODO some caching

                    if (count != 0)
                    {
                        program = PftUtility.CompileProgram(format);
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException("UniforPlus3::SearchFormat", exception);

                    return;
                }

                if (count < 0)
                {
                    Array.Reverse(found);
                    count = -count;
                }

                for (int i = 0; i < count; i++)
                {
                    using (PftContextGuard guard = new PftContextGuard(context))
                    {
                        PftContext nestedContext = guard.ChildContext;
                        nestedContext.Reset();
                        nestedContext.Output = context.Output;

                        int mfn = found[i];
                        MarcRecord record = provider.ReadRecord(mfn);
                        if (!ReferenceEquals(record, null))
                        {
                            nestedContext.Record = record;
                            program.Execute(nestedContext);
                        }
                    }
                }
            }
            finally
            {
                provider.Database = saveDatabase;
            }
        }

        // ================================================================

        //
        // Вывод количества документов, найденных во внешней базе
        // по команде G.
        // (команда возвращает строку RESULT=[кол-во найденных
        // по запросу документов]) – &uf('+3G')
        //
        // Вид функции: +3G.
        //
        // Назначение: Вывод количества документов, найденных
        // во внешней базе по команде G.
        // (команда возвращает строку RESULT=[кол-во найденных
        // по запросу документов]).
        //
        // Формат (передаваемая строка):
        //
        // +3G[URL к внешнему сайту WEB ИРБИС, с запросом G]
        //

        /// <summary>
        /// ibatrak Поиск количества терминов во внешней базе
        /// </summary>
        public static void GetExternalDbTermRecordCount
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                context.Write(node, "0");
                context.OutputFlag = true;

                return;
            }

            string content = string.Empty;
            try
            {
#if WINMOBILE || PocketPC

                // TODO implement

#elif UAP

                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                content = client.GetStringAsync(expression).Result;

#else

                System.Net.WebClient client = new System.Net.WebClient();
                content = client.DownloadString(expression);

#endif
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "UniforPlus3::GetExternalDbTermRecordCount",
                        exception
                    );
            }

            Match match = Regex.Match(content, "RESULT=([0-9]+)");
            int count = 0;
            if (match.Success)
            {
                count = match.Groups[1].Value.SafeToInt32();
            }

            context.Write(node, count.ToInvariantString());
            context.OutputFlag = true;
        }

        // ================================================================

        //
        // ibatrak
        //
        // Функция введена для оптимизации скорости проверки наличия
        // текста в полнотекстовой базе данных – &uf('+3C')
        // Вид функции: +3C.
        //
        // Назначение: Функция предназначена для обрезания перед
        // помещением в словарь базы данных значения, формируемого
        // путём соединения префикса TXT= и содержимого подполя
        // ^B ссылки на текстовый файл полнотекстовой базы.
        // Подробнее см. в подразделе Префикс TXT статьи Схема полнотекстовой базы данных.
        //
        // Присутствует в версиях ИРБИС с 2013.1.
        //
        // http://wiki.elnit.org/index.php/%D0%A1%D1%85%D0%B5%D0%BC%D0%B0_%D0%BF%D0%BE%D0%BB%D0%BD%D0%BE%D1%82%D0%B5%D0%BA%D1%81%D1%82%D0%BE%D0%B2%D0%BE%D0%B9_%D0%B1%D0%B0%D0%B7%D1%8B_%D0%B4%D0%B0%D0%BD%D0%BD%D1%8B%D1%85#.D0.9F.D1.80.D0.B5.D1.84.D0.B8.D0.BA.D1.81_TXT
        //
        // Префикс TXT=
        // Термин словаря, начинающийся с префикса TXT=, предназначен
        // для поиска записи(ей) базы данных, соответствующей(их)
        // известной ссылке на текст.
        //
        // За префиксом TXT= следует:
        //
        // Значение подполя ^B — в том случае, если термин
        // (вместе с префиксом) не превышает определённое количество
        // символов (250 символов).
        // Определённым образом укороченное значение подполя ^B
        // - в том случае, если термин превышает указанное количество символов.
        // Вторая часть правила позволяет уменьшить необходимость
        // перебора терминов словаря при поиске записей по ссылке на 
        // конкретную страницу текста в случае длинных путей и имён
        // файлов (таких, что длина термина превышает 250 символов). 
        // Укорочение происходит таким образом, чтобы в поисковом
        // термине фигурировал номер страницы.
        //

        /// <summary>
        /// ibatrak Усечение имени файла для префикса TXT= полнотекстового поиска
        /// </summary>
        public static void TruncateFullTextFileName
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

            string path = expression;
            if (path.Length > 249)
            {
                Regex pageNumRegex = new Regex("__.*$");
                Match m = pageNumRegex.Match(path);
                if (m.Success)
                {
                    path = path.Substring(0, 249 - m.Length) + m.Value;
                }
                else if (path.Length > 253)
                {
                    path = expression.Substring(0, 253);
                }
            }

            context.Write(node, path);
            context.OutputFlag = true;
        }

        // ================================================================

        //
        // ibatrak
        //
        // Неописанная функция
        // &unifor ('+3T')
        //
        // Делит строку на 2 фрагмента по символу запятой,
        // разбирает как double, делит, возвращает целую часть результата
        //

        /// <summary>
        /// ibatrak Усечение имени файла для префикса TXT= полнотекстового поиска
        /// </summary>
        public static void Divide
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                context.Write(node, "0");
                context.OutputFlag = true;

                return;
            }

            char[] separators = { ',' };
            string[] parts = StringUtility.SplitString(expression, separators, 2);
            if (parts.Length == 1)
            {
                context.Write(node, "0");
                context.OutputFlag = true;

                return;
            }

            double dividend = parts[0].SafeToDouble(0.0);
            double divisor;
            if (!NumericUtility.TryParseDouble(parts[1], out divisor))
            {
                return;
            }

            try
            {
                double result = Math.Truncate(dividend / divisor);
                if (!double.IsInfinity(result) && !double.IsNaN(result))
                {
                    context.Write(node, result.ToInvariantString());
                    context.OutputFlag = true;
                }
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "UniforPlus3::Divide",
                        exception
                    );
            }
        }

    }

    #endregion
}
