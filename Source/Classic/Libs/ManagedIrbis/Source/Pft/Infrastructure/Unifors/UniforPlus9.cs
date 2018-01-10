// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus9.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.Logging;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Группа технических форматных выходов
    //

    static class UniforPlus9
    {
        #region Private members

        private static bool _CheckUrlExist
        (
            [NotNull] string address
        )
        {
#if CLASSIC || DESKTOP

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                try
                {
                    client.DownloadString(address);

                    return true;
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "UniforPlus9::_CheckUrlExist",
                            exception
                        );
                }
            }
#elif UAP

            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                try
                {
                    string text = client.GetStringAsync(address).Result;

                    return !ReferenceEquals(text, null);
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "UniforPlus9::_CheckUrlExist",
                            exception
                        );
                }
            }

#endif

            return false;
        }

        [CanBeNull]
        private static FileSpecification _GetFileSpecification
            (
                [NotNull] string expression
            )
        {
            TextNavigator navigator = new TextNavigator(expression);
            string pathText = navigator.ReadUntil(',');
            navigator.ReadChar();
            string dbName = null;
            if (pathText == "0"
                || pathText == "1"
                || pathText == "11")
            {
                if (navigator.PeekChar() == ',')
                {
                    navigator.ReadChar();
                }
            }
            else
            {
                dbName = navigator.ReadUntil(',');
                navigator.ReadChar();
            }

            string fileName = navigator.GetRemainingText();
            if (string.IsNullOrEmpty(pathText)
                || string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            IrbisPath path = (IrbisPath)NumericUtility.ParseInt32
            (
                pathText
            );
            FileSpecification result = new FileSpecification
            (
                path,
                dbName,
                fileName
            );

            return result;
        }

        #endregion

        #region Public methods

        // ================================================================

        //
        // Вернуть ANSI-символ с заданным кодом – &uf('+9F')
        // Вид функции: +9F.
        // Назначение: Вернуть ANSI-символ с заданным кодом.
        // Присутствует в версиях ИРБИС с 2008.1.
        // Формат (передаваемая строка):
        // +9F<код>
        //
        // Примеры:
        //
        // Такой форматный выход может пригодиться, например,
        // когда надо вывести в литерале символ, совпадающий
        // с ограничителями литерала.
        //
        // Для формата
        //
        // '11111',&Uf('+9F39'),'22222'
        //
        // результат расформатирования будет
        //
        // 11111'22222
        //

        /// <summary>
        /// Get character with given code.
        /// </summary>
        public static void GetCharacter
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                int code;
                if (NumericUtility.TryParseInt32(expression, out code))
                {
                    string output = ((char)code).ToString();
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Вернуть путь из заданного полного пути/имени – &uf('+92')
        // Вид функции: +92.
        // Назначение: Вернуть путь из заданного полного пути/имени.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +92
        //

        /// <summary>
        /// Get directory name from full path.
        /// </summary>
        public static void GetDirectoryName
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = Path.GetDirectoryName(expression);
                if (!string.IsNullOrEmpty(output))
                {
                    if (!output.EndsWith
                        (
                            Path.DirectorySeparatorChar.ToString()
                        ))
                    {
                        output += Path.DirectorySeparatorChar;
                    }

                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Вернуть имя диска из заданного полного пути/имени – &uf('+94')
        // Вид функции: +94.
        // Назначение: Вернуть имя диска из заданного полного пути/имени.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +94
        //

        /// <summary>
        /// Get drive name from full path.
        /// </summary>
        public static void GetDrive
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                Match match = Regex.Match(expression, "^[A-Za-z]:");
                string output = match.Value;
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Вернуть расширение из заданного полного пути/имени – &uf('+93')
        // Вид функции: +93.
        // Назначение: Вернуть расширение из заданного полного пути/имени.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +93
        //

        /// <summary>
        /// Get extension from full path.
        /// </summary>
        public static void GetExtension
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = Path.GetExtension(expression);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Вставить данные из заданного текстового файла – &uf('+9C')
        // Вид функции: +9C.
        // Назначение: Вставить данные из заданного текстового файла.
        // Формат (передаваемая строка):
        // +9С<path>,<dbname>,<filename>
        // где:
        // <path> – определяет путь к файлу и принимает значения:
        // 0 – основная директория системы (для ИРБИС32 – та,
        // где находятся исполняемые модули; для ИРБИС64 – та,
        // где находятся исполняемые модули сервера);
        // 1 – общая директория баз данных(по умолчанию \DATAI);
        // 10 – директория конкретной БД;
        // <dbname> – имя БД (имеет смысл только при path= 10).
        // По умолчанию – предполагается текущая БД;
        // <filename> – имя файла;
        //

        /// <summary>
        /// UNIFOR('+9C'): Get file content.
        /// </summary>
        public static void GetFileContent
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                FileSpecification specification
                    = _GetFileSpecification(expression);
                if (!ReferenceEquals(specification, null))
                {
                    string content = context.Provider
                        .ReadFile(specification);
                    if (!string.IsNullOrEmpty(content))
                    {
                        context.Write(node, content);
                        context.OutputFlag = true;
                    }
                }
            }
        }

        // ================================================================

        //
        // Проверить наличие файла/корректность URL – &uf('+9L')
        // Вид функции: +9L.
        // Назначение: Проверить наличие файла/корректность URL.
        // Присутствует в версиях ИРБИС с 2013.1.
        // Формат (передаваемая строка):
        // +9L<path>,<dbname>,<filename>
        // где:
        // <path> – определяет путь к файлу и принимает значения:
        // 0 – папка, в которой установлена серверная часть ИРБИС
        // (<IRBIS_SERVER_ROOT>); 1 – общая директория баз данных
        // (по умолчанию <IRBIS_SERVER_ROOT>\DATAI);
        // 2, 3, 10 – папка БД<dbname>; 11 – абсолютный путь/URL.
        // <dbname> – имя БД(имеет смысл только при path= 2, 3, 10).
        // <filename> – путь и имя файла или URL.
        // Функция возвращает: 0 – если файл отсутствует/некорректный URL;
        // 1 – если файл присутствует/корректный URL.
        // Примеры:
        // &uf('+9L1,,\deposit\rksu.fst')
        // (&uf('+9L10,',&uf('+D'),',',v951^A))
        //

        /// <summary>
        /// UNIFOR('+9L'): check whether the file exist
        /// </summary>
        public static void FileExist
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                FileSpecification specification
                    = _GetFileSpecification(expression);
                if (!ReferenceEquals(specification, null))
                {
                    bool result;

                    string fileName = specification.FileName
                        .ThrowIfNull("specification.FileName");
                    if (fileName.StartsWith("http:")
                        || fileName.StartsWith("https:")
                        || fileName.StartsWith("ftp:"))
                    {
                        result = _CheckUrlExist(fileName);
                    }
                    else
                    {
                        result = context.Provider
                            .FileExist(specification);
                    }

                    string output = result ? "1" : "0";
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Вернуть имя файла из заданного полного пути/имени – &uf('+91')
        // Вид функции: +91.
        // Назначение: Вернуть имя файла из заданного полного пути/имени.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +91
        //

        /// <summary>
        /// Get file name from full path.
        /// </summary>
        public static void GetFileName
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = Path.GetFileName(expression);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Вернуть размер файла в байтах. – &uf('+9A')
        // Вид функции: +9A.
        // Назначение: Вернуть размер файла в байтах.
        // Формат (передаваемая строка):
        // +9A
        //

        /// <summary>
        /// UNIFOR('+9A'):  Get file size.
        /// </summary>
        public static void GetFileSize
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                long fileSize = 0;

                try
                {
                    FileInfo fileInfo = new FileInfo(expression);
                    fileSize = fileInfo.Length;
                }
                catch (Exception exception)
                {
                    Log.TraceException
                    (
                        "UniforPlus9::GetFileSize",
                        exception
                    );
                }

                string output = fileSize.ToInvariantString();
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Вернуть номер текущего повторения в повторяющейся группе – &uf('+90')
        // Вид функции: +90.
        // Назначение: Вернуть номер текущего повторения в повторяющейся группе.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +90
        //

        /// <summary>
        /// Get field repeat.
        /// </summary>
        public static void GetIndex
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            int index = context.Index;
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                index++;
            }

            string text = index.ToInvariantString();
            context.Write(node, text);
            context.OutputFlag = true;
        }

        // ================================================================

        //
        // Вернуть номер поколения ИРБИС – &uf('+9V')
        // Вид функции: +9V.
        // Назначение: Возвращает поколение системы, в которой
        // осуществляется расформатирование. Может быть полезен при
        // разработке единых форматов, которые по-разному выполняются
        // в ИРБИС 32 и ИРБИС 64.
        // Присутствует в версиях ИРБИС с 2011.1.
        // Формат (передаваемая строка):
        // +9V
        // который возвращает:
        // 32 – если форматирование выполняется в ИРБИС 32;
        // 64 – если в ИРБИС 64 (и ИРБИС 128).
        //

        /// <summary>
        /// Get IRBIS generation (family): 32 or 64
        /// </summary>
        public static void GetGeneration
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            context.Write
                (
                    node,
                    context.Provider.GetGeneration()
                );
            context.OutputFlag = true;
        }

        // ================================================================

        //
        // Вернуть длину исходной строки – &uf('+95')
        // Вид функции: +95.
        // Назначение: Вернуть длину исходной строки.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +95<строка>
        //

        /// <summary>
        /// Get string length.
        /// </summary>
        public static void StringLength
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            string output = "0";
            if (!string.IsNullOrEmpty(expression))
            {
                output = expression.Length.ToInvariantString();
            }

            if (!string.IsNullOrEmpty(output))
            {
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Заменить в заданной строке один символ на другой – &uf('+98')
        // Вид функции: +98.
        // Назначение: Заменить в заданной строке один символ
        // на другой (регистр учитывается).
        // Присутствует в версиях ИРБИС с 2007.2.
        // Формат (передаваемая строка):
        // +98ab<строка>
        // где:
        // a – заменяемый символ;
        // b – заменяющий символ.
        // Примеры:
        // В результате выполнения формата
        // &uf('+98 0', f(1,5,0))
        // получится значение
        // 00001
        //

        /// <summary>
        /// Replace character in text.
        /// </summary>
        public static void ReplaceCharacter
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char first = navigator.ReadChar();
                char second = navigator.ReadChar();
                string text = navigator.GetRemainingText();
                if (!string.IsNullOrEmpty(text))
                {
                    string output = text.Replace(first, second);
                    if (!string.IsNullOrEmpty(output))
                    {
                        context.Write(node, output);
                        context.OutputFlag = true;
                    }
                }
            }
        }

        // ================================================================

        //
        // Заменить в исходных данных некоторую заданную последовательность
        // символов другой заданной последовательностью символов – &uf('+9I')
        // Вид функции: +9I.
        // Назначение: Заменить в исходных данных некоторую заданную
        // последовательность символов другой заданной
        // последовательностью символов.
        // Присутствует в версиях ИРБИС с 2009.1.
        // Формат (передаваемая строка):
        // +9I!AAAA!/BBBB/<данные>
        // где АААА – последовательность символов, подлежащая замене;
        // ВВВВ – заменяющая последовательность символов;
        // символ ! – уникальный разделитель, отсутствующий в строке АААА;
        // символ / – уникальный разделитель, отсутствующий в строке ВВВВ.
        // ВВВВ может быть пустым значением, в этом случае последовательность
        // АААА будет удаляться.Обрабатываются ВСЕ (а не только первое)
        // вхождения АААА в исходные данные. В качестве разделителей можно
        // использовать ТОЛЬКО символы стандартного набора(с кодом менее 128).
        //

        /// <summary>
        /// Replace substring.
        /// </summary>
        public static void ReplaceString
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                char firstDelimiter = navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    return;
                }

                string first = navigator.ReadUntil(firstDelimiter);
                navigator.ReadChar();
                if (navigator.IsEOF
                    || string.IsNullOrEmpty(first))
                {
                    return;
                }

                char secondDelimiter = navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    return;
                }

                string second = navigator.ReadUntil(secondDelimiter);
                if (navigator.ReadChar() != secondDelimiter)
                {
                    return;
                }

                string text = navigator.GetRemainingText();
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                string output = text.Replace(first, second);
                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Преобразовать заданную строку в список слов – &uf('+9G')
        // Вид функции: +9G.
        // Назначение: Преобразовать заданную строку в список слов.
        // Присутствует в версиях ИРБИС с 2008.1.
        // Формат (передаваемая строка):
        // +9G<строка>
        // Границы слов определяются на основе таблицы алфавитных символов.
        //

        /// <summary>
        /// Split text to word array.
        /// </summary>
        public static void SplitWords
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] words = PftUtility.ExtractWords(expression);
                for (int i = 0; i < words.Length; i++)
                {
                    string word = StringUtility.ToUpperInvariant(words[i]);
                    context.WriteLine(node, word);
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Вернуть часть строки – &uf('+96')
        // Вид функции: +96.
        // Назначение: Вернуть часть строки.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +96A*SSS.NNN#<строка>
        // где:
        // A – направление: 0 – с начала строки; 1 – с конца;
        // SSS – смещение;
        // NNN – кол-во символов.
        //
        // Примеры:
        //
        // &uf('+960*0.4#'v100)
        // &uf('+960*5.4#'v100)
        // &uf('+961*0.4#'v100)
        //

        /// <summary>
        /// Extract substring.
        /// </summary>
        public static void Substring
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string text;
                string[] parts = StringUtility.SplitString
                    (
                        expression,
                        new[] { '#' },
                        2
                    );

                if (parts.Length == 2)
                {
                    string prefix = parts[0];
                    text = parts[1];

                    TextNavigator navigator = new TextNavigator(prefix);
                    char direction = navigator.ReadChar();
                    int offset = 0;
                    int length = text.Length;
                    string temp;
                    bool haveLength = false;
                    if (navigator.PeekChar() == '*')
                    {
                        navigator.ReadChar();
                        temp = navigator.ReadInteger();
                        NumericUtility.TryParseInt32(temp, out offset);
                    }

                    if (navigator.PeekChar() == '.')
                    {
                        navigator.ReadChar();
                        temp = navigator.ReadInteger();
                        NumericUtility.TryParseInt32(temp, out length);
                        haveLength = true;
                    }

                    if (direction != '0')
                    {
                        if (!haveLength)
                        {
                            length -= offset;
                            offset = 0;
                        }
                        else
                        {
                            offset = text.Length - offset - length;
                        }
                    }

                    text = PftUtility.SafeSubString(text, offset, length);
                }
                else
                {
                    text = expression.Substring(1);
                }

                string output = text;
                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Вернуть заданную строку в верхнем регистре – &uf('+97')
        // Вид функции: +97.
        // Назначение: Вернуть заданную строку в верхнем регистре.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +97<строка>
        //

        /// <summary>
        /// Convert text to upper case.
        /// </summary>
        public static void ToUpper
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output = StringUtility.ToUpperInvariant(expression);

                context.Write(node, output);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Найти подстроку – &uf('+9S')
        // Вид функции: +9S.
        // Назначение: Возвращает позицию первого символа найденного
        // вхождения подстроки в исходную строку. Считается, что
        // символы в строке нумеруются с 1. Если подстрока не найдена,
        // то возвращает 0. Комментарий: следует отметить, что
        // в функции Вернуть часть строки – &uf('+96 указывают смещение,
        // а не позицию символа.
        // Присутствует в версиях ИРБИС с 2013.1.
        // Формат (передаваемая строка):
        // +9S!подстрока!<исходная_строка>
        // где подстрока – подстрока, которую нужно найти;
        // <исходная_строка> – исходная строка для поиска;
        // символ ! – уникальный разделитель, отсутствующий
        // в искомой подстроке.
        //

        /// <summary>
        /// Возвращает позицию первого символа найденного вхождения подстроки
        /// в исходную строку. Считается, что символы в строке нумеруются с 1.
        /// Если подстрока не найдена, то возвращает 0.
        /// </summary>
        public static void FindSubstring
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                goto NOTFOUND;
            }

            TextNavigator navigator = new TextNavigator(expression);
            char delimiter = navigator.ReadChar();
            string substring = navigator.ReadUntil(delimiter);
            if (string.IsNullOrEmpty(substring)
                || navigator.ReadChar() != delimiter)
            {
                goto NOTFOUND;
            }

            string text = navigator.GetRemainingText();
            if (string.IsNullOrEmpty(text))
            {
                goto NOTFOUND;
            }

            int position = text.IndexOf
            (
                substring,
                StringComparison.CurrentCultureIgnoreCase
            );
            if (position < 0)
            {
                goto NOTFOUND;
            }

            string output = (position + 1).ToInvariantString();
            context.Write(node, output);
            context.OutputFlag = true;

            return;

            NOTFOUND:
            context.Write(node, "0");
            context.OutputFlag = true;
        }

        // ================================================================

        //
        // ibatrak
        //
        // &uf('+9T')
        // Неописанная функция
        // Выводит последовательные числа.
        // Вид: &uf('+9TA/B')
        // где A - начальное число, B - конечное (включая)
        // Выводимые числа выравниваются нулями по правому краю
        // по ширине числа A
        // В числе A лишние символы (пробелы и не-цифры) игнорируются.
        // Если A не удаётся интерпретировать как число,
        // оно считается равным 0.
        // Если слэш и число B отсутствуют, то выводится лишь A.
        // Если слэш присутствует и B не удаётся интерпретировать
        // как число, оно считается равным 0.
        //

        public static void PrintNumbers
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

            if (!expression.Contains("/"))
            {
                context.Write(node, expression);
                context.OutputFlag = true;

                return;
            }

            char[] separators = { '/' };
            string[] parts = StringUtility.SplitString(expression, separators, 2);
            string left = parts[0], right = parts[1];
            int width = left.Length;
            string digits = Regex.Match(left, "-?\\d+").Value;
            long start = digits.SafeToInt64(), stop = right.SafeToInt64();
            bool first = true;
            while (start <= stop)
            {
                if (!first)
                {
                    context.WriteLine(node);
                }

                string text = start.ToInvariantString();
                int length = text.Length;
                if (width < length)
                {
                    text = text.Substring(length - width);
                }
                else if (width > length)
                {
                    text = text.PadLeft(width, '0');
                }

                context.Write(node, text);
                context.OutputFlag = true;

                start++;
                first = false;
            }
        }

        // ================================================================

        struct DecimalSuffix
        {
            public string Suffix { get; set; }
            public double Value { get; set; }
        }

        static readonly DecimalSuffix[] _suffixes =
        {
            new DecimalSuffix{Suffix =  "b", Value = 1E0},
            new DecimalSuffix{Suffix = "Kb", Value = 1E3},
            new DecimalSuffix{Suffix = "Mb", Value = 1E6},
            new DecimalSuffix{Suffix = "Gb", Value = 1E9},
            new DecimalSuffix{Suffix = "Tb", Value = 1E12},
            new DecimalSuffix{Suffix = "Pb", Value = 1E15},
            new DecimalSuffix{Suffix = "Eb", Value = 1E18},
            new DecimalSuffix{Suffix = "Zb", Value = 1E21},
            new DecimalSuffix{Suffix = "Yb", Value = 1E24},
        };

        //
        // ibatrak
        //
        // &uf('+9E')
        // Неописанная функция
        // Форматирует переданное число как размер файла.
        // Вид: &uf('+9EN')
        // где N - размер файла.
        // Если N < 1000, выводится суффикс b,
        // иначе, если N < 1000000, выводится суффкис Kb,
        // и так далее.
        //

        public static void FormatFileSize
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            string result = "0";

            // irbis64 использует int32, все что больше становится 0

            if (string.IsNullOrEmpty(expression))
            {
                goto DONE;
            }

            double size = expression.SafeToDouble(0.0);
            if (size <= 0)
            {
                goto DONE;
            }

            for (int i = _suffixes.Length - 1; i >= 0; i--)
            {
                DecimalSuffix entry = _suffixes[i];
                if (size >= entry.Value)
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (entry.Value == 1.0)
                    {
                        result = string.Format
                            (
                                CultureInfo.InvariantCulture,
                                "{0} {1}",
                                size,
                                entry.Suffix
                            );
                    }
                    else
                    {
                        result = string.Format
                            (
                                CultureInfo.InvariantCulture,
                                "{0:F3} {1}",
                                size / entry.Value,
                                entry.Suffix
                            );
                    }

                    break;
                }
            }

            DONE:
            context.Write(node, result);
            context.OutputFlag = true;
        }

        // ================================================================

        //
        // Удалить заданный файл – &uf('+9K')
        // Вид функции: +9K.
        //
        // Назначение: Удалить заданный файл. Если имя файла задано
        // в виде маски, то удаляться будут все соответствующие
        // маске файлы.
        //
        // Присутствует в версиях ИРБИС с 2010.1.
        //
        // Формат (передаваемая строка):
        //
        // +9K<полный путь и имя файла>
        //

        public static void DeleteFiles
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

            string directoryName = Path.GetDirectoryName(expression);
            if (string.IsNullOrEmpty(directoryName))
            {
                return;
            }

            string[] files = Directory.GetFiles
                (
                    directoryName,
                    Path.GetFileName(expression)
                );
            foreach (string oneFile in files)
            {
                File.Delete(oneFile);
            }
        }

        // ================================================================

        //
        // Сохранить заданный внутренний двоичный объект в заданном файле - &uf('+9D')
        // Вид функции: +9D.
        //
        // Назначение: Сохранить заданный внутренний двоичный объект в заданном файле.
        //
        // Присутствует в версиях ИРБИС с 2006.2.
        //
        // Формат (передаваемая строка):
        //
        // +9DNN#<полный путь>
        // где NN – номер внутреннего двоичного объекта.
        //

        public static void SaveBinaryResource
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

            char[] separators = {'#'};
            string[] parts = StringUtility.SplitString(expression, separators, 2);
            if (parts.Length != 2)
            {
                return;
            }

            int occurrence = parts[0].SafeToInt32();
            string path = parts[1];
            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null) && occurrence > 0)
            {
                // TODO implement
                // var tag = Irbis64Config.IniParam("MAIN", "TAGINTERNALRESOURCE", "953").SafeParseInt32();
                int tag = 953;

                RecordField field = record.Fields.GetField(tag, occurrence - 1);
                if (!ReferenceEquals(field, null))
                {
                    BinaryResource resource = BinaryResource.Parse(field);
                    string format = resource.Kind;
                    string content = resource.Resource;
                    if (!ReferenceEquals(content, null))
                    {
                        if (!string.IsNullOrEmpty(format)
                            && string.IsNullOrEmpty(Path.GetExtension(path)))
                        {
                            path += "." + format;
                        }

                        byte[] bytes = resource.Decode();
                        File.WriteAllBytes(path, bytes);
                    }
                }
            }
        }

        // ================================================================

        //
        // Представить заданный двоичный файл в кодированном виде – &uf('+9J')
        // Вид функции: +9J.
        //
        // Назначение: Представить заданный двоичный файл в виде:
        // ^A<тип_файла>^B<данные файла перекодированные на основе URLEncode>.
        //
        // Присутствует в версиях ИРБИС с 2010.1.
        //
        // Формат (передаваемая строка):
        // 
        // +9J<полный путь и имя файла>
        //
        // Имя файла может задаваться в виде маски, в этом случае использоваться
        // будет первый найденный соответствующей маске файл.
        //

        /// <summary>
        /// Чтение файла как двоичного ресурса.
        /// </summary>
        public static void ReadFileAsBinaryResource
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

            string path = Path.GetDirectoryName(expression);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            string name = Path.GetFileName(expression);
            string[] files = Directory.GetFiles(path, name);
            if (files.Length == 0)
            {
                return;
            }
            string file = files[0];
            string format = Path.GetExtension(file).TrimStart('.');
            byte[] content;
            try
            {
                content = File.ReadAllBytes(file);
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "UniforPlus9::ReadFileAsBinaryResource",
                        exception
                    );

                return;
            }

            BinaryResource resource = new BinaryResource
            {
                Kind = format
            };
            resource.Resource = resource.Encode(content);
            RecordField field = resource.ToField();
            string output = field.ToText();
            context.Write(node, output);
            context.OutputFlag = true;
        }

        // ================================================================

        //
        // ibatrak
        //
        // Неописанная функция unifor('+9H')
        // Параметры
        // +9H!строка А!строка Б
        // символ ! - разделитель
        // Если строка Б не пустая возвращает конкатенацию строк А и Б
        // иначе пустую строку
        //

        /// <summary>
        /// Условная конкатенация строк.
        /// </summary>
        public static void ConcatenateStrings
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression)
                || expression.Length < 3)
            {
                return;
            }

            char separator = expression[0];
            int position = expression.IndexOf(separator, 1);
            if (position < 0)
            {
                return;
            }

            string first = expression.Substring(1, position - 1);
            string second = expression.Substring(position + 1);
            if (string.IsNullOrEmpty(second))
            {
                return;
            }

            string output = first + second;
            context.Write(node, output);
            context.OutputFlag = true;
        }

        // ================================================================

        //
        // Групповая установка глобальных переменных – &uf('+99')
        // Вид функции: +99.
        //
        // Назначение: Групповая установка глобальных переменных.
        // Применяется для ИРБИС-Навигатора.
        // Исходная строка закодирована с помощью URL-кодировки.
        // После раскодировки рассматривается как список строк
        // (аналогично TStringList.Text в Delphi).
        // Каждая строка этого списка имеет структуру:
        // NNN#<значение_глобальной_переменной_NNN>.
        //
        // Присутствует в версиях ИРБИС с 2006.1.
        //
        // Формат (передаваемая строка):
        //
        // +99
        //

        public static void AssignGlobals
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                PftGlobalManager globals = context.Globals;
                globals.Clear(); // ???
                string decoded = StringUtility
                    .UrlDecode(expression, IrbisEncoding.Utf8)
                    .ThrowIfNull();
                string[] lines = decoded.SplitLines();
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    RecordField field = RecordField.Parse(line).ThrowIfNull();
                    globals.Append(field.Tag, field.ToText());
                }
            }
        }

        #endregion
    }
}
