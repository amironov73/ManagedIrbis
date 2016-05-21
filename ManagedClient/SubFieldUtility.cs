/* SubFieldUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
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
    public static class SubFieldUtility
    {
        #region Private members

        #endregion

        #region Public methods

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
            Code.NotNull(subFields, "subFields");

            return subFields
                .Where(sub => sub.Code.SameChar(code))
                .ToArray();
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
            Code.NotNull(subFields, "subFields");

            return subFields
                .Where(sub => sub.Code.OneOf(codes))
                .ToArray();
        }

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
            Code.NotNull(subFields, "subFields");
            Code.NotNull(codeRegex, "codeRegex");

            return subFields
                .Where
                (
                    subField => 
                        !ReferenceEquals(subField.CodeString, null)
                        && Regex.IsMatch
                            (
                                subField.CodeString,
                                codeRegex
                            )
                )
                .ToArray();
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
            Code.NotNull(subFields, "subFields");
            Code.NotNull(codes, "codes");
            Code.NotNull(textRegex, "textRegex");

            return subFields
                .GetSubField(codes)
                .Where
                (
                    subField => !ReferenceEquals(subField.Value, null)
                              && Regex.IsMatch
                                (
                                    subField.Value,
                                    textRegex
                                )
                              )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                [NotNull] char[] codes,
                [NotNull] string textRegex
            )
        {
            return fields
                .GetField(tags)
                .AllSubFields()
                .Where
                    (
                        subField => !ReferenceEquals(subField.Value, null)
                              && Regex.IsMatch
                                (
                                    subField.Value,
                                    textRegex
                                )
                              
                    )
                .ToArray();
        }

        /// <summary>
        /// Получение значения подполя.
        /// </summary>
        [CanBeNull]
        public static string GetSubFieldValue
            (
                [CanBeNull] this SubField subField
            )
        {
            return subField == null
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
            Code.NotNull(subFields, "subFields");

            return subFields
                .NonNullItems()
                .Select(subField => subField.Value)
                .NonEmptyLines()
                .ToArray();
        }

        /// <summary>
        /// Получение значения подполя.
        /// </summary>
        [CanBeNull]
        public static string GetSubFieldText
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [CanBeNull] string tag,
                char code
            )
        {
            Code.NotNull(fields, "fields");

            return fields
                .NonNullItems()
                .GetField(tag)
                .GetSubField(code)
                .FirstOrDefault()
                .GetSubFieldValue();
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
            Code.NotNull(subFields, "subFields");

            SubField[] result = subFields.ToArray();

            if (!ReferenceEquals(action, null))
            {
                foreach (SubField subField in result)
                {
                    action(subField);
                }
            }

            return result;
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
            Code.NotNull(fields, "fields");
            Code.NotNull(fieldPredicate, "fieldPredicate");
            Code.NotNull(subPredicate, "subPredicate");

            return fields
                .NonNullItems()
                .Where(fieldPredicate)
                .NonNullItems()
                .GetSubField()
                .Where(subPredicate)
                .ToArray();
        }

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                [NotNull] char[] codes
            )
        {
            Code.NotNull(fields, "fields");
            Code.NotNull(tags, "tags");
            Code.NotNull(codes, "codes");

            return fields
                .NonNullItems()
                .GetField(tags)
                .NonNullItems()
                .GetSubField(codes)
                .ToArray();
        }

        #endregion
    }
}
