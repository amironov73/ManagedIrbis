using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.NetworkDebugging.Commands
{
    //[TestClass]
    public sealed class DatabaseInfoTest
        : NetworkTestBase
    {
        //[TestMethod]
        public void DatabaseInfo_1()
        {
            using (IrbisConnection connection = GetConnection())
            {
                DatabaseInfo info = connection.GetDatabaseInfo(connection.Database);
                Assert.AreEqual(188583, info.MaxMfn);
                Assert.AreEqual(0, info.LockedRecords.Length);
                Assert.AreEqual(508, info.LogicallyDeletedRecords.Length);
                Assert.AreEqual(8842, info.PhysicallyDeletedRecords.Length);
                Assert.AreEqual(0, info.NonActualizedRecords.Length);
                Assert.IsFalse(info.DatabaseLocked);
            }
        }
    }
}
