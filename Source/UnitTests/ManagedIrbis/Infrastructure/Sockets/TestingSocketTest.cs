using System;
using System.IO;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Infrastructure.Sockets;

using Moq;

namespace UnitTests.ManagedIrbis.Infrastructure.Sockets
{
    [TestClass]
    public class TestingSocketTest
    {
        [NotNull]
        private Mock<IIrbisConnection> _GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();

            return result;
        }

        [TestMethod]
        public void TestingSocket_Construction_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            TestingSocket socket = new TestingSocket(connection);
            Assert.IsFalse(socket.RequiresConnection);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestingSocket_AbortRequest_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            TestingSocket socket = new TestingSocket(connection);
            socket.AbortRequest();
        }

        [TestMethod]
        public void TestingSocket_ExecuteRequest_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            byte[] expected = new byte[0];
            TestingSocket socket = new TestingSocket(connection)
            {
                Response = expected,
                ExpectedRequest = expected
            };
            byte[] request = new byte[0];
            byte[] actual = socket.ExecuteRequest(request);
            CollectionAssert.AreEqual(expected, actual);
            CollectionAssert.AreEqual(request, socket.ActualRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestingSocket_ExecuteRequest_2()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            byte[] expected = { 1 };
            TestingSocket socket = new TestingSocket(connection)
            {
                Response = expected,
                ExpectedRequest = expected
            };
            byte[] request = { 2 };
            socket.ExecuteRequest(request);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestingSocket_ExecuteRequest_3()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            byte[] expected = { 1 };
            TestingSocket socket = new TestingSocket(connection)
            {
                ExpectedRequest = expected
            };
            byte[] request = { 1 };
            socket.ExecuteRequest(request);
        }
    }
}
