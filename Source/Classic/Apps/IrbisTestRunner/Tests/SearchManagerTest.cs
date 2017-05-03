/* SearchManagerTest.cs -- IRBIS64 test template
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

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Search;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class SearchManagerTest
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
        public void SearchManager_LoadSearchScenarios()
        {
            IrbisConnection connection
                = Connection.ThrowIfNull();

            IrbisProvider provider
                = new ConnectedClient(connection);
            SearchManager manager
                = new SearchManager(provider);
            FileSpecification file = new FileSpecification
                (
                    IrbisPath.MasterFile, 
                    "ISTU",
                    "istu.ini"
                );
            SearchScenario[] scenarios
                = manager.LoadSearchScenarios(file);

            string[] items = scenarios.Select
                (
                    scenario => scenario.Prefix
                                + " " + scenario.Name
                )
                .ToArray();

            string text = string.Join
                (
                    Environment.NewLine,
                    items
                );
            Write(text);
        }

        #endregion
    }
}
