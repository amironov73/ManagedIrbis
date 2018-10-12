// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ViafLink.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Json;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json.Linq;

#endregion

// ReSharper disable StringLiteralTypo

namespace RestfulIrbis.Viaf
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ViafLink
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string S { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Sid { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the object.
        /// </summary>
        [NotNull]
        public static ViafLink Parse
            (
                [NotNull] JObject obj
            )
        {
            return new ViafLink
            {
                Url = obj["#text"].NullableToString(),
                S = obj["sources"]["s"].NullableToString(),
                Sid = obj["sources"]["sid"].NullableToString()
            };
        }

        /// <summary>
        /// Parse the array.
        /// </summary>
        [NotNull]
        public static ViafLink[] Parse
            (
                [CanBeNull] JArray array
            )
        {
            if (ReferenceEquals(array, null))
            {
                return new ViafLink[0];
            }

            ViafLink[] result = new ViafLink[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                result[i] = Parse((JObject)array[i]);
            }

            return result;
        }

        #endregion
    }
}
