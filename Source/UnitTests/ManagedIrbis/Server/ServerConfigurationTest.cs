using System.IO;
using System.Net;
using System.Threading.Tasks;

using AM;
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

namespace UnitTests.ManagedIrbis.Server
{
    [TestClass]
    public class ServerConfigurationTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine(Irbis64RootPath, "irbis_server.ini");
        }

        [TestMethod]
        public void ServerConfiguration_Construction_1()
        {
            ServerConfiguration configuration = new ServerConfiguration();
            Assert.IsNull(configuration.AlphabetTablePath);
            Assert.IsNull(configuration.DataPath);
            Assert.IsNull(configuration.SystemPath);
            Assert.IsNull(configuration.UpperCaseTable);
        }

        private void _Check
            (
                [NotNull] ServerConfiguration configuration
            )
        {
            Assert.AreEqual(@"D:\IRBIS64_2015\isisacw", configuration.AlphabetTablePath);
            Assert.AreEqual(@"D:\IRBIS64_2015\DATAI\", configuration.DataPath);
            Assert.AreEqual(@"D:\IRBIS64_2015\", configuration.SystemPath);
            Assert.AreEqual(@"D:\IRBIS64_2015\isisucw", configuration.UpperCaseTable);
        }

        [TestMethod]
        public void ServerConfiguration_FromIniFile_1()
        {
            string fileName = _GetFileName();
            IniFile iniFile = new IniFile(fileName, IrbisEncoding.Ansi, false);
            ServerIniFile serverIni = new ServerIniFile(iniFile);
            ServerConfiguration configuration
                = ServerConfiguration.FromIniFile(serverIni);
            _Check(configuration);
        }

        [TestMethod]
        public void ServerConfiguration_FromIniFile_2()
        {
            string fileName = _GetFileName();
            ServerConfiguration configuration
                = ServerConfiguration.FromIniFile(fileName);
            _Check(configuration);
        }

        [TestMethod]
        public void ServerConfiguration_FromPath_1()
        {
            ServerConfiguration configuration
                = ServerConfiguration.FromPath(@"D:\IRBIS64_2015");
            _Check(configuration);
        }

        [TestMethod]
        public void ServerConfiguration_FromPath_2()
        {
            ServerConfiguration configuration
                = ServerConfiguration.FromPath(@"D:\IRBIS64_2015\");
            _Check(configuration);
        }

        [TestMethod]
        public void ServerConfiguration_FromPath_3()
        {
            ServerConfiguration configuration
                = ServerConfiguration.FromPath(@"D:/IRBIS64_2015");
            _Check(configuration);
        }

        [TestMethod]
        public void ServerConfiguration_FromPath_4()
        {
            ServerConfiguration configuration
                = ServerConfiguration.FromPath(@"D:/IRBIS64_2015/");
            _Check(configuration);
        }

        private void _TestSerialization
            (
                [NotNull] ServerConfiguration first
            )
        {
            byte[] bytes = first.SaveToMemory();
            ServerConfiguration second = bytes.RestoreObjectFromMemory<ServerConfiguration>();
            Assert.AreEqual(first.AlphabetTablePath, second.AlphabetTablePath);
            Assert.AreEqual(first.DataPath, second.DataPath);
            Assert.AreEqual(first.SystemPath, second.SystemPath);
            Assert.AreEqual(first.UpperCaseTable, second.UpperCaseTable);
        }

        [TestMethod]
        public void ServerConfiguration_Serialization_1()
        {
            ServerConfiguration configuration = new ServerConfiguration();
            _TestSerialization(configuration);

            configuration = ServerConfiguration.FromIniFile(_GetFileName());
            _TestSerialization(configuration);
        }

        [TestMethod]
        public void ServerConfiguration_Verify_1()
        {
            ServerConfiguration configuration = new ServerConfiguration();
            Assert.IsFalse(configuration.Verify(false));

            configuration = ServerConfiguration.FromIniFile(_GetFileName());
            Assert.IsTrue(configuration.Verify(false));
        }

        [TestMethod]
        public void ServerConfiguration_ToXml_1()
        {
            ServerConfiguration configuration = new ServerConfiguration();
            Assert.AreEqual("<configuration />", XmlUtility.SerializeShort(configuration));

            configuration = ServerConfiguration.FromIniFile(_GetFileName());
            Assert.AreEqual("<configuration><alphabetTablePath>D:\\IRBIS64_2015\\isisacw</alphabetTablePath><dataPath>D:\\IRBIS64_2015\\DATAI\\</dataPath><systemPath>D:\\IRBIS64_2015\\</systemPath><upperCaseTable>D:\\IRBIS64_2015\\isisucw</upperCaseTable></configuration>", XmlUtility.SerializeShort(configuration));
        }

        [TestMethod]
        public void ServerConfiguration_ToJson_1()
        {
            ServerConfiguration configuration = new ServerConfiguration();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(configuration));

            configuration = ServerConfiguration.FromIniFile(_GetFileName());
            Assert.AreEqual(@"{'alphabetTablePath':'D:\\IRBIS64_2015\\isisacw','dataPath':'D:\\IRBIS64_2015\\DATAI\\','systemPath':'D:\\IRBIS64_2015\\','upperCaseTable':'D:\\IRBIS64_2015\\isisucw'}", JsonUtility.SerializeShort(configuration));
        }

        [TestMethod]
        public void ServerConfiguration_ToString_1()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                ServerConfiguration configuration = new ServerConfiguration();
                Assert.AreEqual("(null)", configuration.ToString());

                configuration = ServerConfiguration.FromIniFile(_GetFileName());
                Assert.AreEqual(@"D:\IRBIS64_2015\", configuration.ToString());
            }
        }
    }
}
