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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

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

        /// <inheritdoc />
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
            protected set
            {
                // Nothing to do here
            }
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
                PftContext context,
                string fileName
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
                    context.Environment.Database,
                    fileName
                );
            string source = context.Environment.ReadFile
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

        private void ParseProgram
            (
                PftContext context
            )
        {
            string fileName = context.Evaluate(Children);
            ParseProgram
                (
                    context,
                    fileName
                );
        }

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc />
        public override object Clone()
        {
            PftInclude result = (PftInclude) base.Clone();

            if (!ReferenceEquals(Program, null))
            {
                result.Program = (PftProgram) Program.Clone();
            }

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (ReferenceEquals(Program, null))
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    ParseProgram
                        (
                            context,
                            Text
                        );
                }
                else
                {
                    ParseProgram(context);
                }
            }

            if (!ReferenceEquals(Program, null))
            {
                Program.Execute(context);
            }

            OnAfterExecution(context);
        }

        #endregion
    }
}
