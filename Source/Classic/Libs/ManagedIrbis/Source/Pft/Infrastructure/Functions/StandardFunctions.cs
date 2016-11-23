/* PftFunctionManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
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

        private static void AddField(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
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

        //=================================================

        private static void Bold(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, "<b>" + expression + "</b>");
            }
        }

        //=================================================

        private static void Cat(PftContext context, PftNode node, PftNode[] arguments)
        {
            //
            // TODO: add some caching
            //

            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                FileSpecification specification
                    = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        context.Environment.Database,
                        expression
                    );
                string source = context.Environment.ReadFile
                    (
                        specification
                    );
                context.Write(node, source);
            }
        }

        //=================================================

        private static void Chr(PftContext context, PftNode node, PftNode[] arguments)
        {
            int code;
            char c;

            PftFunctionCall call = (PftFunctionCall)node;
            PftNumeric numeric = call.Arguments[0] as PftNumeric;
            if (!ReferenceEquals(numeric, null))
            {
                code = (int)numeric.Value;
                c = (char)code;
                context.Write(numeric, c.ToString());
            }
            else
            {
                string expression = context.GetStringArgument(arguments, 0);
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

        //=================================================

        private static void CommandLine(PftContext context, PftNode node, PftNode[] arguments)
        {
#if DESKTOP
            context.Write(node, global::System.Environment.CommandLine);
#endif
        }

        private static void COut(PftContext context, PftNode node, PftNode[] arguments)
        {
#if DESKTOP || NETCORE
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                Console.Write(expression);
            }
#endif
        }

        //=================================================

        private static void Debug(PftContext context, PftNode node, PftNode[] arguments)
        {
#if CLASSIC
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                global::System.Diagnostics.Debug.WriteLine(expression);
            }
#endif
        }

        //=================================================

        private static void DelField(PftContext context, PftNode node, PftNode[] arguments)
        {
            MarcRecord record = context.Record;

            string expression = context.GetStringArgument(arguments, 0);
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

        //=================================================

        private static void Error(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Output.Error.WriteLine(expression);
            }
        }

        //=================================================

        private static void Fatal(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            string message = expression ?? string.Empty;
            global::System.Environment.FailFast(message);
        }

        //=================================================

        private static void GetEnv(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                string result = global::System.Environment.GetEnvironmentVariable(expression);
                context.Write(node, result);
            }
        }

        //=================================================

        private static void Include(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                Unifors.Unifor6.ExecuteNestedFormat
                    (
                        context,
                        node,
                        expression
                    );
            }
        }

        //=================================================

        private static void IOcc(PftContext context, PftNode node, PftNode[] arguments)
        {
            int index = context.Index;
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                index++;
            }
            string text = index.ToInvariantString();
            context.Write(node, text);
        }

        //=================================================

        private static void Italic(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, "<i>" + expression + "</i>");
            }
        }

        //=================================================

        private static void Len(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            int size = string.IsNullOrEmpty(expression)
                ? 0
                : expression.Length;
            string text = size.ToInvariantString();
            context.Write(node, text);
        }

        //=================================================

        private static void LoadRecord(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                int mfn;
                if (int.TryParse(expression, out mfn))
                {
                    MarcRecord record = context.Environment
                        .ReadRecord(mfn);

                    if (ReferenceEquals(record, null))
                    {
                        context.Write(node, "0");
                    }
                    else
                    {
                        PftContext ctx = context;
                        while (!ReferenceEquals(ctx, null))
                        {
                            ctx.Record = record;
                            ctx = ctx.Parent;
                        }
                        context.Write(node, "1");
                    }
                }
            }
        }

        //=================================================

        private static void MachineName(PftContext context, PftNode node, PftNode[] arguments)
        {
            context.Write(node, global::System.Environment.MachineName);
        }

        //=================================================

        private static void NOcc(PftContext context, PftNode node, PftNode[] arguments)
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

        //=================================================

        private static void Now(PftContext context, PftNode node, PftNode[] arguments)
        {
            DateTime now = DateTime.Today;

            string expression = context.GetStringArgument(arguments, 0);
            string output = string.IsNullOrEmpty(expression)
                ? now.ToString(CultureInfo.CurrentCulture)
                : now.ToString(expression);

            context.Write(node, output);
        }

        //=================================================

        private static void NPost(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                FieldSpecification specification = new FieldSpecification();
                specification.Parse(expression);
                MarcRecord record = context.Record;
                int count = record.Fields.GetField(specification.Tag).Length;
                string text = count.ToInvariantString();
                context.Write(node, text);
            }
        }

        //=================================================

        private static void OsVersion(PftContext context, PftNode node, PftNode[] arguments)
        {
#if CLASSIC
            context.Write(node, global::System.Environment.OSVersion.ToString());
#endif
        }

        //=================================================

        private static void PadLeft(PftContext context, PftNode node, PftNode[] arguments)
        {
            string text = context.GetStringArgument(arguments, 0);
            double? width = context.GetNumericArgument(arguments, 1);
            string padding = context.GetStringArgument(arguments, 2);

            if (ReferenceEquals(text, null)
                || !width.HasValue)
            {
                return;
            }

            char pad = ' ';
            if (!string.IsNullOrEmpty(padding))
            {
                pad = padding[0];
            }

            string output = text.PadLeft
                (
                    (int)width.Value,
                    pad
                );
            context.Write(node, output);
        }

        //=================================================

        private static void PadRight(PftContext context, PftNode node, PftNode[] arguments)
        {
            string text = context.GetStringArgument(arguments, 0);
            double? width = context.GetNumericArgument(arguments, 1);
            string padding = context.GetStringArgument(arguments, 2);

            if (ReferenceEquals(text, null)
                || !width.HasValue)
            {
                return;
            }

            char pad = ' ';
            if (!string.IsNullOrEmpty(padding))
            {
                pad = padding[0];
            }

            string output = text.PadRight
                (
                    (int)width.Value,
                    pad
                );
            context.Write(node, output);
        }

        //=================================================

        private static void Replace(PftContext context, PftNode node, PftNode[] arguments)
        {
            string text = context.GetStringArgument(arguments, 0);
            string oldValue = context.GetStringArgument(arguments, 1);
            string newValue = context.GetStringArgument(arguments, 2);

            if (ReferenceEquals(text, null)
                || ReferenceEquals(oldValue, null)
                || ReferenceEquals(newValue, null))
            {
                return;
            }

            string output = text.Replace(oldValue, newValue);
            context.Write(node, output);
        }

        //=================================================

        private static void Search(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                int[] foundMfns = context.Environment.Search(expression);
                if (foundMfns.Length != 0)
                {
                    string[] foundLines = foundMfns.Select
                        (
                            item => item.ToInvariantString()
                        )
                        .ToArray();
                    string output = string.Join
                        (
                            global::System.Environment.NewLine,
                            foundLines
                        );
                    context.Write(node, output);
                }
            }
        }

        //=================================================

        private static void Size(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            int size = string.IsNullOrEmpty(expression)
                ? 0
                : expression.SplitLines().Length;
            string text = size.ToInvariantString();
            context.Write(node, text);
        }


        //=================================================

        private static void Sort(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
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

        //=================================================

        private static void Split(PftContext context, PftNode node, PftNode[] arguments)
        {
            string text = context.GetStringArgument(arguments, 0);
            string separator = context.GetStringArgument(arguments, 1);

            if (ReferenceEquals(text, null)
                || ReferenceEquals(separator, null))
            {
                return;
            }

            string[] lines = text.Split(new[] { separator }, StringSplitOptions.None);
            string output = string.Join(Environment.NewLine, lines);
            context.Write(node, output);
        }

        //=================================================

        private static void Substring(PftContext context, PftNode node, PftNode[] arguments)
        {
            string text = context.GetStringArgument(arguments, 0);
            double? offset = context.GetNumericArgument(arguments, 1);
            double? length = context.GetNumericArgument(arguments, 2);

            if (ReferenceEquals(text, null)
                || !offset.HasValue
                || !length.HasValue)
            {
                return;
            }

            string output = text.SafeSubstring
                (
                    (int) offset.Value,
                    (int) length.Value
                );
            context.Write(node, output);
        }

        //=================================================

        private static void System(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
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

        //=================================================

        private static void Today(PftContext context, PftNode node, PftNode[] arguments)
        {
            DateTime today = DateTime.Today;

#if CLASSIC

            string expression = context.GetStringArgument(arguments, 0);
            string output = string.IsNullOrEmpty(expression)
                ? today.ToShortDateString()
                : today.ToString(expression);

#else

            // TODO Implement properly

            string expression = context.GetStringArgument(arguments, 0);
            string output = string.IsNullOrEmpty(expression)
                ? today.ToString()
                : today.ToString(expression);

#endif

            context.Write(node, output);
        }

        //=================================================

        private static void ToLower(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, expression.ToLower());
            }
        }

        //=================================================

        private static void ToUpper(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, expression.ToUpper());
            }
        }

        //=================================================

        private static void Trace(PftContext context, PftNode node, PftNode[] arguments)
        {
#if CLASSIC
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                global::System.Diagnostics.Trace.WriteLine(expression);
            }
#endif
        }

        //=================================================

        private static void Trim(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, expression.Trim());
            }
        }

        //=================================================

        private static void Warn(PftContext context, PftNode node, PftNode[] arguments)
        {
            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Output.Warning.WriteLine(expression);
            }
        }

        #endregion

        #region Public methods

        //=================================================

        internal static void Register()
        {
            var reg = PftFunctionManager.BuiltinFunctions.Registry;

            reg.Add("addField", AddField);
            reg.Add("bold", Bold);
            reg.Add("cat", Cat);
            reg.Add("chr", Chr);
            reg.Add("commandline", CommandLine);
            reg.Add("cOut", COut);
            reg.Add("debug", Debug);
            reg.Add("delField", DelField);
            reg.Add("error", Error);
            reg.Add("fatal", Fatal);
            reg.Add("getenv", GetEnv);
            reg.Add("iocc", IOcc);
            reg.Add("include", Include);
            reg.Add("italic", Italic);
            reg.Add("len", Len);
            reg.Add("loadRecord", LoadRecord);
            reg.Add("machineName", MachineName);
            reg.Add("nocc", NOcc);
            reg.Add("now", Now);
            reg.Add("nPost", NPost);
            reg.Add("osVersion", OsVersion);
            reg.Add("padLeft", PadLeft);
            reg.Add("padRight", PadRight);
            reg.Add("replace", Replace);
            reg.Add("size", Size);
            reg.Add("search", Search);
            reg.Add("sort", Sort);
            reg.Add("split", Split);
            reg.Add("substring", Substring);
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
