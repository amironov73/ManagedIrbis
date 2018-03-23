using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable PossibleInvalidOperationException

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class CodeDigitTest
    {
        [TestMethod]
        public void CodeDigit_ExtractDigits_1()
        {
            CodeDigit[] allowed =
            {
                new CodeDigit('1', 1),
                new CodeDigit('2', 2),
                new CodeDigit('X', 10)
            };
            string identifier = "AXB2C1Z";
            CodeDigit[] extracted = CodeDigit.ExtractDigits(identifier, allowed);
            Assert.AreEqual(3, extracted.Length);
            Assert.AreEqual('X', extracted[0].Digit);
            Assert.AreEqual('2', extracted[1].Digit);
            Assert.AreEqual('1', extracted[2].Digit);
        }

        [TestMethod]
        public void CodeDigit_FindDigit_1()
        {
            CodeDigit[] allowed =
            {
                new CodeDigit('1', 1),
                new CodeDigit('2', 2),
                new CodeDigit('X', 10)
            };
            Assert.AreEqual('1', CodeDigit.FindDigit('1', allowed).Value.Digit);
            Assert.IsNull(CodeDigit.FindDigit('Z', allowed));
        }

        [TestMethod]
        public void CodeDigit_ToString_1()
        {
            CodeDigit code = new CodeDigit('1', 1);
            Assert.AreEqual("1", code.ToString());
        }
    }
}
