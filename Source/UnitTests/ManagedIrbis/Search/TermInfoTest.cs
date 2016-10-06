using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class TermInfoTest
    {
        private void _TestSerialization
            (
                TermInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            TermInfo second
                = bytes.RestoreObjectFromMemory<TermInfo>();

            Assert.AreEqual(first.Count, second.Count);
            Assert.AreEqual(first.Text, second.Text);
        }

        [TestMethod]
        public void TestTermInfo_Serialization()
        {
            TermInfo termInfo = new TermInfo();
            _TestSerialization(termInfo);

            termInfo.Count = 10;
            termInfo.Text = "T=HELLO";
            _TestSerialization(termInfo);
        }

        [TestMethod]
        public void TestTermInfo_Verify()
        {
            TermInfo termInfo = new TermInfo();
            Assert.IsFalse(termInfo.Verify(false));

            termInfo.Count = 10;
            termInfo.Text = "T=HELLO";
            Assert.IsTrue(termInfo.Verify(false));
        }

        [TestMethod]
        public void TestTermInfo_ToString()
        {
            TermInfo termInfo = new TermInfo
            {
                Count = 10,
                Text = "T=HELLO"
            };
            string actual = termInfo.ToString();
            Assert.AreEqual("10#T=HELLO", actual);
        }

        [TestMethod]
        public void TestTermInfo_TrimPrefix()
        {
            TermInfo[] terms =
            {
                new TermInfo {Count=1, Text = "T=HELLO"},
                new TermInfo {Count=2, Text = "T=IRBIS"},
                new TermInfo {Count=3, Text = "T=WORLD"},
            };
            TermInfo[] actual = TermInfo.TrimPrefix(terms, "T=");
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("HELLO", actual[0].Text);
            Assert.AreEqual("IRBIS", actual[1].Text);
            Assert.AreEqual("WORLD", actual[2].Text);

            terms = new []
            {
                new TermInfo {Count=1, Text = "HELLO"},
                new TermInfo {Count=2, Text = "IRBIS"},
                new TermInfo {Count=3, Text = "WORLD"},
            };
            actual = TermInfo.TrimPrefix(terms, string.Empty);
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("HELLO", actual[0].Text);
            Assert.AreEqual("IRBIS", actual[1].Text);
            Assert.AreEqual("WORLD", actual[2].Text);
        }

        [TestMethod]
        public void TestTermInfo_Clone()
        {
            TermInfo expected = new TermInfo
            {
                Count = 10,
                Text = "T=HELLLO"
            };
            TermInfo actual = expected.Clone();
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected.Text, actual.Text);
        }
    }
}
