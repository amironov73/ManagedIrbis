using System;
using System.Text;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;


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

        [TestMethod]
        public void IrbisEncoding_Ansi_1()
        {
            Assert.AreEqual(1251, IrbisEncoding.Ansi.CodePage);
        }

        [TestMethod]
        public void IrbisEncoding_Oem_1()
        {
            Assert.AreEqual(866, IrbisEncoding.Oem.CodePage);
        }

        [TestMethod]
        public void IrbisEncoding_Utf8_1()
        {
            Assert.AreEqual(65001, IrbisEncoding.Utf8.CodePage);
        }

        [TestMethod]
        public void IrbisEncoding_ByName_1()
        {
            Assert.AreEqual(1251, IrbisEncoding.ByName("Ansi").CodePage);
            Assert.AreEqual(866, IrbisEncoding.ByName("Oem").CodePage);
            Assert.AreEqual(65001, IrbisEncoding.ByName("Utf").CodePage);
            Assert.AreEqual(65001, IrbisEncoding.ByName(string.Empty).CodePage);
            Assert.AreEqual(1252, IrbisEncoding.ByName("windows-1252").CodePage);
        }

        [TestMethod]
        public void IrbisEncoding_RelaxUtf8_1()
        {
            IrbisEncoding.RelaxUtf8();
            IrbisEncoding.StrongUtf8();
        }

        [TestMethod]
        public void IrbisEncoding_SetAnsiEncoding_1()
        {
            Encoding saveEncoding = IrbisEncoding.Ansi;
            Encoding encoding = IrbisEncoding.ByName("windows-1252");
            IrbisEncoding.SetAnsiEncoding(encoding);
            Assert.AreSame(encoding, IrbisEncoding.Ansi);
            IrbisEncoding.SetAnsiEncoding(saveEncoding);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IrbisEncoding_SetAnsiEncoding_2()
        {
            IrbisEncoding.SetAnsiEncoding(Encoding.Unicode);
        }

        [TestMethod]
        public void IrbisEncoding_SetOemEncoding_1()
        {
            Encoding saveEncoding = IrbisEncoding.Oem;
            Encoding encoding = IrbisEncoding.ByName("windows-1252");
            IrbisEncoding.SetOemEncoding(encoding);
            Assert.AreSame(encoding, IrbisEncoding.Oem);
            IrbisEncoding.SetOemEncoding(saveEncoding);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IrbisEncoding_SetOemEncoding_2()
        {
            IrbisEncoding.SetOemEncoding(Encoding.Unicode);
        }
    }
}
