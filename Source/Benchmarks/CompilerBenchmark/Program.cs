// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Text.Output;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

namespace CompilerBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }

            string rootPath = args[0];
            string formatName = args[1];

            try
            {
                using (LocalProvider provider = new LocalProvider(rootPath))
                {
                    FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        provider.Database,
                        formatName
                    );
                    string source = provider.ReadFile(specification);
                    if (string.IsNullOrEmpty(source))
                    {
                        Console.WriteLine("No file: {0}", formatName);
                    }
                    else
                    {
                        PftContext context = new PftContext(null);
                        context.SetProvider(provider);
                        PftFormatter formatter = new PftFormatter(context);
                        formatter.ParseProgram(source);

                        PftProgram program 
                            = (PftProgram) formatter.Program.Clone();
                        program.Optimize();

                        //Console.WriteLine(program.DumpToText());
                        //Console.WriteLine();

                        if (!Directory.Exists("Out"))
                        {
                            Directory.CreateDirectory("Out");
                        }

                        PftCompiler compiler = new PftCompiler
                        {
                            KeepSource = true,
                            OutputPath = "Out"
                        };
                        string className = compiler.CompileProgram
                            (
                                program
                            );

                        //string sourceCode = compiler.GetSourceCode();
                        //Console.WriteLine(sourceCode);

                        AbstractOutput output = AbstractOutput.Console;
                        string assemblyPath = compiler.CompileToDll
                            (
                                output,
                                className
                            );
                        if (!ReferenceEquals(assemblyPath, null))
                        {
                            Console.WriteLine
                                (
                                    "Compiled to {0}",
                                    assemblyPath
                                );
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
