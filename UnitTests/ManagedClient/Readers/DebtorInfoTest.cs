using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Readers;

namespace UnitTests.ManagedClient.Readers
{
    [TestClass]
    public class DebtorInfoTest
    {
        private void _TestSerialization
            (
                DebtorInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            DebtorInfo second = bytes
                .RestoreObjectFromMemory<DebtorInfo>();

            Assert.AreEqual(first.Age, second.Age);
            Assert.AreEqual(first.Category, second.Category);
            Assert.AreEqual(first.Fio, second.Fio);
        }

        [TestMethod]
        public void TestDebtorInfoSerialization()
        {
            DebtorInfo debtorInfo = new DebtorInfo();
            _TestSerialization(debtorInfo);

            debtorInfo.Fio = "Иванов Иван Иванович";
            _TestSerialization(debtorInfo);
        }
    }
}
