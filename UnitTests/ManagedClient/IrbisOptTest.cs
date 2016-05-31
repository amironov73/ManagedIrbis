using System;
using System.IO;

using ManagedClient;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedClient
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
        }
    }
}
