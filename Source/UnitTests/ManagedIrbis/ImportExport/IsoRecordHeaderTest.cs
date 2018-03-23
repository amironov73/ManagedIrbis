using ManagedIrbis.ImportExport;
using ManagedIrbis.Marc;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.ImportExport
{
    [TestClass]
    public class IsoRecordHeaderTest
    {
        [TestMethod]
        public void IsoRecordHeader_Construction_1()
        {
            IsoRecordHeader header = new IsoRecordHeader();
            Assert.AreEqual((MarcRecordStatus)0, header.RecordStatus);
            Assert.AreEqual((MarcRecordType)0, header.RecordType);
            Assert.AreEqual((MarcBibliographicalIndex)0, header.BibliographicalIndex);
            Assert.AreEqual((MarcBibliographicalLevel)0, header.BibliographicalLevel);
            Assert.AreEqual((MarcCatalogingRules)0, header.CatalogingRules);
            Assert.AreEqual((MarcRelatedRecord)0, header.RelatedRecord);
        }

        [TestMethod]
        public void IsoRecordHeader_Encode_1()
        {
            IsoRecordHeader header = IsoRecordHeader.GetDefault();
            byte[] expected = { 0, 0, 110, 97, 109, 117, 32, 32, 0, 0 };
            byte[] actual = new byte[10];
            header.Encode(actual, 2);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsoRecordHeader_Parse_1()
        {
            string text = "namu  ";
            IsoRecordHeader header = IsoRecordHeader.Parse(text);
            Assert.AreEqual(MarcRecordStatus.New, header.RecordStatus);
            Assert.AreEqual(MarcRecordType.Text, header.RecordType);
            Assert.AreEqual(MarcBibliographicalIndex.Monograph, header.BibliographicalIndex);
            Assert.AreEqual(MarcBibliographicalLevel.Unknown, header.BibliographicalLevel);
            Assert.AreEqual(MarcCatalogingRules.NotConforming, header.CatalogingRules);
            Assert.AreEqual(MarcRelatedRecord.NotRequired, header.RelatedRecord);
        }

        [TestMethod]
        public void IsoRecordHeader_Parse_2()
        {
            byte[] bytes = { 0, 0, 110, 97, 109, 117, 32, 32, 0, 0 };
            IsoRecordHeader header = IsoRecordHeader.Parse(bytes, 2);
            Assert.AreEqual(MarcRecordStatus.New, header.RecordStatus);
            Assert.AreEqual(MarcRecordType.Text, header.RecordType);
            Assert.AreEqual(MarcBibliographicalIndex.Monograph, header.BibliographicalIndex);
            Assert.AreEqual(MarcBibliographicalLevel.Unknown, header.BibliographicalLevel);
            Assert.AreEqual(MarcCatalogingRules.NotConforming, header.CatalogingRules);
            Assert.AreEqual(MarcRelatedRecord.NotRequired, header.RelatedRecord);
        }

        [TestMethod]
        public void IsoRecordHeader_ToString_1()
        {
            Assert.AreEqual("namu  ", IsoRecordHeader.GetDefault().ToString());
        }
    }
}
