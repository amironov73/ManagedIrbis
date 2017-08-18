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
        public void JsonUtility_Include()
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
    }
}
