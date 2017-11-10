using System;

using ManagedIrbis;
using ManagedIrbis.Caching;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Caching
{
    [TestClass]
    public class TextCacheTest
    {
        [TestMethod]
        public void TextCache_Construciton_1()
        {
            IrbisConnection connection = new IrbisConnection();
            TextCache cache = new TextCache(connection);
            Assert.AreSame(connection, cache.Connection);
            Assert.AreEqual(0, cache.RequestCount);
            Assert.AreEqual(MenuCache.DefaultLifetime, cache.Lifetime);
            Assert.IsNotNull(cache.Requester);
        }

        [TestMethod]
        public void TextCache_GetOrRequest_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "Hello world";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(expected);

            IIrbisConnection connection = mock.Object;
            TextCache cache = new TextCache(connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "text.txt"
                );
            string actual = cache.GetOrRequest(specification);
            Assert.AreEqual(expected, actual);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void TextCache_GetOrRequest_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "Hello world";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(expected);

            IIrbisConnection connection = mock.Object;
            TextCache cache = new TextCache(connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "text.txt"
                );
            string actual1 = cache.GetOrRequest(specification);
            Assert.AreEqual(expected, actual1);
            string actual2 = cache.GetOrRequest(specification);
            Assert.AreEqual(expected, actual2);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void TextCache_GetOrRequest_3()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = null;
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(expected);

            IIrbisConnection connection = mock.Object;
            TextCache cache = new TextCache(connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "text.txt"
                );
            string actual1 = cache.GetOrRequest(specification);
            Assert.IsNull(actual1);
            string actual2 = cache.GetOrRequest(specification);
            Assert.IsNull(actual2);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Exactly(2)
                );
        }

        [TestMethod]
        public void TextCache_GetOrRequest_4()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            string expected = "Hello world";
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(expected);

            IIrbisConnection connection = mock.Object;
            TextCache cache = new TextCache(connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "text.txt"
                );
            string actual1 = cache.GetOrRequest(specification);
            Assert.AreEqual(expected, actual1);

            cache.Clear();
            string actual2 = cache.GetOrRequest(specification);
            Assert.AreEqual(expected, actual2);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Exactly(2)
                );
        }
    }
}
