// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftCompiler.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.CodeDom.Compiler;
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

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
        #region MyRegion

        /// <summary>
        /// Prefix for node methods.
        /// </summary>
        public const string NodeMethodPrefix = "NodeMethod";

        #endregion

        #region Properties

        [NotNull]
        internal NodeDictionary Dictionary { get; private set; }

        [NotNull]
        internal TextWriter Output { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCompiler()
        {
            Dictionary = new NodeDictionary();
            Output = new StringWriter();
        }

        #endregion

        #region Private members

        private PftNode _currentNode;

        private void _RenumberNodes
            (
                [NotNull] PftNode rootNode
            )
        {
            NumberingVisitor visitor = new NumberingVisitor(Dictionary);
            bool result = rootNode.AcceptVisitor(visitor);
            if (!result)
            {
                throw new PftCompilerException();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Call method for the node.
        /// </summary>
        public void CallNodeMethod
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            NodeInfo info = Dictionary.Get(node);
            Output.WriteLine
                (
                    "{0}{1} (); // {2}",
                    NodeMethodPrefix,
                    info.Id,
                    PftNode.SimplifyTypeName(node.GetType().Name)
                );
        }

        /// <summary>
        /// Call nodes.
        /// </summary>
        public void CallNodes
            (
                [NotNull] IList<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            foreach (PftNode node in nodes)
            {
                CallNodeMethod(node);
            }
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

            _RenumberNodes(program);

            string result = StartClass();

            program.Compile(this);

            EndClass();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndClass()
        {
            if (!ReferenceEquals(_currentNode, null))
            {
                throw new PftCompilerException();
            }

            Output.WriteLine("}");
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

            Output.WriteLine("}");
            Output.WriteLine();

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
        /// Mark the node as ready.
        /// </summary>
        public void MarkReady
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            NodeInfo info = Dictionary.Get(node);
            if (info.Ready)
            {
                throw new IrbisException();
            }
            info.Ready = true;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public string StartClass()
        {
            string result = StringUtility.Random(10);

            Output.WriteLine("using System;");
            Output.WriteLine("using System.CodeDom.Compiler;");
            Output.WriteLine("using System.Collections.Generic;");
            Output.WriteLine("using System.Diagnostics;");
            Output.WriteLine("using System.IO;");
            Output.WriteLine("using System.Linq;");
            Output.WriteLine("using System.Text;");
            Output.WriteLine("using System.Threading.Tasks;");
            Output.WriteLine("using AM;");
            Output.WriteLine("using AM.Logging;");
            Output.WriteLine();
            Output.WriteLine("public class {0}", result);
            Output.WriteLine("{");
            Output.WriteLine();

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

            NodeInfo info = Dictionary.Get(node);

            Output.WriteLine
                (
                    "// {0}: {1}",
                    PftNode.SimplifyTypeName(node.GetType().Name),
                    CompilerUtility.ShortenText(node.ToString())
                );
            Output.WriteLine
                (
                    "public void {0}{1}",
                    NodeMethodPrefix,
                    info.Id
                );
            Output.WriteLine("{");
        }

        #endregion

        #region Object members

        #endregion
    }
}
