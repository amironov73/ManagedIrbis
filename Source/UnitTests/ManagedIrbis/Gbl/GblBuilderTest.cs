using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblBuilderTest
    {
        [TestMethod]
        public void GblBuilder_Construction()
        {
            GblBuilder builder = new GblBuilder();
            Assert.AreEqual(0, builder.ToStatements().Length);
        }

        [TestMethod]
        public void GblBuilder_Add()
        {
            GblBuilder builder = new GblBuilder();
            builder.Add("910", "^a0^b1");
            GblStatement[] statements = builder.ToStatements();
            Assert.AreEqual("ADD", statements[0].Command);
            Assert.AreEqual("910", statements[0].Parameter1);
            Assert.AreEqual("^a0^b1", statements[0].Format1);
        }

        [TestMethod]
        public void GblBuilder_Change()
        {
            GblBuilder builder = new GblBuilder();
            builder.Change("910", "^a0^b1", "^a9^b1");
            GblStatement[] statements = builder.ToStatements();
            Assert.AreEqual("CHA", statements[0].Command);
            Assert.AreEqual("910", statements[0].Parameter1);
            Assert.AreEqual("^a0^b1", statements[0].Format1);
            Assert.AreEqual("^a9^b1", statements[0].Format2);
        }

        [TestMethod]
        public void GblBuilder_Delete()
        {
            GblBuilder builder = new GblBuilder();
            builder.Delete("910", "1");
            GblStatement[] statements = builder.ToStatements();
            Assert.AreEqual("DEL", statements[0].Command);
            Assert.AreEqual("910", statements[0].Parameter1);
            Assert.AreEqual("1", statements[0].Parameter2);
        }

        [TestMethod]
        public void GblBuilder_DeleteRecord()
        {
            GblBuilder builder = new GblBuilder();
            builder.DeleteRecord();
            GblStatement[] statements = builder.ToStatements();
            Assert.AreEqual("DELR", statements[0].Command);
        }
    }
}
