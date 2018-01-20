// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ContinuousIntegrationUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Detect CI build environments.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static class ContinuousIntegrationUtility
    {
        #region Public methods

        /// <summary>
        /// Detect AppVeyor CI environment.
        /// </summary>
        public static bool DetectAppVeyor()
        {
#if WINMOBILE || PocketPC

            return false;

#else

            return Environment.GetEnvironmentVariable("APPVEYOR") == "True";

#endif
        }

        /// <summary>
        /// Detect generic CI environment.
        /// </summary>
        public static bool DetectCI()
        {
#if WINMOBILE || PocketPC

            return false;

#else

            return Environment.GetEnvironmentVariable("CI") == "True";

#endif
        }

        /// <summary>
        /// Detect generic Travis CI environment.
        /// </summary>
        public static bool DetectTravis()
        {
#if WINMOBILE || PocketPC

            return false;

#else

            return Environment.GetEnvironmentVariable("TRAVIS") == "True";

#endif
        }

        #endregion
    }
}
