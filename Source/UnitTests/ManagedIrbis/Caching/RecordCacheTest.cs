using System;

using ManagedIrbis;
using ManagedIrbis.Caching;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Caching
{
    [TestClass]
    public class RecordCacheTest
    {
        [TestMethod]
        public void RecordCache_Construciton_1()
        {
            IrbisConnection connection = new IrbisConnection();
            RecordCache cache = new RecordCache(connection);
            Assert.AreSame(connection, cache.Connection);
            Assert.AreEqual(0, cache.RequestCount);
            Assert.AreEqual(MenuCache.DefaultLifetime, cache.Lifetime);
            Assert.IsNotNull(cache.Requester);
        }

        [TestMethod]
        public void RecordCache_GetOrRequest_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            MarcRecord expected = new MarcRecord();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(expected);

            IIrbisConnection connection = mock.Object;
            RecordCache cache = new RecordCache(connection);
            MarcRecord actual = cache.GetOrRequest(1);
            Assert.AreSame(expected, actual);

            mock.Verify
                (
                    c => c.ReadRecord(It.IsAny<string>(),
                        It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void RecordCache_GetOrRequest_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            MarcRecord expected = new MarcRecord();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(expected);

            IIrbisConnection connection = mock.Object;
            RecordCache cache = new RecordCache(connection);
            MarcRecord record1 = cache.GetOrRequest(1);
            Assert.AreSame(expected, record1);
            MarcRecord record2 = cache.GetOrRequest(1);
            Assert.AreSame(expected, record2);

            mock.Verify
                (
                    c => c.ReadRecord(It.IsAny<string>(),
                        It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void RecordCache_GetOrRequest_3()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            MarcRecord expected = null;
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(expected);

            IIrbisConnection connection = mock.Object;
            RecordCache cache = new RecordCache(connection);
            MarcRecord actual = cache.GetOrRequest(1);
            Assert.IsNull(actual);

            mock.Verify
                (
                    c => c.ReadRecord(It.IsAny<string>(),
                        It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void MenuCache_GetOrRequest_4()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            MarcRecord expected = new MarcRecord();
            mock.Setup(c => c.ReadRecord(It.IsAny<string>(),
                    It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(expected);

            IIrbisConnection connection = mock.Object;
            RecordCache cache = new RecordCache(connection);
            MarcRecord record1 = cache.GetOrRequest(1);
            Assert.AreSame(expected, record1);

            cache.Clear();
            MarcRecord record2 = cache.GetOrRequest(1);
            Assert.AreSame(expected, record2);

            mock.Verify
                (
                    c => c.ReadRecord(It.IsAny<string>(),
                        It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()),
                    Times.Exactly(2)
                );
        }
    }
}
