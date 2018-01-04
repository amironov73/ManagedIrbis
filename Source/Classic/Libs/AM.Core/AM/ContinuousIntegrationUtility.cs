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
            return Environment.GetEnvironmentVariable("APPVEYOR") == "True";
        }

        /// <summary>
        /// Detect generic CI environment.
        /// </summary>
        public static bool DetectCI()
        {
            return Environment.GetEnvironmentVariable("CI") == "True";
        }

        /// <summary>
        /// Detect generic Travis CI environment.
        /// </summary>
        public static bool DetectTravis()
        {
            return Environment.GetEnvironmentVariable("TRAVIS") == "True";
        }

        #endregion
    }
}
