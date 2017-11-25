using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;

using Moq;

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class ParallelRecordReaderTest
        : CommonBatchTest
    {
        [TestMethod]
        public void ParallelRecordReader_ReadAll_1()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            string previousConnectionString
                = IrbisConnectionUtility.DefaultConnectionString;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                IrbisConnectionUtility.DefaultConnectionString
                    = "Connection String";
                ParallelRecordReader batch = new ParallelRecordReader();
                Assert.IsTrue(batch.Parallelism >= 2);
                MarcRecord[] records = batch.ReadAll();
                Assert.AreEqual(3, records.Length);
                batch.Dispose();
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
                IrbisConnectionUtility.DefaultConnectionString
                    = previousConnectionString;
            }
        }

        [TestMethod]
        public void ParallelRecordReader_ReadAll_2()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            string previousConnectionString
                = IrbisConnectionUtility.DefaultConnectionString;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                IrbisConnectionUtility.DefaultConnectionString
                    = "Connection String";
                ParallelRecordReader batch = new ParallelRecordReader(3);
                Assert.IsTrue(batch.Parallelism >= 2);
                MarcRecord[] records = batch.ReadAll();
                Assert.AreEqual(3, records.Length);
                batch.Dispose();
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
                IrbisConnectionUtility.DefaultConnectionString
                    = previousConnectionString;
            }
        }

        [TestMethod]
        public void ParallelRecordReader_ReadAll_3()
        {
            string connectionString = "Connection String";
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            string previousConnectionString
                = IrbisConnectionUtility.DefaultConnectionString;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                IrbisConnectionUtility.DefaultConnectionString
                    = "Connection String";
                ParallelRecordReader batch = new ParallelRecordReader
                    (
                        3,
                        connectionString
                    );
                Assert.IsTrue(batch.Parallelism >= 2);
                Assert.AreSame(connectionString, batch.ConnectionString);
                MarcRecord[] records = batch.ReadAll();
                Assert.AreEqual(3, records.Length);
                batch.Dispose();
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
                IrbisConnectionUtility.DefaultConnectionString
                    = previousConnectionString;
            }
        }

        [TestMethod]
        public void ParallelRecordReader_ReadAll_4()
        {
            string connectionString = "Connection String";
            int[] mfnList = {1, 2, 3};
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            string previousConnectionString
                = IrbisConnectionUtility.DefaultConnectionString;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => connection;
                IrbisConnectionUtility.DefaultConnectionString
                    = "Connection String";
                ParallelRecordReader batch = new ParallelRecordReader
                    (
                        3,
                        connectionString,
                        mfnList
                    );
                Assert.IsTrue(batch.Parallelism >= 2);
                Assert.AreSame(connectionString, batch.ConnectionString);
                MarcRecord[] records = batch.ReadAll();
                Assert.AreEqual(3, records.Length);
                batch.Dispose();
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
                IrbisConnectionUtility.DefaultConnectionString
                    = previousConnectionString;
            }
        }
    }
}
