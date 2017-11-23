using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Reports;

namespace UnitTests.ManagedIrbis.Reports
{
    [TestClass]
    public class ReportCellTest
    {
        [TestMethod]
        public void ReportCell_Construction_1()
        {
            ReportCell cell = new TextCell();
            Assert.IsNotNull(cell.Attributes);
            Assert.IsNull(cell.Band);
            Assert.IsNull(cell.Report);
            Assert.IsNull(cell.UserData);
        }
    }
}
