/* DllCache.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// DLL cache.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class DllCache
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static Dictionary<string, DynamicLibrary> Registry { get; private set; }

        #endregion

        #region Construction

        static DllCache()
        {
            Registry = new Dictionary<string, DynamicLibrary>
                (
                    StringComparer.InvariantCultureIgnoreCase
                );
            _sync = new object();
        }

        #endregion

        #region Private members

        private static object _sync;

        #endregion

        #region Public methods

        /// <summary>
        /// Load dynamic library.
        /// </summary>
        [NotNull]
        public static DynamicLibrary LoadLibrary
            (
                [NotNull] string libraryName
            )
        {
            Code.NotNullNorEmpty(libraryName, "libraryName");

            DynamicLibrary result;

            lock (_sync)
            {
                if (!Registry.TryGetValue(libraryName, out result))
                {
                    result = new DynamicLibrary(libraryName);
                    Registry.Add(libraryName, result);
                }
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
