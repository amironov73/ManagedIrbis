using System;
using System.IO;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MappedInvertedFile64Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetInvertedFilePath()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "Irbis64/Datai/IBIS/ibis.ifp"
                );
        }

        [NotNull]
        private MappedInvertedFile64 _GetInvertedFile()
        {
            return new MappedInvertedFile64(_GetInvertedFilePath());
        }

        [TestMethod]
        public void MappedInvertedFile64_Construction_1()
        {
            string fileName = _GetInvertedFilePath();
            MappedInvertedFile64 inverted = _GetInvertedFile();
            Assert.AreEqual(fileName, inverted.FileName);
            Assert.IsNotNull(inverted.IfpControlRecord);
            inverted.Dispose();
        }

        [TestMethod]
        public void MappedInvertedFile64_ReadNode_1()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                NodeRecord64 node = inverted.ReadNode(1);
                Assert.IsNotNull(node);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_ReadLeaf_1()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                NodeRecord64 node = inverted.ReadLeaf(1);
                Assert.IsNotNull(node);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_ReadNext_1()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                NodeRecord64 node = inverted.ReadNode(1);
                Assert.IsNotNull(node);
                NodeRecord64 next = inverted.ReadNext(node);
                Assert.IsNotNull(next);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_ReadNext_2()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                NodeRecord64 node = inverted.ReadNode(1);
                Assert.IsNotNull(node);
                NodeRecord64 next = node;
                do
                {
                    next = inverted.ReadNext(next);
                } while(!ReferenceEquals(next, null));
                Assert.IsNull(next);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_ReadPrevious_1()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                NodeRecord64 node = inverted.ReadNode(1);
                Assert.IsNotNull(node);
                NodeRecord64 next = inverted.ReadNext(node);
                Assert.IsNotNull(next);
                NodeRecord64 previous = inverted.ReadPrevious(next);
                Assert.IsNotNull(previous);
                Assert.AreEqual(node.Leader.Number, previous.Leader.Number);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_ReadPrevious_2()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                NodeRecord64 node = inverted.ReadNode(1);
                Assert.IsNotNull(node);
                NodeRecord64 previous = inverted.ReadPrevious(node);
                Assert.IsNull(previous);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_ReadTerms_1()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                TermParameters parameters = new TermParameters
                {
                    StartTerm = "K=",
                    NumberOfTerms = 10
                };
                TermInfo[] terms = inverted.ReadTerms(parameters);
                Assert.AreEqual(10, terms.Length);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_SearchExact_1()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                TermLink[] links = inverted.SearchExact("K=CASE");
                Assert.AreEqual(2, links.Length);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_SearchExact_2()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                TermLink[] links = inverted.SearchExact("K=CAS0");
                Assert.AreEqual(0, links.Length);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_SearchStart_1()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                TermLink[] links = inverted.SearchStart("K=C");
                Assert.AreEqual(35, links.Length);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_SearchSimple_1()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                int[] found = inverted.SearchSimple("K=CASE");
                Assert.AreEqual(2, found.Length);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_SearchSimple_2()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                int[] found = inverted.SearchSimple("K=CAS0");
                Assert.AreEqual(0, found.Length);
            }
        }

        [TestMethod]
        public void MappedInvertedFile64_SearchSimple_3()
        {
            using (MappedInvertedFile64 inverted = _GetInvertedFile())
            {
                int[] found = inverted.SearchSimple("K=C$");
                Assert.AreEqual(19, found.Length);
            }
        }
    }
}
