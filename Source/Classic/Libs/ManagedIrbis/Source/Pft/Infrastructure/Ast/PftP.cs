// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftP.cs --
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftP
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        [CanBeNull]
        public PftField Field { get; set; }

        /// <inheritdoc />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Field, null))
                    {
                        nodes.Add(Field);
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
        public PftP()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftP
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc />
        public override object Clone()
        {
            PftP result = (PftP) base.Clone();

            if (!ReferenceEquals(Field, null))
            {
                result.Field = (PftField) Field.Clone();
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

            if (ReferenceEquals(Field, null))
            {
                throw new PftSyntaxException(this);
            }

            Value = Field.HaveRepeat(context);

            OnAfterExecution(context);
        }

        /// <inheritdoc/>
        public override void Write
            (
                StreamWriter writer
            )
        {
            writer.Write("p(");
            if (!ReferenceEquals(Field, null))
            {
                Field.Write(writer);
            }
            writer.Write(')');
        }

        #endregion
    }
}
