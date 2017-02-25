using System;
using AM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisFormatTest
    {
        private void _TestFormat
            (
                string text,
                string expected
            )
        {
            string actual = IrbisFormat.PrepareFormat(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IrbisFormat_PrepareFormat_1()
        {
            _TestFormat("", "");
            _TestFormat(" ", " ");
            _TestFormat("v100,/,v200", "v100,/,v200");
            _TestFormat("\tv100\r\n", " v100  ");
            _TestFormat
                (
                    "v100/*comment\r\nv200",
                    "v100  v200"
                );
        }

        [TestMethod]
        public void IrbisFormat_VerifyFormat_1()
        {
            Assert.IsFalse(IrbisFormat.VerifyFormat(null, false));
        }

        [TestMethod]
        public void IrbisFormat_VerifyFormat_2()
        {
            Assert.IsTrue(IrbisFormat.VerifyFormat("v200^a", false));
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void IrbisFormat_VerifyFormat_Exception_1()
        {
            IrbisFormat.VerifyFormat(null, true);
        }
    }
}
