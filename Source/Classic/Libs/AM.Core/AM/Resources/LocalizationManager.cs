// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalizationManager.cs -- localization manager
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || DROID

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Resources;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Resources
{
    /// <summary>
    /// Localization manager. Inspired by same name RSDN code.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LocalizationManager
    {
        #region Properties

        private Dictionary<string, ResourceManager> _managers;

        private static LocalizationManager _instance;

        /// <summary>
        /// Singleton.
        /// </summary>
        [NotNull]
        public static LocalizationManager Instance
        {
            [DebuggerStepThrough]
            get
            {
                lock (typeof(LocalizationManager))
                {
                    return _instance
                        ?? (_instance = new LocalizationManager());
                }
            }
        }

        #endregion

        #region Construction

        private LocalizationManager()
        {
            _managers = new Dictionary<string, ResourceManager>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get resource manager for given assembly and resource file.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="resFile">Resource file.</param>
        /// <returns>Resource manager.</returns>
        public ResourceManager GetResourceManager
            (
                [NotNull] Assembly assembly,
                [NotNull] string resFile
            )
        {
            Code.NotNull(assembly, "assembly");
            Code.NotNull(resFile, "resFile");

            string rmName = assembly.FullName + "@" + resFile;

            lock (_managers)
            {
                ResourceManager result = _managers[rmName];

                if (result == null)
                {
                    result = new ResourceManager(resFile, assembly);
                    _managers.Add(rmName, result);
                }

                return result;
            }
        }

        /// <summary>
        /// Get localized string.
        /// </summary>
        /// <param name="asm">Assembly</param>
        /// <param name="resFile">Resource file.</param>
        /// <param name="resName">Resource name.</param>
        /// <returns>Localized string.</returns>
        public string GetString
            (
                [NotNull] Assembly asm,
                [NotNull] string resFile,
                [NotNull] string resName
            )
        {
            ResourceManager rm = GetResourceManager(asm, resFile);
            return rm.GetString(resName);
        }

        /// <summary>
        /// Get localized string.
        /// </summary>
        /// <param name="resFile">Resource file (located in
        /// calling assembly).</param>
        /// <param name="resName">Resource name.</param>
        /// <returns>Localized string.</returns>
        public string GetString
            (
                [NotNull] string resFile,
                [NotNull] string resName
            )
        {
            return GetString
                (
                    Assembly.GetCallingAssembly(),
                    resFile,
                    resName
                );
        }

        /// <summary>
        /// Get localized string.
        /// </summary>
        /// <param name="resClass">Class type.</param>
        /// <param name="resName">Resource name.</param>
        /// <returns>Localized string.</returns>
        public string GetString
            (
                [NotNull] Type resClass,
                [NotNull] string resName
            )
        {
            return GetString
                (
                    resClass.Namespace + ".LocStrings",
                    resName
                );
        }

        ///// <summary>
        ///// Get localized string.
        ///// </summary>
        ///// <param name="resName">Resource name.</param>
        ///// <returns>Localized string.</returns>
        //public string GetString
        //    (
        //        [NotNull] string resName
        //    )
        //{
        //    Type declaringType = (new StackFrame(1)).GetMethod().DeclaringType;
        //    if (declaringType == null)
        //    {
        //        throw new ArsMagnaException("declaringType == null");
        //    }
        //    return GetString
        //        (
        //            declaringType,
        //            resName
        //        );
        //}

        ///// <summary>
        ///// Get localized string.
        ///// </summary>
        //[CanBeNull]
        //public string this
        //    [
        //        [NotNull] string resName
        //    ]
        //{
        //    [DebuggerStepThrough]
        //    get
        //    {
        //        return GetString(resName);
        //    }
        //}

        /// <summary>
        /// Get localized string.
        /// </summary>
        [CanBeNull]
        public string this
            [
                [NotNull] Type resClass,
                [NotNull] string resName
            ]
        {
            [DebuggerStepThrough]
            get
            {
                return GetString(resClass, resName);
            }
        }

        /// <summary>
        /// Get localized string.
        /// </summary>
        [CanBeNull]
        public string this
            [
                [NotNull] string resFile,
                [NotNull] string resName
            ]
        {
            [DebuggerStepThrough]
            get
            {
                return GetString(resFile, resName);
            }
        }

        /// <summary>
        /// Get localized string.
        /// </summary>
        [CanBeNull]
        public string this
            [
                [NotNull] Assembly asm,
                [NotNull] string resFile,
                [NotNull] string resName
            ]
        {
            [DebuggerStepThrough]
            get
            {
                return GetString(asm, resFile, resName);
            }
        }

        #endregion
    }
}

#endif
