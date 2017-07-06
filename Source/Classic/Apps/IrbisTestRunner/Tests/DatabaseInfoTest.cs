/* DatabaseInfoTest.cs --
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
    class DatabaseInfoTest
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
        public void DatabaseInfo_Test1()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            connection.ReadRecord("IBIS", 3, true, null);
            DatabaseInfo info = connection.GetDatabaseInfo("IBIS");
            Write(info);
            Write(" ");

            connection.UnlockRecords("IBIS", 3);
            info = connection.GetDatabaseInfo("IBIS");
            Write(info);
            Write(" ");
        }

        #endregion
    }
}
