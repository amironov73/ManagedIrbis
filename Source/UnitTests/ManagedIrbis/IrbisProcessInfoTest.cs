using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisProcessInfoTest
    {
        [NotNull]
        private IrbisProcessInfo _GetProcess()
        {
            return new IrbisProcessInfo
            {
                Number = "1",
                IPAddress = "Disconnected",
                Name = "name",
                ClientID = "994334",
                Workstation = "\"Каталогизатор\"",
                Started = "20.11.2017 15:53:40",
                LastCommand = "IRBIS_MAXMFN",
                CommandNumber = "76",
                ProcessID = "4284",
                State = "Активный"
            };
        }

        [TestMethod]
        public void IrbisProcessInfo_Construction_1()
        {
            IrbisProcessInfo info = new IrbisProcessInfo();
            Assert.IsNull(info.ClientID);
            Assert.IsNull(info.CommandNumber);
            Assert.IsNull(info.IPAddress);
            Assert.IsNull(info.LastCommand);
            Assert.IsNull(info.Name);
            Assert.IsNull(info.Number);
            Assert.IsNull(info.ProcessID);
            Assert.IsNull(info.Started);
            Assert.IsNull(info.State);
            Assert.IsNull(info.Workstation);
        }

        private void _TestSerialization
            (
                IrbisProcessInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisProcessInfo second = bytes
                .RestoreObjectFromMemory<IrbisProcessInfo>();

            Assert.AreEqual(first.ClientID, second.ClientID);
            Assert.AreEqual(first.CommandNumber, second.CommandNumber);
            Assert.AreEqual(first.IPAddress, second.IPAddress);
            Assert.AreEqual(first.LastCommand, second.LastCommand);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.ProcessID, second.ProcessID);
            Assert.AreEqual(first.Started, second.Started);
            Assert.AreEqual(first.State, second.State);
            Assert.AreEqual(first.Workstation, second.Workstation);
        }

        [TestMethod]
        public void IrbisProcessInfo_Serialization_1()
        {
            IrbisProcessInfo info = new IrbisProcessInfo();
            _TestSerialization(info);

            info.Name = "abc";
            _TestSerialization(info);
        }

        [TestMethod]
        public void IrbisProcessInfo_Parse_1()
        {
            IrbisConnection connection = new IrbisConnection();
            ResponseBuilder builder = new ResponseBuilder();
            builder.AppendAnsi
                (
                    "2\r\n"
                    + "9\r\n"
                    + "*\r\n"
                    + "Local IP address\r\n"
                    + "Сервер ИРБИС\r\n"
                    + "*****\r\n"
                    + "*****\r\n"
                    + "20.11.2017 15:52:40\r\n"
                    + "*****\r\n"
                    + "*****\r\n"
                    + "4284\r\n"
                    + "Активный\r\n"
                    + "1\r\n"
                    + "Disconnected\r\n"
                    + "1\r\n"
                    + "994334\r\n"
                    + "\"Каталогизатор\"\r\n"
                    + "20.11.2017 15:53:40\r\n"
                    + "IRBIS_MAXMFN\r\n"
                    + "76\r\n"
                    + "1036\r\n"
                    + "Пассивный\r\n"
                );
            byte[] rawAnswer = builder.Encode();
            byte[][] rawRequest = { new byte[0], new byte[0] };
            ServerResponse response = new ServerResponse
                (
                    connection,
                    rawAnswer,
                    rawRequest,
                    true
                );
            IrbisProcessInfo[] processes = IrbisProcessInfo.Parse(response);
            Assert.AreEqual(2, processes.Length);
        }

        [TestMethod]
        public void IrbisProcessInfo_ToJson_1()
        {
            IrbisProcessInfo process = new IrbisProcessInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(process));

            process = _GetProcess();
            Assert.AreEqual("{'number':'1','ip-address':'Disconnected','name':'name','client-id':'994334','workstation':'\"Каталогизатор\"','started':'20.11.2017 15:53:40','last-command':'IRBIS_MAXMFN','command-number':'76','process-id':'4284','state':'Активный'}", JsonUtility.SerializeShort(process));
        }

        [TestMethod]
        public void IrbisProcessInfo_ToXml_1()
        {
            IrbisProcessInfo process = new IrbisProcessInfo();
            Assert.AreEqual("<process />", XmlUtility.SerializeShort(process));

            process = _GetProcess();
            Assert.AreEqual("<process number=\"1\" ip-address=\"Disconnected\" name=\"name\" client-id=\"994334\" workstation=\"&quot;Каталогизатор&quot;\" started=\"20.11.2017 15:53:40\" last-command=\"IRBIS_MAXMFN\" command-number=\"76\" process-id=\"4284\" state=\"Активный\" />", XmlUtility.SerializeShort(process));
        }

        [TestMethod]
        public void IrbisProcessInfo_ToString_1()
        {
            IrbisProcessInfo process = new IrbisProcessInfo();
            Assert.AreEqual("Number: (null), IPAddress: (null), Name: (null), ID: (null), Workstation: (null), Started: (null), LastCommand: (null), CommandNumber: (null), ProcessID: (null), State: (null)", process.ToString());

            process = _GetProcess();
            Assert.AreEqual("Number: 1, IPAddress: Disconnected, Name: name, ID: 994334, Workstation: \"Каталогизатор\", Started: 20.11.2017 15:53:40, LastCommand: IRBIS_MAXMFN, CommandNumber: 76, ProcessID: 4284, State: Активный", process.ToString());
        }
    }
}
