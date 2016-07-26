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
        public void TestDatabaseInfo()
        {
            Connection.ReadRecord("IBIS", 3, true, null);
            DatabaseInfo info = Connection.GetDatabaseInfo("IBIS");
            Write(info);
            Connection.UnlockRecords("IBIS", 3);
            info = Connection.GetDatabaseInfo("ISTU");
            Write(info);
        }

        #endregion
    }
}
