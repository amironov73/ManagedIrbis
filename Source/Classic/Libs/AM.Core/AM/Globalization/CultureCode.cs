// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CultureCode.cs -- 
 * Ars Magna project, https://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class CultureCode
    {
        /// <summary>
        /// 
        /// </summary>
        public const string AmericanEnglish = "en-US";

        /// <summary>
        /// 
        /// </summary>
        public const string Russian = "ru-RU";
    }
}
