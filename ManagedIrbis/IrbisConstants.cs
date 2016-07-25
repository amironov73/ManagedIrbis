/* IrbisConstants.cs -- common constants
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Network;
using ManagedIrbis.Network.Commands;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Common constants.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisConstants
    {
        #region Constants

        /// <summary>
        /// Max postings in the packet.
        /// </summary>
        public const int MaxPostings = 32758;

        #endregion
    }
}
