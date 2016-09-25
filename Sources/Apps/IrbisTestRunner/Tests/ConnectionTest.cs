/* ConnectionTest.cs --
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
    class ConnectionTest
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
        public void Connection_IniFile()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string dbNameCat
                = connection.IniFile["Main", "DBNNAMECAT"];
            Write("DBNNAMECAT=" + dbNameCat);
        }

        //[TestMethod]
        public void Connection_Failure()
        {
            IrbisConnection secondConnection
                = Connection.Clone(false);
            secondConnection.Username = "NoSuchUser";
            secondConnection.Password = "BadPassword";

            bool allOk = false;

            try
            {
                secondConnection.Connect();
            }
            catch (IrbisException)
            {
                allOk = true;
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // Nothing to do
            }

            Write
                (
                    allOk
                    ? "good"
                    : "bad"
                );

        }

        #endregion
    }
}
