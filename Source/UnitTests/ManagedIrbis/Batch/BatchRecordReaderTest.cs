using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure.Commands;

using Moq;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable MustUseReturnValue
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class BatchRecordReaderTest
        : CommonBatchTest
    {
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
            new BatchRecordReader
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
            new BatchRecordReader
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
            BatchRecordReader.Interval
                (
                    connection,
                    database,
                    1,
                    3,
                    batchSize
                );
        }

        [TestMethod]
        public void BatchRecordReader_Interval_2()
        {
            string database = "IBIS";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            bool flag = false;
            Action<BatchRecordReader> action = brr => flag = true;
            BatchRecordReader batch
                = (BatchRecordReader)BatchRecordReader.Interval
                (
                    connection,
                    database,
                    1,
                    3,
                    batchSize,
                    true,
                    action
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsRead);

            List<MarcRecord> list = batch.ReadAll();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(3, batch.RecordsRead);
            Assert.IsTrue(flag);

            mock.Verify(c => c.GetMaxMfn(It.IsAny<string>()), Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()),
                Times.Once());
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
            BatchRecordReader.Search
                (
                    connection,
                    database,
                    "searchQuery",
                    batchSize
                );
        }

        [TestMethod]
        public void BatchRecordReader_Search_2()
        {
            string database = "IBIS";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            bool flag = false;
            Action<BatchRecordReader> action = brr => flag = true;
            BatchRecordReader batch
                = (BatchRecordReader)BatchRecordReader.Search
                (
                    connection,
                    database,
                    "searchQuery",
                    batchSize,
                    action
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsRead);

            List<MarcRecord> list = batch.ReadAll();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(3, batch.RecordsRead);
            Assert.IsTrue(flag);

            mock.Verify(c => c.Search(It.IsAny<string>()), Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()),
                Times.Once());
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
            BatchRecordReader.WholeDatabase
                    (
                        connection,
                        database,
                        batchSize
                    );
        }

        [TestMethod]
        public void BatchRecordReader_WholeDatabase_2()
        {
            string database = "IBIS";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            bool flag = false;
            Action<BatchRecordReader> action = brr => flag = true;
            BatchRecordReader batch
                = (BatchRecordReader)BatchRecordReader.WholeDatabase
                (
                    connection,
                    database,
                    batchSize,
                    action
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsRead);

            List<MarcRecord> list = batch.ReadAll();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(3, batch.RecordsRead);
            Assert.IsTrue(flag);

            mock.Verify(c => c.GetMaxMfn(It.IsAny<string>()), Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()),
                Times.Once());
        }

        [TestMethod]
        public void BatchRecordReader_WholeDatabase_2a()
        {
            string database = "IBIS";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            bool flag = false;
            Action<BatchRecordReader> action = brr => flag = true;
            BatchRecordReader batch
                = (BatchRecordReader)BatchRecordReader.WholeDatabase
                (
                    connection,
                    database,
                    batchSize,
                    action
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsRead);

            List<MarcRecord> list = batch.ReadAll(true);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(3, batch.RecordsRead);
            Assert.IsTrue(flag);

            mock.Verify(c => c.GetMaxMfn(It.IsAny<string>()), Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()),
                Times.Once());
        }

        [TestMethod]
        public void BatchRecordReader_WholeDatabase_3()
        {
            string database = "IBIS";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            bool flag = false;
            Action<BatchRecordReader> action = brr => flag = true;
            BatchRecordReader batch
                = (BatchRecordReader)BatchRecordReader.WholeDatabase
                (
                    connection,
                    database,
                    batchSize,
                    true,
                    action
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsRead);

            List<MarcRecord> list = batch.ReadAll();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(3, batch.RecordsRead);
            Assert.IsTrue(flag);

            mock.Verify(c => c.GetMaxMfn(It.IsAny<string>()), Times.Once);
            mock.Verify(c => c.ExecuteCommand(It.IsAny<AbstractCommand>()),
                Times.Once());
        }

    }
}
