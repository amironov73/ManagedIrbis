// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordFieldUtility.cs --
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
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.Text;
using AM.Xml;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if !WINMOBILE && !PocketPC

using Formatting = Newtonsoft.Json.Formatting;

#endif

// ReSharper disable ForCanBeConvertedToForeach

#endregion

namespace ManagedIrbis
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class RecordFieldUtility
    {
        #region Properties and fields

        /// <summary>
        /// Empty array of <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public static readonly RecordField[] EmptyArray = new RecordField[0];

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        [NotNull]
        public static RecordField AddNonEmptySubField
            (
                [NotNull] this RecordField field,
                char code,
                [CanBeNull] string value
            )
        {
            Code.NotNull(field, "field");

            if (!string.IsNullOrEmpty(value))
            {
                field.AddSubField(code, value);
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Добавление подполей.
        /// </summary>
        [NotNull]
        public static RecordField AddSubFields
            (
                [NotNull] this RecordField field,
                [CanBeNull] IEnumerable<SubField> subFields
            )
        {
            Code.NotNull(field, "field");

            if (!ReferenceEquals(subFields, null))
            {
                foreach (SubField subField in subFields)
                {
                    field.SubFields.Add(subField);
                }
            }

            return field;
        }

        /// <summary>
        /// Добавление подполей.
        /// </summary>
        [NotNull]
        public static RecordField AddSubFields
            (
                [NotNull] this RecordField field,
                [CanBeNull] SubField[] subFields
            )
        {
            Code.NotNull(field, "field");

            if (!ReferenceEquals(subFields, null))
            {
                field.SubFields.AddRange(subFields);
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Все подполя.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] AllSubFields
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (!ReferenceEquals(subField, null))
                        {
                            result.Add(subField);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Apply subfield value.
        /// </summary>
        [NotNull]
        public static RecordField ApplySubField
            (
                [NotNull] this RecordField field,
                char code,
                [CanBeNull] object value
            )
        {
            Code.NotNull(field, "field");

            if (code == SubField.NoCode)
            {
                return field;
            }

            if (ReferenceEquals(value, null))
            {
                field.RemoveSubField(code);
            }
            else
            {
                SubField subField = field.GetFirstSubField(code);
                if (ReferenceEquals(subField, null))
                {
                    subField = new SubField(code);
                    field.SubFields.Add(subField);
                }
                subField.Value = value.ToString();
            }

            return field;
        }

        /// <summary>
        /// Apply subfield value.
        /// </summary>
        [NotNull]
        public static RecordField ApplySubField
            (
                [NotNull] this RecordField field,
                char code,
                bool value,
                [NotNull] string text
            )
        {
            Code.NotNull(field, "field");
            Code.NotNullNorEmpty(text, "text");

            if (code == SubField.NoCode)
            {
                return field;
            }

            if (value == false)
            {
                field.RemoveSubField(code);
            }
            else
            {
                SubField subField = field.GetFirstSubField(code);
                if (ReferenceEquals(subField, null))
                {
                    subField = new SubField(code);
                    field.SubFields.Add(subField);
                }
                subField.Value = text;
            }

            return field;
        }

        /// <summary>
        /// Apply subfield value.
        /// </summary>
        [NotNull]
        public static RecordField ApplySubField
            (
                [NotNull] this RecordField field,
                char code,
                [CanBeNull] string value
            )
        {
            Code.NotNull(field, "field");

            if (code == SubField.NoCode)
            {
                return field;
            }

            if (string.IsNullOrEmpty(value))
            {
                field.RemoveSubField(code);
            }
            else
            {
                SubField subField = field.GetFirstSubField(code);
                if (ReferenceEquals(subField, null))
                {
                    subField = new SubField(code);
                    field.SubFields.Add(subField);
                }
                subField.Value = value;
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Отбор подполей с указанными кодами.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] FilterSubFields
            (
                [NotNull] this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {
            Code.NotNull(subFields, "subFields");

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null))
                {
                    if (subField.Code.OneOf(codes))
                    {
                        result.Add(subField);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Отбор подполей с указанными кодами.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] FilterSubFields
            (
                [NotNull] this RecordField field,
                params char[] codes
            )
        {
            Code.NotNull(field, "field");

            return field.SubFields
                .FilterSubFields(codes);
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [CanBeNull]
        public static RecordField GetField
            (
                [NotNull] this RecordFieldCollection fields,
                int tag,
                int occurrence
            )
        {
            Code.NotNull(fields, "fields");

            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag == tag)
                {
                    if (occurrence == 0)
                    {
                        return fields[i];
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag == tag)
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this RecordFieldCollection fields,
                int tag
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag == tag)
                {
                    result.Add(fields[i]);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [CanBeNull]
        public static RecordField GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag,
                int occurrence
            )
        {
            Code.NotNull(fields, "fields");

            foreach (RecordField field in fields)
            {
                if (field.Tag == tag)
                {
                    if (occurrence == 0)
                    {
                        return field;
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                params int[] tags
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this RecordFieldCollection fields,
                params int[] tags
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].Tag.OneOf(tags))
                {
                    result.Add(fields[i]);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [CanBeNull]
        public static RecordField GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] int[] tags,
                int occurrence
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(tags, "tags");

            return fields
                .GetField(tags)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [CanBeNull]
        public static RecordField GetField
            (
                [NotNull] this RecordFieldCollection fields,
                [NotNull] int[] tags,
                int occurrence
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(tags, "tags");

            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag.OneOf(tags))
                {
                    if (occurrence == 0)
                    {
                        return fields[i];
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] Func<RecordField, bool> predicate
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(predicate, "predicate");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (predicate(field))
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this RecordFieldCollection fields,
                [NotNull] Func<RecordField, bool> predicate
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(predicate, "predicate");

            LocalList<RecordField> result = new LocalList<RecordField>();
            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (predicate(fields[i]))
                {
                    result.Add(fields[i]);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Выполнение неких действий над полями.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [CanBeNull] Action<RecordField> action
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                result.Add(field);
                if (!ReferenceEquals(action, null))
                {
                    action(field);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Выполнение неких действий над полями и подполями.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [CanBeNull] Action<RecordField> fieldAction,
                [CanBeNull] Action<SubField> subFieldAction
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            if (!ReferenceEquals(fieldAction, null)
                || !ReferenceEquals(subFieldAction, null))
            {
                foreach (RecordField field in fields)
                {
                    result.Add(field);
                    if (!ReferenceEquals(fieldAction, null))
                    {
                        fieldAction(field);
                    }

                    if (!ReferenceEquals(subFieldAction, null))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            subFieldAction(subField);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Выполнение неких действий над подполями.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [CanBeNull] Action<SubField> action
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            if (!ReferenceEquals(action, null))
            {
                foreach (RecordField field in fields)
                {
                    result.Add(field);
                    foreach (SubField subField in field.SubFields)
                    {
                        action(subField);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] Func<SubField, bool> predicate
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(predicate, "predicate");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (field.SubFields.Any(predicate))
                {
                    result.Add(field);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] char[] codes,
                [NotNull] Func<SubField, bool> predicate
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(codes, "codes");
            Code.NotNull(predicate, "predicate");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (!ReferenceEquals(subField, null))
                        {
                            if (subField.Code.OneOf(codes))
                            {
                                if (predicate(subField))
                                {
                                    result.Add(field);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] char[] codes,
                params string[] values
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(codes, "codes");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (!ReferenceEquals(subField, null))
                        {
                            if (subField.Code.OneOf(codes))
                            {
                                if (subField.Value.OneOf(values))
                                {
                                    result.Add(field);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                char code,
                [CanBeNull] string value
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (!ReferenceEquals(subField, null))
                        {
                            if (subField.Code.SameChar(code))
                            {
                                if (subField.Value.SameString(value))
                                {
                                    result.Add(field);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] int[] tags,
                [NotNull] char[] codes,
                [NotNull] string[] values
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(tags, "tags");
            Code.NotNull(codes, "codes");
            Code.NotNull(values, "values");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (!ReferenceEquals(subField, null))
                            {
                                if (subField.Code.OneOf(codes))
                                {
                                    if (subField.Value.OneOf(values))
                                    {
                                        result.Add(field);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] Func<RecordField, bool> fieldPredicate,
                [NotNull] Func<SubField, bool> subPredicate
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(fieldPredicate, "fieldPredicate");
            Code.NotNull(subPredicate, "subPredicate");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (fieldPredicate(field))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (!ReferenceEquals(subField, null))
                            {
                                if (subPredicate(subField))
                                {
                                    result.Add(field);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Количество повторений поля.
        /// </summary>
        public static int GetFieldCount
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag
            )
        {
            Code.NotNull(fields, "fields");

            int result = 0;

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag == tag)
                    {
                        result++;
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Количество повторений поля.
        /// </summary>
        public static int GetFieldCount
            (
                [NotNull] this RecordFieldCollection fields,
                int tag
            )
        {
            Code.NotNull(fields, "fields");

            int result = 0;
            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                RecordField field = fields[i];
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag == tag)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tagRegex
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNullNorEmpty(tagRegex, "tagRegex");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    string tag = field.Tag.ToInvariantString();
                    if (Regex.IsMatch(tag, tagRegex))
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [CanBeNull]
        public static RecordField GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tagRegex,
                int occurrence
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNullNorEmpty(tagRegex, "tagRegex");

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    string tag = field.Tag.ToInvariantString();
                    if (Regex.IsMatch(tag, tagRegex))
                    {
                        if (occurrence == 0)
                        {
                            return field;
                        }

                        occurrence--;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] int[] tags,
                [NotNull] string textRegex
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(tags, "tags");
            Code.NotNull(textRegex, textRegex);

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags)
                       && !string.IsNullOrEmpty(field.Value))
                    {
                        if (Regex.IsMatch(field.Value, textRegex))
                        {
                            result.Add(field);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [CanBeNull]
        public static RecordField GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] int[] tags,
                [NotNull] string textRegex,
                int occurrence
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(tags, "tags");
            Code.NotNullNorEmpty(textRegex, "textRegex");

            return fields
                .GetFieldRegex(tags, textRegex)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        public static RecordField[] GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] int[] tags,
                [NotNull] char[] codes,
                [NotNull] string textRegex
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(tags, "tags");
            Code.NotNull(codes, "codes");
            Code.NotNullNorEmpty(textRegex, "textRegex");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (!ReferenceEquals(subField, null))
                            {
                                if (subField.Code.OneOf(codes)
                                    && !string.IsNullOrEmpty(subField.Value))
                                {
                                    if (Regex.IsMatch(subField.Value, textRegex))
                                    {
                                        result.Add(field);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static RecordField GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] int[] tags,
                [NotNull] char[] codes,
                [NotNull] string textRegex,
                int occurrence
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(tags, "tags");
            Code.NotNull(codes, "codes");
            Code.NotNullNorEmpty(textRegex, "textRegex");

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (!ReferenceEquals(subField, null))
                            {
                                if (subField.Code.OneOf(codes)
                                    && !string.IsNullOrEmpty(subField.Value))
                                {
                                    if (Regex.IsMatch(subField.Value, textRegex))
                                    {
                                        if (occurrence == 0)
                                        {
                                            return field;
                                        }

                                        occurrence--;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Получение значения поля.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetFieldValue
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<string> result = new LocalList<string>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    string value = field.Value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        result.Add(value);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение значения поля.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetFieldValue
            (
                [NotNull] this RecordFieldCollection fields
            )
        {
            Code.NotNull(fields, "fields");

            List<string> result = null;
            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                string value = fields[i].Value;
                if (!string.IsNullOrEmpty(value))
                {
                    if (ReferenceEquals(result, null))
                    {
                        result = new List<string>();
                    }
                    result.Add(value);
                }
            }

            return ReferenceEquals(result, null)
                ? StringUtility.EmptyArray
                : result.ToArray();
        }

        /// <summary>
        /// Получение значения поля.
        /// </summary>
        [CanBeNull]
        public static string GetFieldValue
            (
                [CanBeNull] this RecordField field
            )
        {
            return ReferenceEquals(field, null)
                ? null
                : field.Value;
        }

        /// <summary>
        /// Непустые значения полей с указанным тегом.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetFieldValue
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<string> result = new LocalList<string>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag == tag
                    && !string.IsNullOrEmpty(field.Value))
                {
                    result.Add(field.Value);
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Первое вхождение поля с указанным тегом.
        /// </summary>
        [CanBeNull]
        public static RecordField GetFirstField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag
            )
        {
            Code.NotNull(fields, "fields");

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag == tag)
                {
                    return field;
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение поля с указанным тегом.
        /// </summary>
        [CanBeNull]
        public static RecordField GetFirstField
            (
                [NotNull] this RecordFieldCollection fields,
                int tag
            )
        {
            Code.NotNull(fields, "fields");

            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag == tag)
                {
                    return fields[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение поля с любым из перечисленных тегов.
        /// </summary>
        [CanBeNull]
        public static RecordField GetFirstField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                params int[] tags
            )
        {
            Code.NotNull(fields, "fields");

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag.OneOf(tags))
                {
                    return field;
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение поля с любым из перечисленных тегов.
        /// </summary>
        [CanBeNull]
        public static RecordField GetFirstField
            (
                [NotNull] this RecordFieldCollection fields,
                params int[] tags
            )
        {
            Code.NotNull(fields, "fields");

            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag.OneOf(tags))
                {
                    return fields[i];
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Значение первого поля с указанным тегом или <c>null</c>.
        /// </summary>
        [CanBeNull]
        public static string GetFirstFieldValue
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag
            )
        {
            Code.NotNull(fields, "fields");

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag == tag)
                {
                    return field.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Значение первого поля с указанным тегом или <c>null</c>.
        /// </summary>
        [CanBeNull]
        public static string GetFirstFieldValue
            (
                [NotNull] this RecordFieldCollection fields,
                int tag
            )
        {
            Code.NotNull(fields, "fields");

            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag == tag)
                {
                    return fields[i].Value;
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Gets the first subfield.
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this RecordField field,
                char code
            )
        {
            Code.NotNull(field, "field");

            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return subFields[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя, соответствующего указанным
        /// критериям.
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag,
                char code
            )
        {
            Code.NotNull(fields, "fields");

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag == tag)
                {
                    SubFieldCollection subFields = field.SubFields;
                    int count = subFields.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (subFields[i].Code.SameChar(code))
                        {
                            return subFields[i];
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя, соответствующего указанным
        /// критериям.
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this RecordFieldCollection fields,
                int tag,
                char code
            )
        {
            Code.NotNull(fields, "fields");

            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                RecordField field = fields[i];
                if (field.Tag == tag)
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.SameChar(code))
                        {
                            return subField;
                        }
                    }
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Получение текста указанного подполя
        /// </summary>
        [CanBeNull]
        public static string GetFirstSubFieldValue
            (
                [NotNull] this RecordField field,
                char code
            )
        {
            Code.NotNull(field, "field");

            Code.NotNull(field, "field");

            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return subFields[i].Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Значение первого подполя с указанными тегом и кодом
        /// или <c>null</c>.
        /// </summary>
        [CanBeNull]
        public static string GetFirstSubFieldValue
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag,
                char code
            )
        {
            Code.NotNull(fields, "fields");

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag == tag)
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.SameChar(code))
                        {
                            return subField.Value;
                        }
                    }
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Перечень подполей с указанным кодом.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this RecordField field,
                char code
            )
        {
            Code.NotNull(field, "field");

            LocalList<SubField> result = new LocalList<SubField>();
            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    result.Add(subFields[i]);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Указанное повторение подполя с данным кодом.
        /// </summary>
        [CanBeNull]
        public static SubField GetSubField
            (
                [NotNull] this RecordField field,
                char code,
                int occurrence
            )
        {
            Code.NotNull(field, "field");

            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    if (occurrence == 0)
                    {
                        return subFields[i];
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                char code
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    SubFieldCollection subFields = field.SubFields;
                    int count = subFields.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (subFields[i].Code.SameChar(code))
                        {
                            result.Add(subFields[i]);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                params char[] codes
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    SubFieldCollection subFields = field.SubFields;
                    int count = subFields.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (subFields[i].Code.OneOf(codes))
                        {
                            result.Add(subFields[i]);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag,
                char code
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag == tag)
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.SameChar(code))
                        {
                            result.Add(subField);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this RecordFieldCollection fields,
                int tag,
                char code
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<SubField> result = new LocalList<SubField>();
            int fieldCount = fields.Count;
            for (int i = 0; i < fieldCount; i++)
            {
                RecordField field = fields[i];
                if (field.Tag == tag)
                {
                    SubFieldCollection subFields = field.SubFields;
                    int subCount = subFields.Count;
                    for (int j = 0; j < subCount; j++)
                    {
                        if (subFields[j].Code.SameChar(code))
                        {
                            result.Add(subFields[j]);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение подполя.
        /// </summary>
        [CanBeNull]
        public static SubField GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag,
                int fieldOccurrence,
                char code,
                int subOccurrence
            )
        {
            Code.NotNull(fields, "fields");

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag == tag)
                {
                    if (fieldOccurrence == 0)
                    {
                        SubFieldCollection subFields = field.SubFields;
                        int subCount = subFields.Count;
                        for (int j = 0; j < subCount; j++)
                        {
                            if (subFields[j].Code.SameChar(code))
                            {
                                if (subOccurrence == 0)
                                {
                                    return subFields[j];
                                }

                                subOccurrence--;
                            }
                        }

                        return null;
                    }

                    fieldOccurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Получение подполя.
        /// </summary>
        [CanBeNull]
        public static SubField GetSubField
            (
                [NotNull] this RecordFieldCollection fields,
                int tag,
                int fieldOccurrence,
                char code,
                int subOccurrence
            )
        {
            Code.NotNull(fields, "fields");

            int fieldCount = fields.Count;
            for (int i = 0; i < fieldCount; i++)
            {
                RecordField field = fields[i];
                if (field.Tag == tag)
                {
                    if (fieldOccurrence == 0)
                    {
                        SubFieldCollection subFields = field.SubFields;
                        int subCount = subFields.Count;
                        for (int j = 0; j < subCount; j++)
                        {
                            if (subFields[j].Code.SameChar(code))
                            {
                                if (subOccurrence == 0)
                                {
                                    return subFields[j];
                                }
                                subOccurrence--;
                            }
                        }

                        return null;
                    }

                    fieldOccurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Получение подполя.
        /// </summary>
        public static SubField GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag,
                char code,
                int occurrence
            )
        {
            Code.NotNull(fields, "fields");

            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag == tag)
                {
                    SubFieldCollection subFields = field.SubFields;
                    int subCount = subFields.Count;
                    for (int j = 0; j < subCount; j++)
                    {
                        if (subFields[j].Code.SameChar(code))
                        {
                            if (occurrence == 0)
                            {
                                return subFields[j];
                            }
                            occurrence--;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получение подполя.
        /// </summary>
        [CanBeNull]
        public static SubField GetSubField
            (
                [NotNull] this RecordFieldCollection fields,
                int tag,
                char code,
                int occurrence
            )
        {
            Code.NotNull(fields, "fields");

            int fieldCount = fields.Count;
            for (int i = 0; i < fieldCount; i++)
            {
                RecordField field = fields[i];
                if (field.Tag == tag)
                {
                    SubFieldCollection subFields = field.SubFields;
                    int subCount = subFields.Count;
                    for (int j = 0; j < subCount; j++)
                    {
                        if (subFields[j].Code.SameChar(code))
                        {
                            if (occurrence == 0)
                            {
                                return subFields[j];
                            }
                            occurrence--;
                        }
                    }
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Получение текста указанного подполя.
        /// </summary>
        [CanBeNull]
        public static string GetSubFieldValue
            (
                [NotNull] this RecordField field,
                char code,
                int occurrence
            )
        {
            Code.NotNull(field, "field");

            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    if (occurrence == 0)
                    {
                        return subFields[i].Value;
                    }
                    occurrence--;
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Непустые значения подполей с указанными тегом и кодом.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetSubFieldValue
            (
                [NotNull] this IEnumerable<RecordField> fields,
                int tag,
                char code
            )
        {
            Code.NotNull(fields, "fields");

            List<string> result = new List<string>();

            foreach (RecordField field in fields)
            {
                if (field.Tag == tag)
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.SameChar(code)
                            && !string.IsNullOrEmpty(subField.Value))
                        {
                            result.Add(subField.Value);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Есть хотя бы одно подполе с указанным кодом?
        /// </summary>
        public static bool HaveSubField
            (
                [NotNull] this RecordField field,
                char code
            )
        {
            Code.NotNull(field, "field");

            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return true;
                }
            }

            return false;
        }

        // ==========================================================

        /// <summary>
        /// Есть хотя бы одно поле с любым из указанных кодов?
        /// </summary>
        public static bool HaveSubField
            (
                [NotNull] this RecordField field,
                params char[] codes
            )
        {
            Code.NotNull(field, "field");

            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.OneOf(codes))
                {
                    return true;
                }
            }

            return false;
        }

        // ==========================================================

        /// <summary>
        /// Нет ни одного подполя с указанным кодом?
        /// </summary>
        public static bool HaveNotSubField
            (
                [NotNull] this RecordField field,
                char code
            )
        {
            Code.NotNull(field, "field");

            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return false;
                }
            }

            return true;
        }

        // ==========================================================

        /// <summary>
        /// Нет ни одного подполя с указанными кодами?
        /// </summary>
        public static bool HaveNotSubField
            (
                [NotNull] this RecordField field,
                params char[] codes
            )
        {
            Code.NotNull(field, "field");

            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.OneOf(codes))
                {
                    return false;
                }
            }

            return true;
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] NotNullTag
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag != 0)
                {
                    result.Add(field);
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] NotNullValue
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && !string.IsNullOrEmpty(field.Value))
                {
                    result.Add(field);
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        ///// <summary>
        ///// Удаляем подполе.
        ///// </summary>
        ///// <remarks>Удаляет все повторения подполей
        ///// с указанным кодом.
        ///// </remarks>
        //[NotNull]
        //public static RecordField RemoveSubField
        //    (
        //        [NotNull] this RecordField field,
        //        char code
        //    )
        //{
        //    SubField[] found = field.SubFields
        //        .FindAll(_ => char.ToLowerInvariant(_.Code) == code)
        //        .ToArray();

        //    foreach (SubField subField in found)
        //    {
        //        field.SubFields.Remove(subField);
        //    }

        //    return field;
        //}

        // ==========================================================

        /// <summary>
        /// Меняем значение подполя.
        /// </summary>
        [NotNull]
        public static RecordField ReplaceSubField
            (
                [NotNull] this RecordField field,
                char code,
                [CanBeNull] string oldValue,
                [CanBeNull] string newValue
            )
        {
            Code.NotNull(field, "field");

            SubFieldCollection subFields = field.SubFields;
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                SubField subField = subFields[i];
                if (subField.Code.SameChar(code))
                {
                    if (subField.Value.SameStringSensitive(oldValue))
                    {
                        subField.SetValue(newValue);
                    }
                }
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Меняем значение подполя.
        /// </summary>
        [NotNull]
        public static RecordField ReplaceSubField
            (
                [NotNull] this RecordField field,
                char code,
                [CanBeNull] string newValue,
                bool ignoreCase
            )
        {
            StringComparison comparison =
#if UAP
                ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
#else
                ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
#endif

            string oldValue = field.GetSubFieldValue(code, 0);
            bool changed = string.Compare(oldValue, newValue, comparison) != 0;

            if (changed)
            {
                field.SetSubField(code, newValue);
            }

            return field;

        }

        // ==========================================================

        /// <summary>
        /// Get unknown subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetUnknownSubFields
            (
                [NotNull] this IEnumerable<SubField> subFields,
                [NotNull] string knownCodes
            )
        {
            Code.NotNull(subFields, "subFields");
            Code.NotNullNorEmpty(knownCodes, "knownCodes");

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null))
                {
                    if (!subField.Code.OneOf(knownCodes))
                    {
                        result.Add(subField);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get unknown subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetUnknownSubFields
            (
                [NotNull] this SubFieldCollection subFields,
                [NotNull] string knownCodes
            )
        {
            Code.NotNull(subFields, "subFields");
            Code.NotNullNorEmpty(knownCodes, "knownCodes");

            LocalList<SubField> result = new LocalList<SubField>();
            for (int i = 0; i < subFields.Count; i++)
            {
                if (!subFields[i].Code.OneOf(knownCodes))
                {
                    result.Add(subFields[i]);
                }
            }

            return result.ToArray();
        }

        // ==========================================================

#if !WINMOBILE && !PocketPC

        /// <summary>
        /// Convert the field to <see cref="JObject"/>.
        /// </summary>
        [NotNull]
        public static JObject ToJObject
            (
                [NotNull] this RecordField field
            )
        {
            Code.NotNull(field, "field");

            JObject result = JObject.FromObject(field);

            return result;
        }

        /// <summary>
        /// Convert the field to JSON.
        /// </summary>
        [NotNull]
        public static string ToJson
            (
                [NotNull] this RecordField field
            )
        {
            Code.NotNull(field, "field");

            string result = JObject.FromObject(field)
                .ToString(Formatting.None);

            return result;
        }


        /// <summary>
        /// Restore field from <see cref="JObject"/>.
        /// </summary>
        [NotNull]
        public static RecordField FromJObject
            (
                [NotNull] JObject jObject
            )
        {
            Code.NotNull(jObject, "jObject");

            RecordField result = jObject.ToObject<RecordField>();

            return result;
        }

        /// <summary>
        /// Restore subfield from JSON.
        /// </summary>
        public static RecordField FromJson
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            RecordField result = JsonConvert.DeserializeObject<RecordField>(text);

            return result;
        }

#endif

        // ==========================================================

        /// <summary>
        /// Парсинг текстового представления поля.
        /// </summary>
        [NotNull]
        public static RecordField Parse
            (
                [NotNull] string tag,
                [NotNull] string body
            )
        {
            RecordField result = new RecordField(tag);

            int first = body.IndexOf(RecordField.Delimiter);
            if (first != 0)
            {
                if (first < 0)
                {
                    result.Value = body;
                    body = string.Empty;
                }
                else
                {
                    result.Value = body.Substring
                        (
                            0,
                            first
                        );
                    body = body.Substring(first);
                }
            }

            var code = (char)0;
            var value = new StringBuilder();
            foreach (char c in body)
            {
                if (c == RecordField.Delimiter)
                {
                    if (code != '\0')
                    {
                        result.AddSubField
                            (
                                code,
                                value
                            );
                    }
                    value.Length = 0;
                    code = (char)0;
                }
                else
                {
                    if (code == 0)
                    {
                        code = c;
                    }
                    else
                    {
                        value.Append(c);
                    }
                }
            }

            if (code != (char)0)
            {
                result.AddSubField
                (
                    code,
                    value
                );
            }

            if (string.IsNullOrEmpty(result.Value))
            {
                result.Value = null;
            }

            return result;
        }

        /// <summary>
        /// Парсинг строкового представления поля.
        /// </summary>
        [CanBeNull]
        public static RecordField Parse
            (
                [CanBeNull] string line
            )
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            string[] parts = line.SplitFirst('#');
            string tag = parts[0];
            string body = parts[1];
            return Parse(tag, body);
        }

        // ==========================================================

        /// <summary>
        /// Converts the field to XML.
        /// </summary>
        [NotNull]
        public static string ToXml
            (
                [NotNull] this RecordField field
            )
        {
            Code.NotNull(field, "field");

            return XmlUtility.SerializeShort(field);
        }

        // ==========================================================

        /// <summary>
        /// Restore the field from XML.
        /// </summary>
        [NotNull]
        public static RecordField FromXml
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            XmlSerializer serializer = new XmlSerializer(typeof(RecordField));
            StringReader reader = new StringReader(text);
            RecordField result = (RecordField)serializer.Deserialize(reader);

            return result;
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] WithNullTag
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag == 0)
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] WithNullValue
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (string.IsNullOrEmpty(field.Value))
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        public static RecordField[] WithoutSubFields
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.SubFields.Count == 0)
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        public static RecordField[] WithSubFields
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.SubFields.Count != 0)
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Convert the field to C# source code.
        /// </summary>
        [NotNull]
        public static string ToSourceCode
            (
                [NotNull] this RecordField field
            )
        {
            Code.NotNull(field, "field");

            StringBuilder result = new StringBuilder();
            result.AppendFormat
                (
                    "new RecordField({0}",
                    field.Tag.ToInvariantString()
                );
            if (!ReferenceEquals(field.Value, null))
            {
                result.AppendFormat
                    (
                        ", {0}",
                        SourceCodeUtility.ToSourceCode(field.Value)
                    );
            }
            foreach (SubField subField in field.SubFields)
            {
                result.AppendLine(",");
                result.Append(subField.ToSourceCode());
            }
            result.Append(")");

            return result.ToString();
        }

        #endregion
    }
}

