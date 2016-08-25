/* BuiltinCultures.cs -- 
 * Ars Magna project, https://arsmagna.ru
 */

#region Using directives

using System.Globalization;

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
    public static class BuiltinCultures
    {
        #region Properties

        /// <summary>
        /// Gets the american english.
        /// </summary>
        [NotNull]
        public static CultureInfo AmericanEnglish
        {
            get
            {
                return new CultureInfo(CultureCode.AmericanEnglish);
            }
        }

        /// <summary>
        /// Gets the russian culture.
        /// </summary>
        [NotNull]
        public static CultureInfo Russian
        {
            get
            {
                return new CultureInfo(CultureCode.Russian);
            }
        }

        #endregion
    }
}
