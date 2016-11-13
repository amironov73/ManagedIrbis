/* PftProcedureManager.cs --
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

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Procedure manager.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftProcedureManager
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public Dictionary<string, PftProcedure> Registry { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedureManager()
        {
            Registry = new Dictionary<string, PftProcedure>
                (
#if NETCORE || UAP || WIN81

                    StringComparer.OrdinalIgnoreCase

#else

                    StringComparer.InvariantCultureIgnoreCase

#endif
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Find specified procedure.
        /// </summary>
        [CanBeNull]
        public PftProcedure FindProcedure
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            PftProcedure result;
            Registry.TryGetValue(name, out result);

            return result;
        }

        /// <summary>
        /// Execute the procedure.
        /// </summary>
        public void Execute
            (
                [NotNull] PftContext context,
                [NotNull] string name,
                [CanBeNull] string argument
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(name, "name");
        }

        #endregion
    }
}
