using ManagedIrbis;
using ManagedIrbis.Magazines;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Magazines
{
    [TestClass]
    public class BindingManagerTest
    {
        [TestMethod]
        public void BindingManager_Construction_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;

            BindingManager manager = new BindingManager(connection);
            Assert.AreSame(connection, manager.Connection);
        }
    }
}
