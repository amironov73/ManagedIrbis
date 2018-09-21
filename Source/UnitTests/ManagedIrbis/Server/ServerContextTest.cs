using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;
using ManagedIrbis.Server;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Server
{
    [TestClass]
    public class ServerContextTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine(Irbis64RootPath, ServerIniFile.FileName);
        }

        [NotNull]
        private ServerContext _GetContext()
        {
            //IniFile iniFile = new IniFile(_GetFileName());
            //ServerIniFile serverIni = new ServerIniFile(iniFile);
            //ServerSetup setup = new ServerSetup(serverIni);
            //IrbisServerEngine engine = new IrbisServerEngine(setup);

            return new ServerContext
            {
                Address = "127.0.0.1",
                CommandCount = 123,
                Connected = new DateTime(2017, 12, 11, 17, 29, 0),
                Id = "1234567",
                LastActivity = new DateTime(2017, 12, 11, 17, 30, 0),
                Password = "password",
                Username = "username",
                Workstation = "A",
                UserData = "User data"
            };
        }

        [TestMethod]
        public void ServerContext_Construction_1()
        {
            ServerContext context = new ServerContext();
            Assert.IsNull(context.Address);
            Assert.AreEqual(0, context.CommandCount);
            Assert.AreEqual(DateTime.MinValue, context.Connected);
            Assert.IsNull(context.Id);
            Assert.AreEqual(DateTime.MinValue, context.LastActivity);
            Assert.IsNull(context.Password);
            Assert.IsNull(context.Username);
            Assert.IsNull(context.Workstation);
            Assert.IsNull(context.UserData);
        }

        [TestMethod]
        public void ServerContext_Properties_1()
        {
            ServerContext context = new ServerContext();
            string address = "gpntb.ru";
            context.Address = address;
            Assert.AreEqual(address, context.Address);
            int commandCount = 123;
            context.CommandCount = commandCount;
            Assert.AreEqual(commandCount, context.CommandCount);
            DateTime connected = new DateTime(2017, 12, 11, 17, 29, 0);
            context.Connected = connected;
            Assert.AreEqual(connected, context.Connected);
            string id = "1234567";
            context.Id = id;
            Assert.AreEqual(id, context.Id);
            DateTime lastActivity = new DateTime(2017, 12, 11, 17, 30, 0);
            context.LastActivity = lastActivity;
            Assert.AreEqual(lastActivity, context.LastActivity);
            string password = "password";
            context.Password = password;
            Assert.AreEqual(password, context.Password);

            string username = "username";
            context.Username = username;
            Assert.AreEqual(username, context.Username);
            string workstation = "A";
            context.Workstation = workstation;
            Assert.AreEqual(workstation, context.Workstation);
            string userData = "User Data";
            context.UserData = userData;
            Assert.AreSame(userData, context.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] ServerContext first
            )
        {
            byte[] bytes = first.SaveToMemory();
            ServerContext second = bytes.RestoreObjectFromMemory<ServerContext>();
            Assert.AreEqual(first.Address, second.Address);
            Assert.AreEqual(first.CommandCount, second.CommandCount);
            Assert.AreEqual(first.Id, second.Id);
            Assert.AreEqual(first.LastActivity, second.LastActivity);
            Assert.AreEqual(first.Password, second.Password);
            Assert.AreEqual(first.Username, second.Username);
            Assert.AreEqual(first.Workstation, second.Workstation);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void ServerContext_Serialization_1()
        {
            ServerContext context = new ServerContext();
            _TestSerialization(context);

            context = _GetContext();
            _TestSerialization(context);
        }

        [TestMethod]
        public void ServerContext_Verify_1()
        {
            ServerContext context = new ServerContext();
            Assert.IsFalse(context.Verify(false));

            context = _GetContext();
            Assert.IsTrue(context.Verify(false));
        }

        [TestMethod]
        public void ServerContext_ToXml_1()
        {
            ServerContext context = new ServerContext();
            Assert.AreEqual("<context />", XmlUtility.SerializeShort(context));

            context = _GetContext();
            Assert.AreEqual("<context><address>127.0.0.1</address><commandCount>123</commandCount><connected>2017-12-11T17:29:00</connected><id>1234567</id><lastActivity>2017-12-11T17:30:00</lastActivity><password>password</password><username>username</username><workstation>A</workstation></context>", XmlUtility.SerializeShort(context));
        }

        [TestMethod]
        public void ServerContext_ToJson_1()
        {
            ServerContext context = new ServerContext();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(context));

            context = _GetContext();
            Assert.AreEqual("{'address':'127.0.0.1','commandCount':123,'connected':'2017-12-11T17:29:00','id':'1234567','lastActivity':'2017-12-11T17:30:00','password':'password','username':'username','workstation':'A'}", JsonUtility.SerializeShort(context));
        }

        [TestMethod]
        public void ServerContext_ToString_1()
        {
            ServerContext context = new ServerContext();
            Assert.AreEqual("(null)", context.ToString());

            context = _GetContext();
            Assert.AreEqual("1234567", context.ToString());
        }
    }
}
