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
            PftUtility.AssignField
                (
                    context,
                    tag,
                    field.FieldRepeat,
                    value
                );

            OnAfterExecution(context);
        }

        #endregion
    }
}
