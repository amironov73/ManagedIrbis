using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

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
    }
}
