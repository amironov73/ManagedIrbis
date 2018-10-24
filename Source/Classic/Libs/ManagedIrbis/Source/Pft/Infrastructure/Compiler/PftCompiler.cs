// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCompiler.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure.Ast;

#if !WINMOBILE

using Microsoft.CSharp;

#endif

using MoonSharp.Interpreter;

#if CLASSIC || NETCORE

using System.CodeDom.Compiler;

#endif

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftCompiler
    {
        #region Constants

        /// <summary>
        /// Prefix for node methods.
        /// </summary>
        public const string NodeMethodPrefix = "NodeMethod";

        #endregion

        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        [NotNull]
        internal FieldDictionary Fields { get; private set; }

        [NotNull]
        internal NodeDictionary Nodes { get; private set; }

        [NotNull]
        internal IndexDictionary Indexes { get; private set; }

        [NotNull]
        internal TextWriter Output { get; private set; }

        /// <summary>
        /// Last used node id.
        /// </summary>
        internal int LastNodeId { get; private set; }

        /// <summary>
        /// Indentation level.
        /// </summary>
        internal int Indent { get; private set; }

        /// <summary>
        /// Path to store compiled assemblies.
        /// </summary>
        public NonNullValue<string> OutputPath { get; set; }

        /// <summary>
        /// Keep source.
        /// </summary>
        public bool KeepSource { get; set; }

        /// <summary>
        /// Compile debug version?
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// References.
        /// </summary>
        [NotNull]
        public NonNullCollection<string> References { get; private set; }

        /// <summary>
        /// Usings.
        /// </summary>
        [NotNull]
        public NonNullCollection<string> Usings { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCompiler()
        {
            Indent = 0;
            Fields = new FieldDictionary();
            Nodes = new NodeDictionary();
            Indexes = new IndexDictionary();
            Output = new StringWriter();
            Provider = new LocalProvider();
            OutputPath = Path.GetTempPath();
            References = new NonNullCollection<string>
            {
                "AM.Core.dll",
                "JetBrains.Annotations.dll",
                "ManagedIrbis.dll",
                "Microsoft.CSharp.dll",
                "MoonSharp.Interpreter.dll",
                "Newtonsoft.Json.dll",
                "System.dll",
                "System.Core.dll",
                "System.Data.dll",
                "System.Data.DataSetExtensions.dll",
                "System.Xml.dll",
                "System.Xml.Linq.dll"
            };

            Usings = new NonNullCollection<string>
            {
                "System",
                "System.Collections.Generic",
                "System.Diagnostics",
                "System.Globalization",
                "System.IO",
                "System.Linq",
                "System.Text",
                "System.Threading",
                "System.Threading.Tasks",
                "AM",
                "AM.IO",
                "AM.Logging",
                "ManagedIrbis",
                "ManagedIrbis.Client",
                "ManagedIrbis.Direct",
                "ManagedIrbis.Menus",
                "ManagedIrbis.Pft",
                "ManagedIrbis.Pft.Infrastructure",
                "ManagedIrbis.Pft.Infrastructure.Ast",
                "ManagedIrbis.Pft.Infrastructure.Compiler"
            };
        }

        #endregion

        #region Private members

        private PftNode _currentNode;

        private int _actionCount;

        internal void RenumberNodes
            (
                [NotNull] PftNode rootNode
            )
        {
            NumberingVisitor visitor = new NumberingVisitor(Nodes, LastNodeId);
            bool result = rootNode.AcceptVisitor(visitor);
            if (!result)
            {
                throw new PftCompilerException();
            }
            LastNodeId = visitor.LastId;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Call method for the node.
        /// </summary>
        [NotNull]
        public PftCompiler CallNodeMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            NodeInfo info = Nodes.Get(node);

            if (!info.Ready)
            {
                throw new PftCompilerException();
            }

            WriteLine
                (
                    "{0}{1} ();",
                    NodeMethodPrefix,
                    info.Id
                );

            return this;
        }

        /// <summary>
        /// Call nodes.
        /// </summary>
        [NotNull]
        public PftCompiler CallNodes
            (
                [NotNull] IList<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            foreach (PftNode node in nodes)
            {
                WriteIndent();
                CallNodeMethod(node);
            }

            return this;
        }

        /// <summary>
        /// Compile action method.
        /// </summary>
        [CanBeNull]
        public string CompileAction
            (
                [NotNull] IList<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            if (nodes.Count == 0)
            {
                // Means: do not call action method
                return null;
            }
            if (nodes.Count == 1)
            {
                NodeInfo info = Nodes.Get(nodes[0]);

                return NodeMethodPrefix + info.Id;
            }

            string methodName = "ActionMethod" + ++_actionCount;

            WriteIndent();
            WriteLine("void {0} ()", methodName);
            WriteIndent();
            WriteLine("{");
            IncreaseIndent();
            CallNodes(nodes);
            DecreaseIndent();
            WriteIndent();
            WriteLine("}");
            WriteLine();

            return methodName;
        }

        /// <summary>
        /// Compile the field.
        /// </summary>
        [NotNull]
        internal FieldInfo CompileField
            (
                [NotNull] PftField field
            )
        {
            Code.NotNull(field, "field");

            FieldInfo result = Fields.Get(field);
            if (ReferenceEquals(result, null))
            {
                result = Fields.Create(field);
                FieldSpecification spec = result.Specification;
                WriteIndent();
                WriteLine
                    (
                        "// {0}",
                        result.Text
                    );
                WriteIndent();
                WriteLine
                    (
                        "FieldSpecification {0} = new FieldSpecification()",
                        result.Reference
                    );
                WriteIndent();
                WriteLine("{");
                IncreaseIndent();
                WriteIndent();
                WriteLine("Command = '{0}',", spec.Command);
                WriteIndent();
                WriteLine("Tag = {0},", spec.Tag);
                if (spec.SubField != SubField.NoCode)
                {
                    WriteIndent();
                    WriteLine("SubField = '{0}', ", spec.SubField);
                }
                DecreaseIndent();
                WriteIndent();
                WriteLine("};");
                WriteLine();
            }

            return result;
        }

        [NotNull]
        internal IndexInfo CompileIndex
            (
                IndexSpecification specification
            )
        {
            IndexInfo result = Indexes.Get(specification);
            if (ReferenceEquals(result, null))
            {
                result = Indexes.Create(specification);
                string text = specification.ToText();
                WriteIndent();
                WriteLine
                    (
                        "// {0}",
                        text
                    );
                WriteIndent();
                Write
                    (
                        "IndexSpecification {0} = new IndexSpecification()",
                        result.Reference
                    );
                if (specification.Kind == IndexKind.None)
                {
                    WriteLine(";");
                }
                else
                {
                    WriteLine();
                    WriteIndent();
                    WriteLine("{");
                    IncreaseIndent();
                    WriteIndent();
                    WriteLine("Kind = IndexKind.{0},", specification.Kind);
                    if (specification.Kind == IndexKind.Literal)
                    {
                        WriteIndent();
                        WriteLine
                            (
                                "Literal = {0},",
                                specification.Literal.ToInvariantString()
                            );
                    }
                    if (specification.Kind == IndexKind.Expression)
                    {
                        WriteIndent();
                        WriteLine
                            (
                                "Expression = \"{0}\",",
                                CompilerUtility.Escape(specification.Expression)
                            );
                    }
                    DecreaseIndent();
                    WriteIndent();
                    WriteLine("};");
                }
                WriteLine();
            }

            return result;
        }

        /// <summary>
        /// Compile nodes.
        /// </summary>
        public void CompileNodes
            (
                [NotNull] IList<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            foreach (PftNode node in nodes)
            {
                node.Compile(this);
            }
        }

        /// <summary>
        /// Compile the program.
        /// </summary>
        [NotNull]
        public string CompileProgram
            (
                [NotNull] PftProgram program
            )
        {
            Code.NotNull(program, "program");

            RenumberNodes(program);

            string result = StartClass();

            program.Compile(this);

            EndClass(result);

            return result;
        }

        /// <summary>
        /// Compile to DLL.
        /// </summary>
        [CanBeNull]
        public string CompileToDll
            (
                [NotNull] AbstractOutput output,
                [NotNull] string fileName
            )
        {
            Code.NotNull(output, "output");
            Code.NotNullNorEmpty(fileName, "fileName");

#if !CLASSIC && !NETCORE

            return null;

#else

            string outputAssemblyPath = Path.Combine
                (
                    OutputPath,
                    fileName + ".dll"
                );
            outputAssemblyPath = Path.GetFullPath(outputAssemblyPath);

            string sourceCode = GetSourceCode();
            string sourceFileName = Path.ChangeExtension
                (
                    outputAssemblyPath,
                    ".cs"
                );
            File.WriteAllText(sourceFileName, sourceCode);

            string[] sources =
            {
                sourceFileName
            };

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters
                (
                    References.ToArray()
                )
            {
                GenerateExecutable = false,
                GenerateInMemory = false,
                WarningLevel = 4,
                OutputAssembly = outputAssemblyPath
            };
            if (Debug)
            {
                parameters.CompilerOptions = "/d:DEBUG";
                parameters.IncludeDebugInformation = true;
            }
            CompilerResults results = provider.CompileAssemblyFromFile
                (
                    parameters,
                    sources
                );
            foreach (CompilerError error in results.Errors)
            {
                output.WriteLine(error.ToString());
            }

            if (!KeepSource)
            {
                File.Delete(sourceFileName);
            }

            if (results.Errors.Count != 0)
            {
                return null;
            }

            return outputAssemblyPath;

#endif
        }

        /// <summary>
        /// Decrease indent.
        /// </summary>
        [NotNull]
        public PftCompiler DecreaseIndent()
        {
            Indent--;

            return this;
        }

        /// <summary>
        ///
        /// </summary>
        public void EndClass
            (
                [NotNull] string className
            )
        {
            if (!ReferenceEquals(_currentNode, null))
            {
                throw new PftCompilerException();
            }

            WriteIndent();
            WriteLine("public override string Execute(MarcRecord record)");
            WriteIndent();
            WriteLine("{");
            IncreaseIndent();

            WriteIndent();
            WriteLine("base.Execute(record);");
            WriteIndent();
            WriteLine("NodeMethod1();");
            WriteIndent();
            WriteLine("string result = Context.GetProcessedOutput();");
            WriteIndent();
            WriteLine("return result;");

            DecreaseIndent();
            WriteIndent();
            WriteLine("}");
            WriteLine();

            WriteIndent();
            WriteLine("public static PftPacket CreateInstance(PftContext context)");
            WriteIndent();
            WriteLine("{");
            IncreaseIndent();
            WriteIndent();
            WriteLine("PftPacket result = new {0} (context);", className);
            WriteIndent();
            WriteLine("return result;");
            DecreaseIndent();
            WriteIndent();
            WriteLine("}");
            WriteLine();

            DecreaseIndent();
            WriteLine("} // end of class");
        }

        /// <summary>
        /// End method for the node.
        /// </summary>
        public void EndMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            if (!ReferenceEquals(_currentNode, node))
            {
                throw new PftCompilerException();
            }

            DecreaseIndent();
            WriteIndent();
            WriteLine("} // end of method");
            WriteLine();

            _currentNode = null;
        }

        /// <summary>
        /// Get source code.
        /// </summary>
        [NotNull]
        public string GetSourceCode()
        {
            string result = Output.ToString();

            return result;
        }

        /// <summary>
        /// Increase indent.
        /// </summary>
        [NotNull]
        public PftCompiler IncreaseIndent()
        {
            Indent++;

            return this;
        }

        /// <summary>
        /// Mark the node as ready.
        /// </summary>
        public void MarkReady
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            NodeInfo info = Nodes.Get(node);
            if (info.Ready)
            {
                throw new IrbisException();
            }
            info.Ready = true;
        }

        /// <summary>
        /// Referenct method for the node.
        /// </summary>
        [NotNull]
        public PftCompiler RefNodeMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            NodeInfo info = Nodes.Get(node);

            if (!info.Ready)
            {
                throw new PftCompilerException();
            }

            Write
                (
                    "{0}{1}",
                    NodeMethodPrefix,
                    info.Id
                );

            return this;
        }

        /// <summary>
        /// Set the provider.
        /// </summary>
        public void SetProvider
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string StartClass()
        {
            string result = "CompiledPft_" + StringUtility.Random(10);

            foreach (string nameSpace in Usings)
            {
                WriteLine("using {0};", nameSpace);
            }
            WriteLine();

            WriteLine("public sealed class {0}", result);
            IncreaseIndent();
            WriteIndent();
            WriteLine(": PftPacket");
            DecreaseIndent();
            WriteIndent();
            WriteLine("{");
            IncreaseIndent();
            WriteIndent();
            WriteLine("public {0} (PftContext context)", result);
            IncreaseIndent();
            WriteIndent();
            WriteLine(": base(context)");
            DecreaseIndent();
            WriteIndent();
            WriteLine("{");
            WriteIndent(); //-V3010
            WriteLine("}"); //-V3010
            WriteLine();

            return result;
        }

        /// <summary>
        /// Start method for the node.
        /// </summary>
        public void StartMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            if (!ReferenceEquals(_currentNode, null))
            {
                throw new IrbisException();
            }

            _currentNode = node;

            NodeInfo info = Nodes.Get(node);

            if (info.Ready)
            {
                throw new PftCompilerException();
            }

            WriteIndent(); //-V3010
            WriteLine
                ( //-V3010
                    "// {0}: {1}",
                    PftNode.SimplifyTypeName(node.GetType().Name),
                    CompilerUtility.ShortenText(node.ToString())
                );
            string returnType = "void";
            if (node is PftBoolean)
            {
                returnType = "bool";
            }
            else if (node is PftNumeric)
            {
                returnType = "double";
            }
            WriteIndent(); //-V3010
            WriteLine
                ( //-V3010
                    "public {0} {1}{2}()",
                    returnType,
                    NodeMethodPrefix,
                    info.Id
                );
            WriteIndent(); //-V3010
            WriteLine("{"); //-V3010
            IncreaseIndent();

            if (Debug)
            {
                WriteIndent(); //-V3010
                WriteLine
                    ( //-V3010
                        "DebuggerHook({0});",
                        info.Id
                    );
            }
        }

        #region Write

        /// <inheritdoc cref="TextWriter.Write(char)" />
        [NotNull]
        public PftCompiler Write
            (
                char value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(int)" />
        [NotNull]
        public PftCompiler Write
            (
                int value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(double)" />
        [NotNull]
        public PftCompiler Write
            (
                double value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(string)" />
        [NotNull]
        public PftCompiler Write
            (
                [NotNull] string value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(object)" />
        [NotNull]
        public PftCompiler Write
            (
                [NotNull] object value
            )
        {
            Output.Write(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.Write(string,object[])" />
        [NotNull]
        public PftCompiler Write
            (
                [NotNull] string format,
                params object[] arg
            )
        {
            Output.Write(format, arg);

            return this;
        }

        /// <summary>
        /// Write indentation.
        /// </summary>
        [NotNull]
        public PftCompiler WriteIndent()
        {
            for (int i = 0; i < Indent; i++)
            {
                Output.Write("    ");
            }

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine()" />
        [NotNull]
        public PftCompiler WriteLine()
        {
            Output.WriteLine();

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(char)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                char value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(int)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                int value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(double)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                double value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(string)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                [NotNull] string value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(object)" />
        [NotNull]
        public PftCompiler WriteLine
            (
                [NotNull] object value
            )
        {
            Output.WriteLine(value);

            return this;
        }

        /// <inheritdoc cref="TextWriter.WriteLine(string,object[])" />
        [NotNull]
        public PftCompiler WriteLine
            (
                [NotNull] string format,
                params object[] arg
            )
        {
            Output.WriteLine(format, arg);

            return this;
        }

        #endregion

        #endregion

        #region Object members

        #endregion
    }
}
