using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Text;

namespace UnitTests.UnsafeAM.Text
{
    [TestClass]
    public class SourceCodeUtilityTest
    {
        [TestMethod]
        public void SourceCodeUtility_EncodeCharacter_1()
        {
            Assert.AreEqual("a", SourceCodeUtility.EncodeCharacter('a'));

            Assert.AreEqual("\\a", SourceCodeUtility.EncodeCharacter('\a'));
            Assert.AreEqual("\\b", SourceCodeUtility.EncodeCharacter('\b'));
            Assert.AreEqual("\\f", SourceCodeUtility.EncodeCharacter('\f'));
            Assert.AreEqual("\\n", SourceCodeUtility.EncodeCharacter('\n'));
            Assert.AreEqual("\\r", SourceCodeUtility.EncodeCharacter('\r'));
            Assert.AreEqual("\\t", SourceCodeUtility.EncodeCharacter('\t'));
            Assert.AreEqual("\\v", SourceCodeUtility.EncodeCharacter('\v'));
            Assert.AreEqual("\\\\", SourceCodeUtility.EncodeCharacter('\\'));
            Assert.AreEqual("\\'", SourceCodeUtility.EncodeCharacter('\''));
            Assert.AreEqual("\\\"", SourceCodeUtility.EncodeCharacter('"'));

            Assert.AreEqual("\\0", SourceCodeUtility.EncodeCharacter((char)0));
        }

        [TestMethod]
        public void SourceCodeUtility_ToSourceCode_1()
        {
            Assert.AreEqual("0x00", SourceCodeUtility.ToSourceCode(0));
            Assert.AreEqual("0x01", SourceCodeUtility.ToSourceCode(1));
            Assert.AreEqual("0xFF", SourceCodeUtility.ToSourceCode(255));
        }

        [TestMethod]
        public void SourceCodeUtility_ToSourceCode_2()
        {
            Assert.AreEqual("{}", SourceCodeUtility.ToSourceCode(new byte[0]));
            Assert.AreEqual("{0x00}", SourceCodeUtility.ToSourceCode(new byte[] { 0 }));
            Assert.AreEqual("{0x00, 0xFF}", SourceCodeUtility.ToSourceCode(new byte[] { 0, 255 }));

            Assert.AreEqual("{0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, \n  0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0xFF}",
                SourceCodeUtility.ToSourceCode(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 }).DosToUnix());
        }

        [TestMethod]
        public void SourceCodeUtility_ToSourceCode_3()
        {
            Assert.AreEqual("{}", SourceCodeUtility.ToSourceCode(new int[0]));
            Assert.AreEqual("{0}", SourceCodeUtility.ToSourceCode(new[] { 0 }));
            Assert.AreEqual("{0, 255}", SourceCodeUtility.ToSourceCode(new[] { 0, 255 }));

            Assert.AreEqual("{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, \n  10, 11, 12, 13, 14, 15, 255}",
                SourceCodeUtility.ToSourceCode(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 }).DosToUnix());
        }

        [TestMethod]
        public void SourceCodeUtility_ToSourceCode_4()
        {
            Assert.AreEqual("'a'", SourceCodeUtility.ToSourceCode('a'));
            Assert.AreEqual("' '", SourceCodeUtility.ToSourceCode(' '));
            Assert.AreEqual("'\\n'", SourceCodeUtility.ToSourceCode('\n'));
        }

        [TestMethod]
        public void SourceCodeUtility_ToSourceCode_5()
        {
            Assert.AreEqual("{}", SourceCodeUtility.ToSourceCode(new char[0]));
            Assert.AreEqual("{'a'}", SourceCodeUtility.ToSourceCode(new[] { 'a' }));
            Assert.AreEqual("{'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', \n  'k', 'l', 'm'}",
                SourceCodeUtility.ToSourceCode(new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm' }).DosToUnix());
        }
    }
}
