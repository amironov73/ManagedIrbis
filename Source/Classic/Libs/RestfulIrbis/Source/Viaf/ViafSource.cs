// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ViafSource.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

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
    public class ViafSource
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string Nsid { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Public method

        /// <summary>
        /// Parse the object.
        /// </summary>
        [NotNull]
        public static ViafSource Parse
            (
                [NotNull] JObject obj
            )
        {
            return new ViafSource
            {
                Nsid = obj["@nsid"].NullableToString(),
                Text = obj["#text"].NullableToString()
            };
        }

        /// <summary>
        /// Parse the array.
        /// </summary>
        [NotNull]
        public static ViafSource[] Parse
            (
                [CanBeNull] JArray array
            )
        {
            if (ReferenceEquals(array, null))
            {
                return new ViafSource[0];
            }

            ViafSource[] result = new ViafSource[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                result[i] = Parse((JObject)array[i]);
            }

            return result;
        }

        #endregion
    }
}
