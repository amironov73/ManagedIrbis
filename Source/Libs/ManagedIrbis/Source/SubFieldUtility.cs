/* SubFieldUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis
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
        /// Первое вхождение подполя с указанным кодом.
        /// </summary>
        [CanBeNull]
        public static SubField GetFirstSubField
            (
                [NotNull] this IEnumerable<SubField> subFields,
                char code
            )
        {
            Code.NotNull(subFields, "subFields");

            return subFields
                .FirstOrDefault(sub => sub.Code.SameChar(code));
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
            Code.NotNull(subFields, "subFields");

            return subFields
                .FirstOrDefault(sub => sub.Code.OneOf(codes));
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
            Code.NotNull(subFields, "subFields");

            return subFields
                .FirstOrDefault
                (
                    sub => sub.Code.SameChar(code)
                           && sub.Value.SameStringSensitive(value)
                );
        }

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

#if !WINMOBILE && !PocketPC

        /// <summary>
        /// Convert the subfield to <see cref="JObject"/>.
        /// </summary>
        [NotNull]
        public static JObject ToJObject
            (
                [NotNull] this SubField subField
            )
        {
            Code.NotNull(subField, "subField");

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
            Code.NotNull(subField, "subField");

            string result = JObject.FromObject(subField).ToString();

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
            Code.NotNull(jObject, "jObject");

            SubField result = new SubField
                (
                    jObject["code"].ToString()[0],
                    jObject["value"].ToString()
                );

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
            Code.NotNullNorEmpty(text, "text");

            SubField result = JsonConvert.DeserializeObject<SubField>(text);

            return result;
        }

#endif

#if !NETCORE

        /// <summary>
        /// Convert the subfield to XML.
        /// </summary>
        [NotNull]
        public static string ToXml
            (
                [NotNull] this SubField subField
            )
        {
            Code.NotNull(subField, "subField");

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                NewLineOnAttributes = true,
                Indent = true,
                CloseOutput = true
            };
            StringWriter writer = new StringWriter();
            XmlWriter xml = XmlWriter.Create(writer, settings);
            XmlSerializer serializer = new XmlSerializer
                (
                    typeof(SubField)
                );
            serializer.Serialize(writer, subField);
            
            return writer.ToString();
        }

        /// <summary>
        /// Restore the subfield from XML.
        /// </summary>
        [NotNull]
        public static SubField FromXml
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            XmlSerializer serializer = new XmlSerializer(typeof(SubField));
            StringReader reader = new StringReader(text);
            SubField result = (SubField) serializer.Deserialize(reader);
            
            return result;
        }

#endif

        #endregion
    }
}
