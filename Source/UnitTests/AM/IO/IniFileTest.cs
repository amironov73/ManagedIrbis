using System;
using System.IO;
using System.Linq;
using AM;
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
            Assert.AreEqual(first.Count(), second.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateKeyException))]
        public void IniFile_Add_Exception()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");
            section.Add("Parameter1", "Value2");
        }

        [TestMethod]
        public void IniFile_Clear()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter1";
            file.SetValue(main, parameter1, expected);

            file.Clear();

            Assert.AreEqual(0, file.Count());

            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_GetValue()
        {
            IniFile file = new IniFile();
            file["Main", "Parameter1"] = "123";

            int result = file.GetValue("Main", "Parameter1", 0);
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void IniFile_ContainsSection()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter1";
            file.SetValue(main, parameter1, expected);

            Assert.IsTrue(file.ContainsSection(main));
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateKeyException))]
        public void IniFile_CreateSection_Exception()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter1";
            file.SetValue(main, parameter1, expected);
            file.CreateSection(main);
        }

        [TestMethod]
        public void IniFile_Dispose()
        {
            using (new IniFile())
            {
            }
        }

        [TestMethod]
        public void IniFile_Read()
        {
            string fileName = Path.Combine
            (
                TestDataPath,
                "IniFile1.ini"
            );
            IniFile file = new IniFile(fileName);

            Assert.AreEqual(2, file.Count());

            Assert.AreEqual("1", file["Main","FirstParameter"]);
            Assert.IsNull(file["Main", "NotExist"]);
            Assert.IsNull(file["NoSection", "NotExits"]);

            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_RemoveSection()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter1";
            file.SetValue(main, parameter1, expected);
            file.RemoveSection(main);

            Assert.AreEqual(0, file.Count());

            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_RemoveValue()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter1";
            file.SetValue(main, parameter1, expected);
            file.RemoveValue(main, parameter1);

            Assert.AreEqual(1, file.Count());
            Assert.IsNull(file.GetValue(main, parameter1, null));

            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_Save()
        {
            IniFile file = new IniFile();
            file["Main", "Parameter1"] = "Value1";
            file["Aux", "Parameter2"] = "Value2";

            Assert.AreEqual("Value1", file["Main", "Parameter1"]);

            Assert.AreEqual(2, file.Count());
            Assert.AreEqual(1, file["Main"].Count);
            Assert.AreEqual(1, file["Aux"].Count);

            StringWriter writer = new StringWriter();
            file.Save(writer);
            string text = writer.ToString()
                .Replace("\r\n", "|")
                .Replace("\n", "|"); ;

            Assert.AreEqual
            (
                "[Main]|Parameter1=Value1||[Aux]|Parameter2=Value2|",
                text
            );

            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_SetValue1()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter1";
            file.SetValue(main, parameter1, expected);

            int actual = file.GetValue(main, parameter1, 0);

            Assert.AreEqual(expected, actual);

            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_SetValue2()
        {
            IniFile file = new IniFile();

            const string main = "Main";
            const string parameter1 = "Parameter1";
            const int expected = 123;

            IniFile.Section section = file.CreateSection(main);
            section.SetValue(parameter1, expected);

            int actual = section.GetValue(parameter1, 0);

            Assert.AreEqual(expected, actual);

            _TestSerialization(file);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IniFile_SetValue_Exception1()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter=1";
            file.SetValue(main, parameter1, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IniFile_SetValue_Exception2()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = null;
            file.SetValue(main, parameter1, expected);
        }

        [TestMethod]
        public void IniFile_Serialization()
        {
            IniFile file = new IniFile();
            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_WriteModifiedValues()
        {
            string fileName = Path.Combine
            (
                TestDataPath,
                "IniFile1.ini"
            );
            IniFile file = new IniFile(fileName);

            file["Main", "Greeting"] = "Hello";
            file["Main", "Count"] = "123";

            StringWriter writer = new StringWriter();
            file.WriteModifiedValues(writer);
            const string expected = @"[Main]|Greeting=Hello|Count=123|";

            string actual = writer.ToString()
                .Replace("\r\n", "|")
                .Replace("\n", "|");

            Assert.AreEqual(expected, actual);

            _TestSerialization(file);
        }
    }
}
