// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RuntimeUtility.cs -- some useful methods for runtime
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || DROID || NETCORE || WINMOBILE

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

using AM.Logging;
using AM.Reflection;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// Some useful methods for runtime.
    /// </summary>
    [PublicAPI]
    public static class RuntimeUtility
    {
        #region Properties

        /// <summary>
        /// Путь к файлам текущей версии Net Framework.
        /// </summary>
        /// <remarks>
        /// Типичная выдача:
        /// C:\WINDOWS\Microsoft.NET\Framework\v2.0.50215
        /// </remarks>
        [NotNull]
        public static string FrameworkLocation
        {
            get
            {
#if WINMOBILE

                throw new NotImplementedException();

#else

                string result = Path.GetDirectoryName
                    (
                        typeof(int).Bridge().Assembly.Location
                    );
                if (string.IsNullOrEmpty(result))
                {
                    throw new ArsMagnaException
                        (
                            "Can't determine framework location"
                        );
                }

                return result;

#endif
            }
        }

        /// <summary>
        /// Имя исполняемого процесса.
        /// </summary>
        [NotNull]
        public static string ExecutableFileName
        {
            get
            {
#if WINMOBILE

                throw new NotImplementedException();

#else

                Process process = Process.GetCurrentProcess();
                ProcessModule module = process.MainModule;

                return module.FileName;

#endif
            }
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Pre-JIT types of the <paramref name="assembly"/>.
        /// </summary>
        public static void PrepareAssembly
            (
                [NotNull] Assembly assembly
            )
        {
            Code.NotNull(assembly, "assembly");

            try
            {
                foreach (Type type in assembly.GetTypes())
                {
                    PrepareType(type);
                }
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "RuntimeUtility::PrepareAssembly",
                        exception
                    );
            }
        }

        /// <summary>
        /// Pre-JIT methods of the <paramref name="type"/>.
        /// </summary>
        public static void PrepareType
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            try
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    RuntimeHelpers.PrepareMethod(method.MethodHandle);
                }
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "RuntimeUtility::PrepareType",
                        exception
                    );
            }

        }

        #endregion
    }
}

#endif
