using System;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;

using Moq;

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class BatchRecordWriterTest
    {
        [NotNull]
        private Mock<IIrbisConnection> GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();

            // PopDatabase
            result.Setup(c => c.PopDatabase())
                .Returns("IBIS");

            // PushDatabase
            result.Setup(c => c.PushDatabase(It.IsAny<string>()))
                .Returns((string database) => database);

            // WriteRecords
            result.Setup(c => c.WriteRecords(It.IsAny<MarcRecord[]>(),
                    It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns((MarcRecord[] records, bool lockFlag,
                    bool actualize) => records);

            return result;
        }

        [TestMethod]
        public void BatchRecordWriter_Construction_1()
        {
            string database = "IBIS";
            int capacity = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordWriter batch = new BatchRecordWriter
                (
                    connection,
                    database,
                    capacity
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreEqual(capacity, batch.Capacity);
            Assert.IsTrue(batch.Actualize);
            Assert.AreEqual(0, batch.RecordsWritten);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordWriter_Construction_1a()
        {
            string database = "IBIS";
            int capacity = 0;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordWriter batch = new BatchRecordWriter
                (
                    connection,
                    database,
                    capacity
                );
        }

        [TestMethod]
        public void BatchRecordWriter_AddRange_1()
        {
            string database = "IBIS";
            int capacity = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            int count = 0;
            using (BatchRecordWriter batch = new BatchRecordWriter
                (
                    connection,
                    database,
                    capacity
                ))
            {
                batch.BatchWrite += (sender, args) =>
                {
                    BatchRecordWriter writer = (BatchRecordWriter) sender;
                    count = writer.RecordsWritten;
                };

                MarcRecord[] records =
                {
                    new MarcRecord(),
                    new MarcRecord(),
                    new MarcRecord(),
                };
                BatchRecordWriter actual = batch.AddRange(records);
                Assert.AreSame(batch, actual);
            }
            Assert.AreEqual(3, count);
            mock.Verify(c => c.WriteRecords(It.IsAny<MarcRecord[]>(),
                It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void BatchRecordWriter_Append_1()
        {
            string database = "IBIS";
            int capacity = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            bool flag  = false;
            using (BatchRecordWriter batch = new BatchRecordWriter
                (
                    connection,
                    database,
                    capacity
                ))
            {
                batch.BatchWrite += (sender, args) =>
                {
                    flag = true;
                };
                MarcRecord record = new MarcRecord();
                BatchRecordWriter actual = batch.Append(record);
                Assert.AreSame(batch, actual);
                Assert.AreEqual(0, batch.RecordsWritten);
            }
            Assert.IsTrue(flag);
            mock.Verify(c => c.WriteRecords(It.IsAny<MarcRecord[]>(),
                It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void BatchRecordWriter_Append_2()
        {
            string database = "IBIS";
            int capacity = 2;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            bool flag  = false;
            using (BatchRecordWriter batch = new BatchRecordWriter
                (
                    connection,
                    database,
                    capacity
                ))
            {
                batch.BatchWrite += (sender, args) =>
                {
                    flag = true;
                };
                MarcRecord record = new MarcRecord();
                BatchRecordWriter actual = batch.Append(record);
                Assert.AreSame(batch, actual);
                Assert.AreEqual(0, batch.RecordsWritten);
                batch.Append(record);
                Assert.AreSame(batch, actual);
                Assert.AreEqual(2, batch.RecordsWritten);
            }
            Assert.IsTrue(flag);
            mock.Verify(c => c.WriteRecords(It.IsAny<MarcRecord[]>(),
                It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void BatchRecordWriter_Dispose_1()
        {
            string database = "IBIS";
            int capacity = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            bool flag = false;
            using (BatchRecordWriter batch = new BatchRecordWriter
                (
                    connection,
                    database,
                    capacity
                ))
            {
                batch.BatchWrite += (sender, args) =>
                {
                    flag = true;
                };
            }

            Assert.IsFalse(flag);
            mock.Verify(c => c.WriteRecords(It.IsAny<MarcRecord[]>(),
                It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public void BatchRecordWriter_Flush_1()
        {
            string database = "IBIS";
            int capacity = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            bool flag = false;
            using (BatchRecordWriter batch = new BatchRecordWriter
                (
                    connection,
                    database,
                    capacity
                ))
            {
                batch.BatchWrite += (sender, args) =>
                {
                    flag = true;
                };

                batch.Flush();
                Assert.IsFalse(flag);
            }

            Assert.IsFalse(flag);
            mock.Verify(c => c.WriteRecords(It.IsAny<MarcRecord[]>(),
                It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }
    }
}
