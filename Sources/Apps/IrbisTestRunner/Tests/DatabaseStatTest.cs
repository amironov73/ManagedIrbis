/* DatabaseStatTest.cs --
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
    class DatabaseStatTest
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
        public void DatabaseStat_Test1()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            StatDefinition.Item item = new StatDefinition.Item
            {
                Field = "v200^a",
                Length = 10,
                Count = 100,
                Sort = StatDefinition.SortMethod.Ascending
            };
            StatDefinition definition = new StatDefinition
            {
                DatabaseName = "IBIS",
                SearchQuery = "T=А$"
            };
            definition.Items.Add(item);
            string text = connection.GetDatabaseStat(definition);
            string filePath = Path.Combine
                (
                    Path.GetTempPath(),
                    "stat.rtf"
                );
            File.WriteAllText
                (
                    filePath,
                    text,
                    IrbisEncoding.Ansi
                );

            Write("stat written to {0}", filePath);
        }

        #endregion
    }
}
