/* PftFunctionManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    static class StandardFunctions
    {
        #region Private members

        //================================================================
        // STANDARD BUILTIN FUNCTIONS
        //================================================================

        private static void Bold(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, "<b>" + expression + "</b>");
            }
        }

        private static void COut(PftContext context, PftNode node, string expression)
        {
#if DESKTOP || NETCORE
            if (!string.IsNullOrEmpty(expression))
            {
                Console.Write(expression);
            }
#endif
        }

        private static void CommandLine(PftContext context, PftNode node, string expression)
        {
#if DESKTOP
            context.Write(node, global::System.Environment.CommandLine);
#endif
        }

        private static void Debug(PftContext context, PftNode node, string expression)
        {
#if CLASSIC
            if (!string.IsNullOrEmpty(expression))
            {
                global::System.Diagnostics.Debug.WriteLine(expression);
            }
#endif
        }

        private static void Error(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Output.Error.WriteLine(expression);
            }
        }

        private static void Fatal(PftContext context, PftNode node, string expression)
        {
            string message = expression ?? string.Empty;
            global::System.Environment.FailFast(message);
        }

        private static void GetEnv(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string result = global::System.Environment.GetEnvironmentVariable(expression);
                context.Write(node, result);
            }
        }

        private static void Italic(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, "<i>" + expression + "</i>");
            }
        }

        private static void MachineName(PftContext context, PftNode node, string expression)
        {
            context.Write(node, global::System.Environment.MachineName);
        }

        private static void Now(PftContext context, PftNode node, string expression)
        {
            DateTime now = DateTime.Today;

            string output = string.IsNullOrEmpty(expression)
                ? now.ToString()
                : now.ToString(expression);

            context.Write(node, output);
        }

        private static void OsVersion(PftContext context, PftNode node, string expression)
        {
#if CLASSIC
            context.Write(node, global::System.Environment.OSVersion.ToString());
#endif
        }

        private static void System(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {

#if CLASSIC || DESKTOP

                string comspec = global::System.Environment
                    .GetEnvironmentVariable("comspec")
                    ?? "cmd.exe";

                expression = "/c " + expression;
                ProcessStartInfo startInfo = new ProcessStartInfo
                    (
                        comspec,
                        expression
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
                string output = process.StandardOutput.ReadToEnd();

                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                }

#endif

            }
        }

        private static void Today(PftContext context, PftNode node, string expression)
        {
            DateTime today = DateTime.Today;

#if CLASSIC

            string output = string.IsNullOrEmpty(expression)
                ? today.ToShortDateString()
                : today.ToString(expression);

#else

            // TODO Implement properly

            string output = string.IsNullOrEmpty(expression)
                ? today.ToString()
                : today.ToString(expression);

#endif

            context.Write(node, output);
        }

        private static void ToLower(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, expression.ToLower());
            }
        }

        private static void ToUpper(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, expression.ToUpper());
            }
        }

        private static void Trace(PftContext context, PftNode node, string expression)
        {
#if CLASSIC
            if (!string.IsNullOrEmpty(expression))
            {
                global::System.Diagnostics.Trace.WriteLine(expression);
            }
#endif
        }

        private static void Trim(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, expression.Trim());
            }
        }

        private static void Warn(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Output.Warning.WriteLine(expression);
            }
        }

        #endregion

        #region Public methods

        internal static void Register()
        {
            var reg = PftFunctionManager.BuiltinFunctions.Registry;

            reg.Add("bold", Bold);
            reg.Add("cout", COut);
            reg.Add("commandline", CommandLine);
            reg.Add("debug", Debug);
            reg.Add("error", Error);
            reg.Add("fatal", Fatal);
            reg.Add("getenv", GetEnv);
            reg.Add("italic", Italic);
            reg.Add("machinename", MachineName);
            reg.Add("now", Now);
            reg.Add("osversion", OsVersion);
            reg.Add("system", System);
            reg.Add("today", Today);
            reg.Add("tolower", ToLower);
            reg.Add("toupper", ToUpper);
            reg.Add("trace", Trace);
            reg.Add("trim", Trim);
            reg.Add("warn", Warn);
        }

        #endregion
    }
}
