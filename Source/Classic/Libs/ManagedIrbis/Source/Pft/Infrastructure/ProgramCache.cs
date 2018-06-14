// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProgramCache.cs -- simple cache for PFT scripts.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Simple cache for PFT scripts.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ProgramCache
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        static Dictionary<string, PftProgram> Registry
        {
            get;
            set;
        }

        #endregion

        #region Construction

        static ProgramCache()
        {
            Registry = new Dictionary<string, PftProgram>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add program for the sourceText.
        /// </summary>
        public static void AddProgram
            (
                [NotNull] string sourceText,
                [NotNull] PftProgram program
            )
        {
            Code.NotNull(sourceText, "sourceText");
            Code.NotNull(program, "program");

            lock (Registry)
            {
                Registry[sourceText] = program;
            }
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public static void Clear()
        {
            lock (Registry)
            {
                Registry.Clear();
            }
        }

        /// <summary>
        /// Get program for the text.
        /// </summary>
        [CanBeNull]
        public static PftProgram GetProgram
            (
                [CanBeNull] string sourceText
            )
        {
            if (string.IsNullOrEmpty(sourceText))
            {
                return null;
            }

            lock (Registry)
            {
                PftProgram result;
                Registry.TryGetValue(sourceText, out result);

                return result;
            }
        }

        #endregion
    }
}
