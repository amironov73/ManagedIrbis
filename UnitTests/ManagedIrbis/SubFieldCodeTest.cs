using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class SubFieldCodeTest
    {
        [TestMethod]
        public void TestSubFieldCodeIsValidCode()
        {
            Assert.IsTrue(SubFieldCode.IsValidCode('c'));
            Assert.IsFalse(SubFieldCode.IsValidCode('\0'));
        }

        [TestMethod]
        public void TestSubFieldCodeNormalize()
        {
            Assert.AreEqual
                (
                    'c',
                    SubFieldCode.Normalize('C')
                );
        }

        [TestMethod]
        public void TestSubFieldCodeVerify()
        {
            Assert.IsTrue(SubFieldCode.Verify('c'));
            Assert.IsFalse(SubFieldCode.Verify('\0'));
        }

    }
}
