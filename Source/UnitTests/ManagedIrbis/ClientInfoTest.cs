using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class ClientInfoTest
    {
        [TestMethod]
        public void ClientInfo_Constructor_1()
        {
            ClientInfo client = new ClientInfo();
            Assert.IsNull(client.Number);
            Assert.IsNull(client.IPAddress);
            Assert.IsNull(client.Port);
            Assert.IsNull(client.Name);
            Assert.IsNull(client.ID);
            Assert.IsNull(client.Workstation);
            Assert.IsNull(client.Registered);
            Assert.IsNull(client.Acknowledged);
            Assert.IsNull(client.LastCommand);
            Assert.IsNull(client.CommandNumber);
        }

        [TestMethod]
        public void ClientInfo_ToString_1()
        {
            ClientInfo client = new ClientInfo
            {
                Number = "1",
                IPAddress = "127.0.0.1",
                Port = "12345",
                Name = "111",
                ID = "123456",
                Workstation = "C",
                Registered = "01:02:03",
                Acknowledged = "02:03:04",
                LastCommand = "N",
                CommandNumber = "123"
            };
            string expected
                = "Number: 1, IPAddress: 127.0.0.1, Port: 12345, "
                  + "Name: 111, ID: 123456, Workstation: C, "
                  + "Registered: 01:02:03, Acknowledged: 02:03:04, "
                  + "LastCommand: N, CommandNumber: 123";
            string actual = client.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
