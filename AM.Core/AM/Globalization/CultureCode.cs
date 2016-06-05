/* CultureCode.cs -- 
 * Ars Magna project, https://arsmagna.ru
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
