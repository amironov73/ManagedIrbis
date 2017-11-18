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
        class DummyClass
        {
            [JsonProperty("first")]
            public string First { get; set; }

            [JsonProperty("third")]
            public string Third { get; set; }

            [JsonProperty("fifth")]
            public string Fifth { get; set; }
        }

        [TestMethod]
        public void JsonUtility_ReadObjectFromFile_1()
        {
            JObject source = new JObject()
            {
                new JProperty("first", "second"),
                new JProperty("third", "fourth"),
                new JProperty("fifth", "sixth")
            };

            string fileName = Path.GetTempFileName();
            JsonUtility.SaveObjectToFile
                (
                    source,
                    fileName
                );

            JObject target = JsonUtility.ReadObjectFromFile
                (
                    fileName
                );

            Assert.AreEqual
                (
                    source.ToString(),
                    target.ToString()
                );
        }

        [TestMethod]
        public void JsonUtility_ReadObjectFromFile_2()
        {
            DummyClass source = new DummyClass
            {
                First = "second",
                Third = "fourth",
                Fifth = "sixth"
            };

            string fileName = Path.GetTempFileName();
            JsonUtility.SaveObjectToFile
                (
                    source,
                    fileName
                );

            DummyClass target = JsonUtility
                .ReadObjectFromFile<DummyClass>
                (
                    fileName
                );

            Assert.AreEqual(source.First, target.First);
            Assert.AreEqual(source.Third, target.Third);
            Assert.AreEqual(source.Fifth, target.Fifth);
        }


        [TestMethod]
        public void JsonUtility_ReadArrayFromFile_1()
        {
            JArray source = new JArray { 1, 2, 3 };

            string fileName = Path.GetTempFileName();
            JsonUtility.SaveArrayToFile
                (
                    source,
                    fileName
                );

            JArray target = JsonUtility.ReadArrayFromFile
                (
                    fileName
                );

            Assert.AreEqual
                (
                    source.ToString(),
                    target.ToString()
                );
        }


        // TODO solve MSTest.exe problem

        //[TestMethod]
        //public void JsonUtility_Include_1()
        //{
        //    string mainFilePath = Path.Combine
        //        (
        //            TestDataPath,
        //            "mainFile.json"
        //        );
        //    mainFilePath = Path.GetFullPath(mainFilePath);
        //    JObject mainObject = JsonUtility.ReadObjectFromFile(mainFilePath);
        //    JsonUtility.Include(mainObject, "included");
        //    const string expected = "{\"first\":1,\"second\":2,\"included\":{\"third\":3,\"fifth\":5},\"fourth\":4}";
        //    string actual = mainObject.ToString(Formatting.None);
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod]
        //public void JsonUtility_Include_2()
        //{
        //    string mainFilePath = Path.Combine
        //        (
        //            TestDataPath,
        //            "mainFile2.json"
        //        );
        //    mainFilePath = Path.GetFullPath(mainFilePath);
        //    JObject mainObject = JsonUtility.ReadObjectFromFile(mainFilePath);
        //    JsonUtility.Include(mainObject);
        //    const string expected = "{\"first\":1,\"second\":2,\"included\":{\"third\":3,\"fifth\":5},\"fourth\":4}";
        //    string actual = mainObject.ToString(Formatting.None);
        //    Assert.AreEqual(expected, actual);
        //}
    }
}
