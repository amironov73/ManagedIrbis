// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DllCache.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || NETCORE || ANDROID

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    [ExcludeFromCodeCoverage]
    public static class DllCache
    {
        #region Properties

        /// <summary>
        /// DLL registry.
        /// </summary>
        [NotNull]
        public static Dictionary<string, DynamicLibrary> DllRegistry { get; private set; }

        /// <summary>
        /// Delegate registry.
        /// </summary>
        [NotNull]
        public static Dictionary<Pair<string, string>, Delegate> DelegateRegistry
        {
            get;
            private set;
        }

        #endregion

        #region Construction

        static DllCache()
        {
            DllRegistry = new Dictionary<string, DynamicLibrary>
                (
                    StringComparer.InvariantCultureIgnoreCase
                );
            DelegateRegistry = new Dictionary<Pair<string, string>, Delegate>();
            _sync = new object();
        }

        #endregion

        #region Private members

        private static object _sync;

        #endregion

        #region Public methods

        /// <summary>
        /// Free dynamic library.
        /// </summary>
        public static void FreeLibrary
            (
                [NotNull] string libraryName
            )
        {
            Code.NotNullNorEmpty(libraryName, "libraryName");

            lock (_sync)
            {
                DynamicLibrary library;

                if (DllRegistry.TryGetValue(libraryName, out library))
                {
                    DllRegistry.Remove(libraryName);
                    library.Dispose();
                }
            }
        }

        /// <summary>
        /// Get delegate for given function from dynamic library.
        /// </summary>
        [NotNull]
        public static Delegate CreateDelegate
            (
                [NotNull] string libraryName,
                [NotNull] string functionName,
                [NotNull] Type type
            )
        {
            Code.NotNullNorEmpty(libraryName, "libraryName");
            Code.NotNullNorEmpty(functionName, "functionName");
            Code.NotNull(type, "type");

            DynamicLibrary library = LoadLibrary(libraryName);
            Pair<string, string> key = new Pair<string, string>
                (
                    libraryName.ToUpperInvariant(),
                    functionName.ToUpperInvariant()
                );
            Delegate result;
            if (!DelegateRegistry.TryGetValue(key, out result))
            {
                result = library.CreateDelegate
                    (
                        functionName,
                        type
                    );
                DelegateRegistry.Add(key, result);
            }

            return result;
        }

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
                if (!DllRegistry.TryGetValue(libraryName, out result))
                {
                    result = new DynamicLibrary(libraryName);
                    DllRegistry.Add(libraryName, result);
                }
            }

            return result;
        }

        #endregion
    }
}

#endif
