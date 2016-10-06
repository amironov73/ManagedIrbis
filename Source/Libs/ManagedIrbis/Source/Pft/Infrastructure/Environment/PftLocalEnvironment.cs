/* PftLocalEnvironment.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.Globalization;
using System.Text;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Environment
{
    /// <summary>
    /// Local operation mode.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftLocalEnvironment
        : PftEnvironmentAbstraction
    {
        #region PftEnvironmentAbstraction members

        #endregion
    }
}
