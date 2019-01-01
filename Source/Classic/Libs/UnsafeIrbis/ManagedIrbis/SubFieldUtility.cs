// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubFieldUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnsafeAM;
using UnsafeAM.Collections;
using UnsafeAM.Json;
using UnsafeAM.Text;

using UnsafeCode;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable ForCanBeConvertedToForeach

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public static class SubFieldUtility
    {
        #region Properties and fields

        /// <summary>
        /// Empty array of <see cref="SubField"/>'s.
        /// </summary>
        public static readonly SubField[] EmptyArray = new SubField[0];

        #endregion

        #region Public methods

        /// <summary>
        /// Первое вхождение подполя с указанным кодом.
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this IEnumerable<SubField> subFields,
                char code
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null)
                    && subField.Code.SameChar(code))
                {
                    return subField;
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя с указанным кодом.
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this SubFieldCollection subFields,
                char code
            )
        {
            Code.NotNull(subFields, nameof(subFields));

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
        /// Первое вхождение подполя с одним из указанных кодов.
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null)
                    && subField.Code.OneOf(codes))
                {
                    return subField;
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя с одним из указанных кодов.
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this SubFieldCollection subFields,
                params char[] codes
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.OneOf(codes))
                {
                    return subFields[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя с указанными кодом
        /// и значением (с учётом регистра символов).
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this IEnumerable<SubField> subFields,
                char code,
                [CanBeNull] string value
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null)
                    && subField.Code.SameChar(code)
                    && subField.Value.SameStringSensitive(value))
                {
                    return subField;
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя с указанными кодом
        /// и значением (с учётом регистра символов).
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this SubFieldCollection subFields,
                char code,
                [CanBeNull] string value
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                SubField subField = subFields[i];
                if (subField.Code.SameChar(code)
                    && subField.Value.SameStringSensitive(value))
                {
                    return subField;
                }
            }

            return null;
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<SubField> subFields,
                char code
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null)
                    && subField.Code.SameChar(code))
                {
                    result.Add(subField);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this SubFieldCollection subFields,
                char code
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            LocalList<SubField> result = new LocalList<SubField>();
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
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null)
                    && subField.Code.OneOf(codes))
                {
                    result.Add(subField);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this SubFieldCollection subFields,
                params char[] codes
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            LocalList<SubField> result = new LocalList<SubField>();
            int count = subFields.Count;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.OneOf(codes))
                {
                    result.Add(subFields[i]);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Выполнение неких действий над подполями.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<SubField> subFields,
                [CanBeNull] Action<SubField> action
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null))
                {
                    result.Add(subField);
                    if (!ReferenceEquals(action, null))
                    {
                        action(subField);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] Func<RecordField, bool> fieldPredicate,
                [NotNull] Func<SubField, bool> subPredicate
            )
        {
            Code.NotNull(fields, nameof(fields));
            Code.NotNull(fieldPredicate, nameof(fieldPredicate));
            Code.NotNull(subPredicate, nameof(subPredicate));

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && fieldPredicate(field))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subPredicate(subField))
                        {
                            result.Add(subField);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] int[] tags,
                [NotNull] char[] codes
            )
        {
            Code.NotNull(fields, nameof(fields));
            Code.NotNull(tags, nameof(tags));
            Code.NotNull(codes, nameof(codes));

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subField.Code.OneOf(codes))
                            {
                                result.Add(subField);
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubFieldRegex
            (
                [NotNull] this IEnumerable<SubField> subFields,
                [NotNull] string codeRegex
            )
        {
            Code.NotNull(subFields, nameof(subFields));
            Code.NotNullNorEmpty(codeRegex, nameof(codeRegex));

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null))
                {
                    string code = subField.CodeString;
                    if (!string.IsNullOrEmpty(code)
                        && Regex.IsMatch(code, codeRegex))
                    {
                        result.Add(subField);
                    }

                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubFieldRegex
            (
                [NotNull]this IEnumerable<SubField> subFields,
                [NotNull] char[] codes,
                [NotNull] string textRegex
            )
        {
            Code.NotNull(subFields, nameof(subFields));
            Code.NotNull(codes, nameof(codes));
            Code.NotNullNorEmpty(textRegex, nameof(textRegex));

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null)
                    && subField.Code.OneOf(codes))
                {
                    string value = subField.Value;
                    if (!string.IsNullOrEmpty(value)
                        && Regex.IsMatch(value, textRegex))
                    {
                        result.Add(subField);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] int[] tags,
                [NotNull] char[] codes,
                [NotNull] string valueRegex
            )
        {
            Code.NotNull(fields, nameof(fields));
            Code.NotNull(tags, nameof(tags));
            Code.NotNull(codes, nameof(codes));
            Code.NotNullNorEmpty(valueRegex, nameof(valueRegex));

            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && field.Tag.OneOf(tags))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.OneOf(codes))
                        {
                            string value = subField.Value;
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (Regex.IsMatch(value, valueRegex))
                                {
                                    result.Add(subField);
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
        /// Получение значения подполя.
        /// </summary>
        [CanBeNull]
        public static string GetSubFieldValue
            (
                [CanBeNull] this SubField subField
            )
        {
            return ReferenceEquals(subField, null)
                       ? null
                       : subField.Value;
        }

        /// <summary>
        /// Получение значения подполя.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetSubFieldValue
            (
                [NotNull] this IEnumerable<SubField> subFields
            )
        {
            Code.NotNull(subFields, nameof(subFields));

            LocalList<string> result = new LocalList<string>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null))
                {
                    string value = subField.Value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        result.Add(value);
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Convert the subfield to C# source code.
        /// </summary>
        [NotNull]
        public static string ToSourceCode
            (
                [NotNull] this SubField subfield
            )
        {
            Code.NotNull(subfield, nameof(subfield));

            return string.Format
                (
                    "new SubField({0}, {1})",
                    SourceCodeUtility.ToSourceCode(subfield.Code),
                    SourceCodeUtility.ToSourceCode(subfield.Value)
                );
        }

        // ==========================================================

        /// <summary>
        /// Convert the subfield to <see cref="JObject"/>.
        /// </summary>
        [NotNull]
        public static JObject ToJObject
            (
                [NotNull] this SubField subField
            )
        {
            Code.NotNull(subField, nameof(subField));

            JObject result = JObject.FromObject(subField);

            return result;
        }

        /// <summary>
        /// Convert the subfield to JSON.
        /// </summary>
        [NotNull]
        public static string ToJson
            (
                [NotNull] this SubField subField
            )
        {
            Code.NotNull(subField, nameof(subField));

            string result = JsonUtility.SerializeShort(subField);

            return result;
        }

        /// <summary>
        /// Restore subfield from <see cref="JObject"/>.
        /// </summary>
        [NotNull]
        public static SubField FromJObject
            (
                [NotNull] JObject jObject
            )
        {
            Code.NotNull(jObject, nameof(jObject));

            SubField result = new SubField
                (
                    jObject["code"].ToString().FirstChar()
                );
            JToken value = jObject["value"];
            if (!ReferenceEquals(value, null))
            {
                result.Value = value.ToString();
            }

            return result;
        }

        /// <summary>
        /// Restore subfield from JSON.
        /// </summary>
        public static SubField FromJson
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, nameof(text));

            SubField result = JsonConvert.DeserializeObject<SubField>(text);

            return result;
        }

        #endregion
    }
}
