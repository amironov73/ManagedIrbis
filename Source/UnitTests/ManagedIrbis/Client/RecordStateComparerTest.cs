using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class RecordStateComparerTest
    {
        [TestMethod]
        public void RecordStateComparer_ById_1()
        {
            RecordState first = new RecordState
            {
                Id = 1,
                Mfn = 123,
                Status = RecordStatus.Last,
                Version = 5
            };
            RecordState second = new RecordState
            {
                Id = 1,
                Mfn = 124,
                Status = RecordStatus.LogicallyDeleted,
                Version = 6
            };
            IEqualityComparer<RecordState> comparer = new RecordStateComparer.ById ();
            Assert.IsTrue(comparer.Equals(first, second));
            Assert.AreEqual(comparer.GetHashCode(first), comparer.GetHashCode(second));

            second.Id = 2;
            Assert.IsFalse(comparer.Equals(first, second));
            Assert.AreNotEqual(comparer.GetHashCode(first), comparer.GetHashCode(second));
        }

        [TestMethod]
        public void RecordStateComparer_ByMfn_1()
        {
            RecordState first = new RecordState
            {
                Id = 1,
                Mfn = 123,
                Status = RecordStatus.Last,
                Version = 5
            };
            RecordState second = new RecordState
            {
                Id = 2,
                Mfn = 123,
                Status = RecordStatus.LogicallyDeleted,
                Version = 6
            };
            IEqualityComparer<RecordState> comparer = new RecordStateComparer.ByMfn ();
            Assert.IsTrue(comparer.Equals(first, second));
            Assert.AreEqual(comparer.GetHashCode(first), comparer.GetHashCode(second));

            second.Mfn = 124;
            Assert.IsFalse(comparer.Equals(first, second));
            Assert.AreNotEqual(comparer.GetHashCode(first), comparer.GetHashCode(second));
        }

        [TestMethod]
        public void RecordStateComparer_ByVersion_1()
        {
            RecordState first = new RecordState
            {
                Id = 1,
                Mfn = 123,
                Status = RecordStatus.Last,
                Version = 5
            };
            RecordState second = new RecordState
            {
                Id = 2,
                Mfn = 123,
                Status = RecordStatus.LogicallyDeleted,
                Version = 5
            };
            IEqualityComparer<RecordState> comparer = new RecordStateComparer.ByVersion ();
            Assert.IsTrue(comparer.Equals(first, second));
            Assert.AreEqual(comparer.GetHashCode(first), comparer.GetHashCode(second));

            second.Mfn = 124;
            Assert.IsFalse(comparer.Equals(first, second));
            Assert.AreNotEqual(comparer.GetHashCode(first), comparer.GetHashCode(second));

            second.Mfn = 123;
            second.Version = 6;
            Assert.IsFalse(comparer.Equals(first, second));
            Assert.AreNotEqual(comparer.GetHashCode(first), comparer.GetHashCode(second));
        }
    }
}
