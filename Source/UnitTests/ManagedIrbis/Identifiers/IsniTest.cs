using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class IsniTest
    {
        [TestMethod]
        public void Isni_GenerateCheckDigit_1()
        {
            Assert.AreEqual(Isni.GenerateCheckDigit("000000029534656"), "X");
            Assert.AreEqual(Isni.GenerateCheckDigit("000000021825009"), "7");
            Assert.AreEqual(Isni.GenerateCheckDigit("000000015109370"), "0");
            Assert.AreEqual(Isni.GenerateCheckDigit("000000021694233"), "X");
        }
    }
}
