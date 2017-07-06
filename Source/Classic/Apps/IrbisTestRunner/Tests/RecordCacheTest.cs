/* RecordCacheTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Caching;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class RecordCacheTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        [TestMethod]
        public void RecordCache_Test1()
        {
            RecordCache cache = new RecordCache(Connection);
            MarcRecord record1 = cache.Get(1);
            Debug.Assert(record1 == null);
            record1 = cache.GetOrRequest(1);
            Debug.Assert(record1 != null);
            Debug.Assert(cache.RequestCount == 1);

            MarcRecord record2 = cache.Get(1);
            Debug.Assert(record2 != null);
            Debug.Assert(cache.RequestCount == 1);
        }

        #endregion
    }
}
