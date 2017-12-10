using System;
using System.IO;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class CatalogDeltaTest
    {
        [NotNull]
        private CatalogDelta _GetDelta()
        {
            return new CatalogDelta
            {
                Id = 10,
                FirstDate = new DateTime(2017, 12, 9),
                SecondDate = new DateTime(2017, 12, 10),
                Database = "IBIS",
                NewRecords = new[] { 101, 102, 103 },
                DeletedRecords = new[] { 80, 81, 82 },
                AlteredRecords = new[] { 90, 91, 92 }
            };
        }

        private void _Compare
            (
                [CanBeNull] int[] first,
                [CanBeNull] int[] second
            )
        {
            if (ReferenceEquals(first, null))
            {
                Assert.IsNull(second);
            }
            else
            {
                Assert.IsNotNull(second);
                Assert.AreEqual(first.Length, second.Length);
                for (int i = 0; i < first.Length; i++)
                {
                    Assert.AreEqual(first[i], second[i]);
                }
            }
        }

        private void _Compare
            (
                [NotNull] CatalogDelta first,
                [NotNull] CatalogDelta second
            )
        {
            Assert.AreEqual(first.Id, second.Id);
            Assert.AreEqual(first.FirstDate, second.FirstDate);
            Assert.AreEqual(first.SecondDate, second.SecondDate);
            Assert.AreEqual(first.Database, second.Database);
            _Compare(first.NewRecords, second.NewRecords);
            _Compare(first.DeletedRecords, second.DeletedRecords);
            _Compare(first.AlteredRecords, second.AlteredRecords);
        }

        [TestMethod]
        public void CatalogDelta_Construciton_1()
        {
            CatalogDelta delta = new CatalogDelta();
            Assert.AreEqual(0, delta.Id);
            Assert.AreEqual(DateTime.MinValue, delta.FirstDate);
            Assert.AreEqual(DateTime.MinValue, delta.SecondDate);
            Assert.IsNull(delta.Database);
            Assert.IsNull(delta.NewRecords);
            Assert.IsNull(delta.DeletedRecords);
            Assert.IsNull(delta.AlteredRecords);
        }

        [TestMethod]
        public void CatalogDelta_Create_1()
        {
            CatalogState firstState = new CatalogState
            {
                Id = 1,
                Date = new DateTime(2017, 12, 8, 22, 42, 0),
                Database = "IBIS",
                MaxMfn = 4,
                LogicallyDeleted = new[] { 1, 2 },
                Records = new[]
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
                        Status = RecordStatus.Last,
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

            CatalogState secondState = new CatalogState
            {
                Id = 2,
                Date = new DateTime(2017, 12, 9, 22, 42, 0),
                Database = "IBIS",
                MaxMfn = 6,
                LogicallyDeleted = new[] { 1, 2, 3 },
                Records = new[]
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
                        Version = 2
                    },
                    new RecordState
                    {
                        Id = 5,
                        Mfn = 4,
                        Status = RecordStatus.Last,
                        Version = 4
                    },
                    new RecordState
                    {
                        Id = 6,
                        Mfn = 5,
                        Status = RecordStatus.Last,
                        Version = 1
                    },
                    new RecordState
                    {
                        Id = 7,
                        Mfn = 6,
                        Status = RecordStatus.Last,
                        Version = 1
                    }
                }
            };

            CatalogDelta delta = CatalogDelta.Create
                (
                    firstState,
                    secondState
                );
            Assert.AreEqual(0, delta.Id);
            Assert.AreEqual(new DateTime(2017, 12, 8, 22, 42, 0, 0), delta.FirstDate);
            Assert.AreEqual(new DateTime(2017, 12, 9, 22, 42, 0, 0), delta.SecondDate);
            Assert.AreEqual("IBIS", delta.Database);
            Assert.IsNotNull(delta.NewRecords);
            Assert.AreEqual(2, delta.NewRecords.Length);
            Assert.AreEqual(5, delta.NewRecords[0]);
            Assert.AreEqual(6, delta.NewRecords[1]);
            Assert.IsNotNull(delta.DeletedRecords);
            Assert.AreEqual(1, delta.DeletedRecords.Length);
            Assert.AreEqual(3, delta.DeletedRecords[0]);
            Assert.IsNotNull(delta.AlteredRecords);
            Assert.AreEqual(1, delta.AlteredRecords.Length);
            Assert.AreEqual(4, delta.AlteredRecords[0]);
        }

        private void _TestSerialization
            (
                [NotNull] CatalogDelta first
            )
        {
            byte[] bytes = first.SaveToMemory();
            CatalogDelta second = bytes.RestoreObjectFromMemory<CatalogDelta>();
            _Compare(first, second);
        }

        [TestMethod]
        public void CatalogDelta_Serialization_1()
        {
            CatalogDelta delta = new CatalogDelta();
            _TestSerialization(delta);

            delta = _GetDelta();
            _TestSerialization(delta);
        }

        [TestMethod]
        public void CatalogDelta_ToXml_1()
        {
            CatalogDelta delta = new CatalogDelta();
            Assert.AreEqual("<catalogDelta />", XmlUtility.SerializeShort(delta));

            delta = _GetDelta();
            Assert.AreEqual("<catalogDelta firstDate=\"2017-12-09T00:00:00\" secondDate=\"2017-12-10T00:00:00\" database=\"IBIS\"><new><mfn>101</mfn><mfn>102</mfn><mfn>103</mfn></new><deleted><mfn>80</mfn><mfn>81</mfn><mfn>82</mfn></deleted><altered><mfn>90</mfn><mfn>91</mfn><mfn>92</mfn></altered></catalogDelta>", XmlUtility.SerializeShort(delta));
        }

        [TestMethod]
        public void CatalogDelta_ToJson_1()
        {
            CatalogDelta delta = new CatalogDelta();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(delta));

            delta = _GetDelta();
            Assert.AreEqual("{'firstDate':'2017-12-09T00:00:00','secondDate':'2017-12-10T00:00:00','database':'IBIS','new':[101,102,103],'deleted':[80,81,82],'altered':[90,91,92]}", JsonUtility.SerializeShort(delta));
        }

        [TestMethod]
        public void CatalogDelta_ToString_1()
        {
            CatalogDelta delta = new CatalogDelta();
            Assert.AreEqual("", delta.ToString().DosToUnix());

            delta = _GetDelta();
            Assert.AreEqual("New: 101, 102, 103\nDeleted: 80, 81, 82\nAltered: 90, 91, 92\n", delta.ToString().DosToUnix());
        }
    }
}
