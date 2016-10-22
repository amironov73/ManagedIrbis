/* PftComment.cs -- comment
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
    /// Comment.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftComment
        : PftNode
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftComment()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftComment
            (
                [NotNull] PftToken token
            )
        {
            Text = token.Text;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            OnAfterExecution(context);
        }

        /// <inheritdoc />
        public override void Write
            (
                StreamWriter writer
            )
        {
            // Добавляем пробел для читабельности
            writer.Write("/* ");
            writer.Write(Text);
        }

        #endregion


        #endregion
    }
}
