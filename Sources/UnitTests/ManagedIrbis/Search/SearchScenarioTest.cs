using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchScenarioTest
        : Common.CommonUnitTest
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
        public void SearchScenario_Clone()
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
        public void SearchScenario_ParseIniFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "istu.ini"
                );
            IniFile file = new IniFile(fileName);

            SearchScenario[] scenarios = SearchScenario.ParseIniFile(file);
            Assert.AreEqual(73, scenarios.Length);

            foreach (SearchScenario scenario in scenarios)
            {
                Assert.IsNotNull(scenario.Name);
                Assert.IsNotNull(scenario.Prefix);
            }
        }

        [TestMethod]
        public void SearchScenario_Serialization()
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
        public void SearchScenario_Verify()
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
