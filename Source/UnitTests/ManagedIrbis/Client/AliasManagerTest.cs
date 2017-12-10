using System.IO;

using AM.IO;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class AliasManagerTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "aliases.txt"
                );
        }

        [TestMethod]
        public void AliasManager_Construction_1()
        {
            AliasManager manager = new AliasManager();
            string[] aliases = manager.ListAliases();
            Assert.IsNotNull(aliases);
            Assert.AreEqual(0, aliases.Length);
        }

        [TestMethod]
        public void AliasManager_Clear_1()
        {
            AliasManager manager = new AliasManager();
            manager.SetAlias("Alias", "Value");
            string[] aliases = manager.ListAliases();
            Assert.IsNotNull(aliases);
            Assert.AreEqual(1, aliases.Length);
            manager.Clear();
            aliases = manager.ListAliases();
            Assert.IsNotNull(aliases);
            Assert.AreEqual(0, aliases.Length);
        }

        [TestMethod]
        public void AliasManager_FromPlainTextFile_1()
        {
            string fileName = _GetFileName();
            AliasManager manager = AliasManager.FromPlainTextFile(fileName);
            string[] aliases = manager.ListAliases();
            Assert.IsNotNull(aliases);
            Assert.AreEqual(2, aliases.Length);
            string value = manager.GetAliasValue("IBIS");
            Assert.AreEqual("Provider=Connected;Host=127.0.0.1;Port=6666;User=1;Password=1;Db=IBIS;", value);
        }

        [TestMethod]
        public void AliasManager_GetAliasValue_1()
        {
            string name = "Name", expected = "Value";
            AliasManager manager = new AliasManager();
            manager.SetAlias(name, expected);
            string actual = manager.GetAliasValue(name);
            Assert.AreEqual(expected, actual);
            actual = manager.GetAliasValue("NoSuchAlias");
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void AliasManager_ListAliases_1()
        {
            string name = "Name", value = "Value";
            AliasManager manager = new AliasManager();
            manager.SetAlias(name, value);
            string[] aliases = manager.ListAliases();
            Assert.IsNotNull(aliases);
            Assert.AreEqual(1, aliases.Length);
            Assert.AreEqual(name, aliases[0]);
        }

        [TestMethod]
        public void AliasManager_SaveToPlainTextFile_1()
        {
            string fileName = Path.GetTempFileName();
            AliasManager manager = new AliasManager();
            manager.SetAlias("FirstName", "FirstValue");
            manager.SetAlias("SecondName", "SecondValue");
            manager.SaveToPlainTextFile(fileName);
            string actual = File.ReadAllText(fileName, IrbisEncoding.Ansi)
                .DosToUnix();
            Assert.AreEqual("FirstName\nFirstValue\nSecondName\nSecondValue\n", actual);
        }

        [TestMethod]
        public void AliasManager_SetAlias_1()
        {
            string name = "Name", expected = "Value";
            AliasManager manager = new AliasManager();
            manager.SetAlias(name, expected);
            string actual = manager.GetAliasValue(name);
            Assert.AreEqual(expected, actual);
            manager.SetAlias(name, null);
            actual = manager.GetAliasValue(name);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void AliasManager_SetAlias_2()
        {
            string name = "Name", expected = "Value";
            AliasManager manager = new AliasManager();
            manager.SetAlias(name, expected);
            string actual = manager.GetAliasValue(name);
            Assert.AreEqual(expected, actual);
            manager.SetAlias(name, string.Empty);
            actual = manager.GetAliasValue(name);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void AliasManager_SetAlias_3()
        {
            string name = "Name", expected1 = "Value1", expected2 = "Value2";
            AliasManager manager = new AliasManager();
            manager.SetAlias(name, expected1);
            string actual = manager.GetAliasValue(name);
            Assert.AreEqual(expected1, actual);
            manager.SetAlias(name, expected2);
            actual = manager.GetAliasValue(name);
            Assert.AreEqual(expected2, actual);
        }
    }
}
