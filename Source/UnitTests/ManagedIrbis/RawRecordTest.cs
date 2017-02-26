using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class RawRecordTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void RawRecord_Constructor_1()
        {
            RawRecord record = new RawRecord();
            Assert.IsNull(record.Database);
            Assert.AreEqual(0, record.Mfn);
            Assert.AreEqual((RecordStatus)0, record.Status);
            Assert.IsFalse(record.Deleted);
            Assert.AreEqual(0, record.Version);
            Assert.IsNull(record.Lines);
        }

        private string[] _GetLines()
        {
            string[] result =
            {
                "154569#0",
                "0#1",
                "920#PAZK",
                "900#^ta^b05",
                "102#RU",
                "101#rus",
                "919#^arus^n02^kPSBO",
                "999#0000000",
                "903#-262592797",
                "2002#7",
                "2020#^a2009030601115651^b2009\03\06^c5^dдудкина",
                "210#^CБашкирское книжное изд-во^AУфа^D1976",
                "215#^A143",
                "907#^CКТ^A20161201^BНижегородова",
                "907#^CПК^A20161205^BАрестова",
                "701#^AАмиров^BЯ. С.^GЯгафар Суфиянович",
                "701#^AАбызгильдин^BЮ. М.",
                "701#^AРусанович^BД. А.",
                "701#^AТищенко^BВ. Е.",
                "200#^AВопросы рационального использования отходов нефтепереработки и нефтехимии^FЯ. С. Амиров [и др.]",
                "907#^CКР^A20161227^BМонахова",
                "910#^A0^b389234^c?^dKX",
                "905#^D1^F2^S1^21",
                "&&&&&Вопросы рационального использования отходов нефтепереработки и нефтехимии / Я. С. Амиров [и др.], 1976. - 143 с. - Кол-во научного: 1"
            };

            return result;
        }

        [TestMethod]
        public void RawRecord_Parse_1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "rawRecord.txt"
                );
            string text = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            RawRecord record = RawRecord.Parse(text);
            Assert.AreEqual(154569, record.Mfn);
            Assert.AreEqual(22, record.Lines.Length);
        }

        [TestMethod]
        public void RawRecord_Parse_2()
        {
            string[] lines = _GetLines();
            RawRecord record = RawRecord.Parse(lines);
            Assert.AreEqual(154569, record.Mfn);
            Assert.AreEqual(22, record.Lines.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RawRecord_Parse_Exception_1()
        {
            RawRecord.Parse(new string[0]);
        }

        [TestMethod]
        public void RawRecord_EncodeRecord_1()
        {
            string[] lines = _GetLines();
            RawRecord record = RawRecord.Parse(lines);
            string encoded = record.EncodeRecord();
            Assert.IsNotNull(encoded);
        }

        [TestMethod]
        public void RawRecord_ToString_1()
        {
            string[] lines = _GetLines();
            RawRecord record = RawRecord.Parse(lines);
            string encoded = record.ToString();
            Assert.IsNotNull(encoded);
        }

        [TestMethod]
        public void RawRecord_Deleted_1()
        {
            string[] lines = _GetLines();
            RawRecord record = RawRecord.Parse(lines);
            record.Deleted = true;
            Assert.AreEqual
                (
                    RecordStatus.LogicallyDeleted,
                    record.Status & RecordStatus.LogicallyDeleted
                );
            record.Deleted = false;
            Assert.AreEqual
                (
                    (RecordStatus)0,
                    record.Status & RecordStatus.LogicallyDeleted
                );
        }

        [TestMethod]
        public void RawRecord_Database_1()
        {
            const string ibis = "IBIS";
            string[] lines = _GetLines();
            RawRecord record = RawRecord.Parse(lines);
            record.Database = ibis;
            Assert.AreEqual(ibis, record.Database);
        }
    }
}
