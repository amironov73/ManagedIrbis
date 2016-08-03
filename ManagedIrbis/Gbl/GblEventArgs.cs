/* GblEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status:poor
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

namespace ManagedIrbis.Gbl
{
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblEventArgs
        : EventArgs
    {
        #region Properties

        [NotNull]
        public GlobalCorrector Corrector { get; set; }

        public bool Cancel { get; set; }

        #endregion

        #region Construction

        public GblEventArgs
            (
                [NotNull] GlobalCorrector corrector
            )
        {
            Code.NotNull(corrector, "corrector");

            Corrector = corrector;
        }

        #endregion
    }
}
