using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Systematization;

namespace UnitTests.ManagedIrbis.Systematization
{
    [TestClass]
    public class BbkRangeTest
    {
        [TestMethod]
        public void BbkRange_Construction_1()
        {
            BbkRange bbk = new BbkRange("84.3/5");
            Assert.AreEqual("84.3/5", bbk.OriginalIndex);
            Assert.AreEqual("84.3", bbk.FirstIndex);
            Assert.AreEqual("84.5", bbk.LastIndex);
            string[] all = bbk.GetAllIndexes();
            Assert.AreEqual(3, all.Length);
            Assert.AreEqual("84.3/5", bbk.ToString());
        }

        [TestMethod]
        public void BbkRange_Construction_2()
        {
            BbkRange bbk = new BbkRange("81/89");
            Assert.AreEqual("81/89", bbk.OriginalIndex);
            Assert.AreEqual("81", bbk.FirstIndex);
            Assert.AreEqual("89", bbk.LastIndex);
            string[] all = bbk.GetAllIndexes();
            Assert.AreEqual(9, all.Length);
            Assert.AreEqual("81/89", bbk.ToString());
        }

        [TestMethod]
        public void BbkRange_Construction_3()
        {
            BbkRange bbk = new BbkRange("84");
            Assert.AreEqual("84", bbk.OriginalIndex);
            Assert.AreEqual("84", bbk.FirstIndex);
            Assert.AreEqual("84", bbk.LastIndex);
            string[] all = bbk.GetAllIndexes();
            Assert.AreEqual(1, all.Length);
            Assert.AreEqual("84", bbk.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(BbkException))]
        public void BbkRange_Construction_Exception_1()
        {
            BbkRange bbk = new BbkRange("84./3/5");
            Assert.AreEqual("84.3/5", bbk.OriginalIndex);
            Assert.AreEqual("84.3", bbk.FirstIndex);
            Assert.AreEqual("84.5", bbk.LastIndex);
            string[] all = bbk.GetAllIndexes();
            Assert.AreEqual(3, all.Length);
            Assert.AreEqual("84.3/5", bbk.ToString());
        }
    }
}
