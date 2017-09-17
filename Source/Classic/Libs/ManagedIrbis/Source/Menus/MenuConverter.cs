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

        /// <inheritdoc cref="JsonConverter.WriteJson" />
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

        /// <inheritdoc cref="JsonConverter.ReadJson" />
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

        /// <inheritdoc cref="JsonConverter.CanConvert" />
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

