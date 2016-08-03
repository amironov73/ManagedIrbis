/* GlobalCorrector.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status:poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Gbl;
using ManagedIrbis.Network;
using ManagedIrbis.Network.Commands;
using ManagedIrbis.Network.Sockets;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl
{
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GlobalCorrector
    {
    }
}
