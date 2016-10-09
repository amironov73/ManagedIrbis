/* PftS.cs -- конкатенация строк
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
    /// Функция S возвращает текст, полученный 
    /// в результате вычисления ее аргумента.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftS
        : PftNode
    {
        #region Properties

        #endregion

        #region Construction

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

            if (!context.BreakFlag)
            {
                foreach (PftNode child in Children)
                {
                    child.Execute
                        (
                            context
                        );
                }
            }

            OnAfterExecution(context);
        }

        #endregion
    }
}
