using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Client;
using ManagedIrbis.Reports;

namespace UnitTests.ManagedIrbis.Reports
{
    [TestClass]
    public class TextCellTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void TextCell_Construction_1()
        {
            TextCell cell = new TextCell();
            Assert.IsNotNull(cell.Attributes);
            Assert.IsNull(cell.Band);
            Assert.IsNull(cell.Report);
            Assert.IsNull(cell.UserData);
            Assert.IsNull(cell.Text);
        }

        [TestMethod]
        public void TextCell_Construction_2()
        {
            string text = "Text";
            TextCell cell = new TextCell(text);
            Assert.IsNotNull(cell.Attributes);
            Assert.IsNull(cell.Band);
            Assert.IsNull(cell.Report);
            Assert.IsNull(cell.UserData);
            Assert.AreSame(text, cell.Text);
        }

        [TestMethod]
        public void TextCell_Compute_1()
        {
            string text = "Text";
            TextCell cell = new TextCell(text);
            using (IrbisProvider provider = GetProvider())
            {
                ReportContext context = new ReportContext(provider);
                string output = cell.Compute(context);
                Assert.AreEqual(text, output);
            }
        }

        [TestMethod]
        public void TextCell_Render_1()
        {
            string text = "Text";
            TextCell cell = new TextCell(text);
            using (IrbisProvider provider = GetProvider())
            {
                ReportContext context = new ReportContext(provider);
                cell.Render(context);
                string output = context.Output.Text;
                Assert.AreEqual("Text\t", output);
            }
        }

        [TestMethod]
        public void TextCell_Verify_1()
        {
            TextCell cell = new TextCell();
            Assert.IsTrue(cell.Verify(false));
        }

        [TestMethod]
        public void TextCell_Dispose_1()
        {
            TextCell cell = new TextCell();
            cell.Dispose();
        }

        [TestMethod]
        public void TextCell_Clone_1()
        {
            TextCell first = new TextCell("text")
            {
                UserData = "user data"
            };
            TextCell second = (TextCell) first.Clone();
            Assert.AreEqual(first.Attributes.Count, second.Attributes.Count);
            Assert.AreSame(first.Band, second.Band);
            Assert.AreSame(first.Report, second.Report);
            Assert.AreSame(first.UserData, second.UserData);
            Assert.AreEqual(first.Text, second.Text);
        }
    }
}
