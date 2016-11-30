// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProcessRunner.cs --
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

#if CLASSIC || DESKTOP

using System.Diagnostics;

#endif

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// 
    /// </summary>
    static class ProcessRunner
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Run process and forget about it.
        /// </summary>
        public static void RunAndForget
            (
                [NotNull] string commandLine
            )
        {
            Code.NotNull(commandLine, "commandLine");

#if CLASSIC

            string comspec = Environment
                .GetEnvironmentVariable("comspec")
                ?? "cmd.exe";

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

#if CLASSIC || DESKTOP

            string comspec = Environment
                .GetEnvironmentVariable("comspec")
                ?? "cmd.exe";

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

#if CLASSIC

            string comspec = Environment
                .GetEnvironmentVariable("comspec")
                ?? "cmd.exe";

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
