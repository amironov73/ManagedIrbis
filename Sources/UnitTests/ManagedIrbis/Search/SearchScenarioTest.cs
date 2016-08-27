using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchScenarioTest
    {
        private void _TestSerialization
            (
                SearchScenario first
            )
        {
            byte[] bytes = first.SaveToMemory();

            SearchScenario second = bytes
                .RestoreObjectFromMemory<SearchScenario>();

            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Prefix, second.Prefix);
            Assert.AreEqual(first.DictionaryType, second.DictionaryType);
            Assert.AreEqual(first.MenuName, second.MenuName);
            Assert.AreEqual(first.OldFormat, second.OldFormat);
            Assert.AreEqual(first.Correction, second.Correction);
            Assert.AreEqual(first.Truncation, second.Truncation);
            Assert.AreEqual(first.Hint, second.Hint);
            Assert.AreEqual(first.ModByDicAuto, second.ModByDicAuto);
            Assert.AreEqual(first.Logic, second.Logic);
            Assert.AreEqual(first.Advance, second.Advance);
            Assert.AreEqual(first.Format, second.Format);
        }

        [TestMethod]
        public void TestSearchScenario_Serialization()
        {
            SearchScenario scenario = new SearchScenario();
            _TestSerialization(scenario);

            scenario = new SearchScenario
            {
                Name = "Author",
                Prefix = "A=",
                Truncation = true,
                Logic = SearchLogicType.OrAndNot
            };
            _TestSerialization(scenario);
        }

        [TestMethod]
        public void TestSearchScenario_Clone()
        {
            SearchScenario first = new SearchScenario
            {
                Name = "Author",
                Prefix = "A=",
                Truncation = true,
                Logic = SearchLogicType.OrAndNot
            };
            SearchScenario second = first.Clone();

            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Prefix, second.Prefix);
            Assert.AreEqual(first.DictionaryType, second.DictionaryType);
            Assert.AreEqual(first.MenuName, second.MenuName);
            Assert.AreEqual(first.OldFormat, second.OldFormat);
            Assert.AreEqual(first.Correction, second.Correction);
            Assert.AreEqual(first.Truncation, second.Truncation);
            Assert.AreEqual(first.Hint, second.Hint);
            Assert.AreEqual(first.ModByDicAuto, second.ModByDicAuto);
            Assert.AreEqual(first.Logic, second.Logic);
            Assert.AreEqual(first.Advance, second.Advance);
            Assert.AreEqual(first.Format, second.Format);
        }

        [TestMethod]
        public void TestSearchScenario_Verify()
        {
            SearchScenario scenario = new SearchScenario
            {
                Name = "Author",
                Prefix = "A=",
                Truncation = true,
                Logic = SearchLogicType.OrAndNot
            };
            Assert.IsTrue(scenario.Verify(false));
        }
    }
}
