// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* JsonUtility.cs -- helper routines for JSON
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace AM.Json
{
    /// <summary>
    /// Helper routines for JSON.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class JsonUtility
    {
        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Expand $type's.
        /// </summary>
        public static void ExpandTypes
            (
                [NotNull] JObject obj,
                [NotNull] string nameSpace,
                [NotNull] string assembly
            )
        {
            Code.NotNull(obj, "obj");
            Code.NotNullNorEmpty(nameSpace, "nameSpace");
            Code.NotNullNorEmpty(assembly, "assembly");

            IEnumerable<JValue> values = obj
                .SelectTokens("$..$type")
                .OfType<JValue>();
            foreach (JValue value in values)
            {
                string typeName = value.Value.ToString();
                if (!typeName.Contains('.'))
                {
                    typeName = nameSpace
                               + "."
                               + typeName
                               + ", "
                               + assembly;
                    value.Value = typeName;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Include
            (
                [NotNull] JObject obj,
                [NotNull] Action<JProperty> resolver
            )
        {
            Code.NotNull(obj, "obj");
            Code.NotNull(resolver, "resolver");

            JValue[] values = obj
                .SelectTokens("$..$include")
                .OfType<JValue>()
                .ToArray();

            foreach (JValue value in values)
            {
                JProperty property = (JProperty) value.Parent;
                resolver(property);
            }
        }

#if !WIN81

        /// <summary>
        /// 
        /// </summary>
        public static void Include
            (
                [NotNull] JObject obj
            )
        {
            Code.NotNull(obj, "obj");

            JToken[] tokens = obj
                .SelectTokens("$..$include")
                .ToArray();

            foreach (JToken token in tokens)
            {
                JProperty property = (JProperty)token.Parent;
                Resolve(property);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Include
            (
                [NotNull] JObject obj,
                [NotNull] string newName
            )
        {
            Code.NotNull(obj, "obj");
            Code.NotNullNorEmpty(newName, "newName");

            Action<JProperty> resolver = prop =>
            {
                Resolve(prop, newName);
            };
            Include(obj, resolver);
        }

        /// <summary>
        /// Read <see cref="JArray"/> from specified
        /// local file.
        /// </summary>
        [NotNull]
        public static JArray ReadArrayFromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = File.ReadAllText(fileName);
            JArray result = JArray.Parse(text);

            return result;
        }

        /// <summary>
        /// Read <see cref="JObject"/> from specified
        /// local JSON file.
        /// </summary>
        [NotNull]
        public static JObject ReadObjectFromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = File.ReadAllText(fileName);
            JObject result = JObject.Parse(text);

            return result;
        }

        /// <summary>
        /// Read arbitrary object from specified
        /// local JSON file.
        /// </summary>
        [NotNull]
        public static T ReadObjectFromFile<T>
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = File.ReadAllText(fileName);
            T result = JsonConvert.DeserializeObject<T>(text);

            return result;
        }

        /// <summary>
        /// Save the <see cref="JArray"/>
        /// to the specified local file.
        /// </summary>
        public static void SaveArrayToFile
            (
                [NotNull] JArray array,
                [NotNull] string fileName
            )
        {
            Code.NotNull(array, "array");
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = array.ToString
                (
                    Formatting.Indented
                );
            File.WriteAllText
                (
                    fileName,
                    text
                );
        }

        /// <summary>
        /// Save object to the specified local JSON file.
        /// </summary>
        public static void SaveObjectToFile
            (
                [NotNull] JObject obj,
                [NotNull] string fileName
            )
        {
            Code.NotNull(obj, "obj");
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = obj.ToString
                (
                    Formatting.Indented
                );
            File.WriteAllText
                (
                    fileName,
                    text
                );
        }

        /// <summary>
        /// Save object to the specified local JSON file.
        /// </summary>
        public static void SaveObjectToFile
            (
                [NotNull] object obj,
                [NotNull] string fileName
            )
        {
            JObject json = JObject.FromObject(obj);

            SaveObjectToFile
                (
                    json,
                    fileName
                );
        }

        /// <summary>
        /// Resolver for <see cref="Include(JObject,Action{JProperty})"/>.
        /// </summary>
        public static void Resolve
            (
                [NotNull] JProperty property,
                [NotNull] string newName
            )
        {
            Code.NotNull(property, "property");
            Code.NotNull(newName, "newName");

            // TODO use path for searching

            string fileName = property.Value.ToString();
            string text = File.ReadAllText(fileName);
            JObject value = JObject.Parse(text);
            JProperty newProperty = new JProperty(newName, value);
            property.Replace(newProperty);
        }

        /// <summary>
        /// Resolver for <see cref="Include(JObject,Action{JProperty})"/>.
        /// </summary>
        public static void Resolve
            (
                [NotNull] JProperty property
            )
        {
            Code.NotNull(property, "property");

            // TODO use path for searching

            JObject obj = (JObject) property.Value;
            string newName = obj["name"].Value<string>();
            string fileName = obj["file"].Value<string>();
            string text = File.ReadAllText(fileName);
            JObject value = JObject.Parse(text);
            JProperty newProperty = new JProperty(newName, value);
            property.Replace(newProperty);
        }

#endif

        #endregion
    }
}
