// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisOpt.cs -- .OPT files handling
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
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

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    // Оптимизированный формат – это механизм автоматического
    // переключения форматов показа документов в зависимости
    // от их вида. Переключение производится в соответствии
    // с содержанием специального файла, имя которого
    // определяется параметром PFTOPT. Данный файл оптимизации
    // является текстовым и имеет следующую структуру:
    //
    // <метка>|<формат>|@<имя_формата>
    // <длина>
    // <значение_1> <имя формата_1>
    // <значение_2> <имя формата_2>
    // <значение_3> <имя формата_3>
    // *****
    //
    // где:
    // <метка>|<формат>|@<имя_формата> - ключ, который может
    // задаваться тремя способами:
    // <метка> - метка поля, значение которого определяет
    // вид документа;
    // <формат> - непосредственный формат, с помощью которого
    // определяется значение для вида документа;
    // @<имя_формата> - имя формата с предшествующим символом @,
    // с помощью которого определяется значение для вида документа.
    // <длина> - макс.длина значения для вида документа;
    // <значение_n> <имя формата_n> - значение (вид документа)
    // и соответствующий ему формат, разделенные символом пробела.
    // При этом в элементе <значение_n> могут содержаться символы
    // маскирования «+» (означающие, что на соответствующем месте
    // может быть любой символ).
    // Для БД электронного каталога (IBIS) предлагаются два
    // оптимизационных файла:
    // PFTW.OPT – включает RTF-форматы;
    // PFTW_H.OPT – включает HTML-форматы.

    // В исходном состоянии системы в качестве оптимизированного
    // определены HTML-форматы (т.е. PFTOPT=PFTW_H.OPT).
    // Для перехода на RTF-форматы (в качестве оптимизированного)
    // необходимо установить PFTOPT=PFTW.OPT.


    // 920
    // 5
    // PAZK  PAZK42
    // PVK   PVK42
    // SPEC  SPEC42
    // J     !RPJ51
    // NJ    !NJ31
    // NJP   !NJ31
    // NJK   !NJ31
    // AUNTD AUNTD42
    // ASP   ASP42
    // MUSP  MUSP
    // SZPRF SZPRF
    // BOUNI BOUNI
    // IBIS  IBIS
    // +++++ PAZK42
    // *****


    /// <summary>
    /// OPT files handling
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisOpt
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Подстановочный символ.
        /// </summary>
        public const char Wildcard = '+';

        #endregion

        #region Nested classes

        /// <summary>
        /// Элемент словаря.
        /// </summary>
        [DebuggerDisplay("{Key} {Value}")]
        public sealed class Item
            : IHandmadeSerializable
        {
            #region Properties

            /// <summary>
            /// Ключ.
            /// </summary>
            [NotNull]
            public string Key { get; set; }

            /// <summary>
            /// Значение.
            /// </summary>
            [NotNull]
            public string Value { get; set; }

            #endregion

            #region Private members

            #endregion

            #region Public methods

            /// <summary>
            /// Сравнение строки с ключом.
            /// </summary>
            public bool Compare
                (
                    [CanBeNull] string text
                )
            {
                return CompareString(Key, text);
            }

            /// <summary>
            /// Разбор строки.
            /// </summary>
            [CanBeNull]
            public static Item Parse
                (
                    [NotNull] string line
                )
            {
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }
                char[] separator = { ' ' };

#if !WINMOBILE && !PocketPC

                string[] parts = line.Split
                    (
                        separator,
                        StringSplitOptions.RemoveEmptyEntries
                    );

#else

                // TODO Implement it properly

                string[] parts = line.Split(separator);

#endif

                if (parts.Length != 2)
                {
                    return null;
                }

                Item result = new Item
                {
                    Key = parts[0],
                    Value = parts[1]
                };

                return result;
            }

            #endregion

            #region IHandmadeSerializable

            /// <inheritdoc />
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                Key = reader.ReadString();
                Value = reader.ReadString();
            }

            /// <inheritdoc />
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                writer.Write(Key);
                writer.Write(Value);
            }

            #endregion

            #region Object members

            /// <inheritdoc />
            public override string ToString()
            {
                return string.Format
                    (
                        "{0} {1}",
                        Key,
                        Value
                    );
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Элементы списка.
        /// </summary>
        public NonNullCollection<Item> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Length of worksheet.
        /// </summary>
        public int WorksheetLength { get; private set; }

        /// <summary>
        /// Tag that identifies worksheet.
        /// Common used: 920
        /// </summary>
        public string WorksheetTag { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public IrbisOpt()
        {
            _items = new NonNullCollection<Item>();
        }

        #endregion

        #region Private members

        private NonNullCollection<Item> _items;

        #endregion

        #region Public methods

        /// <summary>
        /// Сравнение символов с учётом подстановочного
        /// знака '+'.
        /// </summary>
        public static bool CompareChar
            (
                char left,
                char right
            )
        {
            if (left == Wildcard)
            {
                return true;
            }

#if !WINMOBILE && !PocketPC

            return char.ToUpperInvariant(left)
                   == char.ToUpperInvariant(right);

#else

            return char.ToUpper(left) == char.ToUpper(right);

#endif

        }

        /// <summary>
        /// Сравнение строк с учётом подстановочного знака '+'.
        /// </summary>
        public static bool CompareString
            (
                [NotNull] string left,
                [CanBeNull] string right
            )
        {
            Code.NotNull(left, "left");

            if (string.IsNullOrEmpty(right))
            {
                if (left.ConsistOf(Wildcard))
                {
                    return true;
                }

                return false;
            }

            IEnumerator leftEnumerator = left.ToCharArray().GetEnumerator();
            IEnumerator rightEnumerator = right.ToCharArray().GetEnumerator();

            while (true)
            {
                char leftChar;
                bool leftNext = leftEnumerator.MoveNext();
                bool rightNext = rightEnumerator.MoveNext();

                if (leftNext && !rightNext)
                {
                    leftChar = (char)leftEnumerator.Current;
                    if (leftChar == Wildcard)
                    {
                        while (leftEnumerator.MoveNext())
                        {
                            leftChar = (char)leftEnumerator.Current;
                            if (leftChar != Wildcard)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }

                if (leftNext != rightNext)
                {
                    return false;
                }
                if (!leftNext)
                {
                    return true;
                }

                leftChar = (char)leftEnumerator.Current;
                char rightChar = (char)rightEnumerator.Current;
                if (!CompareChar(leftChar, rightChar))
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Получаем рабочий лист для указанной записи.
        /// </summary>
        [CanBeNull]
        public string GetWorksheet
            (
                [NotNull] MarcRecord record
            )
        {
            return record.FM(WorksheetTag);
        }

        /// <summary>
        /// Загружаем из OPT-файла.
        /// </summary>
        public static IrbisOpt LoadFromOptFile
            (
                [NotNull] string filePath
            )
        {
            Code.NotNullNorEmpty(filePath, "filePath");

            using (StreamReader reader = TextReaderUtility.OpenRead
                    (
                        filePath,
                        IrbisEncoding.Ansi
                    ))
            {
                return ParseText(reader);
            }
        }

        /// <summary>
        /// Разбор текста.
        /// </summary>
        [NotNull]
        public static IrbisOpt ParseText
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            IrbisOpt result = new IrbisOpt();

            result.SetWorksheetTag(reader.RequireLine().Trim());
            result.SetWorksheetLength(int.Parse(reader.RequireLine().Trim()));

            while (true)
            {
                string line = reader.RequireLine().Trim();
                if (line.StartsWith("*"))
                {
                    break;
                }

                Item item = Item.Parse(line);
                result.Items.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Выбор рабочего листа.
        /// </summary>
        [NotNull]
        public string SelectWorksheet
            (
                [CanBeNull] string tagValue
            )
        {
            foreach (Item item in Items)
            {
                if (item.Compare(tagValue))
                {
                    return item.Value;
                }
            }

            Log.Error
                (
                    "IrbisOpt::SelectWorksheet: "
                    + "can't select worksheet for "
                    + tagValue.ToVisibleString()
                );

            throw new IrbisException("Can't select worksheet");
        }

        /// <summary>
        /// Создание OPT-файла по описанию.
        /// </summary>
        public void WriteOptFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (StreamWriter writer = TextWriterUtility.Create
                    (
                        fileName,
                        IrbisEncoding.Ansi
                    ))
            {
                WriteOptFile(writer);
            }
        }

        /// <summary>
        /// Создание OPT-файла по описанию.
        /// </summary>
        public void WriteOptFile
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteLine(WorksheetTag);
            writer.WriteLine(WorksheetLength);
            foreach (Item item in Items)
            {
                writer.WriteLine
                    (
                        "{0} {1}",
                        item.Key.PadRight(WorksheetLength),
                        item.Value
                    );
            }
            writer.WriteLine("*****");
        }

        /// <summary>
        /// Установка длины названия рабочего листа.
        /// </summary>
        public void SetWorksheetLength
            (
                int length
            )
        {
            if (length <= 0)
            {
                Log.Error
                    (
                        "IrbisOpt::SetWorksheetLength: "
                        + "length="
                        + length
                    );

                throw new ArgumentOutOfRangeException("length");
            }

            WorksheetLength = length;
        }

        /// <summary>
        /// Установка поля для рабочего листа.
        /// </summary>
        public void SetWorksheetTag
            (
                [NotNull] string tag
            )
        {
            Code.NotNullNorEmpty(tag, "tag");

            WorksheetTag = tag;
        }

        /// <summary>
        /// Проверка на валидность. OPT должен содержать
        /// одну строку с плюсами. Она должна быть последней.
        /// OPT не должен быть пустым. Длина ключей в элементах
        /// не должна превышать <see cref="WorksheetLength"/>.
        /// Не должно быть одинаковых ключей.
        /// </summary>
        public bool Validate
            (
                bool throwException
            )
        {
            bool result = Items.Count != 0;

            if (result)
            {
                int count = 0;
                foreach (Item item in Items)
                {
                    if (item.Key.ConsistOf(Wildcard))
                    {
                        count++;
                    }
                }

                result = count == 1;
            }

            if (result)
            {
                result = Items.Last().Key.ConsistOf(Wildcard);
            }

            if (result)
            {
                result = Items.All
                    (
                        item => item.Key.Length <= WorksheetLength
                    );
            }

            if (result)
            {
                result = Items
                    .GroupBy(item => item.Key.ToUpper())
                    .Count(grp => grp.Count() > 1)
                    == 0;
            }

            if (!result && throwException)
            {
                throw new IrbisException("OPT not valid");
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            _items = reader.ReadNonNullCollection<Item>();
            WorksheetLength = reader.ReadPackedInt32();
            WorksheetTag = reader.ReadString();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Items);
            writer.WritePackedInt32(WorksheetLength);
            writer.Write(WorksheetTag);
        }

        #endregion

        #region Object members

        #endregion
    }
}

