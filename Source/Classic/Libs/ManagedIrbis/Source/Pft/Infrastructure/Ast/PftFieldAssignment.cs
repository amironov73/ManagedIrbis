// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftFieldAssignment.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using AM;
using AM.Collections;
using AM.Logging;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftFieldAssignment
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        [CanBeNull]
        public PftField Field { get; set; }

        /// <summary>
        /// Expression.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> Expression { get; private set; }

        /// <inheritdoc cref="PftNode.Children" />
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
                    nodes.AddRange(Expression);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set
            {
                // Nothing to do here

                Log.Error
                    (
                        "PftFieldAssignment::Children: "
                        + "set value="
                        + value.NullableToVisibleString()
                    );
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFieldAssignment()
        {
            Expression = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFieldAssignment
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Expression = new NonNullCollection<PftNode>();
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftFieldAssignment result
                = (PftFieldAssignment) base.Clone();

            if (!ReferenceEquals(Field, null))
            {
                result.Field = (PftField) Field.Clone();
            }
            foreach (PftNode node in Expression)
            {
                result.Expression.Add((PftNode) node.Clone());
            }

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            PftField field = Field;
            if (ReferenceEquals(field, null))
            {
                Log.Error
                    (
                        "PftFieldAssignment::Execute: "
                        + "field not set"
                    );

                throw new IrbisException("Field is null");
            }
            string tag = field.Tag;
            if (string.IsNullOrEmpty(tag))
            {
                Log.Error
                    (
                        "PftFieldAssignment::Execute: "
                        + "field tag not set"
                    );

                throw new IrbisException("Field tag is null");
            }

            string value = context.Evaluate(Expression);
            if (field.SubField == SubField.NoCode)
            {
                PftUtility.AssignField
                    (
                        context,
                        tag,
                        field.FieldRepeat,
                        value
                    );
            }
            else
            {
                PftUtility.AssignSubField
                    (
                        context,
                        tag,
                        field.FieldRepeat,
                        field.SubField,
                        field.SubFieldRepeat,
                        value
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "FieldAssignment"
            };

            if (!ReferenceEquals(Field, null))
            {
                result.Children.Add(Field.GetNodeInfo());
            }

            if (Expression.Count != 0)
            {
                PftNodeInfo expression = new PftNodeInfo
                {
                    Name = "Expression"
                };
                result.Children.Add(expression);

                foreach (PftNode node in Expression)
                {
                    expression.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        }

        #endregion
    }
}
