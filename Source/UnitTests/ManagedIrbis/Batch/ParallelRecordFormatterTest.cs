using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;

using Moq;

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class ParallelRecordFormatterTest
        : CommonBatchTest
    {
        [TestMethod]
        public void ParallelRecordFormatter_ReadAll_1()
        {
            string connectionString = "Connection String";
            int[] mfnList = {1, 2, 3};
            string format = "Format";
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
                ParallelRecordFormatter batch = new ParallelRecordFormatter
                    (
                        3,
                        connectionString,
                        mfnList,
                        format
                    );
                Assert.IsTrue(batch.Parallelism >= 2);
                string[] lines = batch.FormatAll();
                Assert.AreEqual(3, lines.Length);
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
