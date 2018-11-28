// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FeatureManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Features
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FeatureManager
    {
        #region Private members

        private static readonly HashSet<string> Features;

        #endregion

        #region Construction

        static FeatureManager()
        {
            Features = new HashSet<string>();
            RegisterFeature(StandardFeatures.IPv6);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Have the feature?
        /// </summary>
        public static bool HaveFeature
            (
                [NotNull] string featureName
            )
        {
            Code.NotNullNorEmpty(featureName, "featureName");

            return Features.Contains(featureName);
        }

        /// <summary>
        /// Register the feature.
        /// </summary>
        public static void RegisterFeature
            (
                [NotNull] string featureName
            )
        {
            Code.NotNullNorEmpty(featureName, "featureName");

            Features.Add(featureName);
        }

        #endregion
    }
}