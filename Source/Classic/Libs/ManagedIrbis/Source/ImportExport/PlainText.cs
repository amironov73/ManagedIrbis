// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlainText.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.ImportExport
{
    //
    // Текстовые файлы документов используются для 
    // импорта/экспорта данных в режимах ИМПОРТ и ЭКСПОРТ
    // АРМов «Каталогизатор» и «Администратор».
    //
    // Структура
    //
    // Структура текстового файла документов удовлетворяет
    // следующим правилам:
    //
    // Документ
    //
    // * каждый документ начинается с новой строки
    // и может занимать произвольное количество строк
    // произвольной длины;
    // один документ от другого отделяется строкой,
    // содержащей в первых позициях символы *****;
    //
    // Поля
    //
    // документ состоит из полей, каждое из которых
    // начинается с новой строки и имеет следующую структуру:
    //
    // #МММ: <данные поля>
    // <данные поля>
    // ................
    //
    // где МММ - числовая метка поля (лидирующие нули
    // можно не указывать);
    // поля внутри документа могут следовать в произвольном
    // порядке, поля с одинаковыми метками могут повторяться;
    //
    // Подполя
    //
    // * данные поля могут содержать подполя, которые
    // начинаются с признака и разделителя подполя, например:
    //
    // ^A<данные подполя>^B<данные подполя>......
    //
    // * подполя с одинаковыми разделителями не могут
    // повторяться внутри поля.
    //

    /// <summary>
    /// Плоское текстовое представление записи,
    /// используемое, например, при импорте-экспорте
    /// в текстовом формате в АРМ Каталогизатор
    /// и Администратор.
    /// </summary>
    public static class PlainText
    {
        #region Private members

        private static string _ReadTo
            (
                TextReader reader,
                char delimiter
            )
        {
            StringBuilder result = new StringBuilder();

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                char c = (char)next;
                if (c == delimiter)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString();
        }

        private static RecordField _ParseLine
            (
                string line
            )
        {
            StringReader reader = new StringReader(line);

            char c = (char) reader.Read();
            if (c != '#')
            {
                Log.Error
                    (
                        "PlainText::_ParseLine: "
                        + "format error: "
                        + line.NullableToVisibleString()
                    );

                throw new IrbisException();
            }
            RecordField result = new RecordField
            {
                Tag = _ReadTo(reader, ':')
            };

            c = (char) reader.Read();
            if (c != ' ')
            {
                Log.Error
                    (
                        "PlainText::_ParseLine: "
                        + "whitespace required: "
                        + line
                    );
                throw new IrbisException();
            }
            result.Value = _ReadTo(reader, '^');

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                char code = char.ToLower((char)next);
                string text = _ReadTo(reader, '^');
                SubField subField = new SubField
                {
                    Code = code,
                    Value = text
                };
                result.SubFields.Add(subField);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение одной записи из потока.
        /// </summary>
        [CanBeNull]
        public static MarcRecord ReadRecord
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            string line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            MarcRecord result = new MarcRecord();

            if (line.StartsWith("*****"))
            {
                Log.Error
                    (
                        "PlainText::ReadRecord: "
                        + "unexpected five stars"
                    );

                throw new IrbisException();
            }

            RecordField field = _ParseLine(line);
            result.Fields.Add(field);

            bool good = false;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("*****"))
                {
                    good = true;
                    break;
                }
                field = _ParseLine(line);
                result.Fields.Add(field);
            }

            if (!good)
            {
                Log.Error
                    (
                        "PlainText::ReadRecord: "
                        + "bad record"
                    );

                throw new IrbisException();
            }

            return result;
        }

#if !WIN81 && !PORTABLE

        /// <summary>
        /// Read one record from local file.
        /// </summary>
        [CanBeNull]
        public static MarcRecord ReadOneRecord
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            using (StreamReader reader = new StreamReader
                (
                    new FileStream
                    (
                        fileName,
                        FileMode.Open,
                        FileAccess.Read
                    ),
                    encoding
                ))
            {
                MarcRecord result = ReadRecord(reader);

                return result;
            }
        }

        /// <summary>
        /// Read some records from local file.
        /// </summary>
        [NotNull]
        public static MarcRecord[] ReadRecords
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            List<MarcRecord> result = new List<MarcRecord>();

            using (StreamReader reader = new StreamReader
                (
                    new FileStream
                    (
                        fileName,
                        FileMode.Open,
                        FileAccess.Read
                    ),
                    encoding
                ))
            {
                MarcRecord record;
                while ((record = ReadRecord(reader)) != null)
                {
                    result.Add(record);
                }
            }

            return result.ToArray();
        }

#endif

        /// <summary>
        /// Формирует плоское текстовое представление записи.
        /// </summary>
        [NotNull]
        public static string ToPlainText
            (
                [NotNull] this MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            StringBuilder result = new StringBuilder();

            foreach (RecordField field in record.Fields)
            {
                result.AppendFormat("{0}#", field.Tag);
                if (!string.IsNullOrEmpty(field.Value))
                {
                    result.Append(field.Value);
                }
                foreach (SubField subField in field.SubFields)
                {
                    result.Append('^');
                    result.Append(subField.Code);
                    result.Append(subField.Value);
                }
                result.AppendLine();
            }

            return result.ToString();
        }

        #endregion
    }
}
