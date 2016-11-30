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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

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

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFieldAssignment()
        {
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
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            PftField field = Field;
            if (ReferenceEquals(field, null))
            {
                throw new IrbisException("Field is null");
            }
            string tag = field.Tag;
            if (string.IsNullOrEmpty(tag))
            {
                throw new IrbisException("Field tag is null");
            }

            string value = context.Evaluate(Children);
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

        /// <inheritdoc/>
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

            if (Children.Count != 0)
            {
                PftNodeInfo body = new PftNodeInfo
                {
                    Name = "Body"
                };
                result.Children.Add(body);

                foreach (PftNode node in Children)
                {
                    body.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        }

        #endregion
    }
}
