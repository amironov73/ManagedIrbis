using System;
using System.IO;
using AM.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblStatementTest
    {
        [TestMethod]
        public void GblStatement_Construction()
        {
            GblStatement statement = new GblStatement();
            Assert.AreEqual(null, statement.Command);
            Assert.AreEqual(null, statement.Parameter1);
            Assert.AreEqual(null, statement.Parameter2);
            Assert.AreEqual(null, statement.Format1);
            Assert.AreEqual(null, statement.Format2);
        }

        [TestMethod]
        public void GblStatement_EncodeProtocol()
        {
            GblStatement statement = new GblStatement
            {
                Command = "ADD",
                Parameter1 = "910",
                Parameter2 = "XXXXX",
                Format1 = "^a0^b1",
                Format2 = "XXXXX"
            };
            Assert.AreEqual
                (
                    "ADD" + GblStatement.Delimiter
                    + "910" + GblStatement.Delimiter
                    + "XXXXX" + GblStatement.Delimiter
                    + "^a0^b1" + GblStatement.Delimiter
                    + "XXXXX" + GblStatement.Delimiter,
                    statement.EncodeForProtocol()
                );
        }

        [TestMethod]
        public void GblStatement_ParseStream()
        {
            const string text = "ADD\r\n910\r\nXXXXX\r\n^a0^b1\r\nXXXXX\r\n";
            TextReader reader = new StringReader(text);
            GblStatement statement = GblStatement.ParseStream(reader);
            Assert.IsNotNull(statement);
            Assert.AreEqual("ADD", statement.Command);
            Assert.AreEqual("910", statement.Parameter1);
            Assert.AreEqual("XXXXX", statement.Parameter2);
            Assert.AreEqual("^a0^b1", statement.Format1);
            Assert.AreEqual("XXXXX", statement.Format2);
        }

        private void _TestSerialize
            (
                GblStatement first
            )
        {
            byte[] bytes = first.SaveToMemory();

            GblStatement second = bytes
                .RestoreObjectFromMemory<GblStatement>();

            Assert.AreEqual(first.Command, second.Command);
            Assert.AreEqual(first.Parameter1, second.Parameter1);
            Assert.AreEqual(first.Parameter2, second.Parameter2);
            Assert.AreEqual(first.Format1, second.Format1);
            Assert.AreEqual(first.Format2, second.Format2);
        }

        [TestMethod]
        public void GblStatement_Serialize()
        {
            GblStatement statement = new GblStatement();
            _TestSerialize(statement);

            statement = new GblStatement
            {
                Command = "ADD",
                Parameter1 = "910",
                Parameter2 = "XXXXX",
                Format1 = "^a0^b1",
                Format2 = "XXXXX"
            };
            _TestSerialize(statement);
        }

        [TestMethod]
        public void GblStatement_Verify()
        {
            GblStatement statement = new GblStatement();
            Assert.AreEqual(false, statement.Verify(false));

            statement = new GblStatement
            {
                Command = "ADD",
                Parameter1 = "910",
                Parameter2 = "XXXXX",
                Format1 = "^a0^b1",
                Format2 = "XXXXX"
            };
            Assert.AreEqual(true, statement.Verify(false));
        }

        [TestMethod]
        public void GblStatement_ToString()
        {
            GblStatement statement = new GblStatement
            {
                Command = "ADD",
                Parameter1 = "910",
                Parameter2 = "XXXXX",
                Format1 = "^a0^b1",
                Format2 = "XXXXX"
            };
            string expected = "Command: ADD," + Environment.NewLine
                + "Parameter1: 910," + Environment.NewLine
                + "Parameter2: XXXXX," + Environment.NewLine
                + "Format1: ^a0^b1," + Environment.NewLine
                + "Format2: XXXXX";
            Assert.AreEqual
                (
                    expected,
                    statement.ToString()
                );
        }
    }
}
