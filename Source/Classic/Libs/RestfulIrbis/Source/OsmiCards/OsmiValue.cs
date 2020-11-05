// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UseNameofExpression
// ReSharper disable UseStringInterpolation

/* OsmiValue.cs -- пара "ключ-значение" в карте
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
    /// Пара "ключ-значение" в карте.
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

            OsmiValue value = jObject.ToObject<OsmiValue>().ThrowIfNull();

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
