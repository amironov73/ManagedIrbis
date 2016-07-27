/* ServerStatTest.cs --
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
    class ServerStatTest
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
        public void TestServerStat()
        {
            ServerStat serverStat = Connection.GetServerStat();

            Write
                (
                    "total commands: {0} | running clients: ",
                    serverStat.TotalCommandCount
                );

            string text = string.Join
                (
                    ", ",
                    serverStat.RunningClients.Select(c => c.Workstation)
                );
            Write(text);
        }

        #endregion
    }
}
