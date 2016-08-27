/* TestSequentialSearch.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: none
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
using ManagedIrbis.Search;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class TestTemplate
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
        public void TestSequentialSearch()
        {
            SearchParameters parameters = new SearchParameters
            {
                Database = "IBIS",
                SearchExpression = "\"T=A$\"",
                SequentialSpecification = "v200:'O'"
            };

            int[] found = Connection.SequentialSearch
                (
                    parameters
                );

            Write("found: {0}", found.Length);
        }

        #endregion
    }
}
