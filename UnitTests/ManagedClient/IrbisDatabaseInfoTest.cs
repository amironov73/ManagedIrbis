using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IrbisDatabaseInfoTest
    {
        [TestMethod]
        public void TestIrbisDatabaseInfoConstructor()
        {
            IrbisDatabaseInfo info = new IrbisDatabaseInfo();
            Assert.AreEqual(null,info.Name);
            Assert.AreEqual(null,info.Description);
            Assert.AreEqual(0, info.MaxMfn);
        }

        private void _TestSerialization
            (
                IrbisDatabaseInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisDatabaseInfo second = bytes
                .RestoreObjectFromMemory<IrbisDatabaseInfo>();

            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.MaxMfn, second.MaxMfn);
        }

        [TestMethod]
        public void TestIrbisDatabaseInfoSerialization()
        {
            IrbisDatabaseInfo info = new IrbisDatabaseInfo();
            _TestSerialization(info);

            info.Name = "IBIS";
            info.Description = "Электронный каталог";
            info.MaxMfn = 1000;
            _TestSerialization(info);
        }
    }
}
