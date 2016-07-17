using System;
using System.IO;
using System.Text;
using AM.Runtime;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisOptTest
        : Common.CommonUnitTest
    {
        private void _TestCompare
            (
                string left,
                string right,
                bool expected
            )
        {
            Assert.AreEqual
                (
                    expected,
                    IrbisOpt.CompareString
                    (
                        left,
                        right
                    )
                );
        }

        [TestMethod]
        public void TestIrbisOptComparison()
        {
            _TestCompare("PAZK", "pazk", true);
            _TestCompare("PAZK", "PAZ", false);
            _TestCompare("PAZK", "PAZK2", false);
            _TestCompare("PAZ+", "PAZ", true);
            _TestCompare("PAZ+", "PAZK", true);
            _TestCompare("PAZ+", "PAZK2", false);
            _TestCompare("SPEC", "PAZK", false);
            _TestCompare("PA+K", "pazk", true);
            _TestCompare("PA+K", "PARK", true);
            _TestCompare("PA+K", "SPEC", false);
            _TestCompare("PA++", "PAZK", true);
            _TestCompare("+++++", string.Empty, true);
            _TestCompare("+++++", "PAZK", true);
        }

        private void _TestSerialization
            (
                IrbisOpt first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisOpt second = bytes.RestoreObjectFromMemory<IrbisOpt>();

            Assert.AreEqual(first.WorksheetLength, second.WorksheetLength);
            Assert.AreEqual(first.WorksheetTag, second.WorksheetTag);
            Assert.AreEqual(first.Items.Count, second.Items.Count);
            for (int i = 0; i < first.Items.Count; i++)
            {
                Assert.AreEqual(first.Items[i].Key, second.Items[i].Key);
                Assert.AreEqual(first.Items[i].Value, second.Items[i].Value);
            }
        }

        [TestMethod]
        public void TestIrbisOptLoadFromOptFile()
        {
            string filePath = Path.Combine
                (
                    TestDataPath,
                    "WS31.OPT"
                );

            IrbisOpt opt = IrbisOpt.LoadFromOptFile(filePath);
            Assert.IsNotNull(opt);
            Assert.AreEqual("920", opt.WorksheetTag);
            Assert.AreEqual(5, opt.WorksheetLength);
            Assert.AreEqual(14, opt.Items.Count);

            string optimized = opt.SelectWorksheet("UNKN");
            Assert.AreEqual("PAZK42", optimized);

            opt.Validate(true);

            _TestSerialization(opt);

            StringWriter writer = new StringWriter();
            opt.WriteOptFile(writer);
            string actual = writer.ToString().Replace("\r\n", "\n");
            string expected = File.ReadAllText(filePath, Encoding.Default)
                .Replace("\r\n", "\n");
            Assert.AreEqual(actual, expected);
        }
    }
}
