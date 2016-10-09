/* ReadRawRecordTest.cs --
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
    class ReadRawRecordTest
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
        public void TestReadRawRecord()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string[] lines = connection.ReadRawRecord
                (
                    "IBIS",
                    1
                );

            string text = string.Join
                (
                    "|",
                    lines
                ).SafeSubstring(0, 50);

            Write(text);

        }

        #endregion
    }
}
