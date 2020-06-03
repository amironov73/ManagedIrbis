// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OsmiCard.cs --
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
    public sealed class OsmiCard
    {
        #region Properties

        /// <summary>
        /// Values.
        /// </summary>
        [CanBeNull]
        [JsonProperty("values")]
        public OsmiValue[] Values { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert <see cref="JObject"/> to
        /// <see cref="OsmiCard"/>.
        /// </summary>
        [CanBeNull]
        public static OsmiCard FromJObject
            (
                [NotNull] JObject jObject
            )
        {
            Code.NotNull(jObject, "jObject");

            var values = jObject["values"];
            if (ReferenceEquals (values, null))
            {
                return null;
            }
            OsmiCard result = new OsmiCard
            {
                Values = values.ToObject<OsmiValue[]>()
            };

            return result;
        }

        #endregion
    }
}
