// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SpecialSettings.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [JsonConverter(typeof(SpecialSettingsConverter))]
#endif
    public sealed class SpecialSettings
    {
        #region Nested classes

#if !WINMOBILE && !PocketPC

        class SpecialSettingsConverter
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
                throw new NotImplementedException();
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
                SpecialSettings result = new SpecialSettings();
                JToken token = JToken.Load(reader);
                foreach (JProperty child in token.Children<JProperty>())
                {
                    string value = child.Value.ToString();
                    if (child.Name == "name")
                    {
                        result.Name = value;
                    }
                    else
                    {
                        result.Dictionary.Add
                            (
                                child.Name,
                                value
                            );
                    }
                }

                return result;
            }

            /// <inheritdoc cref="JsonConverter.CanConvert" />
            public override bool CanConvert
                (
                    Type objectType
                )
            {
                return true;
            }

            #endregion
        }

#endif

        #endregion

        #region Properties

        /// <summary>
        /// Dictionary.
        /// </summary>
        [NotNull]
        public StringDictionary Dictionary { get; private set; }

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SpecialSettings()
        {
            Dictionary = new StringDictionary();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
