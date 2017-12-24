using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

namespace UnitTests.ManagedIrbis.Infrastructure
{
    [TestClass]
    public class ClientQueryTest
    {
        private ClientQuery _GetClientQuery()
        {
            IrbisConnection connection = new IrbisConnection();

            ClientQuery result = new ClientQuery (connection)
            {
                CommandCode = CommandCode.Nop,
                Workstation = IrbisWorkstation.Cataloger,
                ClientID = 123456,
                CommandNumber = 123,
                UserLogin = "логин",
                UserPassword = "пароль"
            };

            result
                .AddAnsi("Строка ANSI")
                .AddUtf8("Строка UTF8");

            return result;
        }

        [TestMethod]
        public void TestClientQuery_Constructor()
        {
            ClientQuery query = _GetClientQuery();

            Assert.AreEqual(2, query.Arguments.Count);
            Assert.IsNotNull(query.CommandCode);
            Assert.IsNotNull(query.UserLogin);
            Assert.IsNotNull(query.UserPassword);
        }

        [TestMethod]
        public void TestClientQuery_Clear()
        {
            ClientQuery query = _GetClientQuery();

            query.Clear();
            Assert.AreEqual(0, query.Arguments.Count);
        }

        [TestMethod]
        public void TestClientQuery_EncodePacket()
        {
            ClientQuery query = _GetClientQuery();
            byte[] packet = query.EncodePacket();

            Assert.IsNotNull(packet);
            Assert.IsTrue(packet.Length > 10);
        }

        [TestMethod]
        public void TestClientQuery_Verify()
        {
            ClientQuery query = _GetClientQuery();

            Assert.IsTrue(query.Verify(true));
        }

        [TestMethod]
        public void TestClientQuery_Dump()
        {
            ClientQuery query = _GetClientQuery();

            StringWriter writer = new StringWriter();
            query.Dump(writer);
            string text = writer.ToString();
            Assert.IsNotNull(text);
        }
    }
}
