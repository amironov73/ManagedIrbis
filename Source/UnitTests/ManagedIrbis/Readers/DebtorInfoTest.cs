using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Readers;

namespace UnitTests.ManagedIrbis.Readers
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
            Assert.AreEqual(first.Name, second.Name);
        }

        [TestMethod]
        public void DebtorInfo_Serialization_1()
        {
            DebtorInfo debtorInfo = new DebtorInfo();
            _TestSerialization(debtorInfo);

            debtorInfo.Name = "Иванов Иван Иванович";
            _TestSerialization(debtorInfo);
        }
    }
}
