using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisDatabaseInfoTest
    {
        [TestMethod]
        public void TestIrbisDatabaseInfoConstructor()
        {
            DatabaseInfo info = new DatabaseInfo();
            Assert.AreEqual(null,info.Name);
            Assert.AreEqual(null,info.Description);
            Assert.AreEqual(0, info.MaxMfn);
        }

        private void _TestSerialization
            (
                DatabaseInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            DatabaseInfo second = bytes
                .RestoreObjectFromMemory<DatabaseInfo>();

            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.MaxMfn, second.MaxMfn);
        }

        [TestMethod]
        public void TestIrbisDatabaseInfoSerialization()
        {
            DatabaseInfo info = new DatabaseInfo();
            _TestSerialization(info);

            info.Name = "IBIS";
            info.Description = "Электронный каталог";
            info.MaxMfn = 1000;
            _TestSerialization(info);
        }
    }
}
