using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis;
using ManagedIrbis.Authentication;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable PossibleNullReferenceException

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class ConnectedFormatterTest
    {
        [TestMethod]
        public void ConnectedFormatter_Construction_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;

            using (ConnectedFormatter formatter = new ConnectedFormatter(connection))
            {
                Assert.AreSame(connection, formatter.Connection);
                Assert.IsNull(formatter.Source);
                Assert.IsFalse(formatter.SupportsExtendedSyntax);
            }
        }

        [TestMethod]
        public void ConnectedFormatter_FormatRecord_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "formatted";
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<MarcRecord>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedFormatter formatter = new ConnectedFormatter(connection))
            {
                formatter.ParseProgram("program");
                MarcRecord record = new MarcRecord();
                string actual = formatter.FormatRecord(record);
                Assert.AreEqual(expected, actual);
            }

            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<MarcRecord>()));
        }

        [TestMethod]
        public void ConnectedFormatter_FormatRecord_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "formatted";
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<MarcRecord>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedFormatter formatter = new ConnectedFormatter(connection))
            {
                // formatter.ParseProgram("program"); -- типа забыли вызвать
                MarcRecord record = new MarcRecord();
                string actual = formatter.FormatRecord(record);
                Assert.AreEqual(string.Empty, actual);
            }

            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<MarcRecord>()), Times.Never);
        }

        [TestMethod]
        public void ConnectedFormatter_FormatRecord_3()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "formatted";
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedFormatter formatter = new ConnectedFormatter(connection))
            {
                int mfn = 1;
                formatter.ParseProgram("program");
                string actual = formatter.FormatRecord(mfn);
                Assert.AreEqual(expected, actual);
            }

            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void ConnectedFormatter_FormatRecord_4()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "formatted";
            mock.Setup(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedFormatter formatter = new ConnectedFormatter(connection))
            {
                int mfn = 1;
                // formatter.ParseProgram("program"); -- типа забыли вызвать
                string actual = formatter.FormatRecord(mfn);
                Assert.AreEqual(string.Empty, actual);
            }

            mock.Verify(c => c.FormatRecord(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void ConnectedFormatter_FormatRecords_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string[] expected =
            {
                "first line",
                "second line",
                "third line"
            };
            mock.SetupGet(c => c.Database).Returns("IBIS");
            mock.Setup(c => c.FormatRecords(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<IEnumerable<int>>()))
                .Returns(expected);
            IIrbisConnection connection = mock.Object;

            using (ConnectedFormatter formatter = new ConnectedFormatter(connection))
            {
                int[] mfns = {1, 2, 3};
                formatter.ParseProgram("program");
                string[] actual = formatter.FormatRecords(mfns);
                Assert.IsNotNull(actual);
                Assert.AreEqual(3, actual.Length);
            }

            mock.Verify(c => c.Database);
            mock.Verify(c => c.FormatRecords(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<int>>()));
        }
    }
}
