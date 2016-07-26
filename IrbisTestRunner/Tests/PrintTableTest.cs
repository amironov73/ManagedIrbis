/* PrintTableTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
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
    class PrintTableTest
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
        public void TestPrintTable()
        {
            TableDefinition table = new TableDefinition
            {
                DatabaseName = "IBIS",
                Table = "@tabf1w",
                SearchQuery = "T=А$"
            };
            string text = Connection.PrintTable(table);
            string filePath = Path.Combine
                (
                    Path.GetTempPath(),
                    "table.rtf"
                );
            File.WriteAllText
                (
                    filePath,
                    text,
                    IrbisEncoding.Ansi
                );

            Write("table written to {0}", filePath);
        }

        #endregion
    }
}
