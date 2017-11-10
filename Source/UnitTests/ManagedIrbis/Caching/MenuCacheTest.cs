using System;

using ManagedIrbis;
using ManagedIrbis.Caching;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Caching
{
    [TestClass]
    public class MenuCacheTest
    {
        [TestMethod]
        public void MenuCache_Construciton_1()
        {
            IrbisConnection connection = new IrbisConnection();
            MenuCache cache = new MenuCache(connection);
            Assert.AreSame(connection, cache.Connection);
            Assert.AreEqual(0, cache.RequestCount);
            Assert.AreEqual(MenuCache.DefaultLifetime, cache.Lifetime);
            Assert.IsNotNull(cache.Requester);
        }

        [TestMethod]
        public void MenuCache_GetOrRequest_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns("Key1\r\nValue1\r\nKey2\r\nValue2\r\n*****");

            IIrbisConnection connection = mock.Object;
            MenuCache cache = new MenuCache(connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "menu.mnu"
                );
            MenuFile menu = cache.GetOrRequest(specification);
            Assert.AreEqual(2, menu.Entries.Count);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void MenuCache_GetOrRequest_2()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns("Key1\r\nValue1\r\nKey2\r\nValue2\r\n*****");

            IIrbisConnection connection = mock.Object;
            MenuCache cache = new MenuCache(connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "menu.mnu"
                );
            MenuFile menu1 = cache.GetOrRequest(specification);
            Assert.AreEqual(2, menu1.Entries.Count);
            MenuFile menu2 = cache.GetOrRequest(specification);
            Assert.AreSame(menu1, menu2);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void MenuCache_GetOrRequest_3()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(string.Empty);

            IIrbisConnection connection = mock.Object;
            MenuCache cache = new MenuCache(connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "menu.mnu"
                );
            MenuFile menu = cache.GetOrRequest(specification);
            Assert.IsNull(menu);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Once
                );
        }

        [TestMethod]
        public void MenuCache_GetOrRequest_4()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns("Key1\r\nValue1\r\nKey2\r\nValue2\r\n*****");

            IIrbisConnection connection = mock.Object;
            MenuCache cache = new MenuCache(connection);
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "menu.mnu"
                );
            MenuFile menu1 = cache.GetOrRequest(specification);
            Assert.AreEqual(2, menu1.Entries.Count);

            cache.Clear();
            MenuFile menu2 = cache.GetOrRequest(specification);
            Assert.AreEqual(2, menu2.Entries.Count);

            mock.Verify
                (
                    c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                    Times.Exactly(2)
                );
        }

    }
}
