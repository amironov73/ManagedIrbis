using System;

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class CodeDigitTest
    {
        [TestMethod]
        public void CodeDigit_ToString_1()
        {
            CodeDigit code = new CodeDigit('1', 1);
            Assert.AreEqual("1", code.ToString());
        }
    }
}
