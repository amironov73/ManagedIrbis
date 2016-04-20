/* BuiltinCultures.cs -- 
 * Ars Magna project, https://arsmagna.ru
 */

#region Using directives

using System.Globalization;

using JetBrains.Annotations;

#endregion

namespace AM.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class BuiltinCultures
    {
        #region Properties

        /// <summary>
        /// Gets the american english.
        /// </summary>
        /// <value>The american english.</value>
        public static CultureInfo AmericanEnglish
        {
            get
            {
                return new CultureInfo(CultureCode.Russian);
            }
        }

        /// <summary>
        /// Gets the russian culture.
        /// </summary>
        /// <value>The russian.</value>
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
