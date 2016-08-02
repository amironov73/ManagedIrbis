using System;
using System.IO;
using AM.Runtime;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

namespace UnitTests.AM.Gbl
{
    [TestClass]
    public class GblFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void TestGblFileConstruction()
        {
            GblFile gbl = new GblFile();

            Assert.AreEqual(0, gbl.Items.Count);
            Assert.AreEqual(0, gbl.Parameters.Count);

            GblItem item = new GblItem
            {
                Command = GblCode.Add,
                Parameter1 = "300",
                Format1 = "Add field 300"
            };

            gbl.Items.Add(item);
        }

        private void _TestSerialization
            (
                GblFile first
            )
        {
            byte[] bytes = first.SaveToMemory();

            GblFile second = bytes
                .RestoreObjectFromMemory<GblFile>();

            Assert.AreEqual(first.Items.Count, second.Items.Count);
            for (int i = 0; i < first.Items.Count; i++)
            {
                Assert.AreEqual(first.Items[i].Command, second.Items[i].Command);
                Assert.AreEqual(first.Items[i].Parameter1, second.Items[i].Parameter1);
                Assert.AreEqual(first.Items[i].Parameter2, second.Items[i].Parameter2);
                Assert.AreEqual(first.Items[i].Format1, second.Items[i].Format1);
                Assert.AreEqual(first.Items[i].Format2, second.Items[i].Format2);
            }

            Assert.AreEqual(first.Parameters.Count, second.Parameters.Count);
            for (int i = 0; i < first.Parameters.Count; i++)
            {
                Assert.AreEqual(first.Parameters[i].Name, second.Parameters[i].Name);
                Assert.AreEqual(first.Parameters[i].Value, second.Parameters[i].Value);
            }
        }

        private GblFile _GetGbl()
        {
            GblFile result = new GblFile();

            result.Items.Add(new GblItem
            {
                Command = GblCode.Add,
                Parameter1 = "300",
                Format1 = "Add field 300"
            });
            result.Items.Add(new GblItem
            {
                Command = GblCode.Delete,
                Parameter1 = "300",
                Parameter2 = "*"
            });
            result.Items.Add(new GblItem
            {
                Command = "NOP"
            });

            return result;
        }

        [TestMethod]
        public void TestGblFileSerialization()
        {
            GblFile gbl = new GblFile();
            _TestSerialization(gbl);

            gbl = _GetGbl();
            _TestSerialization(gbl);
        }

        [TestMethod]
        public void TestGblFileParseLocalFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "Del910s.gbl"
                );

            GblFile gbl = GblFile.ParseLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );

            Assert.IsNotNull(gbl);
            Assert.AreEqual(3, gbl.Items.Count);
            Assert.AreEqual(0, gbl.Parameters.Count);
            Assert.AreEqual("DEL", gbl.Items[0].Command);

            _TestSerialization(gbl);
        }

        [TestMethod]
        public void TestGblFileToJson()
        {
            GblFile gbl = _GetGbl();

            string actual = gbl.ToJson()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            const string expected = "{'items':[{'command':'ADD','parameter1':'300','format1':'Add field 300'},{'command':'DEL','parameter1':'300','parameter2':'*'},{'command':'NOP'}]}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGblFileFromJson()
        {
            string text = "{'items':[{'command':'ADD','parameter1':'300','format1':'Add field 300'},{'command':'DEL','parameter1':'300','parameter2':'*'},{'command':'NOP'}]}"
                .Replace("'", "\"");

            GblFile gbl = GblUtility.FromJson(text);

            Assert.AreEqual(3, gbl.Items.Count);
            Assert.AreEqual("ADD", gbl.Items[0].Command);
        }

        [TestMethod]
        public void TestGblFileToXml()
        {
            GblFile gbl = _GetGbl();

            string actual = gbl.ToXml()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            const string expected = "<?xml version='1.0' encoding='utf-16'?><gbl xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>  <item>    <command>ADD</command>    <parameter1>300</parameter1>    <format1>Add field 300</format1>  </item>  <item>    <command>DEL</command>    <parameter1>300</parameter1>    <parameter2>*</parameter2>  </item>  <item>    <command>NOP</command>  </item></gbl>";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGblFileFromXml()
        {
            string text = "<?xml version='1.0' encoding='utf-16'?><gbl xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>  <item>    <command>ADD</command>    <parameter1>300</parameter1>    <format1>Add field 300</format1>  </item>  <item>    <command>DEL</command>    <parameter1>300</parameter1>    <parameter2>*</parameter2>  </item>  <item>    <command>NOP</command>  </item></gbl>"
                .Replace("'", "\"");

            GblFile gbl = GblUtility.FromXml(text);

            Assert.IsNotNull(gbl);
            Assert.AreEqual(3, gbl.Items.Count);
            Assert.AreEqual(0, gbl.Parameters.Count);
        }

        [TestMethod]
        public void TestGblFileParseLocalJsonFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "test-gbl.json"
                );

            GblFile gbl = GblUtility.ParseLocalJsonFile(fileName);

            Assert.IsNotNull(gbl);
            Assert.AreEqual(3, gbl.Items.Count);
            Assert.AreEqual(0, gbl.Parameters.Count);
        }

        [TestMethod]
        public void TestGblFileParseLocalXmlFile()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "test-gbl.xml"
                );

            GblFile gbl = GblUtility.ParseLocalXmlFile(fileName);

            Assert.IsNotNull(gbl);
            Assert.AreEqual(3, gbl.Items.Count);
            Assert.AreEqual(0, gbl.Parameters.Count);
        }

        [TestMethod]
        public void TestGblFileVerification()
        {
            GblFile gbl = _GetGbl();

            Assert.IsTrue(gbl.Verify(false));
        }
    }
}
