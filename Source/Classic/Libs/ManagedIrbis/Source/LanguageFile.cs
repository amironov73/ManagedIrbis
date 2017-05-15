// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LanguageFile.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 * TODO use case-sensitive dictionary?
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Collections;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    //
    // Лингвистические файлы расположены в директории
    // cgi\irbis64r_XX, рисунки с изображениями флажков
    // в папке htdocs\irbis64r_XX\images\flags.
    // Изображения должны иметь расширения PNG,
    // лингвистические файлы — LNG. Имена файлов
    // и изображений должны соответствовать кодам языков
    // в файле lng.mnu.
    //
    // Лингвистический файл аналогичен по структуре
    // файлу справочника, но имеет кодировку UTF8
    // и состоит из парных строк:
    // строка текста на русском языке
    // строка текста на национальном языке
    //
    // Для нормальной работы системы такая структура
    // файла должна неукоснительно соблюдаться.
    // При отсутствии перевода после строки
    // на русском языке необходимо обязательно оставлять
    // пустую строку.
    //
    // Все русскоязычные литералы (любые текстовые строки
    // на русском) в фреймах, форматах и MNU файлах
    // WEB ИРБИС обрамлены двойными тильдами
    // (например: ~~Русский язык~~). Тильды определят
    // фрагмент текста как потенциальную константу
    // для замены.При переключении на альтернативный
    // язык интерфейса, наличии лингвистического файла
    // и перевода этот литерал заменяется на национальный
    // аналог. В противном случае тильды удаляются шлюзом,
    // и литерал выводится без изменений.
    //

    //
    // Example: uk.lng
    //
    // Абхазский
    // Абхазька
    // Август
    // Серпень
    // Австралия
    // Австралія
    // Австрия
    // Австрія
    // Автор(ы)
    // Автор(и)
    // Автор, Вид издания,
    // Автор, Різновид видання,
    // Автор, редактор, составитель
    // Автор, редактор, упорядник
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LanguageFile
    {
        #region Properties

        /// <summary>
        /// Name of the file.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LanguageFile()
        {
            _dictionary = new CaseInsensitiveDictionary<string>();
        }

        #endregion

        #region Private members

        private readonly CaseInsensitiveDictionary<string> _dictionary;

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the dictionary.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Get translation for the text.
        /// </summary>
        [NotNull]
        public string GetTranslation
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            string result;
            if (!_dictionary.TryGetValue(text, out result))
            {
                result = text;
            }

            return result;
        }

        /// <summary>
        /// Read the dictionary from the <see cref="TextReader"/>.
        /// </summary>
        public void ReadFrom
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            string firstLine;
            while ((firstLine = reader.ReadLine()) != null)
            {
                string secondLine;
                if ((secondLine = reader.ReadLine()) == null)
                {
                    break;
                }

                _dictionary[firstLine] = secondLine;
            }
        }

        /// <summary>
        /// Read local file.
        /// </summary>
        [NotNull]
        public static LanguageFile ReadLocalFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            LanguageFile result = new LanguageFile
            {
                Name = fileName
            };

            using (StreamReader reader
                = TextReaderUtility.OpenRead
                    (
                        fileName,
                        IrbisEncoding.Utf8
                    ))
            {
                result.ReadFrom(reader);
            }

            return result;
        }

        /// <summary>
        /// Write the dictionary to the <see cref="TextWriter"/>.
        /// </summary>
        public void WriteTo
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            foreach (var pair in _dictionary.OrderBy(p => p.Key))
            {
                writer.WriteLine(pair.Key);
                writer.WriteLine(pair.Value);
            }
        }

        /// <summary>
        /// Write the dictionary to the file.
        /// </summary>
        public void WriteLocalFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (TextWriter writer = TextWriterUtility.OpenWrite
                (
                    fileName,
                    IrbisEncoding.Utf8
                ))
            {
                WriteTo(writer);
            }
        }

        #endregion
    }
}
