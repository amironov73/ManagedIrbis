using System;
using AM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisFormatTest
    {
        private void _TestPrepareFormat
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
            _TestPrepareFormat("", "");
            _TestPrepareFormat(" ", " ");
            _TestPrepareFormat("v100,/,v200", "v100,/,v200");
            _TestPrepareFormat("\tv100\r\n", " v100");
            _TestPrepareFormat
                (
                    "v100/*comment\r\nv200",
                    "v100v200"
                );
        }

        [TestMethod]
        public void IrbisFormat_PrepareFormat_2()
        {
            _TestPrepareFormat("\r\n", "");
        }

        [TestMethod]
        public void IrbisFormat_PrepareFormat_3()
        {
            _TestPrepareFormat
            (
                "v100 '\t'\r\nv200",
                "v100 '\t'v200"
            );
            _TestPrepareFormat
            (
                "v100 \"\t\"\r\nv200",
                "v100 \"\t\"v200"
            );
            _TestPrepareFormat
            (
                "v100 |\t|\r\nv200",
                "v100 |\t|v200"
            );
        }

        private void _TestRemoveComments
        (
            string text,
            string expected
        )
        {
            string actual = IrbisFormat.RemoveComments(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IrbisFormat_RemoveComments_1()
        {
            _TestRemoveComments("", "");
            _TestRemoveComments(" ", " ");
            _TestRemoveComments("v100,/,v200", "v100,/,v200");
            _TestRemoveComments("\tv100\r\n", "\tv100\r\n");
            _TestRemoveComments
                (
                    "v100/*comment\r\nv200",
                    "v100\nv200"
                );
            _TestRemoveComments
                (
                    "v100, '/*not comment' v200",
                    "v100, '/*not comment' v200"
                );
            _TestRemoveComments
                (
                    "v100, |/*not comment| v200",
                    "v100, |/*not comment| v200"
                );
            _TestRemoveComments
                (
                    "v100, \"/*not comment\" v200",
                    "v100, \"/*not comment\" v200"
                );
        }

        [TestMethod]
        public void IrbisFormat_RemoveComments_2()
        {
            _TestRemoveComments
                (
                    "v100, '/*not comment' v200, /*comment\r\nv300",
                    "v100, '/*not comment' v200, \nv300"
                );
            _TestRemoveComments
            (
                "v100, '/*not comment' v200, /,\r\nv300",
                "v100, '/*not comment' v200, /,\r\nv300"
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
