/* TextCacheTest.cs --
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
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class TextCacheTest
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
        public void TestTextCache()
        {
            TextCache cache = new TextCache(Connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "!101.PFT"
                );
            string first = cache.Get(specification);
            Debug.Assert(first == null);
            first = cache.GetOrRequest(specification);
            Debug.Assert(first != null);
            Debug.Assert(cache.RequestCount == 1);

            string second = cache.Get(specification);
            Debug.Assert(second != null);
            Debug.Assert(cache.RequestCount == 1);
        }

        #endregion
    }
}
