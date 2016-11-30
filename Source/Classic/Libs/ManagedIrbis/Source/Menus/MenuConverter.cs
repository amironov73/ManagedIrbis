// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuConverter.cs -- convert MNU structure to JSON
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC

#region Using directives

using System;

using AM.Collections;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// Converts the <see cref="MenuFile"/> to JSON.
    /// </summary>
    public sealed class MenuConverter
        : JsonConverter
    {
        #region JsonConverter members

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The
        /// <see cref="T:Newtonsoft.Json.JsonWriter" />
        /// to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.
        /// </param>
        public override void WriteJson
            (
                JsonWriter writer,
                object value,
                JsonSerializer serializer
            )
        {
            MenuFile menu = (MenuFile)value;
            serializer.Serialize(writer, menu.Entries);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The
        /// <see cref="T:Newtonsoft.Json.JsonReader" />
        /// to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value
        /// of object being read.</param>
        /// <param name="serializer">The calling serializer.
        /// </param>
        /// <returns>The object value.</returns>
        public override object ReadJson
            (
                JsonReader reader,
                Type objectType,
                object existingValue,
                JsonSerializer serializer
            )
        {
            MenuFile menu = (MenuFile)existingValue;
            NonNullCollection<MenuEntry> entries = serializer
                .Deserialize<NonNullCollection<MenuEntry>>
                    (
                        reader
                    );
            menu._entries.AddRange(entries);

            return menu;
        }

        /// <summary>
        /// Determines whether this instance can convert
        /// the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert
        /// the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert
            (
                Type objectType
            )
        {
            return objectType == typeof(MenuFile);
        }

        #endregion
    }
}

#endif

