// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftInclude.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftInclude
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Parsed program of the included file.
        /// </summary>
        [CanBeNull]
        public PftProgram Program { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Program, null))
                    {
                        nodes.Add(Program);
                    }
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            [ExcludeFromCodeCoverage]
            protected set
            {
                // Nothing to do here

                Log.Error
                (
                    "PftInclude::Children: "
                    + "set value="
                    + value.ToVisibleString()
                );
            }
        }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftInclude()
        {
        }

        /// <summary>
        /// Construction.
        /// </summary>
        public PftInclude
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Text = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftInclude
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        private void ParseProgram
            (
                [NotNull] PftContext context,
                [NotNull] string fileName
            )
        {
            string ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext))
            {
                fileName += ".pft";
            }
            FileSpecification specification
                = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    context.Provider.Database,
                    fileName
                );
            string source = context.Provider.ReadFile
                (
                    specification
                );
            if (string.IsNullOrEmpty(source))
            {
                return;
            }
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(source);
            PftParser parser = new PftParser(tokens);
            Program = parser.Parse();
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="PftNode.Clone" />
        public override object Clone()
        {
            PftInclude result = (PftInclude)base.Clone();

            if (!ReferenceEquals(Program, null))
            {
                result.Program = (PftProgram)Program.Clone();
            }

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (string.IsNullOrEmpty(Text))
            {
                throw new PftCompilerException();
            }

            if (ReferenceEquals(Program, null))
            {
                using (PftContext context = new PftContext(null))
                {
                    context.SetProvider(compiler.Provider);
                    ParseProgram(context, Text);
                }
            }

            PftProgram program = (PftProgram)Program.ThrowIfNull().Clone();
            program.Optimize();

            compiler.RenumberNodes(program);
            program.Compile(compiler);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .CallNodeMethod(program);

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

            Program = (PftProgram)PftSerializer.DeserializeNullable(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (ReferenceEquals(Program, null))
            {
                try
                {
                    if (string.IsNullOrEmpty(Text))
                    {
                        throw new PftSyntaxException();
                    }
                    ParseProgram
                        (
                            context,
                            Text
                        );
                }
                catch (Exception exception)
                {
                    PftException pftException = new PftException
                        (
                            "Include: " + Text.ToVisibleString(),
                            exception
                        );

                    throw pftException;
                }
            }

            if (!ReferenceEquals(Program, null))
            {
                Program.Execute(context);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "Include",
                Value = Text
            };

            if (!ReferenceEquals(Program, null))
            {
                PftNodeInfo program = new PftNodeInfo
                {
                    Name = "Program"
                };
                result.Children.Add(program);
                program.Children.Add(Program.GetNodeInfo());
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, Program);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return "include(" + Text + ")";
        }

        #endregion
    }
}
