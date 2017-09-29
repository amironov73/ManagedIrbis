using System;
using System.IO;

using AM.Runtime;

using JetBrains.Annotations;

using ManagedIrbis;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    class MySocket
        : AbstractClientSocket
    {
        public MySocket([NotNull] IrbisConnection connection)
            : base(connection)
        {
        }

        public override void AbortRequest()
        {
            throw new NotImplementedException();
        }

        public override byte[] ExecuteRequest(byte[] request)
        {
            throw new NotImplementedException();
        }
    }

    class MyEngine
        : AbstractEngine
    {
        public MyEngine
            (
                [NotNull] IrbisConnection connection,
                [CanBeNull] AbstractEngine nestedEngine
            )
            : base(connection, nestedEngine)
        {
        }
    }

    class MyFactory
        : CommandFactory
    {
        public MyFactory
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }
    }

    [TestClass]
    public class ConnectionSettingsTest
    {
        const string GoodConnectionString
            = "host=127.0.0.1;port=5555;user=john galt;" +
              "pwd=who is;db=NODB;arm=A";

        const string BadConnectionString
            = "host=127.0.0.1;port=5555;user=john galt;" +
              "pwd=who is;db=NODB;arm=A;unknown=nothing";

        [TestMethod]
        public void ConnectionSettings_ParseConnectionString_1()
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString(GoodConnectionString);

            Assert.AreEqual("127.0.0.1", settings.Host);
            Assert.AreEqual(5555, settings.Port);
            Assert.AreEqual("john galt", settings.Username);
            Assert.AreEqual("who is", settings.Password);
            Assert.AreEqual("NODB", settings.Database);
            Assert.AreEqual
                (
                    IrbisWorkstation.Administrator,
                    settings.Workstation
                );
        }

        [TestMethod]
        public void ConnectionSettings_ParseConnectionString_2()
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString
                (
                    GoodConnectionString + ";smart=yes;"
                );

            Assert.AreEqual("127.0.0.1", settings.Host);
            Assert.AreEqual(5555, settings.Port);
            Assert.AreEqual("john galt", settings.Username);
            Assert.AreEqual("who is", settings.Password);
            Assert.AreEqual("NODB", settings.Database);
            Assert.AreEqual("yes", settings.Smart);
            Assert.AreEqual
                (
                    IrbisWorkstation.Administrator,
                    settings.Workstation
                );
        }

        [TestMethod]
        public void ConnectionSettings_ParseConnectionString_3()
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString
                (
                    GoodConnectionString + ";broken=yes;"
                );

            Assert.AreEqual("127.0.0.1", settings.Host);
            Assert.AreEqual(5555, settings.Port);
            Assert.AreEqual("john galt", settings.Username);
            Assert.AreEqual("who is", settings.Password);
            Assert.AreEqual("NODB", settings.Database);
            Assert.AreEqual("yes", settings.Broken);
            Assert.AreEqual
                (
                    IrbisWorkstation.Administrator,
                    settings.Workstation
                );
        }

        [TestMethod]
        public void ConnectionSettings_ParseConnectionString_4()
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString
                (
                    GoodConnectionString + ";slow=yes;"
                );

            Assert.AreEqual("127.0.0.1", settings.Host);
            Assert.AreEqual(5555, settings.Port);
            Assert.AreEqual("john galt", settings.Username);
            Assert.AreEqual("who is", settings.Password);
            Assert.AreEqual("NODB", settings.Database);
            Assert.AreEqual("yes", settings.Slow);
            Assert.AreEqual
                (
                    IrbisWorkstation.Administrator,
                    settings.Workstation
                );
        }

        [TestMethod]
        public void ConnectionSettings_ParseConnectionString_5()
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString
                (
                    GoodConnectionString + ";provider=connected;"
                );

            Assert.AreEqual("127.0.0.1", settings.Host);
            Assert.AreEqual(5555, settings.Port);
            Assert.AreEqual("john galt", settings.Username);
            Assert.AreEqual("who is", settings.Password);
            Assert.AreEqual("NODB", settings.Database);
            Assert.AreEqual
                (
                    IrbisWorkstation.Administrator,
                    settings.Workstation
                );
        }

        [TestMethod]
        public void ConnectionSettings_ParseConnectionString_6()
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString
            (
                GoodConnectionString + ";cgi=/irbis/webcgi;"
            );

            Assert.AreEqual("127.0.0.1", settings.Host);
            Assert.AreEqual(5555, settings.Port);
            Assert.AreEqual("john galt", settings.Username);
            Assert.AreEqual("who is", settings.Password);
            Assert.AreEqual("NODB", settings.Database);
            Assert.AreEqual("/irbis/webcgi", settings.WebCgi);
            Assert.AreEqual
                (
                    IrbisWorkstation.Administrator,
                    settings.Workstation
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConnectionSettings_ParseConnectionString_Exception_1()
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString(BadConnectionString);

        }

        [TestMethod]
        public void ConnectionSettings_Encode_1()
        {
            ConnectionSettings settings = new ConnectionSettings
            {
                Host = "127.0.0.1",
                Port = 5555,
                Username = "john galt",
                Password = "who is",
                Database = "NODB",
                Workstation = IrbisWorkstation.Administrator
            };
            string actual = settings.Encode();
            const string expected = "host=127.0.0.1;port=5555;"
                + "database=NODB;username=john galt;password=who is;"
                + "workstation=A;";

            Assert.AreEqual(expected, actual);

            actual = settings.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConnectionSettings_Clone_1()
        {
            ConnectionSettings expected = new ConnectionSettings
            {
                Host = "127.0.0.1",
                Port = 5555,
                Username = "john galt",
                Password = "who is",
                Database = "NODB",
                Workstation = IrbisWorkstation.Administrator
            };
            ConnectionSettings actual = expected.Clone();

            Assert.AreEqual(expected.Host, actual.Host);
            Assert.AreEqual(expected.Port, actual.Port);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.Workstation, actual.Workstation);
        }

        private void _TestSerialization
            (
                ConnectionSettings expected
            )
        {
            byte[] bytes = expected.SaveToMemory();

            ConnectionSettings actual = bytes
                .RestoreObjectFromMemory<ConnectionSettings>();

            Assert.AreEqual(expected.Host, actual.Host);
            Assert.AreEqual(expected.Port, actual.Port);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.Workstation, actual.Workstation);
        }

        [TestMethod]
        public void ConnectionSettings_Serialization_1()
        {
            ConnectionSettings settings = new ConnectionSettings();
            _TestSerialization(settings);

            settings.ParseConnectionString(GoodConnectionString);
            _TestSerialization(settings);
        }

        [TestMethod]
        public void ConnectionSettings_Encrypt_1()
        {
            ConnectionSettings expected = new ConnectionSettings()
                .ParseConnectionString(GoodConnectionString);

            string text = expected.Encrypt();

            ConnectionSettings actual = ConnectionSettings
                .Decrypt(text);

            Assert.AreEqual(expected.Host, actual.Host);
            Assert.AreEqual(expected.Port, actual.Port);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.Workstation, actual.Workstation);
        }

        [TestMethod]
        public void ConnectionSettings_ApplyToConnection_1()
        {
            ConnectionSettings expected = new ConnectionSettings()
                .ParseConnectionString(GoodConnectionString);
            IrbisConnection connection = new IrbisConnection();
            expected.ApplyToConnection(connection);

            Assert.AreEqual(expected.Host, connection.Host);
            Assert.AreEqual(expected.Port, connection.Port);
            Assert.AreEqual(expected.Username, connection.Username);
            Assert.AreEqual(expected.Password, connection.Password);
            Assert.AreEqual(expected.Database, connection.Database);
            Assert.AreEqual(expected.Workstation, connection.Workstation);
        }

        [TestMethod]
        public void ConnectionSettings_ApplyToConnection_2()
        {
            ConnectionSettings expected = new ConnectionSettings()
                .ParseConnectionString
                    (
                        GoodConnectionString + ";smart=yes;"
                    );
            IrbisConnection connection = new IrbisConnection();
            expected.ApplyToConnection(connection);

            Assert.AreEqual(expected.Host, connection.Host);
            Assert.AreEqual(expected.Port, connection.Port);
            Assert.AreEqual(expected.Username, connection.Username);
            Assert.AreEqual(expected.Password, connection.Password);
            Assert.AreEqual(expected.Database, connection.Database);
            Assert.AreEqual(expected.Workstation, connection.Workstation);
        }

        [TestMethod]
        public void ConnectionSettings_ApplyToConnection_3()
        {
            ConnectionSettings expected = new ConnectionSettings()
                .ParseConnectionString
                    (
                        GoodConnectionString + ";broken=yes;"
                    );
            IrbisConnection connection = new IrbisConnection();
            expected.ApplyToConnection(connection);

            Assert.AreEqual(expected.Host, connection.Host);
            Assert.AreEqual(expected.Port, connection.Port);
            Assert.AreEqual(expected.Username, connection.Username);
            Assert.AreEqual(expected.Password, connection.Password);
            Assert.AreEqual(expected.Database, connection.Database);
            Assert.AreEqual(expected.Workstation, connection.Workstation);
        }

        [TestMethod]
        public void ConnectionSettings_ApplyToConnection_3a()
        {
            ConnectionSettings expected = new ConnectionSettings()
                .ParseConnectionString
                    (
                        GoodConnectionString + ";broken=0.5;"
                    );
            IrbisConnection connection = new IrbisConnection();
            expected.ApplyToConnection(connection);

            Assert.AreEqual(expected.Host, connection.Host);
            Assert.AreEqual(expected.Port, connection.Port);
            Assert.AreEqual(expected.Username, connection.Username);
            Assert.AreEqual(expected.Password, connection.Password);
            Assert.AreEqual(expected.Database, connection.Database);
            Assert.AreEqual(expected.Workstation, connection.Workstation);
        }

        [TestMethod]
        public void ConnectionSettings_ApplyToConnection_4()
        {
            ConnectionSettings expected = new ConnectionSettings()
                .ParseConnectionString
                    (
                        GoodConnectionString + ";slow=yes;"
                    );
            IrbisConnection connection = new IrbisConnection();
            expected.ApplyToConnection(connection);

            Assert.AreEqual(expected.Host, connection.Host);
            Assert.AreEqual(expected.Port, connection.Port);
            Assert.AreEqual(expected.Username, connection.Username);
            Assert.AreEqual(expected.Password, connection.Password);
            Assert.AreEqual(expected.Database, connection.Database);
            Assert.AreEqual(expected.Workstation, connection.Workstation);
        }

        [TestMethod]
        public void ConnectionSettings_ApplyToConnection_4a()
        {
            ConnectionSettings expected = new ConnectionSettings()
                .ParseConnectionString
                    (
                        GoodConnectionString + ";slow=1000;"
                    );
            IrbisConnection connection = new IrbisConnection();
            expected.ApplyToConnection(connection);

            Assert.AreEqual(expected.Host, connection.Host);
            Assert.AreEqual(expected.Port, connection.Port);
            Assert.AreEqual(expected.Username, connection.Username);
            Assert.AreEqual(expected.Password, connection.Password);
            Assert.AreEqual(expected.Database, connection.Database);
            Assert.AreEqual(expected.Workstation, connection.Workstation);
        }

        [TestMethod]
        public void ConnectionSettings_SocketTypeName_1()
        {
            string connectionString = string.Format
                (
                    "socket={0};",
                    typeof(MySocket).AssemblyQualifiedName
                );

            ConnectionSettings settings = new ConnectionSettings()
                .ParseConnectionString(connectionString);
            IrbisConnection connection = new IrbisConnection();
            settings.ApplyToConnection(connection);

            Assert.AreEqual
                (
                    typeof(MySocket),
                    connection.Socket.GetType()
                );
        }

        [TestMethod]
        public void ConnectionSettings_EngineTypeName_1()
        {
            string connectionString = string.Format
                (
                    "engine={0};",
                    typeof(MyEngine).AssemblyQualifiedName
                );

            ConnectionSettings settings = new ConnectionSettings()
                .ParseConnectionString(connectionString);
            IrbisConnection connection = new IrbisConnection();
            settings.ApplyToConnection(connection);

            Assert.AreEqual
                (
                    typeof(MyEngine),
                    connection.Executive.GetType()
                );
        }

        [TestMethod]
        public void ConnectionSettings_FactoryTypeName_1()
        {
            string connectionString = string.Format
                (
                    "factory={0};",
                    typeof(MyFactory).AssemblyQualifiedName
            );

            ConnectionSettings settings = new ConnectionSettings()
                .ParseConnectionString(connectionString);
            IrbisConnection connection = new IrbisConnection();
            settings.ApplyToConnection(connection);

            Assert.AreEqual
                (
                    typeof(MyFactory),
                    connection.CommandFactory.GetType()
                );
        }

        [TestMethod]
        public void ConnectionSettings_UserData_1()
        {
            string connectionString = "userdata=hello;";

            ConnectionSettings settings = new ConnectionSettings()
                .ParseConnectionString(connectionString);
            IrbisConnection connection = new IrbisConnection();
            settings.ApplyToConnection(connection);

            Assert.AreEqual
                (
                    "hello",
                    connection.UserData
                );
        }

        [TestMethod]
        public void ConnectionSettings_NetworkLogging_1()
        {
            string logPath = Path.Combine
                (
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString()
                )
                .Replace('\\', '/');
            Directory.CreateDirectory(logPath);
            try
            {

                string connectionString = "log="
                                          + logPath
                                          + ";";

                ConnectionSettings settings = new ConnectionSettings()
                    .ParseConnectionString(connectionString);
                IrbisConnection connection = new IrbisConnection();
                settings.ApplyToConnection(connection);

                Assert.AreEqual
                (
                    typeof(LoggingClientSocket),
                    connection.Socket.GetType()
                );
            }
            finally
            {
                Directory.Delete(logPath);
            }
        }

        [TestMethod]
        public void ConnectionSettings_RetryCount_1()
        {
            string connectionString = "retry=3;";

            ConnectionSettings settings = new ConnectionSettings()
                .ParseConnectionString(connectionString);
            IrbisConnection connection = new IrbisConnection();
            settings.ApplyToConnection(connection);

            Assert.AreEqual
                (
                    typeof(RetryClientSocket),
                    connection.Socket.GetType()
                );
        }

        [TestMethod]
        public void ConnectionSettings_FromConnection_1()
        {
            string source = "host=127.0.0.1;port=5555;"
                + "database=NODB;username=john galt;password=who is;"
                + "workstation=A;"
                + "engine=" + typeof(MyEngine).AssemblyQualifiedName + ";"
                + "factory=" + typeof(MyFactory).AssemblyQualifiedName + ";"
                + "retry=3;data=hello;";

            ConnectionSettings expected = new ConnectionSettings()
                .ParseConnectionString(source);
            IrbisConnection connection = new IrbisConnection();
            expected.ApplyToConnection(connection);

            ConnectionSettings actual
                = ConnectionSettings.FromConnection(connection);

            Assert.AreEqual(expected.Host, connection.Host);
            Assert.AreEqual(expected.Port, connection.Port);
            Assert.AreEqual(expected.Username, connection.Username);
            Assert.AreEqual(expected.Password, connection.Password);
            Assert.AreEqual(expected.Database, connection.Database);
            Assert.AreEqual(expected.Workstation, connection.Workstation);

            string target = actual.Encode();
            Assert.AreEqual(source, target);
        }

        [TestMethod]
        public void ConnectionSettings_FromConnection_2()
        {
            string logPath = Path.Combine
                (
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString()
                )
                .Replace('\\', '/');
            Directory.CreateDirectory(logPath);

            try
            {
                string source = "host=127.0.0.1;port=5555;"
                    + "database=NODB;username=john galt;password=who is;"
                    + "workstation=A;"
                    + "log=" + logPath + ";";

                ConnectionSettings expected = new ConnectionSettings()
                    .ParseConnectionString(source);
                IrbisConnection connection = new IrbisConnection();
                expected.ApplyToConnection(connection);

                ConnectionSettings actual
                    = ConnectionSettings.FromConnection(connection);

                Assert.AreEqual(expected.Host, connection.Host);
                Assert.AreEqual(expected.Port, connection.Port);
                Assert.AreEqual(expected.Username, connection.Username);
                Assert.AreEqual(expected.Password, connection.Password);
                Assert.AreEqual(expected.Database, connection.Database);
                Assert.AreEqual(expected.Workstation, connection.Workstation);

                string target = actual.Encode();
                Assert.AreEqual(source, target);
            }
            finally
            {
                Directory.Delete(logPath);
            }
        }

        [TestMethod]
        public void ConnectionSettings_FromConnection_3()
        {
            string source = "host=127.0.0.1;port=5555;"
                + "database=NODB;username=john galt;password=who is;"
                + "workstation=A;"
                + "socket=" + typeof(MySocket).AssemblyQualifiedName + ";";

            ConnectionSettings expected = new ConnectionSettings()
                .ParseConnectionString(source);
            IrbisConnection connection = new IrbisConnection();
            expected.ApplyToConnection(connection);

            ConnectionSettings actual
                = ConnectionSettings.FromConnection(connection);

            Assert.AreEqual(expected.Host, connection.Host);
            Assert.AreEqual(expected.Port, connection.Port);
            Assert.AreEqual(expected.Username, connection.Username);
            Assert.AreEqual(expected.Password, connection.Password);
            Assert.AreEqual(expected.Database, connection.Database);
            Assert.AreEqual(expected.Workstation, connection.Workstation);

            string target = actual.Encode();
            Assert.AreEqual(source, target);
        }

        [TestMethod]
        public void ConnectionSettings_GetMissingElements_1()
        {
            ConnectionSettings settings = new ConnectionSettings();

            Assert.AreEqual
                (
                    ConnectionElement.Username|ConnectionElement.Password,
                    settings.GetMissingElements()
                );

            settings.Host = null;
            settings.Port = 0;
            settings.Workstation = IrbisWorkstation.None;
            Assert.AreEqual
                (
                    ConnectionElement.Host | ConnectionElement.Port
                    | ConnectionElement.Workstation
                    | ConnectionElement.Username | ConnectionElement.Password,
                    settings.GetMissingElements()
                );
        }

        [TestMethod]
        public void ConnectionSettings_Verify_1()
        {
            ConnectionSettings settings = new ConnectionSettings();
            Assert.IsFalse(settings.Verify(false));

            settings = settings.ParseConnectionString(GoodConnectionString);
            Assert.IsTrue(settings.Verify(false));
        }
    }
}
