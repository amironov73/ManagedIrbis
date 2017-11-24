using System;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Batch;

using Moq;

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public class BatchAccessorTest
    {
        [NotNull]
        private Mock<IIrbisConnection> GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();

            return result;
        }

        [TestMethod]
        public void BatchAccessor_Construction_1()
        {
            Mock<IIrbisConnection> mock = GetMock();
            IIrbisConnection connection = mock.Object;
            BatchAccessor batch = new BatchAccessor(connection);
            Assert.AreSame(connection, batch.Connection);
        }
    }
}
