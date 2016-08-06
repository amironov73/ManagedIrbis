using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTests.AM
{
    [TestClass]
    public class JsonUtilityTest
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
        public void TestJsonUtility_ReadObjectFromFile1()
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
        public void TestJsonUtility_ReadObjectFromFile2()
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
        public void TestJsonUtility_ReadArrayFromFile()
        {
            JArray source = new JArray {1, 2, 3};

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
    }
}
