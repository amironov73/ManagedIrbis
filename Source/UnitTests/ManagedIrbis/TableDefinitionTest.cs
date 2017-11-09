using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class TableDefinitionTest
    {
        [NotNull]
        private TableDefinition _GetTable()
        {
            return new TableDefinition
            {
                DatabaseName = "IBIS",
                Table = "@tabf1w",
                Headers = { "Первая строка", "Вторая строка" },
                SearchQuery = "T=A$",
                MfnList = { 2, 12, 85, 6 }
            };
        }

        [TestMethod]
        public void TableDefinition_Construction_1()
        {
            TableDefinition table = new TableDefinition();
            Assert.IsNull(table.DatabaseName);
            Assert.IsNull(table.Table);
            Assert.IsNotNull(table.Headers);
            Assert.AreEqual(0, table.Headers.Count);
            Assert.IsNull(table.Mode);
            Assert.IsNull(table.SearchQuery);
            Assert.AreEqual(0, table.MinMfn);
            Assert.AreEqual(0, table.MaxMfn);
            Assert.IsNull(table.SequentialQuery);
            Assert.IsNotNull(table.MfnList);
            Assert.AreEqual(0, table.MfnList.Count);
        }

        [TestMethod]
        public void TableDefinition_Properties_1()
        {
            TableDefinition table = new TableDefinition();
            table.DatabaseName = "IBIS";
            Assert.AreEqual("IBIS", table.DatabaseName);
            table.Table = "@tabf1w";
            Assert.AreEqual("@tabf1w", table.Table);
            table.Headers.Add("Первая строка");
            Assert.AreEqual(1, table.Headers.Count);
            Assert.AreEqual("Первая строка", table.Headers[0]);
            table.Mode = "1";
            Assert.AreEqual("1", table.Mode);
            table.SearchQuery = "T=A$";
            Assert.AreEqual("T=A$", table.SearchQuery);
            table.MinMfn = 1;
            Assert.AreEqual(1, table.MinMfn);
            table.MaxMfn = 2;
            Assert.AreEqual(2, table.MaxMfn);
            table.SequentialQuery = "p(v910^b)";
            Assert.AreEqual("p(v910^b)", table.SequentialQuery);
            table.MfnList.Add(123);
            Assert.AreEqual(1, table.MfnList.Count);
            Assert.AreEqual(123, table.MfnList[0]);
        }

        [TestMethod]
        public void TableDefinition_ToXml_1()
        {
            TableDefinition table = new TableDefinition();
            Assert.AreEqual("<table />", XmlUtility.SerializeShort(table));

            table = _GetTable();
            Assert.AreEqual("<table database=\"IBIS\" table=\"@tabf1w\" search=\"T=A$\"><header>Первая строка</header><header>Вторая строка</header><mfn>2</mfn><mfn>12</mfn><mfn>85</mfn><mfn>6</mfn></table>", XmlUtility.SerializeShort(table));
        }

        [TestMethod]
        public void TableDefinition_ToJson_1()
        {
            TableDefinition table = new TableDefinition();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(table));

            table = _GetTable();
            Assert.AreEqual("{'database':'IBIS','table':'@tabf1w','headers':['Первая строка','Вторая строка'],'search':'T=A$','mfn':[2,12,85,6]}", JsonUtility.SerializeShort(table));
        }
    }
}
