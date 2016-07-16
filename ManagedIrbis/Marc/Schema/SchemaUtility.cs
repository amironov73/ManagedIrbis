/* SchemaUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

#endregion

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class SchemaUtility
    {
        #region Public methods

        /// <summary>
        /// Get attribute boolean value for element.
        /// </summary>
        public static bool GetAttributeBoolean
            (
                [NotNull] this XElement element,
                [NotNull] string attributeName
            )
        {
            Code.NotNull(element, "element");
            Code.NotNull(attributeName, "attributeName");

            string value = element.Attribute(attributeName).Value;

            return value.SameString("y");
        }

        /// <summary>
        /// Get attribute boolean value for element.
        /// </summary>
        public static bool GetAttributeBoolean
            (
                [NotNull] this XElement element,
                [NotNull] string attributeName,
                bool defaultValue
            )
        {
            Code.NotNull(element, "element");
            Code.NotNull(attributeName, "attributeName");

            XAttribute attribute = element.Attribute(attributeName);
            if (ReferenceEquals(attribute, null))
            {
                return defaultValue;
            }

            return attribute.Value.SameString("y");
        }

        /// <summary>
        /// Get first char of attribute value.
        /// </summary>
        /// <returns></returns>
        public static char GetAttributeCharacter
            (
                [NotNull] this XElement element,
                [NotNull] string attributeName
            )
        {
            Code.NotNull(element, "element");
            Code.NotNull(attributeName, "attributeName");

            XAttribute attribute = element.Attribute(attributeName);
            if (ReferenceEquals(attribute, null))
            {
                return '\0';
            }

            string value = attribute.Value;

            return string.IsNullOrEmpty(value)
                ? '\0'
                : value[0];
        }

        /// <summary>
        /// Get integer value for given attribute.
        /// </summary>
        public static int GetAttributeInt32
            (
                [NotNull] this XElement element,
                [NotNull] string attributeName,
                int defaultValue
            )
        {
            Code.NotNull(element, "element");
            Code.NotNull(attributeName, "attributeName");

            XAttribute attribute = element.Attribute(attributeName);
            if (ReferenceEquals(attribute, null))
            {
                return defaultValue;
            }

            int result;
            if (!int.TryParse(attribute.Value, out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get attribute text for element.
        /// </summary>
        [CanBeNull]
        public static string GetAttributeText
            (
                [NotNull] this XElement element,
                [NotNull] string attributeName
            )
        {
            Code.NotNull(element, "element");
            Code.NotNull(attributeName, "attributeName");

            return element.Attribute(attributeName).Value;
        }

        /// <summary>
        /// Get attribute text for element.
        /// </summary>
        [CanBeNull]
        public static string GetAttributeText
            (
                [NotNull] this XElement element,
                [NotNull] string attributeName,
                [CanBeNull] string defaultValue
            )
        {
            Code.NotNull(element, "element");
            Code.NotNull(attributeName, "attributeName");

            XAttribute attribute = element.Attribute(attributeName);
            return (ReferenceEquals(attribute, null))
                ? defaultValue
                : attribute.Value;
        }

        /// <summary>
        /// Get attribute text for element.
        /// </summary>
        [CanBeNull]
        public static string GetElementText
            (
                [NotNull] this XElement element,
                [NotNull] string elementName,
                [CanBeNull] string defaultValue
            )
        {
            Code.NotNull(element, "element");
            Code.NotNull(elementName, "elementName");

            XElement subElement = element.Element(elementName);
            return (ReferenceEquals(subElement, null))
                ? defaultValue
                : subElement.Value;
        }

        /// <summary>
        /// Get inner XML for the element.
        /// </summary>
        [CanBeNull]
        public static string GetInnerXml
            (
                [NotNull] this XElement element
            )
        {
            Code.NotNull(element, "element");

            XmlReader reader = element.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }


        /// <summary>
        /// Get inner XML for child element.
        /// </summary>
        [CanBeNull]
        public static string GetInnerXml
            (
                [NotNull] this XElement element,
                [NotNull] string elementName
            )
        {
            Code.NotNull(element, "element");
            Code.NotNull(elementName, "elementName");

            XElement subElement = element.Element(elementName);
            if (ReferenceEquals(subElement, null))
            {
                return null;
            }

            XmlReader reader = subElement.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool ToBoolean
            (
                [NotNull] this XAttribute attribute
            )
        {
            Code.NotNull(attribute, "attribute");

            string value = attribute.Value;

            return value.SameString("y");
        }

        /// <summary>
        /// Convert schema to JSON.
        /// </summary>
        [NotNull]
        public static string ToJson
            (
                [NotNull] this MarcSchema schema
            )
        {
            Code.NotNull(schema, "schema");

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //string result = JsonConvert.SerializeObject
            //    (
            //        schema,
            //        typeof (MarcSchema),
            //        Formatting.Indented,
            //        settings
            //    );

            string result = JObject.FromObject(schema).ToString();

            return result;
        }

        #endregion
    }
}
