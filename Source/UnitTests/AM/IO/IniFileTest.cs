using System;
using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException
// ReSharper disable UseObjectOrCollectionInitializer

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

            IniFile second = bytes.RestoreObjectFromMemory<IniFile>();

            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Count(), second.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateKeyException))]
        public void IniFile_Add_1()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");
            section.Add("Parameter1", "Value2");
        }

        [TestMethod]
        public void IniFile_Clear_1()
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
        public void IniFile_GetValue_1()
        {
            IniFile file = new IniFile();
            file["Main", "Parameter1"] = "123";

            int result = file.GetValue("Main", "Parameter1", 0);
            Assert.AreEqual(123, result);

            result = file.GetValue("Main", "Parameter2", 111);
            Assert.AreEqual(111, result);
        }

        [TestMethod]
        public void IniFile_ContainsSection_1()
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
        public void IniFile_CreateSection_1()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter1";
            file.SetValue(main, parameter1, expected);
            file.CreateSection(main);
        }

        [TestMethod]
        public void IniFile_Dispose_1()
        {
            using (new IniFile())
            {
                // Nothing to do here
            }
        }

        [TestMethod]
        public void IniFile_Dispose_2()
        {
            string fileName = Path.GetTempFileName();
            using (IniFile ini = new IniFile(fileName, null, true))
            {
                IniFile.Section section = ini.CreateSection("Main");
                section.Add("Parameter1", "Value1");
                ini.Modified = true;
            }
        }

        [TestMethod]
        public void IniFile_Read_1()
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
        public void IniFile_Read_2()
        {
            IniFile file = new IniFile();
            file.Read();
            Assert.AreEqual(0, file.Count());
            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_Read_3()
        {
            string text = "[Main]\nFirstParameter=1\nSecondParameter=2\n\n[Private]\nThirdParameter=3\nFourthParameter=4";
            IniFile file = new IniFile();
            StringReader reader = new StringReader(text);
            file.Read(reader);
            Assert.AreEqual(2, file.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void IniFile_Read_4()
        {
            string text = "[Main\nFirstParameter=1\nSecondParameter=2";
            IniFile file = new IniFile();
            StringReader reader = new StringReader(text);
            file.Read(reader);
        }

        [TestMethod]
        public void IniFile_Read_5()
        {
            string text = "FirstParameter=1\nSecondParameter=2";
            IniFile file = new IniFile();
            StringReader reader = new StringReader(text);
            file.Read(reader);
            Assert.AreEqual(1, file.Count());
        }

        [TestMethod]
        public void IniFile_RemoveSection_1()
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
        public void IniFile_RemoveSection_2()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter1";
            file.SetValue(main, parameter1, expected);
            file.RemoveSection("Aux");

            Assert.AreEqual(1, file.Count());

            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_RemoveValue_1()
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
        public void IniFile_Save_1()
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
            string text = writer.ToString().DosToUnix()
                .Replace("\n", "|");

            Assert.AreEqual
                (
                    "[Main]|Parameter1=Value1||[Aux]|Parameter2=Value2|",
                    text
                );

            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_SetValue_1()
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
        public void IniFile_SetValue_2()
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
        public void IniFile_SetValue_3()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = "Parameter=1";
            file.SetValue(main, parameter1, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IniFile_SetValue_4()
        {
            IniFile file = new IniFile();

            const int expected = 123;
            const string main = "Main";
            const string parameter1 = null;
            file.SetValue(main, parameter1, expected);
        }

        [TestMethod]
        public void IniFile_Serialization_1()
        {
            IniFile file = new IniFile();
            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_WriteModifiedValues_1()
        {
            string fileName = Path.Combine
            (
                TestDataPath,
                "IniFile1.ini"
            );
            IniFile file = new IniFile(fileName);

            file["Main", "Greeting"] = "Hello";
            file["Main", "Count"] = "123";
            file["Aux", "K1"] = "V1";
            file["Aux", "K2"] = "V2";
            IniFile.Section section = file.CreateSection("Empty");
            section.Modified = true;

            StringWriter writer = new StringWriter();
            file.WriteModifiedValues(writer);
            const string expected = @"[Main]|Greeting=Hello|Count=123||[Aux]|K1=V1|K2=V2||[Empty]|";

            string actual = writer.ToString().DosToUnix().Replace("\n", "|");

            Assert.AreEqual(expected, actual);

            _TestSerialization(file);
        }

        [TestMethod]
        public void IniFile_Line_Construction_1()
        {
            string key = "key", value = "value";
            bool modified = true;
            IniFile.Line line = new IniFile.Line(key, value, modified);
            Assert.AreEqual(key, line.Key);
            Assert.AreEqual(value, line.Value);
            Assert.AreEqual(modified, line.Modified);
        }

        [TestMethod]
        public void IniFile_Line_ToString_1()
        {
            IniFile.Line line = new IniFile.Line("key", "value", true);
            Assert.AreEqual("key=value", line.ToString());
        }

        [TestMethod]
        public void IniFile_Line_Write_1()
        {
            StringWriter writer = new StringWriter();
            IniFile.Line line = new IniFile.Line("key", "value", true);
            line.Write(writer);
            Assert.AreEqual("key=value\n", writer.ToString().DosToUnix());

            writer = new StringWriter();
            line = new IniFile.Line("key", null, true);
            line.Write(writer);
            Assert.AreEqual("key\n", writer.ToString().DosToUnix());
        }

        [TestMethod]
        public void IniFile_Section_Item_1()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");
            Assert.AreEqual("Value1", section["Parameter1"]);
            Assert.IsNull(section["Parameter2"]);
        }

        [TestMethod]
        public void IniFile_Section_Item_2()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            string key = "Parameter1";
            section.Add(key, "Value1");
            section[key] = "Value2";
            Assert.AreEqual("Value2", section[key]);
        }

        [TestMethod]
        public void IniFile_Section_Keys_1()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");
            section.Add("Parameter2", "Value2");
            string[] keys = section.Keys.ToArray();
            Array.Sort(keys);
            Assert.AreEqual(2, keys.Length);
            Assert.AreEqual("Parameter1", keys[0]);
            Assert.AreEqual("Parameter2", keys[1]);
        }

        [TestMethod]
        public void IniFile_Section_Name_1()
        {
            IniFile file = new IniFile();
            string name1 = "Main";
            IniFile.Section section = file.CreateSection(name1);
            Assert.AreEqual(name1, section.Name);

            string name2 = "Aux";
            section.Name = name2;
            Assert.AreEqual(name2, section.Name);
        }

        [TestMethod]
        public void IniFile_Section_ApplyTo_1()
        {
            IniFile file = new IniFile();
            IniFile.Section section1 = file.CreateSection("Main");
            section1.Add("Parameter1", "Value1");
            section1.Add("Parameter2", "Value2");

            IniFile.Section section2 = file.CreateSection("Aux");
            section2.Add("Parameter1", "Value11");
            section2.Add("Parameter3", "Value3");
            section1.ApplyTo(section2);
            Assert.AreEqual(3, section2.Count);
            Assert.AreEqual("Value1", section2["Parameter1"]);
            Assert.AreEqual("Value2", section2["Parameter2"]);
            Assert.AreEqual("Value3", section2["Parameter3"]);
        }

        [TestMethod]
        public void IniFile_Section_Clear_1()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");
            section.Add("Parameter2", "Value2");
            section.Clear();
            Assert.AreEqual(0, section.Count);
        }

        [TestMethod]
        public void IniFile_Section_Remove_1()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");
            section.Remove("Parameter2");
            Assert.AreEqual(1, section.Count);
        }

        [TestMethod]
        public void IniFile_Section_SetValue_1()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");
            section.SetValue("Parameter1", 111);
            section.SetValue("Parameter2", 123);
            Assert.AreEqual("[Main]\nParameter1=111\nParameter2=123\n", section.ToString().DosToUnix());
        }

        [TestMethod]
        public void IniFile_Section_SetValue_2()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");
            section.SetValue("Parameter1", (object)null);
            section.SetValue("Parameter2", 123);
            Assert.AreEqual("[Main]\nParameter1=\nParameter2=123\n", section.ToString().DosToUnix());
        }

        [TestMethod]
        public void IniFile_Section_ToString_1()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");
            section.Add("Parameter2", "Value2");
            Assert.AreEqual("[Main]\nParameter1=Value1\nParameter2=Value2\n", section.ToString().DosToUnix());
        }

        [TestMethod]
        public void IniFile_Section_TryGetValue_1()
        {
            IniFile file = new IniFile();
            IniFile.Section section = file.CreateSection("Main");
            section.Add("Parameter1", "Value1");

            string value;
            Assert.IsTrue(section.TryGetValue("Parameter1", out value));
            Assert.AreEqual("Value1", value);
            Assert.IsFalse(section.TryGetValue("Parameter2", out value));
        }

        [TestMethod]
        public void IniFile_ApplyTo_1()
        {
            IniFile first = new IniFile();
            IniFile.Section section1 = first.CreateSection("Main");
            section1.Add("Parameter1", "Value111");
            section1.Add("Parameter2", "Value222");

            IniFile second = new IniFile();
            IniFile.Section section2 = second.CreateSection("Main");
            section2.Add("Parameter1", "Value1");
            section2.Add("Parameter3", "Value3");

            first.ApplyTo(second);
            Assert.AreEqual(3, section2.Count);
            Assert.AreEqual("Value111", section2["Parameter1"]);
            Assert.AreEqual("Value222", section2["Parameter2"]);
            Assert.AreEqual("Value3", section2["Parameter3"]);
        }
    }
}
