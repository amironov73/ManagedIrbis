/* IndexSpecification.cs --
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
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Index specification (for fields).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct IndexSpecification
    {
        #region Properties

        /// <summary>
        /// Index kind.
        /// </summary>
        public IndexKind Kind { get; set; }

        /// <summary>
        /// Index specified by literal.
        /// </summary>
        public int Literal { get; set; }

        /// <summary>
        /// Index specified by expression.
        /// </summary>
        [CanBeNull]
        public string Expression { get; set; }

        /// <summary>
        /// Compiled <see cref="Expression"/>.
        /// </summary>
        [CanBeNull]
        public PftNumeric Program { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get node info for debugger visualization.
        /// </summary>
        [NotNull]
        public PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = "Index"
            };
            PftNodeInfo kind = new PftNodeInfo
            {
                Name = "Kind",
                Value = Kind.ToString()
            };
            result.Children.Add(kind);
            PftNodeInfo expression = new PftNodeInfo
            {
                Name = "Expression",
                Value = Expression
            };
            result.Children.Add(expression);

            return result;
        }

        #endregion
    }
}
