using System.IO;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void SearchScenario_Clone_1()
        {
            SearchScenario first = new SearchScenario
            {
                Name = "Author",
                Prefix = "A",
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
        public void SearchScenario_ParseIniFile_1()
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
        public void SearchScenario_Serialization_1()
        {
            SearchScenario scenario = new SearchScenario();
            _TestSerialization(scenario);

            scenario = new SearchScenario
            {
                Name = "Author",
                Prefix = "A",
                Truncation = true,
                Logic = SearchLogicType.OrAndNot
            };
            _TestSerialization(scenario);
        }

        [TestMethod]
        public void SearchScenario_Verify_1()
        {
            SearchScenario scenario = new SearchScenario
            {
                Name = "Author",
                Prefix = "A",
                Truncation = true,
                Logic = SearchLogicType.OrAndNot
            };
            Assert.IsTrue(scenario.Verify(false));
        }

        [TestMethod]
        public void SearchScenario_ToXml_1()
        {
            SearchScenario scenario = new SearchScenario();
            Assert.AreEqual("<search type=\"Standard\" truncation=\"false\" logic=\"Or\" />", XmlUtility.SerializeShort(scenario));

            scenario = new SearchScenario
            {
                Name = "Author",
                Prefix = "A",
                Truncation = true,
                Logic = SearchLogicType.OrAndNot
            };
            Assert.AreEqual("<search name=\"Author\" prefix=\"A\" type=\"Standard\" truncation=\"true\" logic=\"OrAndNot\" />", XmlUtility.SerializeShort(scenario));
        }

        [TestMethod]
        public void SearchScenario_ToJson_1()
        {
            SearchScenario scenario = new SearchScenario();
            Assert.AreEqual("{'type':0,'truncation':false,'logic':0}", JsonUtility.SerializeShort(scenario));

            scenario = new SearchScenario
            {
                Name = "Author",
                Prefix = "A",
                Truncation = true,
                Logic = SearchLogicType.OrAndNot
            };
            Assert.AreEqual("{'name':'Author','prefix':'A','type':0,'truncation':true,'logic':2}", JsonUtility.SerializeShort(scenario));
        }

        [TestMethod]
        public void SearchScenario_ToString_1()
        {
            SearchScenario scenario = new SearchScenario();
            Assert.AreEqual("(null) (null)", scenario.ToString());

            scenario = new SearchScenario
            {
                Name = "Author",
                Prefix = "A",
                Truncation = true,
                Logic = SearchLogicType.OrAndNot
            };
            Assert.AreEqual("A Author", scenario.ToString());
        }
    }
}
