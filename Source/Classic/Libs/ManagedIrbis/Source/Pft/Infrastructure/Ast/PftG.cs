/* PftG.cs -- global variable reference
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
    /// Global variable reference
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftG
        : PftField
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            FieldSpecification specification = new FieldSpecification();
            if (!specification.Parse(text))
            {
                throw new PftSyntaxException();
            }

            Apply(specification);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        #endregion
    }
}
