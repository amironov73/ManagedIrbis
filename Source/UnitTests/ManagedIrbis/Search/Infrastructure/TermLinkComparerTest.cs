using System.Collections.Generic;

using ManagedIrbis.Search;
using ManagedIrbis.Search.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Search.Infrastructure
{
    [TestClass]
    public class TermLinkComparerTest
    {
        [TestMethod]
        public void TermLinkComparer_ByMfn_1()
        {
            TermLink left = new TermLink { Mfn = 10 };
            TermLink right = new TermLink { Mfn = 10 };
            IEqualityComparer<TermLink> comparer
                = new TermLinkComparer.ByMfn();
            Assert.IsTrue(comparer.Equals(left, right));
            Assert.AreEqual(10, comparer.GetHashCode(left));

            right = new TermLink { Mfn = 11 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(11, comparer.GetHashCode(right));
        }

        [TestMethod]
        public void TermLinkComparer_ByTag_1()
        {
            TermLink left = new TermLink { Mfn = 10, Tag = 100 };
            TermLink right = new TermLink { Mfn = 10, Tag = 100 };
            IEqualityComparer<TermLink> comparer
                = new TermLinkComparer.ByTag();
            Assert.IsTrue(comparer.Equals(left, right));
            Assert.AreEqual(470, comparer.GetHashCode(left));

            right = new TermLink { Mfn = 10, Tag = 101 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(471, comparer.GetHashCode(right));

            right = new TermLink { Mfn = 11, Tag = 100 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(507, comparer.GetHashCode(right));
        }

        [TestMethod]
        public void TermLinkComparer_ByOccurrence_1()
        {
            TermLink left = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1 };
            TermLink right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1 };
            IEqualityComparer<TermLink> comparer
                = new TermLinkComparer.ByOccurrence();
            Assert.IsTrue(comparer.Equals(left, right));
            Assert.AreEqual(17391, comparer.GetHashCode(left));

            right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 2 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(17392, comparer.GetHashCode(right));

            right = new TermLink { Mfn = 10, Tag = 101, Occurrence = 1 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(17428, comparer.GetHashCode(right));

            right = new TermLink { Mfn = 11, Tag = 100, Occurrence = 1 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(18760, comparer.GetHashCode(right));
        }

        [TestMethod]
        public void TermLinkComparer_ByIndex_1()
        {
            TermLink left = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1, Index = 2 };
            TermLink right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1, Index = 3 };
            IEqualityComparer<TermLink> comparer
                = new TermLinkComparer.ByIndex();
            Assert.IsTrue(comparer.Equals(left, right));
            Assert.AreEqual(17391, comparer.GetHashCode(left));

            right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 1, Index = 4 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(17391, comparer.GetHashCode(right));

            right = new TermLink { Mfn = 10, Tag = 100, Occurrence = 2, Index = 2 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(17392, comparer.GetHashCode(right));

            right = new TermLink { Mfn = 10, Tag = 101, Occurrence = 1, Index = 2 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(17428, comparer.GetHashCode(right));

            right = new TermLink { Mfn = 11, Tag = 100, Occurrence = 1, Index = 2 };
            Assert.IsFalse(comparer.Equals(left, right));
            Assert.AreEqual(18760, comparer.GetHashCode(right));
        }
    }
}
