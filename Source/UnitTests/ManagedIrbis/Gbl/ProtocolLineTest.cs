using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class ProtocolLineTest
    {
        [TestMethod]
        public void ProtocolLine_Construction()
        {
            ProtocolLine line = new ProtocolLine();
            Assert.AreEqual(false, line.Success);
            Assert.AreEqual(null, line.Database);
            Assert.AreEqual(0, line.Mfn);
            Assert.AreEqual(null, line.Update);
            Assert.AreEqual(null, line.Status);
            Assert.AreEqual(null, line.Error);
            Assert.AreEqual(null, line.UpdUf);
            Assert.AreEqual(null, line.Text);
        }

        [TestMethod]
        public void ProtocolLine_Parse1()
        {
            ProtocolLine line = ProtocolLine.Parse
                (
                    "DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#"
                );
            Assert.AreEqual(true, line.Success);
            Assert.AreEqual("IBIS", line.Database);
            Assert.AreEqual(2, line.Mfn);
            Assert.AreEqual("0", line.Update);
            Assert.AreEqual("8", line.Status);
            Assert.AreEqual(null, line.Error);
            Assert.AreEqual("0", line.UpdUf);
            Assert.AreEqual("DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#", line.Text);
        }

        [TestMethod]
        public void ProtocolLine_Parse2()
        {
            ProtocolLine line = ProtocolLine.Parse
                (
                    "DBN=IBIS#MFN=4#GBL_ERROR=-605"
                );
            Assert.AreEqual(false, line.Success);
            Assert.AreEqual("IBIS", line.Database);
            Assert.AreEqual(4, line.Mfn);
            Assert.AreEqual(null, line.Update);
            Assert.AreEqual(null, line.Status);
            Assert.AreEqual("-605", line.Error);
            Assert.AreEqual(null, line.UpdUf);
            Assert.AreEqual("DBN=IBIS#MFN=4#GBL_ERROR=-605", line.Text);
        }

        [TestMethod]
        public void ProtocolLine_ToString()
        {
            ProtocolLine line = ProtocolLine.Parse
                (
                    "DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#"
                );
            Assert.AreEqual
                (
                    "DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#",
                    line.ToString()
                );
        }
    }
}
