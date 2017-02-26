using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class ParFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void ParFile_Constructor_1()
        {
            ParFile parFile = new ParFile();
            Assert.IsNull(parFile.AnyPath);
            Assert.IsNull(parFile.CntPath);
            Assert.IsNull(parFile.ExtPath);
            Assert.IsNull(parFile.IfpPath);
            Assert.IsNull(parFile.L01Path);
            Assert.IsNull(parFile.L02Path);
            Assert.IsNull(parFile.MstPath);
            Assert.IsNull(parFile.N01Path);
            Assert.IsNull(parFile.N02Path);
            Assert.IsNull(parFile.PftPath);
            Assert.IsNull(parFile.XrfPath);
        }

        [TestMethod]
        public void ParFile_Constructor_2()
        {
            const string mstPath = "IBIS";

            ParFile parFile = new ParFile(mstPath);
            Assert.AreEqual(mstPath, parFile.AnyPath);
            Assert.AreEqual(mstPath, parFile.CntPath);
            Assert.AreEqual(mstPath, parFile.ExtPath);
            Assert.AreEqual(mstPath, parFile.IfpPath);
            Assert.AreEqual(mstPath, parFile.L01Path);
            Assert.AreEqual(mstPath, parFile.L02Path);
            Assert.AreEqual(mstPath, parFile.MstPath);
            Assert.AreEqual(mstPath, parFile.N01Path);
            Assert.AreEqual(mstPath, parFile.N02Path);
            Assert.AreEqual(mstPath, parFile.PftPath);
            Assert.AreEqual(mstPath, parFile.XrfPath);
        }

        private void _TestSerialization
            (
                ParFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            ParFile second = bytes
                .RestoreObjectFromMemory<ParFile>();

            Assert.AreEqual(first.AnyPath, second.AnyPath);
            Assert.AreEqual(first.CntPath, second.CntPath);
            Assert.AreEqual(first.ExtPath, second.ExtPath);
            Assert.AreEqual(first.IfpPath, second.IfpPath);
            Assert.AreEqual(first.L01Path, second.L01Path);
            Assert.AreEqual(first.L02Path, second.L02Path);
            Assert.AreEqual(first.MstPath, second.MstPath);
            Assert.AreEqual(first.N01Path, second.N01Path);
            Assert.AreEqual(first.N02Path, second.N02Path);
            Assert.AreEqual(first.PftPath, second.PftPath);
            Assert.AreEqual(first.XrfPath, second.XrfPath);
        }

        [TestMethod]
        public void ParFile_Serialization_1()
        {
            ParFile parFile = new ParFile();
            _TestSerialization(parFile);

            parFile.MstPath = @".\datai\ibis\";
            _TestSerialization(parFile);
        }

        private ParFile _GetParFile ()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "ibis.par"
                );

            ParFile result = ParFile.ParseFile(fileName);

            return result;
        }

        [TestMethod]
        public void ParFile_ParseFile_1()
        {
            ParFile parFile = _GetParFile();
            Assert.AreEqual(@".\datai\ibis\", parFile.MstPath);
            _TestSerialization(parFile);
        }

        [TestMethod]
        public void ParFile_WriteText_1()
        {
            ParFile parFile = _GetParFile();

            StringWriter writer = new StringWriter();
            parFile.WriteText(writer);
            string text = writer.ToString();
            Assert.IsTrue(text.Length > 0);
        }

        [TestMethod]
        public void ParFile_ReadDictionary_1()
        {
            string text = "1=IBIS" + Environment.NewLine
                + "2=IBIS" + Environment.NewLine
                + Environment.NewLine
                + "3=IBIS" + Environment.NewLine
                + "4=IBIS" + Environment.NewLine
                + "5=IBIS" + Environment.NewLine
                + "6=IBIS" + Environment.NewLine
                + "7=IBIS" + Environment.NewLine
                + "8=IBIS" + Environment.NewLine
                + "9=IBIS" + Environment.NewLine
                + "10=IBIS" + Environment.NewLine;

            TextReader reader = new StringReader(text);
            Dictionary<string, string> dictionary
                = ParFile.ReadDictionary(reader);
            Assert.AreEqual(10, dictionary.Count);
            Assert.AreEqual("IBIS", dictionary["1"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParFile_ReadDictionary_Exception_1()
        {
            string text = "1=IBIS" + Environment.NewLine
                + "2=IBIS" + Environment.NewLine
                + Environment.NewLine
                + "3=" + Environment.NewLine
                + "4=IBIS" + Environment.NewLine
                + "5=IBIS" + Environment.NewLine
                + "6=IBIS" + Environment.NewLine
                + "7=IBIS" + Environment.NewLine
                + "8=IBIS" + Environment.NewLine
                + "9=IBIS" + Environment.NewLine
                + "10=IBIS" + Environment.NewLine;

            TextReader reader = new StringReader(text);
            Dictionary<string, string> dictionary
                = ParFile.ReadDictionary(reader);
            Assert.AreEqual(10, dictionary.Count);
            Assert.AreEqual("IBIS", dictionary["1"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParFile_ReadDictionary_Exception_2()
        {
            string text = "1=IBIS" + Environment.NewLine
                + "2=IBIS" + Environment.NewLine
                + Environment.NewLine
                + "3" + Environment.NewLine
                + "4=IBIS" + Environment.NewLine
                + "5=IBIS" + Environment.NewLine
                + "6=IBIS" + Environment.NewLine
                + "7=IBIS" + Environment.NewLine
                + "8=IBIS" + Environment.NewLine
                + "9=IBIS" + Environment.NewLine
                + "10=IBIS" + Environment.NewLine;

            TextReader reader = new StringReader(text);
            Dictionary<string, string> dictionary
                = ParFile.ReadDictionary(reader);
            Assert.AreEqual(10, dictionary.Count);
            Assert.AreEqual("IBIS", dictionary["1"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParFile_ReadDictionary_Exception_3()
        {
            string text = "1=IBIS" + Environment.NewLine
                + "2=IBIS" + Environment.NewLine
                + Environment.NewLine
                + "3 = " + Environment.NewLine
                + "4=IBIS" + Environment.NewLine
                + "5=IBIS" + Environment.NewLine
                + "6=IBIS" + Environment.NewLine
                + "7=IBIS" + Environment.NewLine
                + "8=IBIS" + Environment.NewLine
                + "9=IBIS" + Environment.NewLine
                + "10=IBIS" + Environment.NewLine;

            TextReader reader = new StringReader(text);
            Dictionary<string, string> dictionary
                = ParFile.ReadDictionary(reader);
            Assert.AreEqual(10, dictionary.Count);
            Assert.AreEqual("IBIS", dictionary["1"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParFile_ReadDictionary_Exception_4()
        {
            string text = "1=IBIS" + Environment.NewLine
                + "2=IBIS" + Environment.NewLine
                + Environment.NewLine
                + "4=IBIS" + Environment.NewLine
                + "5=IBIS" + Environment.NewLine
                + "6=IBIS" + Environment.NewLine
                + "7=IBIS" + Environment.NewLine
                + "8=IBIS" + Environment.NewLine
                + "9=IBIS" + Environment.NewLine
                + "10=IBIS" + Environment.NewLine;

            TextReader reader = new StringReader(text);
            Dictionary<string, string> dictionary
                = ParFile.ReadDictionary(reader);
            Assert.AreEqual(10, dictionary.Count);
            Assert.AreEqual("IBIS", dictionary["1"]);
        }

        [TestMethod]
        public void ParFile_Verify_1()
        {
            ParFile parFile = _GetParFile();
            Assert.IsTrue(parFile.Verify(false));
        }

        [TestMethod]
        public void ParFile_WriteFile_1()
        {
            ParFile first = _GetParFile();
            string fileName = Path.GetTempFileName();
            first.WriteFile(fileName);

            ParFile second = ParFile.ParseFile(fileName);
            Assert.AreEqual(first.AnyPath, second.AnyPath);
            Assert.AreEqual(first.CntPath, second.CntPath);
            Assert.AreEqual(first.ExtPath, second.ExtPath);
            Assert.AreEqual(first.IfpPath, second.IfpPath);
            Assert.AreEqual(first.L01Path, second.L01Path);
            Assert.AreEqual(first.L02Path, second.L02Path);
            Assert.AreEqual(first.MstPath, second.MstPath);
            Assert.AreEqual(first.N01Path, second.N01Path);
            Assert.AreEqual(first.N02Path, second.N02Path);
            Assert.AreEqual(first.PftPath, second.PftPath);
            Assert.AreEqual(first.XrfPath, second.XrfPath);
        }

        [TestMethod]
        public void ParFile_ToDictionary_1()
        {
            ParFile parFile = _GetParFile();
            Dictionary<string, string> dictionary
                = parFile.ToDictionary();
            Assert.AreEqual(11, dictionary.Count);
            Assert.AreEqual(".\\datai\\ibis\\", dictionary["1"]);
        }

        [TestMethod]
        public void ParFile_ToString_1()
        {
            ParFile parFile = _GetParFile();
            Assert.AreEqual(".\\datai\\ibis\\", parFile.ToString());
        }
    }
}
