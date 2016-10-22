/* CreateDatabaseTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    class CreateDatabaseTest
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
        public void CreateDatabase_Test1()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            const string DatabaseName = "NEWDB2";
            const string Description = "New database 2";

            connection.CreateDatabase
                (
                    DatabaseName,
                    Description,
                    false,
                    null
                );

            Write("{0} created | ", DatabaseName);

            Thread.Sleep(700);

            int maxMfn = connection.GetMaxMfn(DatabaseName);
            Write("MaxMfn={0} | ", maxMfn);

            Thread.Sleep(700);

            connection.DeleteDatabase(DatabaseName);

            Write("{0} deleted", DatabaseName);
        }

        #endregion
    }
}
