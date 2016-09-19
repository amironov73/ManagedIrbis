/* ActualizeRecordTest.cs -- IRBIS64 test template
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
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class ActualizeRecordTest
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
        public void ActualizeRecord_OneMfn()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            connection.ActualizeRecord
                (
                    "IBIS",
                    1
                );
        }

        [TestMethod]
        public void ActualizeRecord_WholeDatabase()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            connection.ActualizeDatabase
                (
                    "IBIS"
                );
        }

        #endregion
    }
}
