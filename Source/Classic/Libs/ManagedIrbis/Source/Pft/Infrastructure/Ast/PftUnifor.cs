// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftUnifor.cs --
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

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

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

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.DeserializeAst" />
        protected internal override void DeserializeAst
            (
                BinaryReader reader
            )
        {
            base.DeserializeAst(reader);

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

                foreach (PftNode node in Children)
                {
                    node.Execute(subContext);
                }

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

        /// <inheritdoc cref="PftNode.SerializeAst" />
        protected internal override void SerializeAst
            (
                BinaryWriter writer
            )
        {
            base.SerializeAst(writer);

            writer.WriteNullable(Name);
        }

        #endregion
    }
}
