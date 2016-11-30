// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProgramCache.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    static class ProgramCache
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static Dictionary<string, PftProgram> Registry
        {
            get;
            private set;
        }

        #endregion

        #region Construction

        static ProgramCache()
        {
            Registry = new Dictionary<string, PftProgram>
                (
                    StringComparer.CurrentCultureIgnoreCase
                );
            _sync = new object();
        }

        #endregion

        #region Private members

        // ReSharper disable once InconsistentNaming
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
