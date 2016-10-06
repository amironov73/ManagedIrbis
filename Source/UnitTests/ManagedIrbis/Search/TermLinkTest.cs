using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class TermLinkTest
    {
        private void _TestSerialization
            (
                TermLink first
            )
        {
            byte[] bytes = first.SaveToMemory();

            TermLink second = bytes
                .RestoreObjectFromMemory<TermLink>();

            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Occurrence, second.Occurrence);
            Assert.AreEqual(first.Tag, second.Tag);
        }

        [TestMethod]
        public void TestTermLink_Serialization()
        {
            TermLink termLink = new TermLink();
            _TestSerialization(termLink);

            termLink = new TermLink
            {
                Index = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40
            };
            _TestSerialization(termLink);
        }

        [TestMethod]
        public void TestTermLink_Clone()
        {
            TermLink first = new TermLink
            {
                Index = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40
            };
            TermLink second = first.Clone();

            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Occurrence, second.Occurrence);
            Assert.AreEqual(first.Tag, second.Tag);
        }

        [TestMethod]
        public void TestTermLink_ToString()
        {
            TermLink termLink = new TermLink
            {
                Index = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40
            };
            string actual = termLink.ToString();
            Assert.AreEqual("[20] 40/30 10", actual);
        }

        [TestMethod]
        public void TestTermLink_Equals()
        {
            TermLink first = new TermLink
            {
                Index = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40
            };
            TermLink second = first.Clone();
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(first.Equals((object)second));

            Assert.IsTrue(first.Equals((object)first));

            second.Mfn = 220;
            Assert.IsFalse(first.Equals(second));

            Assert.IsFalse(first.Equals((object)null));
        }
    }
}
