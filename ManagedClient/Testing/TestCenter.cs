/* TestCenter.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedClient;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Testing
{
    /// <summary>
    /// TestCenter.
    /// </summary>
    [PublicAPI]
    public static class TestCenter
    {
        #region Properties

        /// <summary>
        /// Connection for tests.
        /// </summary>
        [CanBeNull]
        public static IrbisConnection Connection { get; set; }

        #endregion
    }
}
