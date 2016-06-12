using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class IniFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void TestIniFileRead()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "IniFile1.ini"
                );
            IniFile file = new IniFile(fileName);

            Assert.AreEqual(3, file.Sections.Count);

            Assert.AreEqual("1", file["Main","FirstParameter"]);
            Assert.IsNull(file["Main", "NotExist"]);
            Assert.IsNull(file["NoSection", "NotExits"]);
        }

        [TestMethod]
        public void TestIniFileSave()
        {
            IniFile file = new IniFile();
            file["Main", "Parameter1"] = "Value1";

            Assert.AreEqual("Value1", file["Main", "Parameter1"]);

            StringWriter writer = new StringWriter();
            file.Save(writer);
            string text = writer.ToString();

            Assert.AreEqual
                (
                    "[Main]\r\nParameter1=Value1\r\n\r\n",
                    text
                );
        }
    }
}
