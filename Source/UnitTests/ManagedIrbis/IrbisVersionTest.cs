using System.Collections.Generic;

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
    public class IrbisVersionTest
    {
        [NotNull]
        private IrbisVersion _GetVersion()
        {
            IrbisVersion result = new IrbisVersion
            {
                Organization = "Иркутский государственный технический университет",
                Version = "64.2015.1",
                MaxClients = 100000,
                ConnectedClients = 10
            };

            return result;
        }

        [TestMethod]
        public void IrbisVersion_Construction_1()
        {
            IrbisVersion version = new IrbisVersion();
            Assert.IsNull(version.Organization);
            Assert.IsNull(version.Version);
            Assert.AreEqual(0, version.ConnectedClients);
            Assert.AreEqual(0, version.MaxClients);
        }

        [TestMethod]
        public void IrbisVersion_ParseServerResponse_1()
        {
            // В четыре строки
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .AppendAnsi("Иркутский государственный технический университет").NewLine()
                .AppendAnsi("64.2015.1").NewLine()
                .AppendAnsi("10").NewLine()
                .AppendAnsi("100000").NewLine();

            IrbisConnection connection = new IrbisConnection();
            byte[][] query = { new byte[0], new byte[0] };
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse
                (
                    connection,
                    answer,
                    query,
                    true
                );

            IrbisVersion version = IrbisVersion.ParseServerResponse(response);
            Assert.AreEqual("Иркутский государственный технический университет", version.Organization);
            Assert.AreEqual("64.2015.1", version.Version);
            Assert.AreEqual(10, version.ConnectedClients);
            Assert.AreEqual(100000, version.MaxClients);
        }

        [TestMethod]
        public void IrbisVersion_ParseServerResponse_2()
        {
            // В три строки
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .AppendAnsi("64.2015.1").NewLine()
                .AppendAnsi("10").NewLine()
                .AppendAnsi("100000").NewLine();

            IrbisConnection connection = new IrbisConnection();
            byte[][] query = { new byte[0], new byte[0] };
            byte[] answer = builder.Encode();
            ServerResponse response = new ServerResponse
                (
                    connection,
                    answer,
                    query,
                    true
                );

            IrbisVersion version = IrbisVersion.ParseServerResponse(response);
            Assert.IsNull(version.Organization);
            Assert.AreEqual("64.2015.1", version.Version);
            Assert.AreEqual(10, version.ConnectedClients);
            Assert.AreEqual(100000, version.MaxClients);
        }

        [TestMethod]
        public void IrbisVersion_ParseServerResponse_3()
        {
            // В четыре строки
            List<string> response = new List<string>
            {
                "Иркутский государственный технический университет",
                "64.2015.1",
                "10",
                "100000"
            };

            IrbisVersion version = IrbisVersion.ParseServerResponse(response);
            Assert.AreEqual("Иркутский государственный технический университет", version.Organization);
            Assert.AreEqual("64.2015.1", version.Version);
            Assert.AreEqual(10, version.ConnectedClients);
            Assert.AreEqual(100000, version.MaxClients);
        }

        [TestMethod]
        public void IrbisVersion_ParseServerResponse_4()
        {
            // В три строки
            List<string> response = new List<string>
            {
                "64.2015.1",
                "10",
                "100000"
            };

            IrbisVersion version = IrbisVersion.ParseServerResponse(response);
            Assert.IsNull(version.Organization);
            Assert.AreEqual("64.2015.1", version.Version);
            Assert.AreEqual(10, version.ConnectedClients);
            Assert.AreEqual(100000, version.MaxClients);
        }

        private void _TestSerialization
            (
                [NotNull] IrbisVersion first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisVersion second = bytes.RestoreObjectFromMemory<IrbisVersion>();

            Assert.AreEqual(first.Organization, second.Organization);
            Assert.AreEqual(first.Version, second.Version);
            Assert.AreEqual(first.MaxClients, second.MaxClients);
            Assert.AreEqual(first.ConnectedClients, second.ConnectedClients);
        }

        [TestMethod]
        public void IrbisVersion_Serialization_1()
        {
            IrbisVersion version = new IrbisVersion();
            _TestSerialization(version);

            version = _GetVersion();
            _TestSerialization(version);
        }

        [TestMethod]
        public void IrbisVersion_ToXml_1()
        {
            IrbisVersion version = new IrbisVersion();
            Assert.AreEqual("<version />", XmlUtility.SerializeShort(version));

            version = _GetVersion();
            Assert.AreEqual("<version organization=\"Иркутский государственный технический университет\" version=\"64.2015.1\" max-clients=\"100000\" connected-clients=\"10\" />", XmlUtility.SerializeShort(version));
        }

        [TestMethod]
        public void IrbisVersion_ToJson_1()
        {
            IrbisVersion version = new IrbisVersion();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(version));

            version = _GetVersion();
            Assert.AreEqual("{'organization':'Иркутский государственный технический университет','version':'64.2015.1','max-clients':100000,'connected-clients':10}", JsonUtility.SerializeShort(version));
        }

        [TestMethod]
        public void IrbisVersion_ToString_1()
        {
            IrbisVersion version = new IrbisVersion();
            Assert.AreEqual("Version: (null), MaxClients: 0, ConnectedClients: 0, Organization: (null)", version.ToString());

            version = _GetVersion();
            Assert.AreEqual("Version: 64.2015.1, MaxClients: 100000, ConnectedClients: 10, Organization: Иркутский государственный технический университет", version.ToString());
        }
    }
}
