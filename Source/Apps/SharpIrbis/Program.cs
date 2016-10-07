/* Program.cs -- program entry point
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

using ManagedIrbis;

using Microsoft.CSharp;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace SharpIrbis
{
    static class Program
    {
        static readonly List<string> _references = new List<string>();
        static readonly List<string> _sources = new List<string>();
        static readonly List<string> _arguments = new List<string>();

        static MethodInfo FindEntryPoint
            (
                Assembly assembly
            )
        {
            foreach (Type type in assembly.GetTypes())
            {
                MethodInfo main = type.GetMethod
                    (
                        "Main",
                        BindingFlags.Static
                        | BindingFlags.Public
                        | BindingFlags.NonPublic
                    );
                if (!ReferenceEquals(main, null))
                {
                    return main;
                }
            }

            return null;
        }

        public static void ShowMessageAndExit
            (
                string format,
                params object[] args
            )
        {
            string text = string.Format(format, args);

            MessageBox.Show
                (
                    text,
                    "SharpIrbis",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
            Environment.Exit(1);

            //Environment.FailFast(text);
        }

        public static void HandleErrors
            (
                CompilerResults results
            )
        {
            StringBuilder builder = new StringBuilder();

            foreach (var error in results.Errors)
            {
                builder.AppendLine(error.ToString());
            }

            if (builder.Length != 0)
            {
                ShowMessageAndExit(builder.ToString());
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int returnValue = 0;

            if (args.Length == 0)
            {
                ShowMessageAndExit
                    (
                        "USAGE: IrbisSharp <source-file or reference>"
                    );
                return 1;
            }

            try
            {
                JObject root = JObject.Parse
                    (
                        File.ReadAllText("config.json", Encoding.UTF8)
                    );

                JToken references = root["references"];
                foreach (JToken child in references.Children())
                {
                    string assemblyReferences = child.ToString();
                    _references.Add(assemblyReferences);
                }

                bool treatAsArguments = false;
                foreach (string s in args)
                {
                    if (s == "--")
                    {
                        treatAsArguments = true;
                        continue;
                    }

                    if (treatAsArguments)
                    {
                        _arguments.Add(s);
                        continue;
                    }

                    string extension = Path.GetExtension(s);
                    if (ReferenceEquals(extension, null))
                    {
                        throw new Exception
                            (
                                "don't know how to handle file: "
                                + s
                            );
                    }

                    extension = extension.ToLower();

                    switch (extension)
                    {
                        case ".cs":
                        case ".csi":
                            string sourceFullName = Path.GetFullPath(s);
                            _sources.Add(sourceFullName);
                            break;
                        case ".dll":
                            string asmRef = Path.GetFullPath(s);
                            _references.Add(asmRef);
                            break;
                        default:
                            throw new Exception
                                (
                                    "don't know how to handle file: "
                                    + s
                                );
                    }
                }

                CSharpCodeProvider provider = new CSharpCodeProvider();

                CompilerParameters parameters = new CompilerParameters
                    (
                        _references.ToArray()
                    )
                {
                    GenerateExecutable = true,
                    GenerateInMemory = true,
                    CompilerOptions = "/d:DEBUG",
                    WarningLevel = 4,
                    IncludeDebugInformation = true
                };
                CompilerResults results
                    = provider.CompileAssemblyFromFile
                    (
                        parameters,
                        _sources.ToArray()
                    );

                HandleErrors(results);
                if (results.Errors.Count != 0)
                {
                    return results.Errors.Count;
                }

                MethodInfo main = FindEntryPoint(results.CompiledAssembly);
                if (ReferenceEquals(main, null))
                {
                    ShowMessageAndExit("Can't find entry point");
                    return 1;
                }

                object obj;
                if (main.GetParameters().Length == 0)
                {
                    obj = main.Invoke(null, null);
                }
                else
                {
                    obj = main.Invoke(null, _arguments.ToArray());
                }
                if (!ReferenceEquals(obj, null))
                {
                    if (obj is int)
                    {
                        returnValue = (int)obj;
                    }
                }

                foreach (var tempFile in results.TempFiles)
                {
                    File.Delete(tempFile.ToString());
                }
            }
            catch (Exception ex)
            {
                ShowMessageAndExit(ex.ToString());
                return 1;
            }

            return returnValue;
        }
    }
}
