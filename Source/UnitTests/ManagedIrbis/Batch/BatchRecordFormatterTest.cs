using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;

using Moq;

// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class BatchRecordFormatterTest
    {
        [NotNull]
        private Mock<IIrbisConnection> GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();

            // FormatRecords
            result.Setup(c => c.FormatRecords(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<IEnumerable<int>>()))
                .Returns
                    (
                        (string db, string format, IEnumerable<int> mfns) =>
                        {
                            List<string> list = new List<string>();
                            foreach (int mfn in mfns)
                            {
                                string text = string.Format
                                    (
                                        "{0} {1} {2}",
                                        format,
                                        db,
                                        mfn
                                    );
                                list.Add(text);
                            }

                            return list.ToArray();
                        }
                    );

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
                .Returns(new[] { 1, 2, 3 });

            return result;
        }

        [TestMethod]
        public void BatchRecordFormatter_Construction_1()
        {
            string database = "IBIS";
            string format = "Format";
            int batchSize = 500;
            int[] range = { 1, 2, 3 };
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordFormatter batch = new BatchRecordFormatter
                (
                    connection,
                    database,
                    format,
                    batchSize,
                    range
                );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreSame(format, batch.Format);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(0, batch.RecordsFormatted);
            Assert.AreEqual(range.Length, batch.TotalRecords);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordFormatter_Construction_1a()
        {
            string database = "IBIS";
            string format = "Format";
            int batchSize = 0;
            int[] range = { 1, 2, 3 };
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            new BatchRecordFormatter
                (
                    connection,
                    database,
                    format,
                    batchSize,
                    range
                );
        }

        [TestMethod]
        public void BatchRecordFormatter_Construction_2()
        {
            string connectionString = "Connection String";
            string database = "IBIS";
            string format = "Format";
            int batchSize = 500;
            int[] range = { 1, 2, 3 };
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                BatchRecordFormatter batch = new BatchRecordFormatter
                    (
                        connectionString,
                        database,
                        format,
                        batchSize,
                        range
                    );
                Assert.AreSame(connection, batch.Connection);
                Assert.AreSame(database, batch.Database);
                Assert.AreSame(format, batch.Format);
                Assert.AreEqual(batchSize, batch.BatchSize);
                Assert.AreEqual(0, batch.RecordsFormatted);
                Assert.AreEqual(range.Length, batch.TotalRecords);
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordFormatter_Construction_2a()
        {
            string connectionString = "Connection String";
            string database = "IBIS";
            string format = "Format";
            int batchSize = 0;
            int[] range = { 1, 2, 3 };
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                new BatchRecordFormatter
                    (
                        connectionString,
                        database,
                        format,
                        batchSize,
                        range
                    );
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }
        }


        [TestMethod]
        public void BatchRecordFormatter_Interval_1()
        {
            string database = "IBIS";
            int batchSize = 500;
            string format = "Format";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordFormatter batch
                = (BatchRecordFormatter)BatchRecordFormatter.Interval
                    (
                        connection,
                        database,
                        format,
                        1,
                        3,
                        batchSize
                    );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreSame(format, batch.Format);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsFormatted);

            mock.Verify(c => c.GetMaxMfn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordFormatter_Interval_1a()
        {
            string database = "IBIS";
            int batchSize = 0;
            string format = "Format";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordFormatter batch
                = (BatchRecordFormatter)BatchRecordFormatter.Interval
                    (
                        connection,
                        database,
                        format,
                        1,
                        3,
                        batchSize
                    );
        }

        [TestMethod]
        public void BatchRecordFormatter_Search_1()
        {
            string database = "IBIS";
            string format = "Format";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordFormatter batch
                = (BatchRecordFormatter)BatchRecordFormatter.Search
                    (
                        connection,
                        database,
                        format,
                        "searchQuery",
                        batchSize
                    );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreSame(format, batch.Format);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsFormatted);

            mock.Verify(c => c.Search(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordFormatter_Search_1a()
        {
            string database = "IBIS";
            string format = "Format";
            int batchSize = 0;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordFormatter batch
                = (BatchRecordFormatter)BatchRecordFormatter.Search
                    (
                        connection,
                        database,
                        format,
                        "searchQuery",
                        batchSize
                    );
        }

        [TestMethod]
        public void BatchRecordFormatter_WholeDatabase_1()
        {
            string database = "IBIS";
            string format = "Format";
            int batchSize = 500;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordFormatter batch
                = (BatchRecordFormatter)BatchRecordFormatter.WholeDatabase
                    (
                        connection,
                        database,
                        format,
                        batchSize
                    );
            Assert.AreSame(connection, batch.Connection);
            Assert.AreSame(database, batch.Database);
            Assert.AreSame(format, batch.Format);
            Assert.AreEqual(batchSize, batch.BatchSize);
            Assert.AreEqual(3, batch.TotalRecords);
            Assert.AreEqual(0, batch.RecordsFormatted);

            mock.Verify(c => c.GetMaxMfn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchRecordFormatter_WholeDatabase_1a()
        {
            string database = "IBIS";
            string format = "Format";
            int batchSize = 0;
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordFormatter batch
                = (BatchRecordFormatter)BatchRecordFormatter.WholeDatabase
                    (
                        connection,
                        database,
                        format,
                        batchSize
                    );
        }

        [TestMethod]
        public void BatchRecordFormatter_FormatAll_1()
        {
            string database = "IBIS";
            int batchSize = 500;
            string format = "Format";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchRecordFormatter batch
                = (BatchRecordFormatter)BatchRecordFormatter.Interval
                (
                    connection,
                    database,
                    format,
                    1,
                    3,
                    batchSize
                );
            List<string> formatted = batch.FormatAll();
            Assert.AreEqual(3, batch.RecordsFormatted);
            Assert.AreEqual(3, formatted.Count);
            Assert.AreEqual("Format IBIS 1", formatted[0]);
            Assert.AreEqual("Format IBIS 2", formatted[1]);
            Assert.AreEqual("Format IBIS 3", formatted[2]);

            mock.Verify(c => c.GetMaxMfn(It.IsAny<string>()), Times.Once);
            mock.Verify(c => c.FormatRecords(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<IEnumerable<int>>()),
                Times.Once);
        }

        [TestMethod]
        public void BatchRecordFormatter_FormatAll_2()
        {
            string connectionString = "Connection String";
            string database = "IBIS";
            int batchSize = 500;
            string format = "Format";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            ConnectionFactory.ConnectionCreator = cs => connection;
            try
            {
                BatchRecordFormatter batch = new BatchRecordFormatter
                    (
                        connectionString,
                        database,
                        format,
                        batchSize,
                        Enumerable.Range(1, 3)
                    );
                List<string> formatted = batch.FormatAll();
                Assert.AreEqual(3, batch.RecordsFormatted);
                Assert.AreEqual(3, formatted.Count);
                Assert.AreEqual("Format IBIS 1", formatted[0]);
                Assert.AreEqual("Format IBIS 2", formatted[1]);
                Assert.AreEqual("Format IBIS 3", formatted[2]);
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }

            mock.Verify(c => c.GetMaxMfn(It.IsAny<string>()), Times.Never);
            mock.Verify(c => c.FormatRecords(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<IEnumerable<int>>()),
                Times.Once);
        }
    }
}
