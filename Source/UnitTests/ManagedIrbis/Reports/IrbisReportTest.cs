using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Client;
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

        private void _TestSaveShortJson
            (
                IrbisReport first
            )
        {
            string fileName = Path.GetTempFileName();
            first.SaveShortJson(fileName);

            IrbisReport second = IrbisReport.LoadShortJson(fileName);
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
                                200,
                                new SubField
                                    (
                                        'a',
                                        "Record" + (i + 1)
                                    )
                            )
                    );
                record.Fields.Add
                    (
                        new RecordField
                            (
                                10,
                                new SubField
                                    (
                                        'd',
                                        (i+1).ToString
                                        (
                                            "F2",
                                            CultureInfo.InvariantCulture
                                        )
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

            ReportBand footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestSaveJson(report);
        }

        [TestMethod]
        public void IrbisReport_SaveJson_2()
        {
            IrbisReport report = new IrbisReport();
            _TestSaveJson(report);

            ReportBand headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            FilterBand filterBand = new FilterBand();
            filterBand.FilterExpression = "if v200^a:' ' then '1' else '0' fi";
            DetailsBand detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new TextCell("This is a text"));
            detailsBand.Cells.Add(new PftCell("'This is a PFT'"));
            filterBand.Body.Add(detailsBand);
            report.Body.Add(filterBand);

            ReportBand footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestSaveJson(report);
        }

        [TestMethod]
        public void IrbisReport_SaveShortJson_1()
        {
            IrbisReport report = new IrbisReport();
            _TestSaveJson(report);

            ReportBand headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            FilterBand filterBand = new FilterBand();
            filterBand.FilterExpression = "if v200^a:' ' then '1' else '0' fi";
            DetailsBand detailsBand = new DetailsBand();
            ReportCell cell = new TextCell("This is a text");
            cell.SetWidth(100).SetHeight(10);
            detailsBand.Cells.Add(cell);
            cell = new PftCell("'This is a PFT'");
            cell.SetWidth(200).SetHeight(10);
            detailsBand.Cells.Add(cell);
            filterBand.Body.Add(detailsBand);
            report.Body.Add(filterBand);

            ReportBand footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;
            _TestSaveShortJson(report);
        }

        private void _TestEvaluatePlainText
            (
                IrbisReport report
            )
        {
            IrbisProvider provider = new LocalProvider();
            ReportContext context = new ReportContext(provider);
            context.Records.AddRange(_GetRecords());
            report.Render(context);
            string text = context.Output.Text;
            Assert.IsNotNull(text);
            string fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, text);
        }

        private void _TestEvaluateHtml
            (
                IrbisReport report
            )
        {
            IrbisProvider client = new LocalProvider();
            ReportContext context = new ReportContext(client);
            context.Records.AddRange(_GetRecords());
            context.SetDriver(new HtmlDriver());
            report.Render(context);
            string text = context.Output.Text;
            Assert.IsNotNull(text);
            string fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, text);
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

        [TestMethod]
        public void IrbisReport_Evaluate_4()
        {
            IrbisReport report = new IrbisReport();

            ReportBand headerBand = new ReportBand();
            headerBand.Cells.Add(new TextCell("Header"));
            report.Header = headerBand;

            ReportBand footerBand = new ReportBand();
            footerBand.Cells.Add(new TextCell("Footer"));
            report.Footer = footerBand;

            CompositeBand compositeBand = new CompositeBand();

            DetailsBand detailsBand = new DetailsBand();
            detailsBand.Cells.Add(new IndexCell());
            detailsBand.Cells.Add(new TextCell("::"));
            detailsBand.Cells.Add(new PftCell("v200^a"));
            detailsBand.Cells.Add(new PftCell("v10^d"));
            compositeBand.Body.Add(detailsBand);

            TotalBand totalBand = new TotalBand();
            totalBand.Cells.Add(new TextCell());
            totalBand.Cells.Add(new TextCell());
            totalBand.Cells.Add(new TextCell("Sum"));
            totalBand.Cells.Add(new TotalCell(0,3,TotalFunction.Sum,
                "F2"));
            compositeBand.Footer = totalBand;

            report.Body.Add(compositeBand);

            _TestSaveJson(report);
            _TestEvaluatePlainText(report);
        }
    }
}
