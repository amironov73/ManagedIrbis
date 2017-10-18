// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FstProcessor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fst
{
    /// <summary>
    /// FST processor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FstProcessor
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// FST file.
        /// </summary>
        [NotNull]
        public FstFile File { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FstProcessor
            (
                [NotNull] IrbisProvider provider,
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(specification, "specification");

            Provider = provider;

            string content = Provider.ReadFile(specification);
            if (string.IsNullOrEmpty(content))
            {
                throw new IrbisException();
            }
            StringReader reader = new StringReader(content);
            File = FstFile.ParseStream(reader);
            if (File.Lines.Count == 0)
            {
                throw new IrbisException();
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FstProcessor
            (
                [NotNull] IrbisProvider provider,
                [NotNull] FstFile file
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(file, "file");

            Provider = provider;
            File = file;
            File.Verify(true);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FstProcessor
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
            File.Verify(true);
        }

        #endregion

        #region Private members

        private static readonly char[] _delimiters = { '\r', '\n', '%' };

        private IrbisAlphabetTable _alphabetTable;

        private IrbisStopWords _stopWords;

        private IrbisUpperCaseTable _upperCaseTable;

        [NotNull]
        [ItemNotNull]
        private string[] _BetweenAngles
            (
                [NotNull][ItemNotNull] string[] items
            )
        {
            List<string> result = new List<string>(items.Length);
            foreach (string item in items)
            {
                if (item.Contains("<"))
                {
                    TextNavigator navigator = new TextNavigator(item);
                    while (!navigator.IsEOF)
                    {
                        navigator.ReadUntil('<');
                        if (navigator.ReadChar() == '<')
                        {
                            string text = navigator.ReadUntil('>');
                            if (navigator.ReadChar() == '>'
                                && !string.IsNullOrEmpty(text))
                            {
                                result.Add(text);
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        [NotNull]
        [ItemNotNull]
        private string[] _BetweenSlashes
            (
                [NotNull][ItemNotNull] string[] items
            )
        {
            List<string> result = new List<string>(items.Length);
            foreach (string item in items)
            {
                if (item.Contains("/"))
                {
                    TextNavigator navigator = new TextNavigator(item);
                    while (!navigator.IsEOF)
                    {
                        navigator.ReadUntil('/');
                        if (navigator.ReadChar() == '/')
                        {
                            string text = navigator.ReadUntil('/');
                            if (navigator.ReadChar() == '/'
                                && !string.IsNullOrEmpty(text))
                            {
                                result.Add(text);
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        [NotNull]
        [ItemNotNull]
        private FstTerm[] _GetTerms
            (
                [NotNull] MarcRecord record,
                [NotNull] FstLine line,
                [NotNull][ItemNotNull] string[] items
            )
        {
            //
            // Индексная ссылка имеет следующие 4 компоненты:
            //
            // * MFN записи, содержащей термин. Эта компонента вводится
            // в состав индексной ссылки при актуализации / формировании
            // словаря автоматически.
            //
            // * Идентификатор поля, используемый в процессе поиска
            // при указании квалификатора. Эта компонента вводится
            // в состав индексной ссылки на основе ТВП.
            // Обратите внимание на то, что один и тот же идентификатор
            // поля может быть присвоен различным полям, указанным
            // в формате выборки.
            //
            // * Номер экземпляра (повторения) повторяющегося поля,
            // необходимый для осуществления поиска на уровне поля
            // и операторов близости расположения терминов
            // в повторяющихся полях (в АРМах ИРБИС это используется
            // при поиске по логике «И (в поле)»).
            // Для того, чтобы можно было использовать указанный
            // метод поиска (обычно для этого необходим метод
            // индексирования 4 или 8), необходимо определить формат
            // в ТВП таким, чтобы в его выходных данных между экземплярами
            // повторяющегося поля располагался знак процента(%),
            // для чего нужно задать его в качестве повторяющегося
            // суффикс-литерала.Например, строка ТВП для инвертирования
            // повторяющегося поля 10 должна содержать формат
            // v10 |%|. Система перед обработкой каждой строки ТВП
            // устанавливает номер повторения в 1 и затем увеличивает
            // его на 1 всякий раз, когда в созданном форматом тексте
            // встречается символ %.
            //
            // * Последовательный номер термина, необходимый для
            // осуществления поиска по близости расположения терминов
            // (в АРМах ИРБИС это используется при поиске по логике
            // «И (фраза)»). Управление присвоением данного номера
            // происходит следующим образом: он устанавливается
            // в 1 перед обработкой каждой строки ТВП и при изменении
            // номера повторения и увеличивается на 1 для каждого элемента,
            // созданного указанным методом индексирования.
            // Например, предположим, что в повторяющемся
            // поле 331 содержится краткое содержание литературного
            // источника, причем каждое повторение состоит из одного
            // абзаца. Пусть данное поле проиндексировано методом 4.
            // Если определить формат выборки данных mdl,v331 |%|,
            // то начиная с каждого абзаца краткого содержания словам
            // будет присваиваться последовательный номер, начиная
            // с 1 в каждом абзаце, а если бы формат выборки
            // был равным mdl, v331, то словам присваивался бы сквозной
            // последовательный номер по всему краткому содержанию,
            // например, первое слово второго абзаца имело бы
            // последовательный номер на 1 больше номера последнего
            // слова первого абзаца.
            //

            if (ReferenceEquals(_upperCaseTable, null))
            {
                _upperCaseTable = Provider.GetUpperCaseTable();
            }

            FstTerm[] result = new FstTerm[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                string text = SearchUtility.TrimTerm
                    (
                        _upperCaseTable.ToUpper(items[i])
                    );
                FstTerm link = new FstTerm
                {
                    Mfn = record.Mfn,
                    Tag = line.Tag,
                    Occurrence = i+1,
                    // TODO offset
                    Text = text
                };
                result[i] = link;
            }

            return result;
        }

        [NotNull]
        [ItemNotNull]
        private FstTerm[] _GetTermsWithPrefix
            (
                [NotNull] MarcRecord record,
                [NotNull] FstLine line,
                [NotNull][ItemNotNull] string[] items
            )
        {
            //
            // Присоединяемый префикс определяется в формате выборки
            // данных в виде безусловного литерала и имеет следующий вид:
            //
            // 'dp...pd', [format]
            //
            // где:
            //
            // d - выбранный по усмотрению пользователя ограничитель
            // (который не попадает в текст префикса;
            // p..p - собственно префикс.
            //
            //    Например, строка ТВП
            //
            // 1  8  '/К=/', v200^a
            //
            // приведет к индексированию каждого слова подполя
            // А поля 200 с предварительным присоединением
            // к каждому термину префикса "К=".
            //

            if (items.Length == 0
                || string.IsNullOrEmpty(items[0]))
            {
                return new FstTerm[0];
            }

            TextNavigator navigator = new TextNavigator(items[0]);
            char delimiter = navigator.ReadChar();
            string prefix = navigator.ReadUntil(delimiter);
            if (navigator.ReadChar() != delimiter
                || string.IsNullOrEmpty(prefix))
            {
                return _GetTerms(record, line, items);
            }
            items[0] = navigator.GetRemainingText();
            items = items.NonEmptyLines()
                .Select(item => prefix + item)
                .ToArray();
            return _GetTerms(record, line, items);
        }

        [NotNull]
        [ItemNotNull]
        private string[] _SplitByCaret
            (
                [NotNull][ItemNotNull] string[] items
            )
        {
            List<string> result = new List<string>(items.Length);
            foreach (string item in items)
            {
                if (!item.Contains("^"))
                {
                    result.Add(item);
                }
                else
                {
                    TextNavigator navigator = new TextNavigator(item);
                    while (!navigator.IsEOF)
                    {
                        string text = navigator.ReadUntil('^');
                        if (!string.IsNullOrEmpty(text))
                        {
                            result.Add(text);
                        }
                        navigator.ReadChar();
                        navigator.ReadChar();
                    }
                }
            }

            return result.ToArray();
        }

        [NotNull]
        [ItemNotNull]
        private string[] _SplitToWords
            (
                [NotNull][ItemNotNull] string[] items
            )
        {
            if (ReferenceEquals(_alphabetTable, null))
            {
                _alphabetTable = Provider.GetAlphabetTable();
                _stopWords = Provider.GetStopWords();
            }

            List<string> result = new List<string>(items.Length);
            foreach (string item in items)
            {
                string[] words = _alphabetTable.SplitWords(item);
                foreach (string word in words)
                {
                    if (!_stopWords.IsStopWord(word))
                    {
                        result.Add(word);
                    }
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Extract terms.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public FstTerm[] ExtractTerms
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<FstTerm> result = new List<FstTerm>();
            foreach (FstLine line in File.Lines)
            {
                IPftFormatter formatter = Provider.AcquireFormatter()
                    .ThrowIfNull("formatter");
                formatter.ParseProgram(line.Format.ThrowIfNull("line.Format"));
                string text = formatter.FormatRecord(record);
                Provider.ReleaseFormatter(formatter);
                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }
                text = text.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }
                string[] parts = text.Split(_delimiters);
                List<string> parts2 = new List<string>();
                for (int i = 0; i < parts.Length; i++)
                {
                    string s = parts[i].Trim();
                    if (!string.IsNullOrEmpty(s))
                    {
                        parts2.Add(s);
                    }
                }
                if (parts2.Count == 0)
                {
                    continue;
                }
                parts = parts2.ToArray();

                switch (line.Method)
                {
                    case FstIndexMethod.Method0:

                        // Метод индексирования 0
                        // Создаёт элемент из каждой строки, сформированной
                        // в соответствии с форматом.
                        // Этот метод обычно используется для индексирования
                        // в целом всего поля или подполя.
                        // Следует обратить особое внимание, что система
                        // в данном случае строит элементы из строк,
                        // а не из полей.
                        // В качестве выходного результата форматирования
                        // выступает строка символов, в которой нет никакого
                        // указания на ее принадлежность (или принадлежность
                        // части строки) тому или иному полю или подполю.
                        // Поэтому следует быть особенно внимательным,
                        // чтобы формат порождал корректные данные,
                        // особенно в тех случаях, когда индексируются
                        // повторяющиеся поля и/или более одного поля.
                        // Другими словами, при использовании данного метода,
                        // выводимые в соответствии с форматом отбора данные
                        // должны быть представлены отдельной строкой
                        // для каждого индексируемого элемента.

                        result.AddRange
                            (
                                _GetTerms(record, line, parts)
                            );
                        break;

                    case FstIndexMethod.Method1:

                        // Метод индексирования 1
                        // Создаёт элемент из каждого подполя или строки,
                        // созданных форматом.
                        // Так как в этом случае система будет производить
                        // поиск кодов разделителей подполей в строке,
                        // созданной форматом, то для обеспечения правильной
                        // работы метода в формате должен быть указан
                        // режим проверки mpl (или вообще не указан никакой
                        // режим, так как режим проверки выбирается по умолчанию),
                        // который обеспечивает сохранность разделителей
                        // подполей в выходном результате формата.
                        // Напомним, что режимы заголовка и данных заменяют
                        // разделители подполей на знаки пунктуации.
                        // Отметим, что метод индексирования 1 позволяет
                        // сделать описание более коротким, чем метод индексирования 0.

                        result.AddRange
                            (
                                _GetTerms(record, line, _SplitByCaret(parts))
                            );
                        break;

                    case FstIndexMethod.Method2:

                        // Метод индексирования 2
                        // Создаёт элемент из каждого термина или фразы,
                        // заключенных в угловые скобки (<…>).
                        // Любой текст, расположенный вне скобок, не индексируется. 

                        result.AddRange
                            (
                                _GetTerms(record, line, _BetweenAngles(parts))
                            );
                        break;

                    case FstIndexMethod.Method3:

                        // Метод индексирования 3
                        // Создаёт элемент из каждого термина или фразы,
                        // заключенных в косые черты (/…/).
                        // Во всём остальном он работает точно так же,
                        // как и метод индексирования 2

                        result.AddRange
                            (
                                _GetTerms(record, line, _BetweenSlashes(parts))
                            );
                        break;


                    case FstIndexMethod.Method4:

                        // Метод индексирования 4
                        // Создаёт элемент из каждого слова в тексте,
                        // созданном форматом.
                        // При использовании данного метода для индексации поля,
                        // содержащего разделители подполей, в формате выборки
                        // данных необходимо указать режимы заголовка
                        // или данных (mhl или mdl) с тем, чтобы замена
                        // разделителей подполей произошла до индексации,
                        // так как в противном случае буква разделителя подполей
                        // будет рассматриваться как составная часть слова.

                        result.AddRange
                            (
                                _GetTerms(record, line, _SplitToWords(parts))
                            );

                        break;

                    case FstIndexMethod.Method5:

                        // Аналогично методу 1, но с префиксом

                        result.AddRange
                            (
                                _GetTermsWithPrefix(record, line, _SplitByCaret(parts))
                            );
                        break;

                    case FstIndexMethod.Method6:

                        // Аналогично методу 2, но с префиксом

                        result.AddRange
                            (
                                _GetTermsWithPrefix(record, line, _BetweenAngles(parts))
                            );
                        break;

                    case FstIndexMethod.Method7:

                        // Аналогично методу 3, но с префиксом

                        result.AddRange
                            (
                                _GetTermsWithPrefix(record, line, _BetweenAngles(parts))
                            );
                        break;

                    case FstIndexMethod.Method8:

                        // Аналогично методу 4, но с префиксом

                        result.AddRange
                            (
                                _GetTermsWithPrefix(record, line, _SplitToWords(parts))
                            );
                        break;

                    default:
                        throw new IrbisException();
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Transform record.
        /// </summary>
        [NotNull]
        public MarcRecord TransformRecord
            (
                [NotNull] MarcRecord record,
                [NotNull] string format
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(format, "format");

            string transformed = Provider.FormatRecord
                (
                    record,
                    format
                )
                .ThrowIfNull("Connection.FormatRecord");

            MarcRecord result = new MarcRecord
            {
                Database = record.Database ?? Provider.Database
            };
            string[] lines = transformed.Split((char)0x07);
            foreach (string line in lines)
            {
                string[] parts = line.SplitLines();
                if (parts.Length == 0)
                {
                    continue;
                }
                string tag = parts[0];
                for (int i = 1; i < parts.Length; i++)
                {
                    string body = parts[i];
                    if (string.IsNullOrEmpty(body))
                    {
                        continue;
                    }
                    RecordField field
                        = RecordFieldUtility.Parse(tag, body);

                    SubField[] badSubFields
                        = field.SubFields
                        .Where(sf => string.IsNullOrEmpty(sf.Value))
                        .ToArray();
                    foreach (SubField subField in badSubFields)
                    {
                        field.SubFields.Remove(subField);
                    }

                    if (!string.IsNullOrEmpty(field.Value)
                        || field.SubFields.Count != 0)
                    {
                        result.Fields.Add(field);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Transform the record.
        /// </summary>
        [NotNull]
        public MarcRecord TransformRecord
            (
                [NotNull] MarcRecord record,
                [NotNull] FstFile fstFile
            )
        {
            Code.NotNull(record, "record");
            Code.NotNull(fstFile, "fstFile");

            string format = fstFile.ConcatenateFormat();
            MarcRecord result = TransformRecord
                (
                    record,
                    format
                );

            return result;
        }

        #endregion
    }
}

