/* FieldReference.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FieldReference
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Нет кода.
        /// </summary>
        public const char NoCode = '\0';

        #endregion

        #region Enum

        /// <summary>
        /// Команда вывода.
        /// </summary>
        public enum Verb
        {
            /// <summary>
            /// Простой вывод.
            /// </summary>
            V,

            /// <summary>
            /// Условный вывод.
            /// </summary>
            D,

            /// <summary>
            /// Негативный вывод.
            /// </summary>
            N
        }

        #endregion

        #region Properties

        /// <summary>
        /// Условный префикс.
        /// </summary>
        [CanBeNull]
        public string ConditionalPrefix { get; set; }

        /// <summary>
        /// Повторяемый префикс.
        /// </summary>
        [CanBeNull]
        public string RepeatablePrefix { get; set; }

        /// <summary>
        /// Наличие + у повторяемого префикса.
        /// </summary>
        public bool PlusPrefix { get; set; }

        /// <summary>
        /// Команда вывода: V, D или N
        /// </summary>
        [DefaultValue(Verb.V)]
        public Verb Command { get; set; }

        /// <summary>
        /// Тег поля.
        /// </summary>
        [CanBeNull]
        public string FieldTag { get; set; }

        /// <summary>
        /// Тег встроенного поля.
        /// </summary>
        [CanBeNull]
        public string EmbeddedTag { get; set; }

        /// <summary>
        /// Код подполя.
        /// </summary>
        public char SubField { get; set; }

        /// <summary>
        /// Начальный номер повторения.
        /// </summary>
        public int IndexFrom { get; set; }

        /// <summary>
        /// Конечный номер повторения.
        /// </summary>
        public int IndexTo { get; set; }

        /// <summary>
        /// Смещение.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Длина.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Условный суффикс.
        /// </summary>
        [CanBeNull]
        public string ConditionalSuffix { get; set; }

        /// <summary>
        /// Повторяемый суффикс.
        /// </summary>
        [CanBeNull]
        public string RepeatableSuffix { get; set; }

        /// <summary>
        /// Наличие '+' у суффикса.
        /// </summary>
        public bool PlusSuffix { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public FieldReference()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldReference
            (
                [NotNull] string fieldTag
            )
        {
            Code.NotNull(() => fieldTag);

            FieldTag = fieldTag;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldReference
            (
                [NotNull] string fieldTag,
                char subField
            )
        {
            Code.NotNull(() => fieldTag);

            FieldTag = fieldTag;
            SubField = subField;
        }

        #endregion

        #region Private members

        private static string _SafeSubString 
            (
                string text,
                int offset,
                int length
            )
        {
            if (string.IsNullOrEmpty (text))
            {
                return text;
            }

            if ( (offset + length) > text.Length )
            {
                length = text.Length - offset;
                if (length < - 0)
                {
                    return string.Empty;
                }
            }

            return text.Substring
                (
                    offset,
                    length 
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление префиксов/суффиксов
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] Decorate
            (
                [NotNull] string[] source
            )
        {
            Code.NotNull(source, "source");

            List<string> result = new List<string>(source.Length);

            source = source.NonEmptyLines().ToArray();

            foreach (string s in source)
            {
                string output = s;

                // TODO

                result.Add(output);
            }

            return result
                .NonEmptyLines()
                .ToArray();
        }

        /// <summary>
        /// Получение значений полей/подполей
        /// </summary>
        [NotNull]
        public string[] GetFieldValue
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");
            if (string.IsNullOrEmpty(FieldTag))
            {
                throw new ApplicationException();
            }

            RecordField[] selected = fields.GetField(FieldTag);
            if (!string.IsNullOrEmpty(EmbeddedTag))
            {
                // TODO
            }

            List<string> result = new List<string>();

            foreach (RecordField field in selected)
            {
                if (SubField == NoCode)
                {
                    result.Add(field.ToString());
                }
                else if (SubField == '*')
                {
                    result.Add(field.Value);
                }
                else
                {
                    string[] range = field
                        .GetSubField(SubField)
                        .GetSubFieldValue();

                    result.AddRange(range);
                }
            }

            return result
                .NonEmptyLines()
                .ToArray();
        }

        /// <summary>
        /// Форматируем строки.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] Format
            (
                [NotNull] string[] source
            )
        {
            Code.NotNull(source, "source");

            string[] result = LimitIndex(source);
            result = LimitLength(result);
            result = Decorate(result);
            
            return result;
        }

        /// <summary>
        /// Форматируем в одну строку.
        /// </summary>
        [NotNull]
        public string FormatSingle
            (
                [NotNull] string[] source
            )
        {
            Code.NotNull(source, "source");

            string[] result = Format(source);
            return string.Join
                (
                    string.Empty,
                    result
                );
        }

        /// <summary>
        /// Форматируем поля.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] Format
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            string[] source = GetFieldValue(fields);
            string[] result = Format(source);
            
            return result;
        }

        /// <summary>
        /// Форматируем поля в одну строку.
        /// </summary>
        [NotNull]
        public string FormatSingle
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            string[] source = GetFieldValue(fields);
            string result = FormatSingle(source);
            
            return result;
        }

        /// <summary>
        /// Форматируем запись.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] Format
            (
                [NotNull] IrbisRecord record
            )
        {
            Code.NotNull(() => record);

            return Format(record.Fields);
        }


        /// <summary>
        /// Форматируем запись в одну строку.
        /// </summary>
        [NotNull]
        public string FormatSingle
            (
                [NotNull] IrbisRecord record
            )
        {
            Code.NotNull(() => record);

            return string.Join
                (
                    string.Empty,
                    Format(record)
                );
        }

        /// <summary>
        /// Отбор по индексу.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] LimitIndex
            (
                [NotNull] string[] source
            )
        {
            Code.NotNull(source, "source");

            source = source.NonEmptyLines().ToArray();

            if (IndexFrom == 0
                && IndexTo == 0)
            {
                return source;
            }

            List<string> result = new List<string>();

            int low = (IndexFrom == 0)
                ? 0
                : IndexFrom - 1;
            int high = (IndexTo == 0)
                ? source.Length - 1
                : IndexTo - 1;

            for (int i = 0; i < source.Length; i++)
            {
                if ((i >= low) && (i <= high))
                {
                    result.Add(source[i]);
                }
            }

            return result
                .NonEmptyLines()
                .ToArray();
        }

        /// <summary>
        /// Усечение строк.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] LimitLength
            (
                [NotNull] string[] source
            )
        {
            Code.NotNull(source, "source");

            source = source.NonEmptyLines().ToArray();

            if (Offset == 0 && Length == 0)
            {
                return source;
            }


            List<string> result = new List<string>();

            int offset = Offset;
            int length = int.MaxValue;
            if (Length != 0)
            {
                length = Length;
            }

            foreach (string s in source)
            {
                string v = _SafeSubString(s, offset, length);
                if (!string.IsNullOrEmpty(v))
                {
                    result.Add(v);
                }
            }

            return result
                .ToArray();
        }

        /// <summary>
        /// Превращаем в исходный код ИРБИС.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public string ToSourceCode()
        {
            if (string.IsNullOrEmpty(FieldTag))
            {
                throw new ApplicationException();
            }

            StringBuilder result = new StringBuilder();

            if (ConditionalPrefix != null)
            {
                result.AppendFormat("\"{0}\"", ConditionalPrefix);
            }

            if (RepeatablePrefix != null)
            {
                result.AppendFormat("|{0}|", RepeatablePrefix);
                if (PlusPrefix)
                {
                    result.Append('+');
                }
            }

            char v;
            switch (Command)
            {
                case Verb.D:
                    v = 'd';
                    break;
                case Verb.N:
                    v = 'n';
                    break;
                case Verb.V:
                    v = 'v';
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            result.Append(v);

            result.Append(FieldTag);

            if (EmbeddedTag != null)
            {
                result.Append('/');
                result.Append(EmbeddedTag);
            }

            if (SubField != NoCode)
            {
                result.Append('^');
                result.Append(SubField);
            }

            if (IndexFrom != 0
                || IndexTo != 0)
            {
                result.Append('[');

                if (IndexFrom == IndexTo)
                {
                    result.Append(IndexFrom);
                }
                else if (IndexFrom != 0)
                {
                    if (IndexTo != 0)
                    {
                        result.AppendFormat("{0}-{1}", IndexFrom, IndexTo);
                    }
                    else
                    {
                        result.Append(IndexFrom);
                        result.Append('-');
                    }
                }
                else
                {
                    result.Append('-');
                    result.Append(IndexTo);
                }

                result.Append(']');
            }

            if (Offset != 0)
            {
                result.AppendFormat("*{0}", Offset);
            }

            if (Length != 0)
            {
                result.AppendFormat(".{0}", Length);
            }

            if (RepeatableSuffix != null)
            {
                if (PlusSuffix)
                {
                    result.Append('+');
                }
                result.AppendFormat("|{0}|", RepeatableSuffix);
            }

            if (ConditionalSuffix != null)
            {
                result.AppendFormat("\"{0}\"", ConditionalSuffix);
            }

            return result.ToString();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ConditionalPrefix = reader.ReadNullableString();
            RepeatablePrefix = reader.ReadNullableString();
            PlusPrefix = reader.ReadBoolean();
            Command = (Verb)reader.ReadPackedInt32();
            FieldTag = reader.ReadNullableString();
            EmbeddedTag = reader.ReadNullableString();
            SubField = reader.ReadChar();
            IndexFrom = reader.ReadPackedInt32();
            IndexTo = reader.ReadPackedInt32();
            Offset = reader.ReadPackedInt32();
            Length = reader.ReadPackedInt32();
            ConditionalSuffix = reader.ReadNullableString();
            RepeatableSuffix = reader.ReadNullableString();
            PlusSuffix = reader.ReadBoolean();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(ConditionalPrefix)
                .WriteNullable(RepeatablePrefix)
                .Write(PlusPrefix);
            writer.WritePackedInt32((int)Command);
            writer
                .WriteNullable(FieldTag)
                .WriteNullable(EmbeddedTag)
                .Write(SubField);
            writer
                .WritePackedInt32(IndexFrom)
                .WritePackedInt32(IndexTo)
                .WritePackedInt32(Offset)
                .WritePackedInt32(Length)
                .WriteNullable(ConditionalSuffix)
                .WriteNullable(RepeatableSuffix)
                .Write(PlusSuffix);
        }

        #endregion
    }
}
