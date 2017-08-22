using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTests.AM.Json
{
    [TestClass]
    public class JsonUtilityTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void JsonUtility_Include_1()
        {
            string mainFilePath = Path.Combine
                (
                    TestDataPath,
                    "mainFile.json"
                );
            mainFilePath = Path.GetFullPath(mainFilePath);
            JObject mainObject = JsonUtility.ReadObjectFromFile(mainFilePath);
            JsonUtility.Include(mainObject, "included");
            const string expected = "{\"first\":1,\"second\":2,\"included\":{\"third\":3,\"fifth\":5},\"fourth\":4}";
            string actual = mainObject.ToString(Formatting.None);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void JsonUtility_Include_2()
        {
            string mainFilePath = Path.Combine
                (
                    TestDataPath,
                    "mainFile2.json"
                );
            mainFilePath = Path.GetFullPath(mainFilePath);
            JObject mainObject = JsonUtility.ReadObjectFromFile(mainFilePath);
            JsonUtility.Include(mainObject);
            const string expected = "{\"first\":1,\"second\":2,\"included\":{\"third\":3,\"fifth\":5},\"fourth\":4}";
            string actual = mainObject.ToString(Formatting.None);
            Assert.AreEqual(expected, actual);
        }
    }
}
