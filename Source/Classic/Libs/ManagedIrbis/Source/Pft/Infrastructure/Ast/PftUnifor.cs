// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftUnifor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Unifor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftUnifor
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnifor()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnifor
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");

            Name = token.Text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUnifor
            (
                [NotNull] string name,
                params PftNode[] body
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
            foreach (PftNode child in body)
            {
                Children.Add(child);
            }
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode" />
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            PftUnifor otherUnifor = (PftUnifor) otherNode;
            bool result = Name == otherUnifor.Name;
            if (!result)
            {
                throw new PftSerializationException();
            }
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.CompileNodes(Children);

            string actionName = compiler.CompileAction(Children);

            compiler.StartMethod(this);

            if (!string.IsNullOrEmpty(actionName))
            {
                compiler
                    .WriteIndent()
                    .WriteLine("string text = Evaluate({0});", actionName);

                compiler
                    .WriteIndent()
                    .WriteLine
                        (
                            "FormatExit.Execute(Context, null, \"{0}\", text);",
                            CompilerUtility.Escape(Name)
                        );
            }

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Name = reader.ReadNullableString();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            string expression;

            using (PftContextGuard guard = new PftContextGuard(context))
            {
                PftContext subContext = guard.ChildContext;
                subContext.Execute(Children);
                expression = subContext.Text;
            }

            FormatExit.Execute
                (
                    context,
                    this,
                    Name.ThrowIfNull("Name"),
                    expression
                );
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "FormatExit"
            };
            PftNodeInfo body = new PftNodeInfo
            {
                Name = "Body"
            };
            result.Children.Add(body);
            foreach (PftNode node in Children)
            {
                PftNodeInfo info = node.GetNodeInfo();
                body.Children.Add(info);
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            PftNodeCollection children = (PftNodeCollection)Children;
            children.Optimize();

            if (children.Count == 0)
            {
                // Take the node away from the AST

                return null;
            }

            return this;
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WriteNullable(Name);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write('&')
                .Write(Name)
                .Write('(');
            base.PrettyPrint(printer);
            printer.Write(')');
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = StringBuilderCache.Acquire();
            result.Append('&');
            result.Append(Name);
            result.Append('(');
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
