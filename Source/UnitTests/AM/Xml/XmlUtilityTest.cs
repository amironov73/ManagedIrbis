using System.IO;

using AM.Xml;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Xml
{
    [TestClass]
    public class XmlUtilityTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private CanaryClass _GetCanary()
        {
            return new CanaryClass
            {
                Name = "John",
                Age = 3
            };
        }

        [TestMethod]
        public void XmlUtility_Deserialize_1()
        {
            string fileName = Path.Combine(TestDataPath, "canary.xml");
            CanaryClass canary = XmlUtility.Deserialize<CanaryClass>(fileName);
            Assert.AreEqual("John", canary.Name);
            Assert.AreEqual(3, canary.Age);
        }

        [TestMethod]
        public void XmlUtility_DeserializeString_1()
        {
            string text = "<canary><name>John</name><age>3</age></canary>";
            CanaryClass canary = XmlUtility.DeserializeString<CanaryClass>(text);
            Assert.AreEqual("John", canary.Name);
            Assert.AreEqual(3, canary.Age);
        }

        [TestMethod]
        public void XmlUtility_Serialize_1()
        {
            string fileName = Path.GetTempFileName();
            CanaryClass canary = _GetCanary();
            XmlUtility.Serialize(fileName, canary);
        }

        [TestMethod]
        public void XmlUtility_SerializeShort_1()
        {
            CanaryClass canary = _GetCanary();
            string expected = "<canary><name>John</name><age>3</age></canary>";
            string actual = XmlUtility.SerializeShort(canary);
            Assert.AreEqual(expected, actual);
        }

    }
}
