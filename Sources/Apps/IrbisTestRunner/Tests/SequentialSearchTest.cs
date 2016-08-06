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
            int[] found = Connection.SequentialSearch
                (
                    "IBIS", // database
                    "\"T=А$\"", // expression
                    1, // first record
                    0, // number of records
                    0, // minimal MFN
                    0, // maximal MFN
                    "v200:'О'", // sequential
                    null // format
                );

            Write("found: {0}", found.Length);
        }

        #endregion
    }
}
