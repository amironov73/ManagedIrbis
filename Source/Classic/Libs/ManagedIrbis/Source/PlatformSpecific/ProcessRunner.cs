// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProcessRunner.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#if CLASSIC || NETCORE || ANDROID

using System.Diagnostics;

#endif

using AM;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    static class ProcessRunner
    {
        #region Public methods

        [NotNull]
        public static string GetShell()
        {
#if WINMOBILE || PocketPC

            throw new NotImplementedException();

#else

            string result = Environment.GetEnvironmentVariable("COMSPEC")
                             ?? "cmd.exe";

            return result;

#endif
        }

        /// <summary>
        /// Run process and forget about it.
        /// </summary>
        public static void RunAndForget
            (
                [NotNull] string commandLine
            )
        {
            Code.NotNull(commandLine, "commandLine");

#if CLASSIC || NETCORE

            string comspec = GetShell();

            commandLine = "/c " + commandLine;
            ProcessStartInfo startInfo = new ProcessStartInfo
                (
                    comspec,
                    commandLine
                )
            {
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            Process process = Process.Start(startInfo)
                .ThrowIfNull("process");
            process.Dispose();

#endif
        }

        /// <summary>
        /// Run process and get its output.
        /// </summary>
        public static string RunAndGetOutput
            (
                [NotNull] string commandLine
            )
        {
            Code.NotNull(commandLine, "commandLine");

#if CLASSIC || NETCORE

            string comspec = GetShell();

            commandLine = "/c " + commandLine;
            ProcessStartInfo startInfo = new ProcessStartInfo
                (
                    comspec,
                    commandLine
                )
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = IrbisEncoding.Oem
            };

            Process process = Process.Start(startInfo)
                .ThrowIfNull("process");
            process.WaitForExit();

            string result = process.StandardOutput.ReadToEnd();

            return result;

#else

            return string.Empty;

#endif
        }

        /// <summary>
        /// Run process and wait for it.
        /// </summary>
        public static void RunAndWait
            (
                [NotNull] string commandLine
            )
        {
            Code.NotNull(commandLine, "commandLine");

#if CLASSIC || NETCORE

            string comspec = GetShell();

            commandLine = "/c " + commandLine;
            ProcessStartInfo startInfo = new ProcessStartInfo
                (
                    comspec,
                    commandLine
                )
            {
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            Process process = Process.Start(startInfo)
                .ThrowIfNull("process");
            process.WaitForExit();

#endif
        }

        #endregion
    }
}
