using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class SourceCodeUtilityTest
    {
        [TestMethod]
        public void SourceCodeUtility_Byte_ToSourceCode()
        {
            Assert.AreEqual("0x00", SourceCodeUtility.ToSourceCode(0));
            Assert.AreEqual("0x01", SourceCodeUtility.ToSourceCode(1));
            Assert.AreEqual("0xFF", SourceCodeUtility.ToSourceCode(255));
        }

        [TestMethod]
        public void SourceCodeUtility_ByteArray_ToSourceCode()
        {
            Assert.AreEqual("{}", SourceCodeUtility.ToSourceCode(new byte[0]));
            Assert.AreEqual("{0x00}", SourceCodeUtility.ToSourceCode(new byte[] { 0 }));
            Assert.AreEqual("{0x00, 0xFF}", SourceCodeUtility.ToSourceCode(new byte[] { 0, 255 }));
        }

        [TestMethod]
        public void SourceCodeUtility_Int32Array_ToSourceCode()
        {
            Assert.AreEqual("{}", SourceCodeUtility.ToSourceCode(new int[0]));
            Assert.AreEqual("{0}", SourceCodeUtility.ToSourceCode(new[] { 0 }));
            Assert.AreEqual("{0, 255}", SourceCodeUtility.ToSourceCode(new[] { 0, 255 }));
        }
    }
}
