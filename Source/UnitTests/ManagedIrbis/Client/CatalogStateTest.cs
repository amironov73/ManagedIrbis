using System;
using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class CatalogStateTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private CatalogState _GetState()
        {
            return new CatalogState
            {
                Id = 1,
                Date = new DateTime(2017, 12, 8, 22, 42, 0),
                Database = "IBIS",
                MaxMfn = 123,
                LogicallyDeleted = new []{1, 2, 3},
                Records = new []
                {
                    new RecordState
                    {
                        Id = 2,
                        Mfn = 1,
                        Status = RecordStatus.LogicallyDeleted,
                        Version = 3
                    },
                    new RecordState
                    {
                        Id = 3,
                        Mfn = 2,
                        Status = RecordStatus.LogicallyDeleted,
                        Version = 2
                    },
                    new RecordState
                    {
                        Id = 4,
                        Mfn = 3,
                        Status = RecordStatus.LogicallyDeleted,
                        Version = 1
                    },
                    new RecordState
                    {
                        Id = 5,
                        Mfn = 4,
                        Status = RecordStatus.Last,
                        Version = 2
                    }
                }
            };
        }

        [TestMethod]
        public void CatalogState_Construction_1()
        {
            CatalogState state = new CatalogState();
            Assert.AreEqual(0, state.Id);
            Assert.AreEqual(DateTime.MinValue, state.Date);
            Assert.IsNull(state.Database);
            Assert.AreEqual(0, state.MaxMfn);
            Assert.IsNull(state.Records);
            Assert.IsNull(state.LogicallyDeleted);
        }

        private void _TestSerialization
            (
                [NotNull] CatalogState first
            )
        {
            byte[] bytes = first.SaveToMemory();
            CatalogState second = bytes.RestoreObjectFromMemory<CatalogState>();
            Assert.AreEqual(first.Id, second.Id);
            Assert.AreEqual(first.Date, second.Date);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.MaxMfn, second.MaxMfn);
            if (ReferenceEquals(first.Records, null))
            {
                Assert.IsNull(second.Records, null);
            }
            else
            {
                Assert.IsNotNull(second.Records);
                Assert.AreEqual(first.Records.Length, second.Records.Length);
                for (int i = 0; i < first.Records.Length; i++)
                {
                    Assert.AreEqual(first.Records[i].Id, second.Records[i].Id);
                    Assert.AreEqual(first.Records[i].Mfn, second.Records[i].Mfn);
                    Assert.AreEqual(first.Records[i].Status, second.Records[i].Status);
                    Assert.AreEqual(first.Records[i].Version, second.Records[i].Version);
                }
            }
            if (ReferenceEquals(first.LogicallyDeleted, null))
            {
                Assert.IsNull(second.LogicallyDeleted);
            }
            else
            {
                Assert.IsNotNull(second.LogicallyDeleted);
                Assert.AreEqual(first.LogicallyDeleted.Length, second.LogicallyDeleted.Length);
                for (int i = 0; i < first.LogicallyDeleted.Length; i++)
                {
                    Assert.AreEqual(first.LogicallyDeleted[i], second.LogicallyDeleted[i]);
                }
            }
        }

        [TestMethod]
        public void CatalogState_Serialization_1()
        {
            CatalogState state = new CatalogState();
            _TestSerialization(state);

            state = _GetState();
            _TestSerialization(state);
        }

        [TestMethod]
        public void CatalogState_ToXml_1()
        {
            CatalogState state = new CatalogState();
            Assert.AreEqual("<database />", XmlUtility.SerializeShort(state));

            state = _GetState();
            Assert.AreEqual("<database date=\"2017-12-08T22:42:00\" database=\"IBIS\" maxMfn=\"123\"><records><record mfn=\"1\" status=\"LogicallyDeleted\" version=\"3\" /><record mfn=\"2\" status=\"LogicallyDeleted\" version=\"2\" /><record mfn=\"3\" status=\"LogicallyDeleted\" version=\"1\" /><record mfn=\"4\" status=\"Last\" version=\"2\" /></records><logicallyDeleted><mfn>1</mfn><mfn>2</mfn><mfn>3</mfn></logicallyDeleted></database>", XmlUtility.SerializeShort(state));
        }

        [TestMethod]
        public void CatalogState_ToJson_1()
        {
            CatalogState state = new CatalogState();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(state));

            state = _GetState();
            Assert.AreEqual("{'date':'2017-12-08T22:42:00','database':'IBIS','maxMfn':123,'records':[{'mfn':1,'status':1,'version':3},{'mfn':2,'status':1,'version':2},{'mfn':3,'status':1,'version':1},{'mfn':4,'status':32,'version':2}],'logicallyDeleted':[1,2,3]}", JsonUtility.SerializeShort(state));
        }

        [TestMethod]
        public void CatalogState_ToString_1()
        {
            CatalogState state = new CatalogState();
            Assert.AreEqual("(null) 0001-01-01 00:00:00 0", state.ToString());

            state = _GetState();
            Assert.AreEqual("IBIS 2017-12-08 22:42:00 123", state.ToString());
        }
    }
}
