using System;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;

using Moq;

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class BatchRecordReaderTest
    {
        [NotNull]
        private Mock<IIrbisConnection> GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();

            // GetMaxMfn
            result.Setup(c => c.GetMaxMfn(It.IsAny<string>()))
                .Returns(4);

            // ReadRecord
            result.Setup(c => c.ReadRecord(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<bool>(),
                    It.IsAny<string>()))
                .Returns
                    (
                        (string db, int mfn, bool flag, string fmt) =>
                            new MarcRecord
                            {
                                Database = db,
                                Mfn = mfn
                            }
                    );

            // Search
            result.Setup(c => c.Search(It.IsAny<string>()))
                .Returns(new [] {1, 2, 3});

            return result;
        }

        [TestMethod]
        public void BatchRecordReader_Construction_1()
        {
            string database = "IBIS";
            int batchSize = 500;
            int[] range = {1, 2, 3};
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    range
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.IsFalse(batch.OmitDeletedRecords);
            Assert.AreEqual(0, batch.RecordsRead);
            Assert.AreEqual(range.Length, batch.TotalRecords);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordReader_Construction_1a()
        {
            string database = "IBIS";
            int batchSize = 0;
            int[] range = {1, 2, 3};
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    range
                );
        }

        [TestMethod]
        public void BatchRecordReader_Construction_2()
        {
            string database = "IBIS";
            int batchSize = 500;
            int[] range = {1, 2, 3};
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    true,
                    range
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.IsTrue(batch.OmitDeletedRecords);
            Assert.AreEqual(0, batch.RecordsRead);
            Assert.AreEqual(range.Length, batch.TotalRecords);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordReader_Construction_2a()
        {
            string database = "IBIS";
            int batchSize = 0;
            int[] range = {1, 2, 3};
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    true,
                    range
                );
        }

        [TestMethod]
        public void BatchRecordReader_Interval_1()
        {
            string database = "IBIS";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch
                = (BatchRecordReader)BatchRecordReader.Interval
                (
                    connection,
                    database,
                    1,
                    3,
                    batchSize
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsRead);

            mock.Verify(c=>c.GetMaxMfn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordReader_Interval_1a()
        {
            string database = "IBIS";
            int batchSize = 0;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch
                = (BatchRecordReader)BatchRecordReader.Interval
                (
                    connection,
                    database,
                    1,
                    3,
                    batchSize
                );
        }

        [TestMethod]
        public void BatchRecordReader_Search_1()
        {
            string database = "IBIS";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch
                = (BatchRecordReader)BatchRecordReader.Search
                (
                    connection,
                    database,
                    "searchQuery",
                    batchSize
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsRead);

            mock.Verify(c=>c.Search(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordReader_Search_1a()
        {
            string database = "IBIS";
            int batchSize = 0;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch
                = (BatchRecordReader)BatchRecordReader.Search
                (
                    connection,
                    database,
                    "searchQuery",
                    batchSize
                );
        }

        [TestMethod]
        public void BatchRecordReader_WholeDatabase_1()
        {
            string database = "IBIS";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch
                = (BatchRecordReader) BatchRecordReader.WholeDatabase
                    (
                        connection,
                        database,
                        batchSize
                    );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsRead);

            mock.Verify(c => c.GetMaxMfn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordReader_WholeDatabase_1a()
        {
            string database = "IBIS";
            int batchSize = 0;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordReader batch
                = (BatchRecordReader) BatchRecordReader.WholeDatabase
                    (
                        connection,
                        database,
                        batchSize
                    );
        }
    }
}
