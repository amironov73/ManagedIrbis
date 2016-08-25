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
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
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

        #endregion
    }
}
