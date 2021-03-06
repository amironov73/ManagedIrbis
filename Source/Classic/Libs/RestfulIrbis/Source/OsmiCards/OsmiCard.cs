﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UseNameofExpression

/* OsmiCard.cs -- карточка пользователя
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// Карточка пользователя системы OSMI Cards.
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
