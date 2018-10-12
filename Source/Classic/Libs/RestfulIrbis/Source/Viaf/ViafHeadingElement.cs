// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ViafHeadingElement.cs --
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
    public class ViafHeadingElement
    {
        #region Properties

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the object.
        /// </summary>
        [NotNull]
        public static ViafHeadingElement Parse
            (
                [NotNull] JObject obj
            )
        {
            return new ViafHeadingElement
            {
            };
        }

        /// <summary>
        /// Parse the array.
        /// </summary>
        [NotNull]
        public static ViafHeadingElement[] Parse
            (
                [CanBeNull] JArray array
            )
        {
            if (ReferenceEquals(array, null))
            {
                return new ViafHeadingElement[0];
            }

            ViafHeadingElement[] result = new ViafHeadingElement[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                result[i] = Parse((JObject)array[i]);
            }

            return result;
        }

        #endregion
    }
}
