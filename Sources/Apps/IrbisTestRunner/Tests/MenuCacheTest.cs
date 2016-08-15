/* MenuCacheTest.cs --
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
using ManagedIrbis.Menus;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class MenuCacheTest
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
        public void TestMenuCache()
        {
            MenuCache cache = new MenuCache(Connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "II.MNU"
                );
            MenuFile first = cache.Get(specification);
            Debug.Assert(first == null);
            first = cache.GetOrRequest(specification);
            Debug.Assert(first != null);
            Debug.Assert(cache.RequestCount == 1);

            MenuFile second = cache.Get(specification);
            Debug.Assert(second != null);
            Debug.Assert(cache.RequestCount == 1);

            Write(second.Entries[0]);
        }

        #endregion
    }
}
