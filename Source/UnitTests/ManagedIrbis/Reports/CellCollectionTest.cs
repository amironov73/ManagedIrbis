using System;
using System.IO;

using AM;
using AM.Runtime;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Reports;

namespace UnitTests.ManagedIrbis.Reports
{
    [TestClass]
    public class CellCollectionTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private ReportCell[] _GetCells()
        {
            return new ReportCell[]
            {
                new TextCell("Cell1"),
                new TextCell("Cell2"),
                new TextCell("Cell3")
            };
        }

        [TestMethod]
        public void CellCollection_Construction_1()
        {
            CellCollection collection = new CellCollection();
            Assert.IsNull(collection.Band);
            Assert.IsNull(collection.Report);
            Assert.IsFalse(collection.ReadOnly);
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void CellCollection_Add_1()
        {
            ReportBand band = new ReportBand();
            ReportCell cell = new TextCell();
            band.Cells.Add(cell);
            Assert.AreSame(band, cell.Band);
        }

        [TestMethod]
        public void CellCollection_Add_2()
        {
            IrbisReport report = new IrbisReport();
            ReportBand band = new ReportBand();
            report.Body.Add(band);
            ReportCell cell = new TextCell();
            band.Cells.Add(cell);
            Assert.AreSame(report, cell.Report);
        }

        [TestMethod]
        public void CellCollection_AddRange_1()
        {
            CellCollection collection = new CellCollection();
            ReportCell[] cells = _GetCells();
            collection.AddRange(cells);
            Assert.AreEqual(cells.Length, collection.Count);
            for (int i = 0; i < cells.Length; i++)
            {
                Assert.AreSame(cells[i], collection[i]);
            }
        }

        [TestMethod]
        public void CellCollection_AsReadOnly_1()
        {
            CellCollection first = new CellCollection();
            first.AddRange(_GetCells());
            CellCollection second = first.AsReadOnly();
            Assert.IsTrue(second.ReadOnly);
            Assert.AreEqual(first.Count, second.Count);
            for (int i = 0; i < first.Count; i++)
            {
                TextCell cell1 = (TextCell) first[i];
                TextCell cell2 = (TextCell) second[i];
                Assert.AreEqual(cell1.Text, cell2.Text);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void CellCollection_AsReadOnly_2()
        {
            CellCollection first = new CellCollection();
            first.AddRange(_GetCells());
            CellCollection second = first.AsReadOnly();
            ReportCell cell = new TextCell();
            second.Add(cell);
        }

        [TestMethod]
        public void CellCollection_Clear_1()
        {
            CellCollection collection = new CellCollection();
            collection.AddRange(_GetCells());
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void CellCollection_Clone_1()
        {
            CellCollection first = new CellCollection();
            first.AddRange(_GetCells());
            CellCollection second = first.Clone();
            Assert.AreEqual(first.Count, second.Count);
            for (int i = 0; i < first.Count; i++)
            {
                TextCell cell1 = (TextCell)first[i];
                TextCell cell2 = (TextCell)second[i];
                Assert.AreEqual(cell1.Text, cell2.Text);
            }
        }

        [TestMethod]
        public void CellCollection_Dispose_1()
        {
            CellCollection collection = new CellCollection();
            collection.AddRange(_GetCells());
            collection.Dispose();
        }

        [TestMethod]
        public void CellCollection_Find_1()
        {
            CellCollection collection = new CellCollection();
            collection.AddRange(_GetCells());
            ReportCell found = collection
                .Find(cell => ((TextCell)cell).Text == "Cell2");
            Assert.IsNotNull(found);
            Assert.AreEqual("Cell2", ((TextCell)found).Text);

            found = collection
                .Find(cell => ((TextCell)cell).Text == "nosuchcell");
            Assert.IsNull(found);
        }

        [TestMethod]
        public void CellCollection_FindAll_1()
        {
            CellCollection collection = new CellCollection();
            collection.AddRange(_GetCells());
            ReportCell[] found = collection
                .FindAll(cell => string.CompareOrdinal
                    (
                        ((TextCell)cell).Text,
                        "Cell2"
                    ) >= 0);
            Assert.AreEqual(2, found.Length);
            Assert.AreEqual("Cell2", ((TextCell)found[0]).Text);
            Assert.AreEqual("Cell3", ((TextCell)found[1]).Text);

            found = collection.FindAll(cell => string.CompareOrdinal
                (
                    ((TextCell)cell).Text,
                    "Cell9"
                ) >= 0);
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void CellCollection_Insert_1()
        {
            CellCollection collection = new CellCollection();
            collection.AddRange(_GetCells());
            ReportCell cell = new TextCell();
            collection.Insert(0, cell);
            Assert.AreSame(cell, collection[0]);
            Assert.AreEqual(4, collection.Count);
        }

        [TestMethod]
        public void CellCollection_Remove_1()
        {
            CellCollection collection = new CellCollection();
            collection.AddRange(_GetCells());
            ReportCell cell = collection[1];
            collection.Remove(cell);
            Assert.AreEqual(2, collection.Count);
        }

        [TestMethod]
        public void CellCollection_RemoveAt_1()
        {
            CellCollection collection = new CellCollection();
            collection.AddRange(_GetCells());
            collection.RemoveAt(1);
            Assert.AreEqual(2, collection.Count);
        }

        private void _TestSerialization
            (
                [NotNull] CellCollection first
            )
        {
            byte[] bytes = first.SaveToMemory();
            CellCollection second = bytes.RestoreObjectFromMemory<CellCollection>();
            Assert.AreEqual(first.Count, second.Count);
            for (int i = 0; i < first.Count; i++)
            {
                TextCell cell1 = (TextCell) first[i];
                TextCell cell2 = (TextCell) second[i];
                Assert.AreEqual(cell1.Text, cell2.Text);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CellCollection_Serialization_1()
        {
            CellCollection collection = new CellCollection();
            _TestSerialization(collection);

            //collection.AddRange(_GetCells());
            //_TestSerialization(collection);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CellCollection_Serialization_2()
        {
            CellCollection collection = new CellCollection();
            MemoryStream stream = new MemoryStream();
            BinaryReader reader = new BinaryReader(stream);
            collection.RestoreFromStream(reader);
        }

        [TestMethod]
        public void CellCollection_SetItem_1()
        {
            CellCollection collection = new CellCollection();
            collection.AddRange(_GetCells());
            TextCell cell = new TextCell();
            collection[2] = cell;
            Assert.AreSame(cell, collection[2]);
        }

        [TestMethod]
        public void CellCollection_Verify_1()
        {
            CellCollection collection = new CellCollection();
            Assert.IsTrue(collection.Verify(false));

            collection.AddRange(_GetCells());
            Assert.IsTrue(collection.Verify(false));
        }
    }
}
