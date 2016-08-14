/* TestTemplate.cs -- IRBIS64 test template
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
    class GblTest
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
        public void TestGbl1()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string field3000 = string.Format
                (
                    "'{0}'",
                    DateTime.Now
                );

            GblStatement[] statements =
            {
                new GblStatement
                {
                    Command = GblCode.Add,
                    Format1 = field3000,
                    Format2 = "XXXXXXXXXXX",
                    Parameter1 = "3000",
                    Parameter2 = "*"
                }, 
            };

            GblSettings settings = GblSettings.ForSearchExpression
                (
                    connection,
                    "IBIS",
                    "\"I=37/К88-602720\"",
                    statements
                );

            GblResult result = connection
                .GlobalCorrection(settings);

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

            Write (text);
        }

        [TestMethod]
        public void TestGbl2()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            string field3000 = string.Format
                (
                    "'{0}'",
                    DateTime.Now
                );

            GblStatement[] statements =
            {
                new GblStatement
                {
                    Command = GblCode.Add,
                    Format1 = field3000,
                    Format2 = "XXXXXXXXXXX",
                    Parameter1 = "3000",
                    Parameter2 = "*"
                }, 
            };

            GblSettings settings = GblSettings.ForList
                (
                    connection,
                    "IBIS",
                    new int[] {1, 2, 3},
                    statements
                );

            GblResult result = connection
                .GlobalCorrection(settings);

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

        [TestMethod]
        public void TestGbl3()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            GblSettings settings = GblSettings.ForList
                (
                    connection,
                    "IBIS",
                    new []{1,2,3}
                )
                .SetFileName("@test");

            GblResult result = connection.GlobalCorrection(settings);

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
