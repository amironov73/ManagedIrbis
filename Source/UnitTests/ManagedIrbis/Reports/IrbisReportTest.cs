using System;
using System.IO;
using ManagedIrbis.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Reports;

namespace UnitTests.ManagedIrbis.Reports
{
    [TestClass]
    public class IrbisReportTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void IrbisReport_Constructor_1()
        {
            IrbisReport report = new IrbisReport();
            Assert.IsNull(report.Header);
            Assert.IsNull(report.Footer);
            Assert.IsNotNull(report.Body);
            Assert.AreEqual(0, report.Body.Count);
        }

        private void _TestSaveJson
            (
                IrbisReport first
            )
        {
            string fileName = Path.GetTempFileName();
            first.SaveJson(fileName);

            IrbisReport second = IrbisReport.LoadJsonFile(fileName);
            Assert.IsNotNull(second);
        }

        [TestMethod]
        public void IrbisReport_SaveJson_1()
        {
            IrbisReport report = new IrbisReport();
            _TestSaveJson(report);

            ReportBand headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            DetailsBand detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new TextCell("This is a text"));
            report.Body.Add(detailsBand);
            _TestSaveJson(report);

            ReportBand footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestSaveJson(report);
        }

        private void _TestEvaluate
            (
                IrbisReport report
            )
        {
            AbstractClient client = new LocalClient();
            ReportContext context = new ReportContext(client);
            report.Evaluate(context);
            string text = context.Output.Text;
            Assert.IsNotNull(text);
        }

        [TestMethod]
        public void IrbisReport_Evaluate_1()
        {
            IrbisReport report = new IrbisReport();
            _TestEvaluate(report);

            ReportBand headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            DetailsBand detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new TextCell("This is a text"));
            report.Body.Add(detailsBand);
            _TestEvaluate(report);

            ReportBand footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestEvaluate(report);
        }
    }
}
