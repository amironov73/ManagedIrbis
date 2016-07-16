/* IrbisMenuUtility.cs -- MNU file extended handling.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AM;
using AM.Collections;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// MNU file extended handling.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisMenuUtility
    {
        #region Public methods

        /// <summary>
        /// Adds the typed value with specified code.
        /// </summary>
        public static IrbisMenuFile Add<T>
            (
                [NotNull] this IrbisMenuFile menu,
                [NotNull] string code,
                [CanBeNull] T value
            )
        {
            Code.NotNull(menu, "menu");
            Code.NotNull(code, "code");

            if (ReferenceEquals(value, null))
            {
                menu.Add(code, string.Empty);
            }
            else
            {
                string textValue =
                    ConversionUtility
                        .ConvertTo<string>(value);
                menu.Add(code, textValue);
            }

            return menu;
        }

        /// <summary>
        /// Collects the comments for code.
        /// </summary>
        [NotNull]
        public static string[] CollectStrings
            (
            [NotNull] this IrbisMenuFile menu,
            [NotNull] string code
            )
        {
            return menu.Entries
                .Where
                (
                    entry => entry.Code.SameString(code)
                             || IrbisMenuFile.TrimCode(entry.Code)
                                 .SameString(code)
                )
                .Select(entry => entry.Comment)
                .ToArray();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        [CanBeNull]
        public static T GetValue<T>
            (
                [NotNull] this IrbisMenuFile menu,
                [NotNull] string code,
                [CanBeNull] T defaultValue
            )
        {
            Code.NotNull(menu, "menu");
            Code.NotNull(code, "code");

            IrbisMenuFile.Entry found = menu.FindEntry(code);

            return found == null
                ? defaultValue
                : ConversionUtility.ConvertTo<T>(found.Comment);
        }

        /// <summary>
        /// Gets the value (case sensitive).
        /// </summary>
        [CanBeNull]
        public static T GetValueSensitive<T>
            (
                [NotNull] this IrbisMenuFile menu,
                [NotNull] string code,
                T defaultValue
            )
        {
            Code.NotNull(menu, "menu");
            Code.NotNull(code, "code");

            IrbisMenuFile.Entry found = menu.FindEntrySensitive(code);

            return found == null
                ? defaultValue
                : ConversionUtility.ConvertTo<T>(found.Comment);
        }

        /// <summary>
        /// Converts the menu to JSON.
        /// </summary>
        [NotNull]
        public static string ToJson
            (
                [NotNull] this IrbisMenuFile menu
            )
        {
            Code.NotNull(menu, "menu");

            string result = JArray.FromObject(menu.Entries)
                .ToString(Formatting.None);

            return result;
        }

        /// <summary>
        /// Restores the menu from JSON.
        /// </summary>
        [NotNull]
        public static IrbisMenuFile FromJson
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            NonNullCollection<IrbisMenuFile.Entry> entries 
                = JsonConvert
                .DeserializeObject<NonNullCollection<IrbisMenuFile.Entry>>
                    (
                        text
                    );
            IrbisMenuFile result = new IrbisMenuFile(entries);

            return result;
        }

        /// <summary>
        /// Saves the menu to local JSON file.
        /// </summary>
        public static void SaveLocalJsonFile
            (
                [NotNull] this IrbisMenuFile menu,
                [NotNull] string fileName
            )
        {
            Code.NotNull(menu, "menu");
            Code.NotNullNorEmpty(fileName, "fileName");

            string contents = JArray.FromObject(menu.Entries)
                .ToString(Formatting.Indented);

            File.WriteAllText
                (
                    fileName, 
                    contents, 
                    IrbisEncoding.Utf8
                );
        }

        /// <summary>
        /// Parses the local json file.
        /// </summary>
        [NotNull]
        public static IrbisMenuFile ParseLocalJsonFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            IrbisMenuFile result = FromJson(text);

            return result;
        }

        /// <summary>
        /// Converts the menu to XML.
        /// </summary>
        [NotNull]
        public static string ToXml
            (
                [NotNull] this IrbisMenuFile menu
            )
        {
            Code.NotNull(menu, "menu");

            XmlSerializer serializer
                = new XmlSerializer(typeof(IrbisMenuFile));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, menu);

            return writer.ToString();
        }

        #endregion
    }
}
