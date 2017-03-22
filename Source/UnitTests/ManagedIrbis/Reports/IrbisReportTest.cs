using System;
using System.Collections.Generic;
using System.IO;
using ManagedIrbis.Client;
using ManagedIrbis.Source.Reports.Drivers;
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

        private List<MarcRecord> _GetRecords()
        {
            List<MarcRecord> result = new List<MarcRecord>();

            for (int i = 0; i < 10; i++)
            {
                MarcRecord record = new MarcRecord();

                record.Fields.Add
                    (
                        new RecordField
                            (
                                "200",
                                new SubField
                                    (
                                        'a',
                                        "Record" + (i + 1)
                                    )
                            )
                    );

                result.Add(record);
            }

            return result;
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

        private void _TestEvaluatePlainText
            (
                IrbisReport report
            )
        {
            AbstractClient client = new LocalClient();
            ReportContext context = new ReportContext(client);
            context.Records.AddRange(_GetRecords());
            report.Evaluate(context);
            string text = context.Output.Text;
            Assert.IsNotNull(text);
        }

        private void _TestEvaluateHtml
            (
                IrbisReport report
            )
        {
            AbstractClient client = new LocalClient();
            ReportContext context = new ReportContext(client);
            context.Records.AddRange(_GetRecords());
            context.SetDriver(new HtmlDriver());
            report.Evaluate(context);
            string text = context.Output.Text;
            Assert.IsNotNull(text);
        }

        [TestMethod]
        public void IrbisReport_Evaluate_1()
        {
            IrbisReport report = new IrbisReport();
            _TestEvaluatePlainText(report);

            ReportBand headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            DetailsBand detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new TextCell("This is a text"));
            report.Body.Add(detailsBand);
            _TestEvaluatePlainText(report);

            ReportBand footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestEvaluatePlainText(report);
        }

        [TestMethod]
        public void IrbisReport_Evaluate_2()
        {
            IrbisReport report = new IrbisReport();
            _TestEvaluateHtml(report);

            ReportBand headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            DetailsBand detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new TextCell("This is a text"));
            report.Body.Add(detailsBand);
            _TestEvaluateHtml(report);

            ReportBand footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestEvaluateHtml(report);
        }

        [TestMethod]
        public void IrbisReport_Evaluate_3()
        {
            IrbisReport report = new IrbisReport();

            ReportBand headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            ReportBand footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;

            DetailsBand detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new IndexCell());
            detailsBand.Cells.Add(new TextCell("::"));
            detailsBand.Cells.Add(new PftCell("v200^a"));
            report.Body.Add(detailsBand);

            _TestSaveJson(report);
            _TestEvaluateHtml(report);
        }
    }
}
