/* Iso2709.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

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
        public const byte RecordDelimiter = 0x1D;

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

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор 2709.
        /// </summary>
        [CanBeNull]
        public static IrbisRecord ReadRecord
            (
                [NotNull] Stream stream,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(stream, "stream");
            Code.NotNull(encoding, "encoding");

            IrbisRecord result = new IrbisRecord();

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

            // простая проверка, что мы имеем дело с нормальной ISO-записью
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


        #endregion
    }
}
