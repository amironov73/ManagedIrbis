using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class TermInfoTest
    {
        [TestMethod]
        public void TermInfo_Construction_1()
        {
            TermInfo term = new TermInfo();
            Assert.AreEqual(0, term.Count);
            Assert.IsNull(term.Text);
        }

        private void _TestSerialization
            (
                [NotNull] TermInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            TermInfo second
                = bytes.RestoreObjectFromMemory<TermInfo>();

            Assert.AreEqual(first.Count, second.Count);
            Assert.AreEqual(first.Text, second.Text);
        }

        [TestMethod]
        public void TermInfo_Serialization_1()
        {
            TermInfo termInfo = new TermInfo();
            _TestSerialization(termInfo);

            termInfo.Count = 10;
            termInfo.Text = "T=HELLO";
            _TestSerialization(termInfo);
        }

        [TestMethod]
        public void TermInfo_Verify_1()
        {
            TermInfo termInfo = new TermInfo();
            Assert.IsFalse(termInfo.Verify(false));

            termInfo.Count = 10;
            termInfo.Text = "T=HELLO";
            Assert.IsTrue(termInfo.Verify(false));
        }

        [TestMethod]
        public void TermInfo_ToString_1()
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
        public void TermInfo_TrimPrefix_1()
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
        public void TermInfo_Clone_1()
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

        [TestMethod]
        public void TermInfo_ToXml_1()
        {
            TermInfo term = new TermInfo();
            Assert.AreEqual("<term />", XmlUtility.SerializeShort(term));

            term.Count = 10;
            term.Text = "T=HELLO";
            Assert.AreEqual("<term count=\"10\" text=\"T=HELLO\" />", XmlUtility.SerializeShort(term));
        }

        [TestMethod]
        public void TermInfo_ToJson_1()
        {
            TermInfo term = new TermInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(term));

            term.Count = 10;
            term.Text = "T=HELLO";
            Assert.AreEqual("{'count':10,'text':'T=HELLO'}", JsonUtility.SerializeShort(term));
        }
    }
}
