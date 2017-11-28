using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis.Monitoring;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Monitoring
{
    [TestClass]
    public class DatabaseDataTest
    {
        [NotNull]
        private DatabaseData _GetData()
        {
            return new DatabaseData
            {
                DeletedRecords = 100,
                Name = "IBIS",
                LockedRecords = new[] { 1, 2, 3 }
            };
        }

        [TestMethod]
        public void DatabaseData_Construction_1()
        {
            DatabaseData data = new DatabaseData();
            Assert.IsNull(data.Name);
            Assert.AreEqual(0, data.DeletedRecords);
            Assert.IsNull(data.LockedRecords);
        }

        private void _TestSerialization
            (
                [NotNull] DatabaseData first
            )
        {
            byte[] bytes = first.SaveToMemory();
            DatabaseData second = bytes.RestoreObjectFromMemory<DatabaseData>();
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.DeletedRecords, second.DeletedRecords);
            if (ReferenceEquals(first.LockedRecords, null))
            {
                Assert.IsNull(second.LockedRecords);
            }
            else
            {
                Assert.IsNotNull(second.LockedRecords);
                for (int i = 0; i < first.LockedRecords.Length; i++)
                {
                    Assert.AreEqual(first.LockedRecords[i], second.LockedRecords[i]);
                }
            }
        }

        [TestMethod]
        public void DatabaseData_Serialization_1()
        {
            DatabaseData database = new DatabaseData();
            _TestSerialization(database);

            database = _GetData();
            _TestSerialization(database);
        }

        [TestMethod]
        public void DatabaseData_ToXml_1()
        {
            DatabaseData data = new DatabaseData();
            Assert.AreEqual("<database deletedRecords=\"0\" />", XmlUtility.SerializeShort(data));

            data = _GetData();
            Assert.AreEqual("<database name=\"IBIS\" deletedRecords=\"100\"><locked><mfn>1</mfn><mfn>2</mfn><mfn>3</mfn></locked></database>", XmlUtility.SerializeShort(data));
        }

        [TestMethod]
        public void DatabaseData_ToJson_1()
        {
            DatabaseData data = new DatabaseData();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(data));

            data = _GetData();
            Assert.AreEqual("{'name':'IBIS','deletedRecords':100,'lockedRecords':[1,2,3]}", JsonUtility.SerializeShort(data));
        }

        [TestMethod]
        public void DatabaseData_ToString_1()
        {
            DatabaseData data = new DatabaseData();
            Assert.AreEqual("(null)", data.ToString());

            data = _GetData();
            Assert.AreEqual("IBIS", data.ToString());
        }
    }
}
