/* GblBuilderTest.cs --
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
using ManagedIrbis.Gbl;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class GblBuilderTest
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
        public void TestGblBuilder()
        {
            string field3000 = string.Format
                (
                    "'{0}'",
                    DateTime.Now
                );

            GblBuilder builder = new GblBuilder()
                .Add("3000", field3000)
                .Nop()
                .Delete("3000");

            GblResult result = builder.Execute
                (
                    Connection.ThrowIfNull("Connection"),
                    "IBIS",
                    100,
                    110
                );

            string text = "Processed: "
                + result.RecordsProcessed
                + ", success: "
                + result.RecordsSucceeded
                + ", failed: "
                + result.RecordsFailed
                + ". "
                + StringUtility.Join
                (
                    "| ",
                    result.Protocol
                )
                .SafeSubstring(0, 70);

            Write(text);
        }

        #endregion
    }
}
