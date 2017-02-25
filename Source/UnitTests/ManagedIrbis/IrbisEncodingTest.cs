using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisEncodingTest
    {
        [TestMethod]
        public void IrbisEncoding_Static_Constructor_1()
        {
            Assert.IsNotNull(IrbisEncoding.Ansi);
            Assert.IsNotNull(IrbisEncoding.Oem);
            Assert.IsNotNull(IrbisEncoding.Utf8);
        }
    }
}
