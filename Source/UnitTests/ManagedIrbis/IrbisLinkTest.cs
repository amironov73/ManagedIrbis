using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisLinkTest
    {
        [TestMethod]
        public void IrbisLink_Constructor_1()
        {
            IrbisLink link = new IrbisLink();

            Assert.IsNull(link.Command);
            Assert.IsNull(link.Database);
            Assert.IsNull(link.FileName);
            Assert.IsNull(link.Format);
            Assert.IsNull(link.Key);
            Assert.IsNull(link.Path);
            Assert.IsNotNull(link.Parameters);
            Assert.AreEqual(0, link.Parameters.Count);
        }

        [TestMethod]
        public void IrbisLink_Parse_1()
        {
            IrbisLink link = IrbisLink.Parse("IRBIS:1,,IBIS,FULLW0_WN,@6");
            Assert.AreEqual("1", link.Command);
            Assert.AreEqual("IBIS", link.Database);
            Assert.AreEqual("FULLW0_WN", link.Format);
            Assert.AreEqual("@6", link.Key);
        }

        [TestMethod]
        public void IrbisLink_Parse_2()
        {
            IrbisLink link = IrbisLink.Parse("IRBIS:?C21COM=1&I21DBN=IBIS&PFTNAME=FULLW0_WN&KEY=@6");
            Assert.AreEqual("1", link.Command);
            Assert.AreEqual("IBIS", link.Database);
            Assert.AreEqual("FULLW0_WN", link.Format);
            Assert.AreEqual("@6", link.Key);
        }

        [TestMethod]
        public void IrbisLink_Parse_3()
        {
            IrbisLink link = IrbisLink.Parse("IRBIS:1,,IBIS,,@6?PFTNAME=FULLW0_WN");
            Assert.AreEqual("1", link.Command);
            Assert.AreEqual("IBIS", link.Database);
            Assert.AreEqual("FULLW0_WN", link.Format);
            Assert.AreEqual("@6", link.Key);
        }

        [TestMethod]
        public void IrbisLink_Parse_4()
        {
            IrbisLink link = IrbisLink.Parse("IRBIS:3,10,IBIS,brief.pft");
            Assert.AreEqual("3", link.Command);
            Assert.AreEqual("10", link.Path);
            Assert.AreEqual("IBIS", link.Database);
            Assert.AreEqual("brief.pft", link.FileName);
        }

        [TestMethod]
        public void IrbisLink_Parse_5()
        {
            IrbisLink link = IrbisLink.Parse("IRBIS:3,10,IBIS,brief.pft?WRONG");
            Assert.AreEqual(0, link.Parameters.Count);
        }

        [TestMethod]
        public void IrbisLink_ParseForImage_1()
        {
            IrbisLink link = IrbisLink.ParseForImage("IRBIS:10,,textfolder.gif");
            Assert.AreEqual("3", link.Command);
            Assert.AreEqual("10", link.Path);
            Assert.IsNull(link.Database);
            Assert.AreEqual("textfolder.gif", link.FileName);
        }

        [TestMethod]
        public void IrbisLink_ParseForImage_2()
        {
            IrbisLink link = IrbisLink.ParseForImage("IRBIS:10?P1=V1&P2=V2&FILENAME=textfolder.gif");
            Assert.AreEqual("3", link.Command);
            Assert.AreEqual("10", link.Path);
            Assert.IsNull(link.Database);
            Assert.AreEqual("textfolder.gif", link.FileName);
        }

        [TestMethod]
        public void IrbisLink_ParseForImage_3()
        {
            IrbisLink link = IrbisLink.ParseForImage("IRBIS:10?FILENAME=textfolder.gif");
            Assert.AreEqual("3", link.Command);
            Assert.AreEqual("10", link.Path);
            Assert.IsNull(link.Database);
            Assert.AreEqual("textfolder.gif", link.FileName);
        }

        [TestMethod]
        public void IrbisLink_ToString()
        {
            IrbisLink link = IrbisLink.Parse("IRBIS:3,10,IBIS,brief.pft");
            const string expected = "Command: 3";
            string actual = link.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
