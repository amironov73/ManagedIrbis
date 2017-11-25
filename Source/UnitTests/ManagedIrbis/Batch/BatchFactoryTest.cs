using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using Moq;

using ConnectionFactory = ManagedIrbis.ConnectionFactory;

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class BatchFactoryTest
        : CommonBatchTest
    {
        [TestMethod]
        public void BatchFactory_GetBatchReader_1()
        {
            string kind = BatchFactory.Simple;
            string connectionString = "Connection String";
            string database = "IBIS";
            int[] range = {1, 2, 3};
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                BatchFactory factory = new BatchFactory();
                BatchRecordReader batch
                    = (BatchRecordReader) factory.GetBatchReader
                        (
                            kind,
                            connectionString,
                            database,
                            range
                        );
                Assert.AreSame(database, batch.Database);
                List<MarcRecord> records = batch.ReadAll();
                Assert.AreEqual(3, records.Count);
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }
        }

        [TestMethod]
        public void BatchFactory_GetBatchReader_2()
        {
            string kind = BatchFactory.Parallel;
            string connectionString = "Connection String";
            string database = "IBIS";
            int[] range = {1, 2, 3};
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                BatchFactory factory = new BatchFactory();
                ParallelRecordReader batch
                    = (ParallelRecordReader) factory.GetBatchReader
                        (
                            kind,
                            connectionString,
                            database,
                            range
                        );
                MarcRecord[] records = batch.ReadAll();
                Assert.AreEqual(3, records.Length);
                batch.Dispose();
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }
        }

        [TestMethod]
        public void BatchFactory_GetFormatter_1()
        {
            string kind = BatchFactory.Simple;
            string connectionString = "Connection String";
            string database = "IBIS";
            string format = "Format";
            int[] range = { 1, 2, 3 };
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                BatchFactory factory = new BatchFactory();
                BatchRecordFormatter batch
                    = (BatchRecordFormatter)factory.GetFormatter
                        (
                            kind,
                            connectionString,
                            database,
                            format,
                            range
                        );
                Assert.AreSame(database, batch.Database);
                List<string> list = batch.FormatAll();
                Assert.AreEqual(3, list.Count);
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }
        }

        [TestMethod]
        public void BatchFactory_GetFormatter_2()
        {
            string kind = BatchFactory.Parallel;
            string connectionString = "Connection String";
            string database = "IBIS";
            string format = "Format";
            int[] range = { 1, 2, 3 };
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                BatchFactory factory = new BatchFactory();
                ParallelRecordFormatter batch
                    = (ParallelRecordFormatter)factory.GetFormatter
                        (
                            kind,
                            connectionString,
                            database,
                            format,
                            range
                        );
                string[] lines = batch.FormatAll();
                Assert.AreEqual(3, lines.Length);
                batch.Dispose();
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }
        }
    }
}
