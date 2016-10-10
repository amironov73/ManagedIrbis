/* FormatExit.cs --
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
    /// Umarci.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FormatExit
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static Dictionary<string, IFormatExit> Registry { get; private set; }

        #endregion

        #region Construction

        static FormatExit()
        {
            Registry = new Dictionary<string, IFormatExit>
                (
                    StringComparer.InvariantCultureIgnoreCase
                );

            Unifor unifor = new Unifor();
            Registry.Add("unifor", unifor);
            Registry.Add("uf", unifor);

            Umarci umarci = new Umarci();
            Registry.Add("umarci", umarci);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the expression on the given context.
        /// </summary>
        public static void Execute
            (
                [NotNull] PftContext context,
                [NotNull] PftNode node,
                [NotNull] string name,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(name, "name");

            IFormatExit format;
            if (!Registry.TryGetValue(name, out format))
            {
                throw new PftSemanticException("unknown format exit: " + name);
            }

            format.Execute
                (
                    context,
                    node,
                    expression
                );
        }

        #endregion
    }
}
