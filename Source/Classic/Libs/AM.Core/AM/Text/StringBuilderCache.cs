// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StringBuilderCache.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Inspired by the Microsoft internal class
    /// http://referencesource.microsoft.com/#mscorlib/system/text/stringbuildercache.cs
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class StringBuilderCache
    {
        #region Private members

        // TODO implement properly

#if !WINMOBILE
        [ThreadStatic]
#endif
        private static StringBuilder _cachedInstance;

        #endregion

        #region Public methods

        /// <summary>
        /// Acquire the <see cref="StringBuilder"/>.
        /// </summary>
        [NotNull]
        public static StringBuilder Acquire()
        {
            StringBuilder result = _cachedInstance;
            if (ReferenceEquals(result, null))
            {
                result = new StringBuilder();
            }
            else
            {
                result.Clear();
            }

            _cachedInstance = null;

            return result;
        }

        /// <summary>
        /// Get string and release the <see cref="StringBuilder"/>.
        /// </summary>
        [NotNull]
        public static string GetStringAndRelease
            (
                [NotNull] StringBuilder builder
            )
        {
            Code.NotNull(builder, "builder");

            string result = builder.ToString();
            Release(builder);

            return result;
        }

        /// <summary>
        /// Release the <see cref="StringBuilder"/>.
        /// </summary>
        public static void Release
            (
                [NotNull] StringBuilder builder
            )
        {
            Code.NotNull(builder, "builder");

            _cachedInstance = builder;
        }

        #endregion
    }
}

