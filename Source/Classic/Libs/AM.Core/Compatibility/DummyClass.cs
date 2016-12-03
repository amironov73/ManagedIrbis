// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DummyClass.cs -- to make compiler happy
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace Compatibility
{
    /// <summary>
    /// Dummy class to make compiler happy.
    /// </summary>
    [PublicAPI]
    public static class DummyClass
    {
        #region Public methods

        /// <summary>
        /// Returns hello string.
        /// </summary>
        [NotNull]
        public static string Hello()
        {
            return "Hello!";
        }

        #endregion
    }
}
