// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftConditionParenthesis.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

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
    public sealed class PftConditionParenthesis
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Inner condition.
        /// </summary>
        [CanBeNull]
        public PftCondition InnerCondition { get; set; }

        /// <inheritdoc />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(InnerCondition, null))
                    {
                        nodes.Add(InnerCondition);
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
        public PftConditionParenthesis()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftConditionParenthesis
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
            PftConditionParenthesis result = (PftConditionParenthesis)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(InnerCondition, null))
            {
                result.InnerCondition
                    = (PftCondition)InnerCondition.Clone();
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

            if (ReferenceEquals(InnerCondition, null))
            {
                throw new PftSyntaxException();
            }

            InnerCondition.Execute(context);
            Value = InnerCondition.Value;

            OnAfterExecution(context);
        }

        #endregion
    }
}
