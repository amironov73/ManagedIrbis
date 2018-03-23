// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Iso2709.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Iso2709
    {
        #region Constants

        /// <summary>
        /// Marker length.
        /// </summary>
        public const int MarkerLength = 24;

        /// <summary>
        /// Record delimiter.
        /// </summary>
        public const char RecordDelimiter = (char)0x1D;

        /// <summary>
        /// Field delimiter.
        /// </summary>
        public const char FieldDelimiter = (char)0x1E;

        /// <summary>
        /// Subfield delimiter.
        /// </summary>
        public const char SubfieldDelimiter = (char)0x1F;

        #endregion

        #region Private members

        private static int _ToInt
            (
                byte[] bytes,
                int offset,
                int count
            )
        {
            int result = 0;

            for (; count > 0; count--, offset++)
            {
                result = result * 10 + (bytes[offset] - ((byte)'0'));
            }

            return result;
        }

        private static void _Encode(char[] chars, int pos, int len, int val)
        {
            // @@@@@@@@@@@@@
            len--;
            for (pos += len; len >= 0; len--)
            {
                chars[pos] = (char)((val % 10) + ((byte)'0'));
                val /= 10;
                // @@@@@@@@@@@@
                pos--;
            }
        }

        private static int _Encode(char[] chars, int pos, string str)
        {
            if (!ReferenceEquals(chars, null) && !ReferenceEquals(str, null))
            {
                for (int i = 0; i < str.Length; pos++, i++)
                {
                    chars[pos] = str[i];
                }
            }

            return pos;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор 2709.
        /// </summary>
        [CanBeNull]
        public static MarcRecord ReadRecord
            (
                [NotNull] Stream stream,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(encoding, "encoding");

            MarcRecord result = new MarcRecord();

            byte[] marker = new byte[5];

            // Считываем длину записи
            if (stream.Read(marker, 0, 5) != 5)
            {
                return null;
            }

            int recordLength = _ToInt(marker, 0, 5);
            byte[] record = new byte[recordLength];
            int need = recordLength - 5;
            // А затем и ее остаток
            if (stream.Read(record, 5, need) != need)
            {
                return null;
            }

            // простая проверка, что мы имеем дело
            // с нормальной ISO-записью
            if (record[recordLength - 1] != RecordDelimiter)
            {
                return null;
            }

            // Превращаем в Unicode
            char[] chars = encoding.GetChars(record, 0, recordLength);
            int baseAddress = _ToInt(record, 12, 5) - 1;
            int start = baseAddress;

            // Пошли по полям (при помощи словаря)
            for (int dic = MarkerLength; ; dic += 12)
            {
                // находим следующее поле
                // Если нарвались на разделитель, заканчиваем
                if ((record[dic] == FieldDelimiter)
                     || (start > (recordLength - 4)))
                {
                    break;
                }

                string tag = new string(chars, dic, 3);
                RecordField fld = new RecordField(tag);
                bool isFixed = tag.StartsWith("00");
                result.Fields.Add(fld);
                start++;
                int end;
                if (isFixed)
                {
                    for (end = start; ; end++)
                    {
                        if (record[end] == FieldDelimiter)
                        {
                            break;
                        }
                    }
                    fld.Value = new string(chars, start, end - start);
                    start = end;
                }
                else // not fixed field
                {
                    start += 2;
                    while (true)
                    {
                        // находим подполя
                        if (record[start] == FieldDelimiter)
                        {
                            break;
                        }
                        if (record[start] != SubfieldDelimiter)
                        {
                            // Нарвались на поле без подполей
                            for (end = start; ; end++)
                            {
                                if ((record[end] == FieldDelimiter)
                                     || (record[end] == SubfieldDelimiter))
                                {
                                    break;
                                }
                            }
                            fld.Value = new string
                                (
                                    chars,
                                    start,
                                    end - start
                                );
                        }
                        else
                        {
                            // Декодируем подполя
                            SubField sub = new SubField
                                (chars[++start]);
                            fld.SubFields.Add(sub);
                            start++;
                            for (end = start; ; end++)
                            {
                                if ((record[end] == FieldDelimiter)
                                     || (record[end] == SubfieldDelimiter))
                                {
                                    break;
                                }
                            }
                            sub.Value = new string
                                (
                                    chars,
                                    start,
                                    end - start
                                );
                        }
                        start = end;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Выводит запись в ISO-поток.
        /// </summary>
        public static void WriteIso
            (
                MarcRecord record,
                Stream stream,
                Encoding encoding
            )
        {
            int recordLength = IsoMarker.MarkerLength;
            int dictionaryLength = 1; // С учетом ограничителя справочника
            int[] fieldLength = new int[record.Fields.Count]; // Длины полей

            // Сначала пытаемся подсчитать полную длину
            for (int i = 0; i < record.Fields.Count; i++)
            {
                dictionaryLength += 12; // Одна статья справочника
                RecordField field = record.Fields[i];
                int fldlen = 0;
                if (field.IsFixed)
                {
                    // В фиксированном поле не бывает подполей.
                    fldlen += (field.Value ?? string.Empty).Length;
                }
                else
                {
                    fldlen += RecordField.IndicatorCount; // Индикаторы
                    for (int j = 0; j < field.SubFields.Count; j++)
                    {
                        fldlen += 2; // Признак подполя и его код
                        fldlen +=
                            (
                                field.SubFields[j].Value
                                ?? string.Empty
                            ).Length;
                    }
                }
                fldlen += 1; // Разделитель полей
                fieldLength[i] = fldlen;
                recordLength += fldlen;
            }
            recordLength += dictionaryLength; // Справочник
            recordLength++; // Разделитель записей

            // Приступаем к кодированию
            int dictionaryPosition = IsoMarker.MarkerLength;
            int baseAddress = IsoMarker.MarkerLength + dictionaryLength;
            int currentAddress = baseAddress;
            char[] chars = new char[recordLength];
            //byte[] bytes = new byte[reclen];

            // Кодируем маркер
            for (int i = 0; i < baseAddress; i++)
                chars[i] = ' ';
            _Encode(chars, 0, 5, recordLength);
            _Encode(chars, 12, 5, baseAddress);

            IsoRecordHeader hdr = IsoRecordHeader.GetDefault();
            chars[5] = (char)hdr.RecordStatus;
            chars[6] = (char)hdr.RecordType;
            chars[7] = (char)hdr.BibliographicalIndex;
            chars[10] = '2';
            chars[11] = '2';
            chars[17] = (char)hdr.BibliographicalLevel;
            chars[18] = (char)hdr.CatalogingRules;
            chars[19] = (char)hdr.RelatedRecord;
            chars[20] = '4';
            chars[21] = '5';
            chars[22] = '0';
            chars[23] = '0';

            // Кодируем конец справочника
            chars[baseAddress - 1] = FieldDelimiter;
            // Проходим по полям
            for (int i = 0; i < record.Fields.Count; i++, dictionaryPosition += 12)
            {
                // Кодируем справочник
                RecordField fld = record.Fields[i];
                _Encode(chars, dictionaryPosition, fld.Tag.ToInvariantString());
                _Encode(chars, dictionaryPosition + 3, 4, fieldLength[i]);
                _Encode(chars, dictionaryPosition + 7, 5, currentAddress - baseAddress);

                // Кодируем поле
                if (fld.IsFixed)
                {
                    // В фиксированном поле не бывает подполей.
                    currentAddress = _Encode(chars, currentAddress, fld.Value);
                }
                else
                {
#if WITH_INDICATORS

                    chars[currentAddress++] = fld.Indicator1.Value[0];
                    chars[currentAddress++] = fld.Indicator2.Value[0];

#else

                    chars[currentAddress++] = ' ';
                    chars[currentAddress++] = ' ';

#endif

                    for (int j = 0; j < fld.SubFields.Count; j++)
                    {
                        chars[currentAddress++] = SubfieldDelimiter;
                        chars[currentAddress++] = fld.SubFields[j].Code;
                        currentAddress = _Encode(chars, currentAddress,
                            fld.SubFields[j].Value);
                    }
                }
                chars[currentAddress++] = FieldDelimiter;
            }
            // Ограничитель записи
            chars[recordLength - 1] = RecordDelimiter;

            // Собственно записываем
            byte[] bytes = encoding.GetBytes(chars);
            stream.Write(bytes, 0, bytes.Length);
        }

        #endregion
    }
}
