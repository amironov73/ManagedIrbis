// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BooleanUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class BooleanUtility
    {
        #region Public methods

        /// <summary>
        /// Try parse the boolean value.
        /// </summary>
        /// <remarks>
        /// For WinMobile compatibility.
        /// </remarks>
        public static bool TryParse
            (
                [CanBeNull] string value,
                out bool result
            )
        {
#if PocketPC || WINMOBILE

            result = false;

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            value = value.ToLower();

            switch (value)
            {
                case "false":
                    return true;

                case "true":
                    result = true;
                    return true;
            }

            return false;

#else

            return bool.TryParse(value, out result);

#endif
        }

        #endregion
    }
}
