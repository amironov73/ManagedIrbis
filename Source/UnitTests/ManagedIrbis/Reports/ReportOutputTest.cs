using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Reports;

namespace UnitTests.ManagedIrbis.Reports
{
    [TestClass]
    public class ReportOutputTest
    {
        [TestMethod]
        public void ReportOutput_Construction_1()
        {
            ReportOutput output = new ReportOutput();
            Assert.AreEqual("", output.Text);
        }

        [TestMethod]
        public void ReportOutput_Write_1()
        {
            string text = "Here is some text";
            ReportOutput output = new ReportOutput();
            output.Write(text);
            Assert.AreEqual(text, output.Text);
        }

        [TestMethod]
        public void ReportOutput_Clear_1()
        {
            ReportOutput output = new ReportOutput();
            output.Write("Some text");
            output.Clear();
            Assert.AreEqual("", output.Text);
        }

        [TestMethod]
        public void ReportOutput_TrimEnd_1()
        {
            ReportOutput output = new ReportOutput();
            output.Write("Some text   ");
            output.TrimEnd();
            Assert.AreEqual("Some text", output.Text);
        }
    }
}
