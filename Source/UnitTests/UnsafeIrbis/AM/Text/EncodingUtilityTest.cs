using System;
using System.Text;

using UnsafeAM;
using UnsafeAM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.UnsafeAM.Text
{
    [TestClass]
    public class EncodingUtilityTest
    {
        [TestMethod]
        public void EncodingUtility_Cp866_1()
        {
            Encoding encoding = EncodingUtility.Cp866;
            Assert.IsNotNull(encoding);
            Assert.AreEqual(866, encoding.CodePage);
        }

        [TestMethod]
        public void EncodingUtility_Windows1251_1()
        {
            Encoding encoding = EncodingUtility.Windows1251;
            Assert.IsNotNull(encoding);
            Assert.AreEqual(1251, encoding.CodePage);
        }

        [TestMethod]
        public void EncodingUtility_ChangeEncoding_1()
        {
            string expected = "Привет";
            string actual = EncodingUtility.ChangeEncoding
                (
                    "╧ЁштхЄ",
                    EncodingUtility.Windows1251,
                    EncodingUtility.Cp866
                );
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodingUtility_ChangeEncoding_2()
        {
            string expected = "Привет";
            string actual = EncodingUtility.ChangeEncoding
                (
                    "ЏаЁўҐв",
                    EncodingUtility.Cp866,
                    EncodingUtility.Windows1251
                );
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodingUtility_ChangeEncoding_3()
        {
            string expected = "";
            string actual = EncodingUtility.ChangeEncoding
                (
                    "",
                    EncodingUtility.Cp866,
                    EncodingUtility.Windows1251
                );
            Assert.AreEqual(expected, actual);
        }
    }
}
