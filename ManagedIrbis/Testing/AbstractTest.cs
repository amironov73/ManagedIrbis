/* AbstractTest.cs --
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

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Testing
{
    /// <summary>
    /// Abstract test.
    /// </summary>
    [PublicAPI]
    public abstract class AbstractTest
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [CanBeNull]
        public IrbisConnection Connection { get; set; }

        /// <summary>
        /// Path to test data.
        /// </summary>
        [CanBeNull]
        public string DataPath { get; set; }

        #endregion

        #region Public methods

        #endregion
    }
}
