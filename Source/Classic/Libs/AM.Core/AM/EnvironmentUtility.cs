// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EnvironmentUtility.cs -- program environment study routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AM.Reflection;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Program environment study routines.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static class EnvironmentUtility
    {
        /// <summary>
        /// Gets a value indicating whether system is 32-bit.
        /// </summary>
        /// <value><c>true</c> if system is 32-bit; otherwise,
        /// <c>false</c>.</value>
        public static bool Is32Bit
        {
            [DebuggerStepThrough]
            get
            {
                return IntPtr.Size == 4;
            }
        }

        /// <summary>
        /// Gets a value indicating whether system is 64-bit.
        /// </summary>
        /// <value><c>true</c> if system is 64-bit; otherwise,
        /// <c>false</c>.</value>
        public static bool Is64Bit
        {
            [DebuggerStepThrough]
            get
            {
                return IntPtr.Size == 8;
            }
        }

        /// <summary>
        /// Running on Microsoft CLR?
        /// </summary>
        public static bool IsMicrosoftClr()
        {
#if UAP

            return true;

#else

            return RuntimeEnvironment.GetRuntimeDirectory().Contains("Microsoft");

#endif
        }

        /// <summary>
        /// Running on Mono?
        /// </summary>
        public static bool IsMono ()
        {
            return !ReferenceEquals(Type.GetType ("Mono.Runtime"), null);
        }

        /// <summary>
        /// Get .NET Core version.
        /// </summary>
        [CanBeNull]
        public static string NetCoreVersion()
        {
#if !UAP

            var assembly = typeof(System.Runtime.GCSettings).Bridge().Assembly;
            var assemblyPath = assembly.CodeBase.Split
                (
                    new[] { '/', '\\' },
                    StringSplitOptions.RemoveEmptyEntries
                );
            int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
            if (netCoreAppIndex > 0
                && netCoreAppIndex < assemblyPath.Length - 2)
            {
                return assemblyPath[netCoreAppIndex + 1];
            }

#endif

            return null;
        }

        /// <summary>
        /// Optimal degree of parallelism.
        /// </summary>
        public static int OptimalParallelism
        {
            get
            {
#if WINMOBILE || PocketPC

                return 1;

#else

                int result = Math.Min
                    (
                        Math.Max
                            (
                                Environment.ProcessorCount - 1,
                                1
                            ),
                        8 // TODO choose good number
                    );

                return result;

#endif
            }
        }

        /// <summary>
        /// System uptime.
        /// </summary>
        public static TimeSpan Uptime
        {
            [DebuggerStepThrough]
            get
            {
                return new TimeSpan(Environment.TickCount);
            }
        }
    }
}