// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ViafMainHeading.cs --
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
    public class ViafMainHeading
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string[] Sources { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string[] Sid { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the object.
        /// </summary>
        [NotNull]
        public static ViafMainHeading Parse
            (
                [NotNull] JObject obj
            )
        {
            return new ViafMainHeading
            {
                Text = obj["text"].NullableToString(),
                Sources = obj["sources"]["s"].GetValues<string>(),
                Sid = obj["sources"]["sid"].GetValues<string>()
            };
        }

        /// <summary>
        /// Parse the array.
        /// </summary>
        [NotNull]
        public static ViafMainHeading[] Parse
            (
                [CanBeNull] JArray array
            )
        {
            if (ReferenceEquals(array, null))
            {
                return new ViafMainHeading[0];
            }

            ViafMainHeading[] result = new ViafMainHeading[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                result[i] = Parse((JObject)array[i]);
            }

            return result;
        }

        #endregion
    }
}
