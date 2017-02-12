// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OsmiImage.cs --
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
    public sealed class OsmiImage
    {
        #region Properties

        /// <summary>
        /// Image type: logo, strip etc.
        /// </summary>
        [CanBeNull]
        [JsonProperty("imgType")]
        public string ImageType { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [CanBeNull]
        [JsonProperty("imgDescription")]
        public string Description { get; set; }

        /// <summary>
        /// Identifier.
        /// </summary>
        [CanBeNull]
        [JsonProperty("imgId")]
        public string Id { get; set; }

        /// <summary>
        /// Usage count.
        /// </summary>
        [JsonProperty("usageCount")]
        public int UsageCount { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert <see cref="JObject"/> to
        /// <see cref="OsmiImage"/>.
        /// </summary>
        [NotNull]
        public static OsmiImage FromJObject
            (
                [NotNull] JObject jObject
            )
        {
            Code.NotNull(jObject, "jObject");

            OsmiImage value = jObject.ToObject<OsmiImage>();

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
                    Id.ToVisibleString(),
                    Description.ToVisibleString()
                );
        }

        #endregion
    }
}
