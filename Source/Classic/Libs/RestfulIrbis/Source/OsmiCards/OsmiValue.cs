// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OsmiValue.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class OsmiValue
    {
        #region Properties

        /// <summary>
        /// Label.
        /// </summary>
        [CanBeNull]
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Alternative value.
        /// </summary>
        [CanBeNull]
        [JsonProperty("altValue")]
        public string AltValue { get; set; }

        /// <summary>
        /// Change message.
        /// </summary>
        [CanBeNull]
        [JsonProperty("changeMsg")]
        public string ChangeMessage { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert <see cref="JObject"/> to
        /// <see cref="OsmiValue"/>.
        /// </summary>
        [NotNull]
        public static OsmiValue FromJObject
            (
                [NotNull] JObject jObject
            )
        {
            Code.NotNull(jObject, "jObject");

            OsmiValue value = jObject.ToObject<OsmiValue>();

            return value;
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format
                (
                    "{0} - {1}",
                    Label.ToVisibleString(),
                    Value.ToVisibleString()
                );
        }

        #endregion
    }
}
