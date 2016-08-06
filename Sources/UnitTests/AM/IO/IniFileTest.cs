using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using AM.Runtime;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class IniFileTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                IniFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IniFile second = bytes
                .RestoreObjectFromMemory<IniFile>();

            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Sections.Count, second.Sections.Count);
        }

        [TestMethod]
        public void TestIniFileGet()
        {
            IniFile file = new IniFile();
            file["Main", "Parameter1"] = "123";

            int result = file.Get("Main", "Parameter1", 0);
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void TestIniFileSet1()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter1";
            file.Set(main, parameter1, expected);

            int actual = file.Get(main, parameter1, 0);

            Assert.AreEqual(expected, actual);

            _TestSerialization(file);
        }

        [TestMethod]
        public void TestIniFileSet2()
        {
            IniFile file = new IniFile();

            const string main = "Main";
            const string parameter1 = "Parameter1";
            const int expected = 123;

            IniFile.Section section = file.CreateSection(main);
            section.Set(parameter1, expected);

            int actual = section.Get(parameter1, 0);

            Assert.AreEqual(expected, actual);

            _TestSerialization(file);
        }

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

            _TestSerialization(file);
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

            _TestSerialization(file);
        }

        [TestMethod]
        public void TestIniFileSerialization()
        {
            IniFile file = new IniFile();
            _TestSerialization(file);
        }
    }
}
