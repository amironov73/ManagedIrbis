// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RuntimeUtility.cs -- some useful methods for runtime
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

using UnsafeAM.Logging;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Runtime
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
                string result = Path.GetDirectoryName
                    (
                        typeof(int).Assembly.Location
                    );
                if (string.IsNullOrEmpty(result))
                {
                    throw new ArsMagnaException
                        (
                            "Can't determine framework location"
                        );
                }

                return result;
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
                Process process = Process.GetCurrentProcess();
                ProcessModule module = process.MainModule;

                return module.FileName;
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
            Code.NotNull(assembly, nameof(assembly));

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
            Code.NotNull(type, nameof(type));

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
