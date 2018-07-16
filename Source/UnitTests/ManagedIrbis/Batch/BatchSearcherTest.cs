using System;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using Moq;

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class BatchSearcherTest
    {
        [NotNull]
        private Mock<IIrbisConnection> GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();
            IIrbisConnection connection = result.Object;

            // CommandFactory
            result.SetupGet(c => c.CommandFactory)
                .Returns(new CommandFactory(connection));

            // ExecuteCommand
            result.Setup(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()))
                .Returns
                (
                    (SearchReadCommand command) =>
                    {
                        MarcRecord[] records =
                        {
                            new MarcRecord{Mfn = 1},
                            new MarcRecord{Mfn = 2},
                            new MarcRecord{Mfn = 3}
                        };
                        command.Records = records;

                        byte[] rawAnswer = new byte[0];
                        byte[][] rawRequest = { new byte[0], new byte[0] };
                        ServerResponse response = new ServerResponse
                            (
                                connection,
                                rawAnswer,
                                rawRequest,
                                true
                            );
                        return response;
                    }
                );

            return result;
        }

        [TestMethod]
        public void BatchSearcher_Construction_1()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
                (
                    connection,
                    database,
                    prefix
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreSame(prefix, batch.Prefix);
            Assert.AreEqual(BatchSearcher.DefaultBatchSize, batch.BatchSize);
            Assert.AreEqual(BatchSearcher.DefaultOperation, batch.Operation);
        }

        [TestMethod]
        public void BatchSearcher_BuildExpression_1()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
                (
                    connection,
                    database,
                    prefix
                );
            string[] terms = {"мене", "текел", "фарес"};
            string actual = batch.BuildExpression(terms);
            Assert.AreEqual("K=мене+K=текел+K=фарес", actual);

            terms = new[] {"у попа", "была собака"};
            actual = batch.BuildExpression(terms);
            Assert.AreEqual("\"K=у попа\"+\"K=была собака\"", actual);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void BatchSearcher_BuildExpression_1a()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
                (
                    connection,
                    database,
                    prefix
                );
            string[] terms = new string[0];
            batch.BuildExpression(terms);
        }

        [TestMethod]
        public void BatchSearcher_Search_1()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();
            string expressionUsed = null;
            mock.Setup(c => c.Search(It.IsAny<string>()))
                .Returns(new[] { 1, 2, 3 })
                .Callback((string expression) => expressionUsed = expression);

            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
                (
                    connection,
                    database,
                    prefix
                );
            string[] terms = { "мене", "текел", "фарес" };
            int[] found = batch.Search(terms);
            Assert.AreEqual(3, found.Length);
            Assert.AreEqual("K=мене+K=текел+K=фарес", expressionUsed);
            mock.Verify(c=> c.Search(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchSearcher_Search_1a()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
                (
                    connection,
                    database,
                    prefix
                );
            batch.BatchSize = 0;
            string[] terms = { "мене", "текел", "фарес" };
            batch.Search(terms);
        }

        [TestMethod]
        public void BatchSearcher_Search_2()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();
            string expressionUsed = null;
            mock.Setup(c => c.Search(It.IsAny<string>()))
                .Returns(new[] { 1, 2, 3 })
                .Callback((string expression) => expressionUsed = expression);

            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
                (
                    connection,
                    database,
                    prefix
                );
            string[] terms = { "у попа", "была собака" };
            int[] found = batch.Search(terms);
            Assert.AreEqual(3, found.Length);
            Assert.AreEqual("\"K=у попа\"+\"K=была собака\"", expressionUsed);
            mock.Verify(c=> c.Search(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void BatchSearcher_Search_3()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();
            string expressionUsed = null;
            mock.Setup(c => c.Search(It.IsAny<string>()))
                .Returns(new[] { 1, 2, 3 })
                .Callback((string expression) => expressionUsed = expression);

            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
                (
                    connection,
                    database,
                    prefix
                );
            string[] terms = new string[0];
            int[] found = batch.Search(terms);
            Assert.AreEqual(0, found.Length);
            Assert.IsNull(expressionUsed);
            mock.Verify(c=> c.Search(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void BatchSearcher_SearchRead_1()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();

            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
                (
                    connection,
                    database,
                    prefix
                );
            string[] terms = { "мене", "текел", "фарес" };
            MarcRecord[] found = batch.SearchRead(terms);
            Assert.AreEqual(3, found.Length);
            Assert.AreEqual(1, found[0].Mfn);
            Assert.AreEqual(2, found[1].Mfn);
            Assert.AreEqual(3, found[2].Mfn);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchSearcher_SearchRead_1a()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
                (
                    connection,
                    database,
                    prefix
                );
            batch.BatchSize = 0;
            string[] terms = { "мене", "текел", "фарес" };
            batch.SearchRead(terms);
        }

        [TestMethod]
        public void BatchSearcher_SearchRead_2()
        {
            string database = "IBIS";
            string prefix = "K=";
            Mock<IIrbisConnection> mock = GetMock();

            IIrbisConnection connection = mock.Object;
            BatchSearcher batch = new BatchSearcher
            (
                connection,
                database,
                prefix
            );
            string[] terms = new string[0];
            MarcRecord[] found = batch.SearchRead(terms);
            Assert.AreEqual(0, found.Length);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()), Times.Never);
        }

    }
}
