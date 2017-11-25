using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

using Moq;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable MustUseReturnValue

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class ConnectionFactoryTest
    {
        [TestMethod]
        public void ConnectionFactory_CreateConnection_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection expected = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => expected;
                IIrbisConnection actual
                    = ConnectionFactory.CreateConnection("Connection String");
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConnectionFactory_CreateConnection_2()
        {
            string connectionString = null;
            ConnectionFactory.CreateConnection(connectionString);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ConnectionFactory_CreateConnection_3()
        {
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            try
            {
                ConnectionFactory.ConnectionCreator = null;
                ConnectionFactory.CreateConnection("Connection String");
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }
        }

        [TestMethod]
        public void ConnectionFactory_RestoreDefaults_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection expected = mock.Object;
            Func<string, IIrbisConnection> previousCreator
                = ConnectionFactory.ConnectionCreator;
            try
            {
                ConnectionFactory.ConnectionCreator = cs => expected;
                ConnectionFactory.RestoreDefaults();
                Assert.AreEqual(previousCreator, ConnectionFactory.ConnectionCreator);
            }
            finally
            {
                ConnectionFactory.ConnectionCreator = previousCreator;
            }
        }
    }
}
