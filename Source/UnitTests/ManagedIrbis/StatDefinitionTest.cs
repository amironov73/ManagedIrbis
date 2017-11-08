using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class StatDefinitionTest
    {
        [TestMethod]
        public void StatDefinition_Construction_1()
        {
            StatDefinition definition = new StatDefinition();
            Assert.IsNotNull(definition.Items);
            Assert.AreEqual(0, definition.Items.Count);
            Assert.IsNotNull(definition.MfnList);
            Assert.AreEqual(0, definition.MfnList.Count);
            Assert.IsNull(definition.DatabaseName);
            Assert.AreEqual(0, definition.MinMfn);
            Assert.AreEqual(0, definition.MaxMfn);
            Assert.IsNull(definition.SearchQuery);
            Assert.IsNull(definition.SequentialQuery);
        }

        [TestMethod]
        public void StatDefinition_MinMfn_MaxMfn_1()
        {
            StatDefinition definition = new StatDefinition
            {
                MinMfn = 1,
                MaxMfn = 2
            };
            Assert.AreEqual(1, definition.MinMfn);
            Assert.AreEqual(2, definition.MaxMfn);
        }

        [TestMethod]
        public void StatDefinition_Query_1()
        {
            const string Search = "A=Author$", Sequential="p(v910)";
            StatDefinition definition = new StatDefinition
            {
                SearchQuery = Search,
                SequentialQuery = Sequential
            };
            Assert.AreEqual(Search, definition.SearchQuery);
            Assert.AreEqual(Sequential, definition.SequentialQuery);
        }

        [TestMethod]
        public void StatDefinition_DatabaseName_1()
        {
            const string Database = "IBIS";
            StatDefinition definition = new StatDefinition
            {
                DatabaseName = Database
            };
            Assert.AreEqual(Database, definition.DatabaseName);
        }

        [TestMethod]
        public void StatDefinition_Item_1()
        {
            StatDefinition.Item item = new StatDefinition.Item();
            Assert.IsNull(item.Field);
            Assert.AreEqual(0, item.Length);
            Assert.AreEqual(0, item.Count);
            Assert.AreEqual(StatDefinition.SortMethod.None, item.Sort);

            item.Field = "910^b";
            Assert.AreEqual("910^b", item.Field);

            item.Length = 10;
            Assert.AreEqual(10, item.Length);

            item.Count = 100;
            Assert.AreEqual(100, item.Count);

            item.Sort = StatDefinition.SortMethod.Ascending;
            Assert.AreEqual(StatDefinition.SortMethod.Ascending, item.Sort);
        }

        [TestMethod]
        public void StatDefinition_Item_ToString_1()
        {
            StatDefinition.Item item = new StatDefinition.Item
            {
                Count = 100,
                Field = "910^b",
                Length = 10,
                Sort = StatDefinition.SortMethod.Ascending
            };
            Assert.AreEqual("910^b,10,100,1", item.ToString());

        }
    }
}
