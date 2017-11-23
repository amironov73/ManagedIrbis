using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Client;
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
        public void CellCollection_Clone_1()
        {
            CellCollection first = new CellCollection();
            first.AddRange(_GetCells());
            CellCollection second = first.Clone();
            Assert.AreEqual(first.Count, second.Count);
            for (int i = 0; i < first.Count; i++)
            {
                TextCell cell1 = (TextCell) first[i];
                TextCell cell2 = (TextCell) second[i];
                Assert.AreEqual(cell1.Text, cell2.Text);
            }
        }
    }
}
