using System.IO;
using System.Text;

using AM.IO;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class ClientLMTest
    {
        [TestMethod]
        public void ClientLM_Construction_1()
        {
            ClientLM manager = new ClientLM();
            Assert.AreEqual(ClientLM.DefaultSalt, manager.Salt);
            Assert.AreSame(IrbisEncoding.Ansi, manager.Encoding);
        }

        [TestMethod]
        public void ClientLM_Construction_2()
        {
            string salt = "Salt";
            Encoding encoding = Encoding.ASCII;
            ClientLM manager = new ClientLM(encoding, salt);
            Assert.AreSame(salt, manager.Salt);
            Assert.AreSame(encoding, manager.Encoding);
        }

        [TestMethod]
        public void ClientLM_ComputeHash_1()
        {
            ClientLM manager = new ClientLM();
            string actual = manager
                .ComputeHash("Иркутский государственный технический университет");
            string expected = "\x040E\x00A0\x00A0\x040E\x045B";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClientLM_ComputeHash_2()
        {
            string user = "Иркутский государственный технический университет";
            IniFile iniFile = new IniFile();
            IniFile.Section section = iniFile.CreateSection("Main");
            section["User"] = user;

            ClientLM manager = new ClientLM();
            section["Common"] = manager.ComputeHash(user);

            Assert.IsTrue(manager.CheckHash(iniFile));
        }

        [TestMethod]
        public void ClientLM_CheckHash_1()
        {
            IniFile iniFile = new IniFile();
            IniFile.Section section = iniFile.CreateSection("Main");
            section["User"] = "Иркутский государственный технический университет";
            section["Common"] = "\x040E\x00A0\x00A0\x040E\x045B";

            ClientLM manager = new ClientLM();
            Assert.IsTrue(manager.CheckHash(iniFile));
        }

        [TestMethod]
        public void ClientLM_CheckHash_2()
        {
            IniFile iniFile = new IniFile();
            IniFile.Section section = iniFile.CreateSection("Main");
            section["User"] = "Иркутский государственный технический университет";
            section["Common"] = "\x040E\x00A0\x00A0\x040E\x045C";

            ClientLM manager = new ClientLM();
            Assert.IsFalse(manager.CheckHash(iniFile));
        }

        [TestMethod]
        public void ClientLM_CheckHash_3()
        {
            IniFile iniFile = new IniFile();
            IniFile.Section section = iniFile.CreateSection("Main");
            section["User"] = "Иркутский государственный технический университет";

            ClientLM manager = new ClientLM();
            Assert.IsFalse(manager.CheckHash(iniFile));
        }
    }
}
