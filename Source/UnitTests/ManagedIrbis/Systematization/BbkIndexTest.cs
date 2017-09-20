using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Systematization;

namespace UnitTests.ManagedIrbis.Systematization
{
    [TestClass]
    public class BbkIndexTest
    {
        [TestMethod]
        public void BbkIndex_Parse_1()
        {
            BbkIndex bbk = BbkIndex.Parse("32.973.26-018.2я75");
            Assert.AreEqual(bbk.MainIndex, "32.973.26");
            Assert.AreEqual(bbk.Qualifiers[0], "я75");
            Assert.AreEqual(bbk.SpecialIndex[0], "-018.2");
        }

        [TestMethod]
        public void BbkIndex_Parse_2()
        {
            BbkIndex bbk = BbkIndex.Parse("32.973-018.2я7");
            Assert.AreEqual(bbk.MainIndex, "32.973");
            Assert.AreEqual(bbk.Qualifiers[0], "я7");
            Assert.AreEqual(bbk.SpecialIndex[0], "-018.2");
        }

        [TestMethod]
        public void BbkIndex_Parse_3()
        {
            BbkIndex bbk = BbkIndex.Parse("22.174");
            Assert.AreEqual(bbk.MainIndex, "22.174");
        }

        [TestMethod]
        public void BbkIndex_Parse_4()
        {
            BbkIndex bbk = BbkIndex.Parse("63.3(2Р-2СПб)");
            Assert.AreEqual(bbk.MainIndex, "63.3");
            Assert.AreEqual(bbk.TerritorialIndex, "(2Р-2СПб)");
        }

        [TestMethod]
        public void BbkIndex_Parse_5()
        {
            BbkIndex bbk = BbkIndex.Parse("34.621-52.004.05-049.002,27-02(2)к6");
            Assert.AreEqual(bbk.MainIndex, "34.621");
            Assert.AreEqual(bbk.SpecialIndex[0], "-52.004.05");
            Assert.AreEqual(bbk.SpecialIndex[1], "-049.002");
            Assert.AreEqual(bbk.SpecialIndex[2], "-02");
            Assert.AreEqual(bbk.Qualifiers[0], "к6");
            Assert.AreEqual(bbk.TerritorialIndex, "(2)");
        }
    }
}
