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
using AM.Collections;
using AM.Text;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;

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

        private static void AddField(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression)
                && !ReferenceEquals(context.Record, null))
            {
                string[] parts = expression.Split(new[] { '#' }, 2);
                if (parts.Length == 2)
                {
                    string tag = parts[0];
                    string text = parts[1];
                    string[] lines = text.SplitLines()
                        .NonEmptyLines().ToArray();

                    foreach (string body in lines)
                    {
                        RecordField field = RecordFieldUtility.Parse
                            (
                                tag,
                                body
                            );
                        if (!ReferenceEquals(field, null))
                        {
                            context.Record.Fields.Add(field);
                        }
                    }
                }
            }
        }

        private static void Bold(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, "<b>" + expression + "</b>");
            }
        }

        private static void Chr(PftContext context, PftNode node, string expression)
        {
            int code;
            char c;

            PftNumeric numeric = node.Children.FirstOrDefault() as PftNumeric;
            if (!ReferenceEquals(numeric, null))
            {
                code = (int)numeric.Value;
                c = (char)code;
                context.Write(numeric, c.ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(expression))
                {
                    if (int.TryParse(expression, out code))
                    {
                        c = (char)code;
                        context.Write(node, c.ToString());
                    }
                }
            }
        }

        private static void CommandLine(PftContext context, PftNode node, string expression)
        {
#if DESKTOP
            context.Write(node, global::System.Environment.CommandLine);
#endif
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

        private static void Debug(PftContext context, PftNode node, string expression)
        {
#if CLASSIC
            if (!string.IsNullOrEmpty(expression))
            {
                global::System.Diagnostics.Debug.WriteLine(expression);
            }
#endif
        }

        private static void DelField(PftContext context, PftNode node, string expression)
        {
            MarcRecord record = context.Record;

            if (!string.IsNullOrEmpty(expression)
                && !ReferenceEquals(record, null))
            {
                int repeat = -1;
                string[] parts = expression.Split(new[] { '#' }, 2);
                string tag = parts[0];
                RecordField[] fields = record.Fields.GetField(tag);

                if (parts.Length == 2)
                {
                    string repeatText = parts[1];
                    if (repeatText == "*")
                    {
                        repeat = fields.Length - 1;
                    }
                    else 
                    {
                        if (!int.TryParse(repeatText, out repeat))
                        {
                            return;
                        }
                        repeat--;
                    }
                }

                if (repeat < 0)
                {
                    foreach (RecordField field in fields)
                    {
                        record.Fields.Remove(field);
                    }
                }
                else
                {
                    RecordField field = fields.GetOccurrence(repeat);
                    if (!ReferenceEquals(field, null))
                    {
                        record.Fields.Remove(field);
                    }
                }
            }
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

        private static void IOcc(PftContext context, PftNode node, string expression)
        {
            int index = context.Index;
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                index++;
            }
            string text = index.ToInvariantString();
            context.Write(node, text);
        }

        private static void Italic(PftContext context, PftNode node, string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, "<i>" + expression + "</i>");
            }
        }

        private static void Len(PftContext context, PftNode node, string expression)
        {
            int size = string.IsNullOrEmpty(expression)
                ? 0
                : expression.Length;
            string text = size.ToInvariantString();
            context.Write(node, text);
        }

        private static void MachineName(PftContext context, PftNode node, string expression)
        {
            context.Write(node, global::System.Environment.MachineName);
        }

        private static void NOcc(PftContext context, PftNode node, string expression)
        {
            if (ReferenceEquals(context.CurrentGroup, null)
                || ReferenceEquals(context.Record, null))
            {
                context.Write(node, "0");
                return;
            }

            int result = 0;
            var fields = context.CurrentGroup.GetDescendants<PftV>()
                .Where(field => field.Command == 'v' || field.Command == 'V')
                .ToArray();

            foreach (PftV field in fields)
            {
                int count = context.Record.Fields.GetField(field.Tag).Length;
                if (count > result)
                {
                    result = count;
                }
            }

            string text = result.ToInvariantString();
            context.Write(node, text);
        }

        private static void Now(PftContext context, PftNode node, string expression)
        {
            DateTime now = DateTime.Today;

            string output = string.IsNullOrEmpty(expression)
                ? now.ToString()
                : now.ToString(expression);

            context.Write(node, output);
        }

        private static void NPost(PftContext context, PftNode node, string expression)
        {
            FieldSpecification specification = new FieldSpecification();
            specification.Parse(expression);
            MarcRecord record = context.Record;
            int count = record.Fields.GetField(specification.Tag).Length;
            string text = count.ToInvariantString();
            context.Write(node, text);
        }

        private static void OsVersion(PftContext context, PftNode node, string expression)
        {
#if CLASSIC
            context.Write(node, global::System.Environment.OSVersion.ToString());
#endif
        }

        private static void Size(PftContext context, PftNode node, string expression)
        {
            int size = string.IsNullOrEmpty(expression)
                ? 0
                : expression.SplitLines().Length;
            string text = size.ToInvariantString();
            context.Write(node, text);
        }

        private static void Sort(PftContext context, PftNode node, string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            string[] lines = expression.SplitLines()
                .NonEmptyLines().ToArray();
            lines = NumberText.Sort(lines).ToArray();
            context.Write
                (
                    node,
                    string.Join
                    (
                        global::System.Environment.NewLine,
                        lines
                    )
                );
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

            reg.Add("addField", AddField);
            reg.Add("bold", Bold);
            reg.Add("chr", Chr);
            reg.Add("commandline", CommandLine);
            reg.Add("cout", COut);
            reg.Add("debug", Debug);
            reg.Add("delField", DelField);
            reg.Add("error", Error);
            reg.Add("fatal", Fatal);
            reg.Add("getenv", GetEnv);
            reg.Add("iocc", IOcc);
            reg.Add("italic", Italic);
            reg.Add("len", Len);
            reg.Add("machinename", MachineName);
            reg.Add("nocc", NOcc);
            reg.Add("now", Now);
            reg.Add("npost", NPost);
            reg.Add("osversion", OsVersion);
            reg.Add("size", Size);
            reg.Add("sort", Sort);
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
