// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProgramCache.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    static class ProgramCache
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static CaseInsensitiveDictionary<PftProgram> Registry
        {
            get;
            private set;
        }

        #endregion

        #region Construction

        static ProgramCache()
        {
            Registry = new CaseInsensitiveDictionary<PftProgram>();
            _sync = new object();
        }

        #endregion

        #region Private members

        private static readonly object _sync;

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

            lock (_sync)
            {
                Registry[sourceText] = program;
            }
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public static void Clear()
        {
            lock (_sync)
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

            lock (_sync)
            {
                PftProgram result;
                Registry.TryGetValue(sourceText, out result);

                return result;
            }
        }

        #endregion
    }
}
