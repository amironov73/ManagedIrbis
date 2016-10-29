using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class InquiryEventArgsTest
    {
        [TestMethod]
        public void InquiryEventArgs_Success()
        {
            InquiryEventArgs args = new InquiryEventArgs();
            Assert.IsFalse(args.Success);
            args.Success = true;
            Assert.IsTrue(args.Success);
        }
    }
}
